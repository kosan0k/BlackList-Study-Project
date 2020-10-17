using BlackList.Domain.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BlackList.Storage.Sql
{
    public class SqlStorage : IStorage
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        private RepositoryContext _repositoryContext;

        public SqlStorage(string connectionString)
        {
            CreateContext(connectionString);
        }

        private void CreateContext(string connectionString)
        {
            TryValidateConnectionString(connectionString, out bool conStringIsValid);

            _repositoryContext = conStringIsValid
                ? new RepositoryContext(connectionString)
                : new RepositoryContext();
        }

        private void TryValidateConnectionString(string connectionString, out bool isValid)
        {
            try
            {
                DbConnectionStringBuilder csb = new DbConnectionStringBuilder
                {
                    ConnectionString = connectionString
                };

                isValid = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                isValid = false;
            }
        }

        #region IStorage implementation   
        public async Task<bool> TrySavePersonAsync(Person person)
        {
            bool result;
            try
            {
                _repositoryContext.Persons.Add(new Entities.PersonEntity(person));
                await _repositoryContext.SaveChangesAsync().ConfigureAwait(false);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result = false;
            }

            return result;
        }

        public async Task<bool> TryDeletePersonAsync(Person person)
        {
            var result = false;

            var entityToDelete = await GetEntityAsync(person);
            if (entityToDelete != null)
            {
                try
                {
                    _repositoryContext.Persons.Remove(entityToDelete);
                    await _repositoryContext.SaveChangesAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    result = false;
                }
            }

            return result;
        }

        public async Task<bool> TryUpdatePersonAsync(Person unchangedPerson, Person modifiedPerson)
        {
            var result = false;

            var entityToUpdate = await GetEntityAsync(unchangedPerson);
            if (entityToUpdate != null)
            {
                try
                {
                    entityToUpdate.DateOfBirth = modifiedPerson.DateOfBirth;
                    entityToUpdate.FullName = modifiedPerson.FullName;
                    entityToUpdate.Notes = modifiedPerson.Notes;
                    entityToUpdate.Position = modifiedPerson.Position;

                    await _repositoryContext.SaveChangesAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    result = false;
                }
            }

            return result;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            var personEntities = await _repositoryContext.Persons.ToListAsync();
            return personEntities.Select(e => new Person()
            {
                FullName = e.FullName,
                DateOfBirth = e.DateOfBirth,
                Notes = e.Notes,
                Position = e.Position
            });
        }

        public Task<IAsyncEnumerable<Person>> FindPersonAsync(string name)
        {
            throw new NotImplementedException();
        }
        #endregion

        private Task<Entities.PersonEntity> GetEntityAsync(Person person)
        {
            return _repositoryContext.Persons.FirstOrDefaultAsync(e => string.Equals(e.FullName.FirstName, person.FullName.FirstName)
                && string.Equals(e.FullName.SecondName, person.FullName.SecondName)
                && string.Equals(e.FullName.Surname, person.FullName.Surname)
                && e.DateOfBirth == e.DateOfBirth);
        }
    }
}
