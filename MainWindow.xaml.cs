using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace LiveChatOverlay;

public partial class MainWindow : Window
{
    private const int HotkeyId = 1;
    private const uint ModControl = 0x0002;
    private const uint ModShift = 0x0004;
    private const int WmHotkey = 0x0312;
    private static readonly string YoutubeChatCss = LoadYoutubeChatCss();
    private bool _hotkeyRegistered;
    private HwndSource? _source;

    public MainWindow()
    {
        InitializeComponent();
        UpdateUrlPlaceholder();
    }

    private static string LoadYoutubeChatCss()
    {
        using var stream = typeof(MainWindow).Assembly.GetManifestResourceStream("LiveChatOverlay.YoutubeChat.css")
            ?? throw new InvalidOperationException("YouTube chat CSS resource was not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        SetClickThrough(ClickThroughCheckBox.IsChecked == true);

        try
        {
            await ChatView.EnsureCoreWebView2Async();
            ChatView.CoreWebView2.NavigationCompleted += ChatView_NavigationCompleted;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"WebView2 could not start. Make sure the Microsoft Edge WebView2 Runtime is installed.\n\n{ex.Message}",
                "Unable to load live chat",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        _hotkeyRegistered = RegisterHotKey(
            new WindowInteropHelper(this).Handle,
            HotkeyId,
            ModControl | ModShift,
            (uint)KeyInterop.VirtualKeyFromKey(Key.O));

        if (!_hotkeyRegistered)
        {
            MessageBox.Show(
                "The Ctrl+Shift+O shortcut could not be registered. It may already be in use by another application.",
                "Shortcut unavailable",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private void Window_SourceInitialized(object? sender, EventArgs e)
    {
        _source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
        _source.AddHook(WindowProc);
    }

    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (_hotkeyRegistered)
        {
            UnregisterHotKey(new WindowInteropHelper(this).Handle, HotkeyId);
        }

        _source?.RemoveHook(WindowProc);
    }

    private void ClickThrough_Changed(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && IsLoaded)
        {
            SetClickThrough(checkBox.IsChecked == true);
        }
    }

    private void SetClickThrough(bool enabled)
    {
        var handle = new WindowInteropHelper(this).Handle;
        var style = GetWindowLongPtr(handle, GwlExStyle).ToInt64();
        style = enabled
            ? style | WsExTransparent | WsExLayered
            : style & ~WsExTransparent;

        SetWindowLongPtr(handle, GwlExStyle, new IntPtr(style));
    }

    private void NavigateButton_Click(object sender, RoutedEventArgs e)
    {
        NavigateToUrl();
    }

    private void UrlBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateUrlPlaceholder();
    }

    private void UpdateUrlPlaceholder()
    {
        if (UrlPlaceholder is not null && UrlBox is not null)
        {
            UrlPlaceholder.Visibility = string.IsNullOrEmpty(UrlBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }

    private void UrlBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            NavigateToUrl();
        }
    }

    private void NavigateToUrl()
    {
        if (string.IsNullOrWhiteSpace(UrlBox.Text))
        {
            MessageBox.Show(
                "Paste a live chat URL first.",
                "URL is empty",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        if (!Uri.TryCreate(UrlBox.Text.Trim(), UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            MessageBox.Show(
                "Enter a valid URL starting with http:// or https://.",
                "Invalid URL",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (ChatView.CoreWebView2 is not null)
        {
            InitialHint.Visibility = Visibility.Visible;
            ChatView.CoreWebView2.Navigate(uri.AbsoluteUri);
        }
    }

    private async void ChatView_NavigationCompleted(
        object? sender,
        Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
    {
        if (!e.IsSuccess)
        {
            InitialHint.Text = "Unable to load chat. Check the URL and your internet connection, then try again.";
            InitialHint.Visibility = Visibility.Visible;
            MessageBox.Show(
                "The live chat page could not be loaded. Check the URL and your internet connection, then try again.",
                "Unable to load live chat",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (ChatView.CoreWebView2 is null)
        {
            return;
        }

        var css = JsonSerializer.Serialize(YoutubeChatCss);
        var script = $$"""
            (() => {
              const styleId = "live-chat-overlay-youtube-style";
              let style = document.getElementById(styleId);
              if (!style) {
                style = document.createElement("style");
                style.id = styleId;
                const nonceSource = document.querySelector("[nonce]");
                if (nonceSource?.nonce) style.nonce = nonceSource.nonce;
                (document.head || document.documentElement).appendChild(style);
              }
              style.textContent = {{css}};
              return true;
            })();
            """;

        try
        {
            var result = await ChatView.CoreWebView2.ExecuteScriptAsync(script);
            if (result != "true")
            {
                throw new InvalidOperationException("YouTube chat CSS could not be applied.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not apply YouTube chat styling.\n\n{ex.Message}",
                "Chat styling failed",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        InitialHint.Visibility = Visibility.Collapsed;
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && !ClickThroughCheckBox.IsChecked.GetValueOrDefault())
        {
            DragMove();
            e.Handled = true;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private IntPtr WindowProc(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (message == WmHotkey && wParam.ToInt32() == HotkeyId)
        {
            ClickThroughCheckBox.IsChecked = ClickThroughCheckBox.IsChecked != true;
            handled = true;
        }

        return IntPtr.Zero;
    }

    private const int GwlExStyle = -20;
    private const long WsExTransparent = 0x00000020L;
    private const long WsExLayered = 0x00080000L;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint virtualKey);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int index);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int index, IntPtr value);
}
