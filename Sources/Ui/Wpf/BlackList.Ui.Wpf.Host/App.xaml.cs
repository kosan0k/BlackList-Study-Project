using BlackList.Storage.Sql;
using BlackList.Ui.Wpf.Host.ViewModels;
using System.Configuration;
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
            var dbConnectionString = ConfigurationManager.ConnectionStrings["BlackListDatabase"].ConnectionString;

            var mainView = new MainWindow()
            {
                DataContext = new MainViewModel(new SqlStorage(dbConnectionString))
            };

            mainView.Show();
        }
    }
}
