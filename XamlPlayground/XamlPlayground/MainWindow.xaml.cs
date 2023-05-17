namespace XamlPlayground;

using System.Reflection;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var navButtons = from type in Assembly.GetExecutingAssembly().DefinedTypes
                    let attr = type.GetCustomAttribute<PageAttribute>()
                    where attr != null
                    let button = new Button[]
                    {
                        new()
                        {
                            Content = attr.Name,
                            Margin = new Thickness(0,0,8,0)
                        }
                    }.Select(x =>
                    {
                        x.Click += (s, e) =>
                        {
                            Dispatcher.Invoke(() => content.Content = Activator.CreateInstance(type));
                        };
                        return x;
                    }).First()
                    select button;

        foreach (var navButton in navButtons)
        {
            pages.Children.Add(navButton);
        }
    }
}