using System.Net;


namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public partial class AocCommuncationException([Field] string title, [Field] HttpStatusCode? status = null, [Field] string details = "") : Exception
{
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
