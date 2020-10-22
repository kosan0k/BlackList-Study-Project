using AsyncAwaitBestPractices.MVVM;
using BlackList.Domain.Models;
using BlackList.Storage;
using BlackList.Ui.Wpf.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackList.Ui.Wpf.Host.ViewModels
{
    public class PersonInfoViewModel : BaseViewModel
    {
        private static IStorage _storage;

        private Person _unchangedPerson;

        private Person _person;
        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        private Action<Person> _confirmAction;

        public ICommand ConfirmCommand { get; set; }

        public PersonInfoViewModel(IStorage storage, ref Person person, Action<Person> confirmAction = null)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _confirmAction = confirmAction;

            if (person != null)
            {
                _unchangedPerson = person;

                Person = new Person()
                {
                    DateOfBirth = person.DateOfBirth,
                    FullName = new FullName()
                    {
                        FirstName = person.FullName.FirstName,
                        SecondName = person.FullName.SecondName,
                        Surname = person.FullName.Surname
                    },
                    Notes = person.Notes,
                    Position = person.Position
                };

                ConfirmCommand = new AsyncCommand(UpdatePersonAsync);
            }
            else 
            {
                Person = new Person() { FullName = new FullName() };

                ConfirmCommand = new AsyncCommand(SavePersonAsync);
            }
        }

        private async Task UpdatePersonAsync() 
        {
            await _storage.TryUpdatePersonAsync(_unchangedPerson, _person);
            _confirmAction?.Invoke(_person);
        }

        private async Task SavePersonAsync()
        {
            var isSucceed = await _storage.TrySavePersonAsync(_person);
            if (isSucceed)
            {
                _confirmAction?.Invoke(_person);
            }
        }
    }
}
