using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace DisRPC.Pages;

public partial class MainPage
{
    private readonly HttpClient _httpClient = new();

    public MainPage()
    {
        InitializeComponent();
    }

    private void FirstTextField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (FirstTextField.Text != "")
            FirstTextPreview.Content = FirstTextField.Text;
        else
            FirstTextPreview.Content = "Первый текст";
    }

    private void SecondTextField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (SecondTextField.Text != "")
            SecondTextPreview.Content = SecondTextField.Text;
        else
            SecondTextPreview.Content = "Второй текст";
    }

    private async void ApplicationIdField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ApplicationIdField.Text))
        {
            ApplicationNamePreview.Content = "Имя приложения";
            return;
        }

        var appId = ApplicationIdField.Text;

        try
        {
            var appName = await GetDiscordAppName(appId);
            ApplicationNamePreview.Content = appName ?? "Имя приложения";
        }
        catch
        {
            ApplicationNamePreview.Content = "Ошибка загрузки имени приложения";
        }
    }

    private async Task<string> GetDiscordAppName(string appId)
    {
        var url = $"https://discordapp.com/api/oauth2/applications/{appId}/rpc";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);

        return json["name"]?.ToString();
    }

    private void BigImageKey_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BigImageKey.Text))
        {
            BigImagePreview.Source = null;
            return;
        }

        var imageUrl = BigImageKey.Text;
        BigImagePreview.Source = new BitmapImage(new Uri(imageUrl));
    }

    private void SmallImageKey_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SmallImageKey.Text))
        {
            SmallImagePreview.Source = null;
            return;
        }

        var imageUrl = SmallImageKey.Text;
        SmallImagePreview.Source = new BitmapImage(new Uri(imageUrl));
    }

    private void StartButton_OnClick(object sender, RoutedEventArgs e)
    {
        var appId = ApplicationIdField.Text;
        var firstText = FirstTextField.Text;
        var secondText = SecondTextField.Text;
        var bigImageKey = BigImageKey.Text;
        var smallImageKey = SmallImageKey.Text;
        var bigImageText = BigImageText.Text;
        var smallImageText = SmallImageText.Text;
        var showTime = DontShowTimeCheckBox.IsChecked ?? false;
        
        if (string.IsNullOrWhiteSpace(appId) || 
            string.IsNullOrWhiteSpace(firstText) || 
            string.IsNullOrWhiteSpace(secondText) || 
            string.IsNullOrWhiteSpace(bigImageKey))
        {
            MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        
        RpcManager.InitializeClient(appId);
        RpcManager.SetRpc(firstText, secondText, bigImageKey, bigImageText, smallImageKey, smallImageText, !showTime);
        StartButton.Visibility = Visibility.Collapsed;
        StopButton.Visibility = Visibility.Visible;
    }

    private void StopButton_OnClick(object sender, RoutedEventArgs e)
    {
        RpcManager.Dispose();

        StartButton.Visibility = Visibility.Visible;
        StopButton.Visibility = Visibility.Collapsed;
    }
}