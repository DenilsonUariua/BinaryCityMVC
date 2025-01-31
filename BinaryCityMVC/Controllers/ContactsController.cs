using BinaryCityMVC.Data;
using BinaryCityMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BinaryCityMVC.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var contacts = _context.Contacts
                .Include(c => c.ClientContacts)
                .AsEnumerable()
                .OrderBy(c => c.FullName)
                .ToList();

            return View(contacts);
        }


        public IActionResult Create()
        {
            var clients = _context.Clients.ToList();  // Get all clients from the database
            var model = new ContactViewModel
            {
                Clients = clients  // Assuming you have a ContactViewModel with Clients property
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        [HttpGet("Contacts/LinkClient/{id}")]
        public IActionResult LinkClient(int id)
        {
            var contact = _context.Contacts
                .Include(c => c.ClientContacts)
                .ThenInclude(cc => cc.Client)
                .FirstOrDefault(c => c.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            // Fetch the IDs of clients that are already linked
            var linkedClientIds = contact.ClientContacts.Select(cc => cc.ClientId).ToList();

            // Filter available clients by excluding linked clients
            var availableClients = _context.Clients
                .Where(c => !linkedClientIds.Contains(c.Id))
                .ToList();

            ViewBag.AvailableClients = availableClients;
            return View(contact);
        }


        [HttpPost]
        public IActionResult LinkClient(int id, int clientId)
        {
            var contact = _context.Contacts.Find(id);
            var client = _context.Clients.Find(clientId);

            if (contact == null || client == null)
            {
                return NotFound();
            }

            contact.ClientContacts.Add(new ClientContact { ContactId = id, ClientId = clientId });
            _context.SaveChanges();
            return RedirectToAction(nameof(LinkClient), new { id });
        }

        [HttpPost]
        public IActionResult UnlinkClient(int id, int clientId)
        {
            var clientContact = _context.ClientContacts
                .FirstOrDefault(cc => cc.ContactId == id && cc.ClientId == clientId);

            if (clientContact != null)
            {
                _context.ClientContacts.Remove(clientContact);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(LinkClient), new { id });
        }
    }
}
