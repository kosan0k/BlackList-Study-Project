using System;

namespace BlackList.Domain.Models
{
    public class Person
    {  
        public FullName FullName { get; set; } 

        public string Position { get; set; }

        public string Notes { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
