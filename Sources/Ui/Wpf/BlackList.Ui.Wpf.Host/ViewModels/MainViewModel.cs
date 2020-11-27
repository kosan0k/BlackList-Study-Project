using AsyncAwaitBestPractices.MVVM;
using BlackList.Domain.Models;
using BlackList.Storage;
using BlackList.Storage.Sql;
using BlackList.Ui.Wpf.Common;
using BlackList.Ui.Wpf.Host.Views;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
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

        internal PersonInfoViewModel PersonInfoViewModel { get; set; } //for test purposes only
        internal ConfirmationViewModel ConfirmationViewModel { get; set; } //for test purposes only

        public MainViewModel(IStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));

            try
            {
                var persons = _storage.GetAllPersonsAsync().Result;
                Persons = new ObservableCollection<Person>(persons);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Cannot download persons from repository");
            }            
        }

        public ICommand AddPersonCommand => new ActionCommand(ShowPersonInfoWindow);
        public ICommand EditPersonCommand => new ActionCommand(ShowPersonInfoWindow, CanExecute);
        public ICommand DeletePersonCommand => new ActionCommand(ShowDeleteConfirmationView, CanExecute);

        private void ShowPersonInfoWindow(object parameter)
        {
            var personParameter = parameter as Person;

            Action<Person> confirmAction = null;
            if (personParameter != null) // updating user
            {
                confirmAction = (person) =>
                {
                    Persons.Remove(SelectedPerson);
                    Persons.Add(person);
                    OnPropertyChanged(nameof(Persons));
                };
            }
            else // adding user
            {
                confirmAction = (person) => Persons.Add(person);                
            }

            PersonInfoViewModel = new PersonInfoViewModel(_storage, ref personParameter, confirmAction);
            var userInfoView = new PersonInfoView()
            {                
                DataContext = PersonInfoViewModel
            };

            ShowDialogWindow(userInfoView);
        }

        internal virtual void ShowDialogWindow(Window window) //made virtual to avoid showing window in tests
        {
            window.Show();
        }

        private bool CanExecute(object parameter)
        {
            return SelectedPerson != null;
        }

        private void ShowDeleteConfirmationView(object parameter) 
        {
            ConfirmationViewModel = ConfirmationViewModel ?? new ConfirmationViewModel();
            ConfirmationViewModel.ConfirmCommand = new AsyncCommand(DeleteSelectedPersonAsync);

            var confirmationView = new DeletionConfirmationView()
            {
                DataContext = ConfirmationViewModel
            };
            ShowDialogWindow(confirmationView);
        }

        private async Task DeleteSelectedPersonAsync() 
        {
            var deletionSucceed = await _storage.TryDeletePersonAsync(_selectedPerson);
            if (deletionSucceed)
            {
                Persons.Remove(_selectedPerson);
            }
        }
    }
}
