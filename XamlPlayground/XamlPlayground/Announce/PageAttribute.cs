namespace XamlPlayground.Announce;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
sealed class PageAttribute : Attribute
{
    public PageAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
