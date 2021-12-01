using System.ComponentModel.DataAnnotations;

namespace BlazingPizza
{
    public class Address
    {
        public int Id { get; set; }

        [MaxLength(100)] public string Name { get; set; } = "test user";

        [MaxLength(100)] public string Line1 { get; set; } = "some address";

        [MaxLength(100)]
        public string Line2 { get; set; }

        [MaxLength(50)] public string City { get; set; } = "NY";

        [MaxLength(20)] public string Region { get; set; } = "NY";

        [MaxLength(20)] public string PostalCode { get; set; } = "10024";
    }
}
