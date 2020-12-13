
using System;
using System.Linq;
using System.Collections.Generic;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace AdventOfCode.Model {
    public class Problem {
        public string Title { get; private init; }
        public string ContentMd { get; private init; }
        public int Day { get; private init; }
        public int Year { get; private init; }
        public string Input { get; private init; }
        public string Answers { get; private init; }

        public static Problem Parse(int year, int day, string url, IDocument document, string input) {

            var md = $"original source: [{url}]({url})\n";
            var answers = "";
            foreach (var article in document.QuerySelectorAll("article")) {
                md += UnparseList("", article) + "\n";

                var answerNode = article.NextSibling; 
                while (answerNode != null && !( 
                    answerNode.NodeName == "P"
                    && ((IElement)answerNode).QuerySelector("code") != null 
                    && answerNode.TextContent.Contains("answer"))
                ) {
                    answerNode = answerNode.NextSibling as IElement;
                }

                var code = (answerNode as IElement)?.QuerySelector("code");
                if (code != null) {
                    answers += code.TextContent + "\n";
                }
            }
            var title = document.QuerySelector("h2").TextContent;

            var match = Regex.Match(title, ".*: (.*) ---");
            if (match.Success) {
                title = match.Groups[1].Value;
            }
            return new Problem {Year = year, Day = day, Title = title, ContentMd = md, Input = input, Answers = answers };
        }

        static string UnparseList(string sep, INode element) {
            return string.Join(sep, element.ChildNodes.SelectMany(Unparse));
        }

        static IEnumerable<string> Unparse(INode node) {
            switch (node.NodeName.ToLower()) {
                case "h2":
                    yield return "## " + UnparseList("", node) + "\n";
                    break;
                case "p":
                    yield return UnparseList("", node) + "\n";
                    break;
                case "em":
                    yield return "*" + UnparseList("", node) + "*";
                    break;
                case "code":
                    if (node.ParentElement.TagName == "PRE") {
                        yield return UnparseList("", node);
                    } else {
                        yield return "`" + UnparseList("", node) + "`";
                    }
                    break;
                case "span":
                    yield return UnparseList("", node);
                    break;
                case "s":
                    yield return "~~" + UnparseList("", node) + "~~";
                    break;
                case "ul":
                    foreach (var unparsed in node.ChildNodes.SelectMany(Unparse)) {
                        yield return unparsed;
                    }
                    break;
                case "li":
                    yield return " - " + UnparseList("", node);
                    break;
                case "pre":
                    yield return "```\n";
                    var freshLine = true;
                    foreach (var item in node.ChildNodes) {
                        foreach (var unparsed in Unparse(item)) {
                            freshLine = unparsed[unparsed.Length - 1] == '\n';
                            yield return unparsed;
                        }
                    }
                    if (freshLine) {
                        yield return "```\n";
                    } else {
                        yield return "\n```\n";
                    }
                    break;
                case "a":
                    yield return "[" + UnparseList("", node) + "](" + ((IElement)node).Attributes["href"].Value + ")";
                    break;
                case "br":
                    yield return "\n";
                    break;
                case "#text":
                    yield return node.TextContent;
                    break;
                default:
                    throw new NotImplementedException(node.NodeName);
            }
        }
    }
}