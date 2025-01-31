using System.ComponentModel.DataAnnotations;

namespace BinaryCityMVC.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string ClientCode { get; set; }

        public string Description { get; set; }

        public ICollection<ClientContact> ClientContacts { get; set; } = new List<ClientContact>();
    }
}
