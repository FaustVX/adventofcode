
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Generator;
using AdventOfCode.Model;
using System.Reflection;

using AngleSharp;
using AngleSharp.Io;

using System.Net.Http;
using System.Collections.Generic;
using System.Net;

namespace AdventOfCode {

    class Updater {

        public async Task Update(int year, int day) {

            if (!System.Environment.GetEnvironmentVariables().Contains("SESSION")) {
                throw new Exception("Specify SESSION environment variable");
            }
            var session = System.Environment.GetEnvironmentVariable("SESSION");
            var baseAddress = new Uri("https://adventofcode.com/");

            var context = BrowsingContext.New(Configuration.Default
                .WithDefaultLoader()
                .WithCss()
                .WithDefaultCookies()
            );
            context.SetCookie(new Url(baseAddress.ToString()), "session=" + session);

            var calendar = await DownloadCalendar(context, baseAddress, year);
            var problem = await DownloadProblem(context, baseAddress, year, day);

            var dir = Dir(year, day);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            var years = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                .Select(tsolver => SolverExtensions.Year(tsolver));

            UpdateProjectReadme(years.Min(), years.Max());
            UpdateReadmeForYear(calendar);
            UpdateSplashScreen(calendar);
            UpdateReadmeForDay(problem);
            UpdateInput(problem);
            UpdateRefout(problem);
            UpdateSolutionTemplate(problem);
        }

        public async Task Upload(int year, int day, int part, string answer) {

            if (!System.Environment.GetEnvironmentVariables().Contains("SESSION")) {
                throw new Exception("Specify SESSION environment variable");
            }
            var session = System.Environment.GetEnvironmentVariable("SESSION");

            // https://adventofcode.com/{year}/day/{day}/answer
            // level={part}&answer={answer}

            var baseAddress = new Uri("https://adventofcode.com");
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler) { BaseAddress = baseAddress };

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string?, string?>("level", part.ToString()),
                new KeyValuePair<string?, string?>("answer", answer),
            });
            cookieContainer.Add(baseAddress, new Cookie("session", session));
            var result = await client.PostAsync($"/{year}/day/{day}/answer", content);
            result.EnsureSuccessStatusCode();
            var responseString = await result.Content.ReadAsStringAsync();

            //Use the default configuration for AngleSharp
            var config = Configuration.Default;

            //Create a new context for evaluating webpages with the given config
            var context = BrowsingContext.New(config);

            //Just get the DOM representation
            var document = await context.OpenAsync(req => req.Content(responseString));
            var article = document.Body.QuerySelector("body > main > article").TextContent;

            var color = Console.ForegroundColor;
            System.Console.WriteLine();

            if(article.StartsWith("That's the right answer"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine(article);
                Console.ForegroundColor = color;
                System.Console.WriteLine();
            }
            else if (article.StartsWith("That's not the right answer"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(article);
                Console.ForegroundColor = color;
                System.Console.WriteLine();
                throw new Exception("That's not the right answer.");
            }
            else if(article.StartsWith("You gave an answer too recently"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(article);
                Console.ForegroundColor = color;
                System.Console.WriteLine();
                throw new Exception("You gave an answer too recently;");
            }
            else if(article.Contains("Did you already complete it"))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                System.Console.WriteLine(article);
                Console.ForegroundColor = color;
                System.Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                System.Console.WriteLine(article);
                Console.ForegroundColor = color;
            }

            await Update(year, day);
        }

        void WriteFile(string file, string content) {
            Console.WriteLine($"Writing {file}");
            File.WriteAllText(file, content);
        }

        string Dir(int year, int day) => SolverExtensions.WorkingDir(year, day);

        async Task<Calendar> DownloadCalendar(IBrowsingContext context, Uri baseUri, int year) {
            var document = await context.OpenAsync(baseUri.ToString() + year);
            return Calendar.Parse(year, document);
        }

        async Task<Problem> DownloadProblem(IBrowsingContext context, Uri baseUri, int year, int day) {
            var problemStatement = await context.OpenAsync(baseUri + $"{year}/day/{day}");
            var input = await context.GetService<IDocumentLoader>().FetchAsync(
                    new DocumentRequest(new Url(baseUri + $"{year}/day/{day}/input"))).Task; 
            return Problem.Parse(
                year, day, baseUri + $"{year}/day/{day}", problemStatement, 
                new StreamReader( input.Content).ReadToEnd() 
            );
        }

        void UpdateReadmeForDay(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "README.md");
            WriteFile(file, problem.ContentMd);
        }

        void UpdateSolutionTemplate(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "Solution.cs");
            if (!File.Exists(file)) {
                WriteFile(file, new SolutionTemplateGenerator().Generate(problem));
            }
        }

        void UpdateProjectReadme(int firstYear, int lastYear) {
            var file = Path.Combine("README.md");
            WriteFile(file, new ProjectReadmeGenerator().Generate(firstYear, lastYear));
        }

        void UpdateReadmeForYear(Calendar calendar) {
            var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "README.md");
            WriteFile(file, new ReadmeGeneratorForYear().Generate(calendar));
        }

        void UpdateSplashScreen(Calendar calendar) {
            var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "SplashScreen.cs");
            WriteFile(file, new SplashScreenGenerator().Generate(calendar));
        }

        void UpdateInput(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "input.in");
            WriteFile(file, problem.Input);

            var test = Path.Combine(Dir(problem.Year, problem.Day), "test");
            Directory.CreateDirectory(test);
            test = Path.Combine(test, "test1.in");
            if (File.Exists(test))
                return;
            WriteFile(test, "");
        }

        void UpdateRefout(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "input.refout");
            if (problem.Answers.Any()) {
                WriteFile(file, problem.Answers);
            }

            var test = Path.Combine(Dir(problem.Year, problem.Day), "test");
            test = Path.Combine(test, "test1.refout");
            if (File.Exists(test))
                return;
            WriteFile(test, "");
        }
    }
}