using BlackList.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackList.Storage
{
    public interface IStorage
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync();

        Task<IAsyncEnumerable<Person>> FindPersonAsync(string name);

        Task<bool> TrySavePersonAsync(Person person);

        Task<bool> TryDeletePersonAsync(Person person);

        Task<bool> TryUpdatePersonAsync(Person unchangedPerson, Person modifiedPerson);
    }
}
