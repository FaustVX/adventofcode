namespace AdventOfCode.Model;

public class Project
{
    private readonly FileInfo _project;
    private readonly string _sslSalt;
    private readonly string _sslPassword;

    public Project(string m, string sslSalt, string sslPassword)
    {
        _project = new(m);
        _sslSalt = sslSalt;
        _sslPassword = sslPassword;
    }

    private static void CopyStream(Stream from, Stream to)
    {
        try
        {
            from?.CopyTo(to);
        }
        finally
        {
            from?.Dispose();
            to?.Dispose();
        }
    }

    public void Init()
    {
        CopyStream(Extensions.GetEmbededResource("adventofcode.adventofcode.csproj"), File.Create("AoC.csproj"));
        LibGit2Sharp.Repository.Init(".");
        new DirectoryInfo("./").EnumerateDirectories("_git*").FirstOrDefault()?.Delete();
        var git = new LibGit2Sharp.Repository(".git");
        CopyStream(Extensions.GetEmbededResource("adventofcode..gitattributes"), File.Create(".gitattributes"));
        CopyStream(Extensions.GetEmbededResource("adventofcode..gitignore"), File.Create(".gitignore"));
        var vscode = Directory.CreateDirectory(".vscode");
        CopyStream(Extensions.GetEmbededResource("adventofcode..vscode.tasks.json"), new FileInfo(Path.Combine(vscode.FullName, "tasks.json")).Create());
        CopyStream(Extensions.GetEmbededResource("adventofcode..vscode.extensions.json"), new FileInfo(Path.Combine(vscode.FullName, "extensions.json")).Create());
        CopyStream(Extensions.GetEmbededResource("adventofcode..vscode.launch.json"), new FileInfo(Path.Combine(vscode.FullName, "launch.json")).Create());
        CopyStream(Extensions.GetEmbededResource("adventofcode..vscode.settings.json"), new FileInfo(Path.Combine(vscode.FullName, "settings.json")).Create());
        File.AppendAllText(".git/config", $$"""
        [filter "crypt"]
            clean = wsl openssl enc -aes-256-cbc -e -iter 10 -salt -S {{_sslSalt}} -a -pass pass:{{_sslPassword}}
            smudge = wsl openssl enc -aes-256-cbc -d -iter 10 -salt -S {{_sslSalt}} -a -pass pass:{{_sslPassword}}
            required

        """);
        var path = new Uri(new DirectoryInfo("./").FullName).MakeRelativeUri(new(_project.Directory.EnumerateDirectories(".git").First().FullName));
        Process.Start("git", $@"-c protocol.file.allow=always submodule add {path} lib/aoc").WaitForExit();
        LibGit2Sharp.Commands.Stage(git, "*");
        var signature = git.Config.BuildSignature(DateTimeOffset.Now);
        git.Commit("Initial commit", signature, signature, new());
    }
}
