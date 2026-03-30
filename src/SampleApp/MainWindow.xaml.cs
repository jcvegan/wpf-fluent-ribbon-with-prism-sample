using ControlzEx.Theming;
using Fluent;
using MahApps.Metro.Controls;
using System.Windows;

namespace SampleApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow, IRibbonWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TitleBarProperty =
            DependencyProperty.Register("TitleBar", typeof(RibbonTitleBar), typeof(MainWindow), new PropertyMetadata(null));

    public RibbonTitleBar? TitleBar
    {
        get { return (RibbonTitleBar?)GetValue(TitleBarProperty); }
        set { SetValue(TitleBarProperty, value); }
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
        this.TitleBar = this.FindChild<RibbonTitleBar>("RibbonTitleBar");

        if (this.TitleBar is null)
        {
            throw new Exception("Ribbon titlebar could not be found.");
        }

        this.MainRibbon.TitleBar = this.TitleBar;

        this.TitleBar.InvalidateArrange();
        this.TitleBar.UpdateLayout();

        ThemeManager.Current.ChangeTheme(this, ThemeManager.Current.DetectTheme(Application.Current)!);

        this.MainRibbon.TitleBar = this.TitleBar;

        ThemeManager.Current.ThemeChanged += this.SyncThemes;
    }

    private void SyncThemes(object? sender, ThemeChangedEventArgs e)
    {
        if (e.Target == this)
        {
            return;
        }

        ThemeManager.Current.ChangeTheme(this, e.NewTheme);
        this.MainRibbon.TitleBar = this.TitleBar;
    }

    private void MetroWindow_Closed(object sender, EventArgs e)
    {
        ThemeManager.Current.ThemeChanged -= this.SyncThemes;
    }
}