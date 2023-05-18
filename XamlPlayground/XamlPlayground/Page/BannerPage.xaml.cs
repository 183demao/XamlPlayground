namespace XamlPlayground.Page;

using System.Windows.Media.Animation;
using XamlPlayground.Control;
using XamlPlayground.Model;

/// <summary>
/// Interaction logic for Banner.xaml
/// </summary>
[Page("Banner")]
public partial class BannerPage : UserControl
{
    public BannerPage()
    {
        InitializeComponent();
    }
    private static readonly DoubleAnimation fadeOutAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    private static readonly DoubleAnimation fadeInAnimation = new(1, new Duration(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    private static readonly ThicknessAnimation addBannerAnimation = new(new(0, 0, 0, 8), new(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    private void RemoveBanner(UserControl banner)
    {
        var height = banner.ActualHeight;

        var margin = banner.Margin;
        margin.Top -= height;

        var moveAnimation = new ThicknessAnimation(margin, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.Stop);
        moveAnimation.Completed += (s, e) =>
        {
            bannerGroup.Children.Remove(banner);
        };
        moveAnimation.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut };
        banner.BeginAnimation(MarginProperty, moveAnimation);
        banner.BeginAnimation(OpacityProperty, fadeOutAnimation);
    }

    private void AddBanner(Banner bannerInfo)
    {
        var banner = new BannerItem()
        {
            Text = bannerInfo.Message,
            Level = bannerInfo.Level,
            Opacity = 0
        };
        bannerGroup.Children.Insert(0, banner);

        EventHandler? startAnimation = default;
        startAnimation = new((s, e) =>
        {
            banner.LayoutUpdated -= startAnimation;

            banner.Margin = new(0, -banner.ActualHeight, 0, 8);
            banner.BeginAnimation(MarginProperty, addBannerAnimation);
            banner.BeginAnimation(OpacityProperty, fadeInAnimation);
        });

        banner.LayoutUpdated += startAnimation;
    }

    private void OnAddClick(object sender, RoutedEventArgs e)
    {
        var banner = new Banner()
        {
            Level = (MessageLevel)Random.Shared.Next(0, 3),
            Message = string.Join(string.Empty, Enumerable.Repeat("123", Random.Shared.Next(5, 30)))
        };
        AddBanner(banner);
    }

    private void OnRemoveClick(object sender, RoutedEventArgs e)
    {
        var banner = (UserControl)bannerGroup.Children[Random.Shared.Next(bannerGroup.Children.Count)];
        RemoveBanner(banner);
    }
}
