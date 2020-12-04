using BlackList.Storage;
using BlackList.Storage.Sql;
using BlackList.Ui.Wpf.Host.ViewModels;
using Ninject;
using System.Configuration;
using System.Windows;

namespace BlackList.Ui.Wpf.Host
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;
        private string _dbConnectionString;

        protected override void OnStartup(StartupEventArgs e)
        {
            _dbConnectionString = ConfigurationManager.ConnectionStrings["BlackListDatabase"].ConnectionString;

            ConfigureContainer();

            var mainView = new MainWindow()
            {
                DataContext = new MainViewModel(_container.Get<IStorage>())
            };

            mainView.Show();
        }

        private void ConfigureContainer()
        {
            this._container = new StandardKernel();
            _container.Bind<IStorage>().To<SqlStorage>().InSingletonScope().WithConstructorArgument(_dbConnectionString);
        }
    }
}
