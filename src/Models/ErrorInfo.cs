namespace Models;

public class ErrorInfo
{
    public string Type { get; set; }
    public string Message { get; set; }
    public ErrorInfo(string message,
                     string type = "Error") =>
    (Type, Message) = (type, message);

    public ErrorInfo(Exception ex) =>
    (Type, Message) = (ex.GetType().ToString(), ex.Message);

    public override string ToString() => $@"{Type}: {Message}";
}
public class ErrorInfo<T> : ErrorInfo where T : class
{
    public List<T>? AdditionalInfo { get; set; }
    public ErrorInfo(string message,
                     string type = "Error",
                     List<T>? additionalInfo = default) : base(message, type) =>
    AdditionalInfo = additionalInfo;
}