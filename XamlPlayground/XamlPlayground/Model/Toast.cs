namespace XamlPlayground.Model;

using XamlPlayground.Control;

public class Toast
{
    public Toast()
    {
        Message = string.Empty;
        Level = ToastLevel.Information;
    }

    public string Message { get; set; }
    public ToastLevel Level { get; set; }
}
