using BlackList.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
