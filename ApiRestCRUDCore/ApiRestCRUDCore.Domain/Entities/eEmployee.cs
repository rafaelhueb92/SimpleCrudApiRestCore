using Components.Entities;
using System;

namespace ApiRestCRUDCore.Domain.Entities
{

    public class eEmployee:Entity
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

    }

}