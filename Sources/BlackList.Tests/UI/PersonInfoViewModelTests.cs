using BlackList.Domain.Models;
using BlackList.Storage;
using BlackList.Ui.Wpf.Host.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackList.Tests.UI
{
    [TestClass]
    public class PersonInfoViewModelTests
    {
        [TestMethod]
        public void ConfirmCommandExecution_AfterPassingNullPersonParameterInCtor_AddsPersonInStorage()
        {
            //Arrange
            var stubStorage = new Mock<IStorage>();
            stubStorage.Setup(stor => stor.TrySavePersonAsync(It.IsAny<Person>())).Returns(Task.FromResult(true));

            Person person = null;
            var personInfoViewModel = new PersonInfoViewModel(stubStorage.Object, ref person);

            //Act
            personInfoViewModel.ConfirmCommand.Execute(null);

            //Assert
            stubStorage.Verify(storage => storage.TrySavePersonAsync(It.IsAny<Person>()), Times.Once);
        }

        [TestMethod]
        public void ConfirmCommandExecution_AfterPassingNotNullPersonParameterInCtor_UpdatesPersonInStorage()
        {
            //Arrange
            var stubStorage = new Mock<IStorage>();
            stubStorage.Setup(stor => stor.TryUpdatePersonAsync(It.IsAny<Person>(), It.IsAny<Person>())).Returns(Task.FromResult(true));

            Person person = CreateTestPerson();
            var personInfoViewModel = new PersonInfoViewModel(stubStorage.Object, ref person);

            //Act
            personInfoViewModel.ConfirmCommand.Execute(null);

            //Assert
            stubStorage.Verify(storage => storage.TryUpdatePersonAsync(It.IsAny<Person>(), It.IsAny<Person>()), Times.Once);
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
