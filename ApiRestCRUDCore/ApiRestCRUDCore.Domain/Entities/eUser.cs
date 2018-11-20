using Components.Entities;

namespace ApiRestCRUDCore.Domain.Entities
{

    public class eUser:Entity
    {

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

    }

}