namespace XamlPlayground.Control;

/// <summary>
/// ToastItem.xaml 的交互逻辑
/// </summary>
public partial class ToastItem : UserControl
{
    public ToastItem()
    {
        InitializeComponent();
        HorizontalAlignment = HorizontalAlignment.Center;
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(ToastItem), new PropertyMetadata("Message", (s, e) =>
        {
            if (s is not ToastItem c) return;
            var text = (string)e.NewValue;
            c.text.Text = text;
        }));

    public ToastLevel Level
    {
        get { return (ToastLevel)GetValue(LevelProperty); }
        set { SetValue(LevelProperty, value); }
    }

    public static readonly DependencyProperty LevelProperty =
        DependencyProperty.Register("Level", typeof(ToastLevel), typeof(ToastItem), new PropertyMetadata(ToastLevel.Information, (s, e) =>
        {
            if (s is not ToastItem c) return;
            var level = (ToastLevel)e.NewValue;
            var (bgColorKey, iconPath, iconColor) = level switch
            {
                ToastLevel.Error => ("ErrorColor", "ErrorPath", "ErrorIconColor"),
                ToastLevel.Information => ("InformationColor", "InformationPath", "InformationIconColor"),
                ToastLevel.Warning => ("WarningColor", "WarningPath", "WarningIconColor"),
                ToastLevel.Success => ("SuccessColor", "SuccessPath", "SuccessIconColor"),
                _ => ("InformationColor", "InformationPath", "InformationIconColor")
            };
            c.background.Background = (Brush)c.Resources[bgColorKey];
            c.icon.Background = (Brush)c.Resources[iconColor];
            c.icon.Child = (Path)c.Resources[iconPath];
        }));
}

public enum ToastLevel
{
    Error,
    Warning,
    Information,
    Success
}
