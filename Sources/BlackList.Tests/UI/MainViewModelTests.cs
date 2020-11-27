using BlackList.Domain.Models;
using BlackList.Storage;
using BlackList.Ui.Wpf.Host.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BlackList.Tests.UI
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void AddPersonToStorage_OnSuсcess_AddPersonToCollectionWithoutGetPersonsStorageQuery()
        {
            //Arrange
            var stubStorage = new Mock<IStorage>();
            stubStorage.Setup(stor => stor.GetAllPersonsAsync()).Returns(Task.FromResult(new List<Person>() as IEnumerable<Person>));
            stubStorage.Setup(stor => stor.TrySavePersonAsync(It.IsAny<Person>())).Returns(Task.FromResult(true));

            var testPerson = CreateTestPerson();
            var mainViewModel = new Mock<MainViewModel>(stubStorage.Object).Object;

            //Act
            mainViewModel.AddPersonCommand.Execute(null); //with null parameter person will be added in storage otherwise updated
            mainViewModel.PersonInfoViewModel.Person = testPerson; //simulating filling person data
            mainViewModel.PersonInfoViewModel.ConfirmCommand.Execute(null); //simulating users confirmation

            //Assert            
            stubStorage.Verify(stor => stor.TrySavePersonAsync(It.IsAny<Person>()), Times.Once); // person was added to storage
            Assert.IsTrue(mainViewModel.Persons.Contains(testPerson)); //person was added in viewModel collection
            stubStorage.Verify(stor => stor.GetAllPersonsAsync(), Times.Exactly(1)); //there were no request to storage after calling in mainViewModel ctor      
        }

        [TestMethod]
        public void AddPersonToStorage_OnFailure_DoesNotAddPersonToCollection()
        {
            //Arrange
            var stubStorage = new Mock<IStorage>();
            stubStorage.Setup(stor => stor.GetAllPersonsAsync()).Returns(Task.FromResult(new List<Person>() as IEnumerable<Person>));
            stubStorage.Setup(stor => stor.TrySavePersonAsync(It.IsAny<Person>())).Returns(Task.FromResult(false)); // simulating adding person failure

            var testPerson = CreateTestPerson();
            var mainViewModel = new Mock<MainViewModel>(stubStorage.Object).Object;

            //Act
            mainViewModel.AddPersonCommand.Execute(null); //with null parameter person will be added in storage otherwise updated
            mainViewModel.PersonInfoViewModel.Person = testPerson; //simulating filling person data
            mainViewModel.PersonInfoViewModel.ConfirmCommand.Execute(null); //simulating users confirmation

            //Assert                        
            Assert.IsFalse(mainViewModel.Persons.Contains(testPerson)); //person was not added in viewModel collection                
        }

        [TestMethod]
        public void DeletePersonFromStorage_OnSuccess_RemovesPersonFromCollectionWithoutGetPersonsStorageQuery() 
        {
            //Arrange
            var testPerson = CreateTestPerson();
            var stubStorage = new Mock<IStorage>();
            var testPersons = new List<Person>() { testPerson } as IEnumerable<Person>;
            stubStorage.Setup(stor => stor.GetAllPersonsAsync()).Returns(Task.FromResult(testPersons));
            stubStorage.Setup(stor => stor.TryDeletePersonAsync(It.IsAny<Person>())).Returns(Task.FromResult(true));

            var mainViewModel = new Mock<MainViewModel>(stubStorage.Object).Object;

            //Act
            mainViewModel.SelectedPerson = testPerson; //simulating selecting person by user
            mainViewModel.DeletePersonCommand.Execute(null);   
            mainViewModel.ConfirmationViewModel.ConfirmCommand.Execute(null); //simulating user confirmation

            //Assert            
            stubStorage.Verify(stor => stor.TryDeletePersonAsync(It.IsAny<Person>()), Times.Once); // person was removed from storage
            Assert.IsFalse(mainViewModel.Persons.Contains(testPerson)); //person was removed from viewModel collection
            stubStorage.Verify(stor => stor.GetAllPersonsAsync(), Times.Exactly(1)); //there were no request to storage after calling in mainViewModel ctor
        }

        private Person CreateTestPerson() 
        {
            return new Person()
            {
                FullName = new FullName() { FirstName = "Name", SecondName = "SecName", Surname = "Surname" },
                DateOfBirth = DateTime.Now,
                Notes = "Test notes",
                Position = "Some position"
            };
        }
    }
}
