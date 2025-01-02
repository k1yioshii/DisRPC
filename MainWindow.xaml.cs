using System.Windows;
using System.Windows.Input;
using DisRPC.Pages;

namespace DisRPC;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        var mainPage = new MainPage();
        MainFrame.Content = mainPage;
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
        RpcManager.Dispose();
    }

    private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void DraggableBar_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
}