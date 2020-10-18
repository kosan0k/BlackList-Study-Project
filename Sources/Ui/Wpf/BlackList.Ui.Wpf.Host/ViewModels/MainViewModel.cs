using BlackList.Storage;
using BlackList.Storage.Sql;
using System.Configuration;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class MainViewModel
    {
        private static IStorage _storage;

        public MainViewModel() 
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["BlackListDatabase"].ConnectionString;
            _storage = new SqlStorage(dbConnectionString);
        }
    }
}
