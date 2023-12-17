using System.Net;
using System.Reflection;
using AdventOfCode.Generator;
using AdventOfCode.Model;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Git = LibGit2Sharp;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
internal static partial class Updater
{
    public static void OpenVsCode(ReadOnlySpan<string> args)
    => Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Microsoft VS Code\Code.exe"), ["--profile", "C#", "--reuse-window", "--", ..args]).WaitForExit();

    public static async Task UpdateWithGit(int year, int day)
    {
        var isBranchExisting = true;
        using (var repo = new Git.Repository(".git"))
        {
            var main = repo.Branches["main"] ?? repo.Branches["master"];
            var branch = repo.Branches[$"problems/Y{year}/D{day}"];
            isBranchExisting = branch is not null;
            var today = Git.Commands.Checkout(repo, branch ?? repo.Branches.Add($"problems/Y{year}/D{day}", main.Tip, allowOverwrite: true));
        }
        if (!isBranchExisting)
        {
            await Update(year, day);
            Process.Start("git", ["add", year.ToString()]).WaitForExit();
            Process.Start("git", ["reset", "**/test/*"]).WaitForExit();
            Process.Start("git", ["commit", "-m", $"Initial commit for Y{year}D{day}"]).WaitForExit();
            using var repo = new Git.Repository(".git");
            repo.Tags.Add($"Y{year}D{day}P1", repo.Head.Tip);
        }
        else
            Console.WriteLine($"{year}/Day{day:00} already exists");
        OpenVsCode([$"{year}/Day{day:00}/README.md", $"{year}/Day{day:00}/Solution.cs", $"{year}/Day{day:00}/input.in"]);
    }

    public static async Task Update(int year, int day)
    {
        var baseAddress = GetBaseAddress();
        var context = GetContext();

        var calendar = await DownloadCalendar(context, baseAddress, year);
        var problem = await DownloadProblem(context, baseAddress, year, day);
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(problem.Title);
        Console.ForegroundColor = color;

        var dir = Dir(year, day);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        UpdateProjectReadme(year);
        UpdateReadmeForYear(calendar);
        UpdateSplashScreen(calendar);
        UpdateReadmeForDay(problem);
        UpdateInput(problem);
        UpdateRefout(problem);
        UpdateTest(problem);
        UpdateSolutionTemplate(problem);
    }

    private static Uri GetBaseAddress()
    => new("https://adventofcode.com");

    private static string GetSession()
    {
        if (!Environment.GetEnvironmentVariables().Contains("SESSION"))
            throw new AocCommuncationException("Specify SESSION environment variable");
        return Environment.GetEnvironmentVariable("SESSION");
    }

    private static IBrowsingContext GetContext()
    {
        var context = BrowsingContext.New(Configuration.Default
            .With(new DefaultHttpRequester("github.com/FaustVX/adventofcode"))
            .WithDefaultLoader()
            .WithCss()
            .WithDefaultCookies()
        );
        context.SetCookie(new(GetBaseAddress().ToString()), "session=" + GetSession());
        return context;
    }

    public static async Task Upload(ISolver solver, bool git, bool benchmark)
    {
        Globals.CurrentRunMode = Mode.Upload;
        var color = Console.ForegroundColor;
        Console.WriteLine();
        var solverResult = Runner.RunSolver(solver);
        Console.WriteLine();

        if (solverResult.errors.Length != 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Uhh-ohh the solution doesn't pass the tests...");
            Console.ForegroundColor = color;
            Console.WriteLine();
            return;
        }

        var problem = await DownloadProblem(GetContext(), GetBaseAddress(), solver.Year(), solver.Day());

        if (problem.Answers.Length == 2)
        {
            Console.WriteLine("Both parts of this puzzle are complete!");
            Console.WriteLine();
        }
        else if (solverResult.answers.Length <= problem.Answers.Length)
        {
            Console.WriteLine($"You need to work on part {problem.Answers.Length + 1}");
            Console.WriteLine();
        }
        else
        {
            var level = problem.Answers.Length + 1;
            var answer = solverResult.answers[problem.Answers.Length];
            Console.WriteLine($"Uploading answer ({answer}) for part {level}...");

            // https://adventofcode.com/{year}/day/{day}/answer
            // level={part}&answer={answer}

            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler) { BaseAddress = GetBaseAddress() };

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("level", level.ToString()),
                new KeyValuePair<string, string>("answer", answer),
            });

            cookieContainer.Add(GetBaseAddress(), new Cookie("session", GetSession()));
            var result = await client.PostAsync($"/{solver.Year()}/day/{solver.Day()}/answer", content);
            result.EnsureSuccessStatusCode();
            var responseString = await result.Content.ReadAsStringAsync();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(responseString));
            var article = document.Body.QuerySelector("body > main > article").TextContent;
            article = ContinueToPart2Regex().Replace(article, "");
            article = YouHaveCompletedDayRegex().Replace(article, "");
            article = YouGuessedRegex().Replace(article, "");
            article = SpacesRegex().Replace(article, "\n");

            if (article.StartsWith("That's the right answer") || article.Contains("You've finished every puzzle"))
            {
                if (git)
                    Process.Start("git", ["add", "*"]).WaitForExit();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
                await Update(solver.Year(), solver.Day());

                if (!git)
                {
                    if (benchmark)
                        Runner.RunBenchmark(solver.GetType());
                    return;
                }

                Git.Signature signature;
                using (var repo = new Git.Repository(".git"))
                    signature = repo.Config.BuildSignature(DateTimeOffset.Now);
                if (problem.Answers.Length == 0)
                {
                    Process.Start("git", ["add", "*/*/input.*"]).WaitForExit();
                    using var repo = new Git.Repository(".git");
                    var tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P1"];
                    var initial = (Git.Commit)tag.Target;
                    var duration = signature.When - initial.Committer.When;
                    Process.Start("git", ["commit", "-m", $"Solved P1 in {(int)duration.TotalHours}:{duration:mm\\:ss}", "--allow-empty"]).WaitForExit();
                    repo.Tags.Add($"Y{problem.Year}D{problem.Day}P2", repo.Head.Tip);
                }
                else
                {
                    Process.Start("git", ["add", "*"]).WaitForExit();
                    Git.Commit initial;
                    TimeSpan duration;
                    using (var repo = new Git.Repository(".git"))
                    {
                        var tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P2"];
                        initial = (Git.Commit)tag.Target;
                        duration = signature.When - initial.Committer.When;
                        Process.Start("git", ["commit", "-m", $"Solved P2 in {(int)duration.TotalHours}:{duration:mm\\:ss}", "--allow-empty"]).WaitForExit();
                        repo.Tags.Remove(tag);
                    }
                    if (benchmark)
                    {
                        Runner.RunBenchmark(solver.GetType());
                        Process.Start("git", ["add", "*"]).WaitForExit();
                        Process.Start("git", ["commit", "-m", "Added Benchmarks", "--allow-empty"]).WaitForExit();
                    }
                    using (var repo = new Git.Repository(".git"))
                    {
                        var branch = repo.Head;
                        var main = repo.Branches["main"] ?? repo.Branches["master"];
                        Git.Commands.Checkout(repo, main);
                        var merge = repo.Merge(branch, signature, new()
                        {
                            FastForwardStrategy = Git.FastForwardStrategy.NoFastForward,
                            CommitOnSuccess = false,
                        });
                        var tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P1"];
                        initial = (Git.Commit)tag.Target;
                        duration = signature.When - initial.Committer.When;
                        Process.Start("git", ["commit", "-m", $"Solved Y{problem.Year}D{problem.Day} in {(int)duration.TotalHours}:{duration:mm\\:ss}", "--allow-empty"]).WaitForExit();
                        Git.Commands.Checkout(repo, branch);
                        repo.Tags.Remove(tag);
                    }
                }
            }
            else if (article.StartsWith("That's not the right answer"))
            {
                Process.Start("git", ["add", "*"]).WaitForExit();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
            }
            else if (article.StartsWith("You gave an answer too recently"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
            }
        }
    }

    private static void WriteFile(string file, string content)
    {
        Console.WriteLine($"Writing {file}");
        File.WriteAllText(file, content);
    }

    private static string Dir(int year, int day)
    => SolverExtensions.WorkingDir(year, day);

    private static async Task<Calendar> DownloadCalendar(IBrowsingContext context, Uri baseUri, int year)
    {
        var document = await context.OpenAsync(baseUri.ToString() + year);
        if (document.StatusCode != HttpStatusCode.OK)
            throw new AocCommuncationException("Could not fetch calendar", document.StatusCode, document.TextContent);
        return Calendar.Parse(year, document);
    }

    private static async Task<Problem> DownloadProblem(IBrowsingContext context, Uri baseUri, int year, int day)
    {
        var uri = baseUri + $"{year}/day/{day}";
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Updating " + uri);
        Console.ForegroundColor = color;

        var problemStatement = await context.OpenAsync(uri);
        var input = await context.GetService<IDocumentLoader>().FetchAsync(new DocumentRequest(new Url(baseUri + $"{year}/day/{day}/input"))).Task;

        if (input.StatusCode != HttpStatusCode.OK)
            throw new AocCommuncationException("Could not fetch input", input.StatusCode, new StreamReader(input.Content).ReadToEnd());

        return Problem.Parse(year, day, baseUri + $"{year}/day/{day}", problemStatement, new StreamReader(input.Content).ReadToEnd());
    }

    private static void UpdateReadmeForDay(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "README.md");
        WriteFile(file, problem.ContentMd);
    }

    private static void UpdateSolutionTemplate(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "Solution.cs");
        if (!File.Exists(file))
            WriteFile(file, SolutionTemplateGenerator.Generate(problem));
    }

    private static void UpdateProjectReadme(int year)
    {
        var file = Path.Combine("README.md");
        WriteFile(file, ProjectReadmeGenerator.Generate(year));
    }

    private static void UpdateReadmeForYear(Calendar calendar)
    {
        var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "README.md");
        WriteFile(file, ReadmeGeneratorForYear.Generate(calendar));

        var svg = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "calendar.svg");
        WriteFile(svg, calendar.ToSvg());
    }

    private static void UpdateSplashScreen(Calendar calendar)
    {
        var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "SplashScreen.cs");
        WriteFile(file, SplashScreenGenerator.Generate(calendar));
    }

    private static void UpdateInput(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "input.in");
        WriteFile(file, problem.Input);
    }

    private static void UpdateRefout(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "input.refout");
        if (problem.Answers.Length != 0)
            WriteFile(file, string.Join("\n", problem.Answers));
    }

    private static void UpdateTest(Problem problem)
    {
        if (problem.Answers.Length != 0)
            return;

        var test = Path.Combine(Dir(problem.Year, problem.Day), "test");
        Directory.CreateDirectory(test);
        var inFile = Path.Combine(test, "test1.in");
        if (!File.Exists(inFile))
            WriteFile(inFile, "");

        var outFile = Path.Combine(test, "test1.refout");
        if (!File.Exists(outFile))
            WriteFile(outFile, "");
    }

    [GeneratedRegex(@"\[Continue to Part Two.*", RegexOptions.Singleline)]
    private static partial Regex ContinueToPart2Regex();
    [GeneratedRegex(@"You have completed Day.*", RegexOptions.Singleline)]
    private static partial Regex YouHaveCompletedDayRegex();
    [GeneratedRegex(@"\(You guessed.*", RegexOptions.Singleline)]
    private static partial Regex YouGuessedRegex();
    [GeneratedRegex(@"  ", RegexOptions.Singleline)]
    private static partial Regex SpacesRegex();
}
