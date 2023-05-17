namespace XamlPlayground.Model;

using XamlPlayground.Control;

internal class Banner
{
    public Banner()
    {
        Message = string.Empty;
        Level = MessageLevel.Information;
    }

    public string Message { get; set; }
    public MessageLevel Level { get; set; }
}
