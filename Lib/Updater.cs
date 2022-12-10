using System.Net;
using System.Reflection;
using AdventOfCode.Generator;
using AdventOfCode.Model;
using AngleSharp;
using AngleSharp.Io;
using Git = LibGit2Sharp;

namespace AdventOfCode;

static class Updater
{

    public static async Task UpdateWithGit(int year, int day)
    {
        using var repo = new Git.Repository(".git");
        var main = repo.Branches["main"] ?? repo.Branches["master"];
        var branch = repo.Branches[$"problems/Y{year}/D{day}"] ?? repo.Branches.Add($"problems/Y{year}/D{day}", main.Tip, allowOverwrite: true);
        var today = Git.Commands.Checkout(repo, branch);

        await Update(year, day);

        Git.Commands.Stage(repo, year.ToString());
        var signature = new Git.Signature(repo.Config.Get<string>("user.name").Value, repo.Config.Get<string>("user.email").Value, DateTime.Now);
        var commit = repo.Commit($"Initial commit for Y{year}D{day}", signature, signature, new());
        repo.Tags.Add($"Y{year}D{day}P1", commit);
    }

    public static async Task Update(int year, int day)
    {
        var baseAddress = GetBaseAddress();
        var context = GetContext();

        var calendar = await DownloadCalendar(context, baseAddress, year);
        var problem = await DownloadProblem(context, baseAddress, year, day);

        var dir = Dir(year, day);
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }

        var years = Assembly.GetEntryAssembly().GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
            .Select(tsolver => SolverExtensions.Year(tsolver))
            .ToArray();

        UpdateProjectReadme(years.Length > 0 ? years.Min() : year, years.Length > 0 ? years.Max() : year);
        UpdateReadmeForYear(calendar);
        UpdateSplashScreen(calendar);
        UpdateReadmeForDay(problem);
        UpdateInput(problem);
        UpdateRefout(problem);
        UpdateSolutionTemplate(problem);
    }

    private static Uri GetBaseAddress()
    => new Uri("https://adventofcode.com");

    private static string GetSession()
    {
        if (!Environment.GetEnvironmentVariables().Contains("SESSION"))
            throw new AocCommuncationError("Specify SESSION environment variable", null);
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
        context.SetCookie(new Url(GetBaseAddress().ToString()), "session=" + GetSession());
        return context;
    }

    public static async Task Upload(Solver solver)
    {
        Globals.CurrentRunMode = Mode.Upload;
        var color = Console.ForegroundColor;
        Console.WriteLine();
        var solverResult = Runner.RunSolver(solver);
        Console.WriteLine();

        if (solverResult.errors.Any())
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
            article = Regex.Replace(article, @"\[Continue to Part Two.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"You have completed Day.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"\(You guessed.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"  ", "\n", RegexOptions.Singleline);

            using (var repo = new Git.Repository(".git"))
                if (article.StartsWith("That's the right answer") || article.Contains("You've finished every puzzle"))
                {
                    Git.Commands.Stage(repo, "*");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(article);
                    Console.ForegroundColor = color;
                    Console.WriteLine();
                    await Update(solver.Year(), solver.Day());

                    var signature = new Git.Signature(repo.Config.Get<string>("user.name").Value, repo.Config.Get<string>("user.email").Value, DateTime.Now);
                    if (problem.Answers.Length == 0)
                    {
                        var tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P1"];
                        var initial = (Git.Commit)tag.Target;
                        var duration = signature.When - initial.Committer.When;
                        Git.Commands.Stage(repo, "*");
                        var commit = repo.Commit($"Solved P1 in {duration:h\\:mm\\:ss}", signature, signature, new());
                        repo.Tags.Add($"Y{problem.Year}D{problem.Day}P2", commit);
                    }
                    else
                    {
                        var tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P2"];
                        var initial = (Git.Commit)tag.Target;
                        var duration = signature.When - initial.Committer.When;
                        Git.Commands.Stage(repo, "*");
                        repo.Commit($"Solved P2 in {duration:h\\:mm\\:ss}", signature, signature, new());
                        repo.Tags.Remove(tag);
                        Runner.RunBenchmark(solver.GetType());
                        Git.Commands.Stage(repo, "*");
                        repo.Commit("Added Benchmarks", signature, signature, new());
                        var branch = repo.Head;
                        var main = repo.Branches["main"] ?? repo.Branches["master"];
                        Git.Commands.Checkout(repo, main);
                        var merge = repo.Merge(branch, signature, new()
                            {
                                FastForwardStrategy = Git.FastForwardStrategy.NoFastForward,
                                CommitOnSuccess = false,
                            });
                        tag = repo.Tags[$"Y{problem.Year}D{problem.Day}P1"];
                        initial = (Git.Commit)tag.Target;
                        duration = signature.When - initial.Committer.When;
                        repo.Commit($"Solved Y{problem.Year}D{problem.Day} in {duration:h\\:mm\\:ss}", signature, signature, new()
                            {
                                AllowEmptyCommit = true
                            });
                        Git.Commands.Checkout(repo, branch);
                        repo.Tags.Remove(tag);
                    }
                }
                else if (article.StartsWith("That's not the right answer"))
                {
                    Git.Commands.Stage(repo, "*");

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
            throw new AocCommuncationError("Could not fetch calendar", document.StatusCode, document.TextContent);
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
            throw new AocCommuncationError("Could not fetch input", input.StatusCode, new StreamReader(input.Content).ReadToEnd());

        return Problem.Parse(year, day, baseUri + $"{year}/day/{day}", problemStatement, new StreamReader(input.Content).ReadToEnd()
        );
    }

    private static void UpdateReadmeForDay(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "README.md");
        WriteFile(file, problem.ContentMd);
    }

    private static void UpdateSolutionTemplate(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "Solution.cs");
        if (!File.Exists(file)) {
            WriteFile(file, SolutionTemplateGenerator.Generate(problem));
        }
    }

    private static void UpdateProjectReadme(int firstYear, int lastYear)
    {
        var file = Path.Combine("README.md");
        WriteFile(file, ProjectReadmeGenerator.Generate(firstYear, lastYear));
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

        var test = Path.Combine(Dir(problem.Year, problem.Day), "test");
        Directory.CreateDirectory(test);
        test = Path.Combine(test, "test1.in");
        if (File.Exists(test))
            return;
        WriteFile(test, "");
    }

    private static void UpdateRefout(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "input.refout");
        if (problem.Answers.Any())
            WriteFile(file, string.Join("\n", problem.Answers));

        var test = Path.Combine(Dir(problem.Year, problem.Day), "test");
        test = Path.Combine(test, "test1.refout");
        if (File.Exists(test))
            return;
        WriteFile(test, "");
    }
}
