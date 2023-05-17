namespace XamlPlayground.Control;

/// <summary>
/// Interaction logic for BannerItem.xaml
/// </summary>
public partial class BannerItem : UserControl
{
    public BannerItem()
    {
        InitializeComponent();
        this.SizeChanged += BannerItem_SizeChanged;
    }

    private void BannerItem_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var textWidth = MeasureTextWidth(Text);
        if (textWidth > ActualWidth - 270)
        {
            panelSingleBtn.Visibility = Visibility.Collapsed;
            panelDoubleBtn.Visibility = Visibility.Visible;
        }
        else
        {
            panelSingleBtn.Visibility = Visibility.Visible;
            panelDoubleBtn.Visibility = Visibility.Collapsed;
        }
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(BannerItem), new PropertyMetadata("Message", (s, e) =>
        {
            if (s is not BannerItem c) return;
            var text = (string)e.NewValue;
            var textWidth = c.MeasureTextWidth(text);
            if (textWidth > c.ActualWidth - 270)
            {
                c.panelSingleBtn.Visibility = Visibility.Collapsed;
                c.panelDoubleBtn.Visibility = Visibility.Visible;
            }
            else
            {
                c.panelSingleBtn.Visibility = Visibility.Visible;
                c.panelDoubleBtn.Visibility = Visibility.Collapsed;
            }
            c.text.Text = text;
        }));


    public MessageLevel Level
    {
        get { return (MessageLevel)GetValue(LevelProperty); }
        set { SetValue(LevelProperty, value); }
    }

    public static readonly DependencyProperty LevelProperty =
        DependencyProperty.Register("Level", typeof(MessageLevel), typeof(BannerItem), new PropertyMetadata(MessageLevel.Information, (s, e) =>
        {
            if (s is not BannerItem c) return;
            var level = (MessageLevel)e.NewValue;
            var (color, icon) = level switch
            {
                MessageLevel.Error => ((Brush)c.Resources["ErrorColor"], (Path)c.Resources["ErrorPath"]),
                MessageLevel.Warning => ((Brush)c.Resources["WarningColor"], (Path)c.Resources["WarningPath"]),
                MessageLevel.Information => ((Brush)c.Resources["InformationColor"], (Path)c.Resources["InformationPath"]),
                _ => ((Brush)c.Resources["InformationColor"], (Path)c.Resources["InformationPath"])
            };
            c.icon.Background = color;
            c.icon.Child = icon;
            c.background.Background = color;
        }));

    private double MeasureTextWidth(string text)
    {
        var formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface(this.text.FontFamily, this.text.FontStyle, this.text.FontWeight, this.text.FontStretch),
            this.text.FontSize,
            Brushes.Black,
            VisualTreeHelper.GetDpi(this).PixelsPerDip
        );
        return formattedText.WidthIncludingTrailingWhitespace;
    }
}

public enum MessageLevel
{
    Error,
    Warning,
    Information
}
