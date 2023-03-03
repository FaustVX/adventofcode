namespace AdventOfCode;

public class AocCommuncationException : System.Exception
{
    private readonly string _title;
    private readonly System.Net.HttpStatusCode? _status;
    private readonly string _details;

    public AocCommuncationException(string title, System.Net.HttpStatusCode? status = null, string details = "")
    {
        _title = title;
        _status = status;
        _details = details;
    }

    public override string Message
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine(_title);
            if (_status != null)
                sb.Append($"[{_status}] ");
            sb.AppendLine(_details);
            return sb.ToString();
        }
    }

    public static AocCommuncationException WrongDate()
    => new("Event is not active. This option works in Dec 1-25 only)");
}
