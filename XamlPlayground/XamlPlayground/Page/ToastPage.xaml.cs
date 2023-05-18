namespace XamlPlayground.Page;

using System.Windows.Media.Animation;
using XamlPlayground.Control;
using XamlPlayground.Model;

/// <summary>
/// ToastPage.xaml 的交互逻辑
/// </summary>
[Page("Toast")]
public partial class ToastPage : UserControl
{
    private static readonly DoubleAnimation fadeOutAnimation = new(0, new Duration(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    private static readonly DoubleAnimation fadeInAnimation = new(1, new Duration(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    private static readonly ThicknessAnimation addToastAnimation = new(new(0, 8, 0, 8), new(TimeSpan.FromMilliseconds(300)))
    {
        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut },
    };

    public ToastPage()
    {
        InitializeComponent();
    }

    private void AddToast(Toast toastInfo)
    {
        var toast = new ToastItem
        {
            Text = toastInfo.Message,
            Level = toastInfo.Level,
            Opacity = 0
        };
        toastGroup.Children.Insert(0, toast);

        EventHandler? startAnimation = default;
        startAnimation = new((s, e) =>
        {
            toast.LayoutUpdated -= startAnimation;

            toast.Margin = new(0, -toast.ActualHeight, 0, 8);
            toast.BeginAnimation(MarginProperty, addToastAnimation);
            toast.BeginAnimation(OpacityProperty, fadeInAnimation);
            if (toastGroup.Children.Count > 4)
            {
                var toRemove = toastGroup.Children.OfType<ToastItem>().Skip(4);
                foreach (var item in toRemove)
                {
                    RemoveToast(item);
                }
            }
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                Dispatcher.Invoke(() =>
                {
                    if (!toastGroup.Children.Contains(toast)) return;
                    RemoveToast(toast);
                });
            });
        });

        toast.LayoutUpdated += startAnimation;
    }

    private void RemoveToast(ToastItem toast)
    {
        var height = toast.ActualHeight;

        var margin = toast.Margin;
        margin.Top -= height;

        var moveAnimation = new ThicknessAnimation(margin, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.Stop);
        moveAnimation.Completed += (s, e) =>
        {
            toastGroup.Children.Remove(toast);
        };
        moveAnimation.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut };
        toast.BeginAnimation(MarginProperty, moveAnimation);
        toast.BeginAnimation(OpacityProperty, fadeOutAnimation);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var toast = new Toast()
        {
            Level = (ToastLevel)Random.Shared.Next(0, 4),
            Message = string.Join(string.Empty, Enumerable.Repeat("Test", Random.Shared.Next(1, 6)))
        };
        AddToast(toast);
    }
}
