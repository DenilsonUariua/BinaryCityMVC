namespace BinaryCityMVC.Models
{
    public class ContactViewModel
    {
        public Contact Contact { get; set; }
        public List<Client> Clients { get; set; }  // This will hold the list of clients for linking
    }
}
