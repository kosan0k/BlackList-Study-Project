using BlackList.Domain.Models;
using BlackList.Storage;
using BlackList.Storage.Sql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Input;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static IStorage _storage;

        private Person _selectedPerson;

        public Person SelectedPerson 
        {
            get => _selectedPerson;
            set 
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public ObservableCollection<Person> Persons { get; set; }

        public MainViewModel() 
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["BlackListDatabase"].ConnectionString;
            _storage = new SqlStorage(dbConnectionString);

            var persons = _storage.GetAllPersonsAsync().GetAwaiter().GetResult();
            Persons = new ObservableCollection<Person>(persons);
        }

        public ICommand AddPersonCommand { get; set; }
        public ICommand EditPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }        
    }
}
