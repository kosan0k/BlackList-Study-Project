using BlackList.Ui.Wpf.Host.ViewModels;
using System.Windows;

namespace BlackList.Ui.Wpf.Host
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainView = new MainWindow()
            {
                DataContext = new MainViewModel()
            };

            mainView.Show();
        }
    }
}
