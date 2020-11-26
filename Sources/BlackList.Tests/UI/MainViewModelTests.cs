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
        private Person _testPerson = new Person()
        {
             FullName = new FullName() {  FirstName = "Name", SecondName = "SecName", Surname = "Surname"},
             DateOfBirth = DateTime.Now,
             Notes = "Test notes",
             Position = "Some position"
        };

        [TestMethod]
        public void AddPersonInStorage_OnSucess_AddPersonToCollectionWithoutGetPersonsStorageQuery()
        {
            //Arrange
            var stubStorage = new Mock<IStorage>();
            stubStorage.Setup(stor => stor.GetAllPersonsAsync()).Returns(Task.FromResult(new List<Person>() as IEnumerable<Person>));
            stubStorage.Setup(stor => stor.TrySavePersonAsync(It.IsAny<Person>())).Returns(Task.FromResult(true));

            var mainViewModel = new Mock<MainViewModel>(stubStorage.Object).Object;

            //Act
            mainViewModel.AddPersonCommand.Execute(null); //with null parameter person will be added in storage otherwise updated
            mainViewModel.PersonInfoViewModel.Person = _testPerson; //simulating filling person data
            mainViewModel.PersonInfoViewModel.ConfirmCommand.Execute(null); //

            //Assert            
            stubStorage.Verify(stor => stor.TrySavePersonAsync(It.IsAny<Person>()), Times.Once); // person was added to storage
            Assert.IsTrue(mainViewModel.Persons.Contains(_testPerson)); //person was added in viewModel collection
            stubStorage.Verify(stor => stor.GetAllPersonsAsync(), Times.Exactly(1)); //there were no request to storage after calling in mainViewModel        
        }
    }
}
