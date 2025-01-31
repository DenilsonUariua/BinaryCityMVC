namespace BinaryCityMVC.Models
{
	public class ClientContact
	{
		public int ClientId { get; set; }
		public Client Client { get; set; }
		public int ContactId { get; set; }
		public Contact Contact { get; set; }
	}
}
