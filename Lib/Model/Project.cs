namespace AdventOfCode.Model;
using PrimaryParameter.SG;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public partial class Project([Field] string repo, [Field] string sslSalt, [Field] string sslPassword)
{
    public required string UserName { get; init; }

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
        CopyStream(Extensions.GetEmbededResource("adventofcode.adventofcode.csproj"), File.Create("adventofcode.csproj"));
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
        CopyStream(Extensions.GetEmbededResource("adventofcode..vscode.AdventOfCode.sln"), new FileInfo(Path.Combine(vscode.FullName, "AdventOfCode.sln")).Create());
        File.AppendAllText(".git/config", $"""
        [filter "crypt"]
            clean = wsl openssl enc -aes-256-cbc -e -iter 10 -salt -S {_sslSalt} -pass pass:{_sslPassword}
            smudge = wsl openssl enc -aes-256-cbc -d -iter 10 -salt -S {_sslSalt} -pass pass:{_sslPassword}
            required

        """);
        File.WriteAllText("LICENSE", $"""
        Copyright (c) {DateTime.Now.Year} {UserName}.

        The license applies to all source code and configuration incluced in
        the repository. It doesn't apply to advent of code problem statements 
        and input files. For the license conditions of those please contact the 
        original author at https://adventofcode.com.

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.

        """);
        Process.Start("git", $"-c protocol.file.allow=always submodule add {_repo} lib/aoc").WaitForExit();
        Process.Start("git", ["add", "*"]).WaitForExit();
        Process.Start("git", ["commit", "-m", "Initial commit"]).WaitForExit();
    }
}
