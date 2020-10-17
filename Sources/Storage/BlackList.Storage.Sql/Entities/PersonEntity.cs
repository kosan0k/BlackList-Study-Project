using BlackList.Domain.Models;

namespace BlackList.Storage.Sql.Entities
{
    public class PersonEntity : Person
    {
        PersonEntity() { }

        public PersonEntity(Person person)
        {
            DateOfBirth = person.DateOfBirth;
            Notes = person.Notes;
            Position = person.Position;
            FullName = person.FullName;
        }

        public int Id { get; set; }
    }
}
