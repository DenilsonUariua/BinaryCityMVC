using BinaryCityMVC.Data;
using BinaryCityMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static BinaryCityMVC.Helpers.ClientCodeHelpers;

namespace BinaryCityMVC.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public IActionResult Index()
        {
            var clients = _context.Clients
                .Include(c => c.ClientContacts)
                .OrderBy(c => c.Name)
                .ToList();

            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            var contacts = _context.Contacts.ToList();  // Get all contacts from the database
            var model = new ClientViewModel
            {
                Contacts = contacts  // Assuming you have a ContactViewModel with Clients property
            };
            return View(model);
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            // Clear ModelState entry for ClientCode to bypass validation
            ModelState.Remove("ClientCode");

            if (ModelState.IsValid)
            {
                try
                {
                    client.ClientCode = ClientCodeGenerator.GenerateClientCode(client.Name, _context);

                    _context.Clients.Add(client);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the client. Please try again.");
                    Debug.WriteLine($"Exception: {ex.Message}");
                }
            }

            // Debugging: Print ModelState errors
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Debug.WriteLine($"Model Error: {modelError.ErrorMessage}");
            }

            return View(client);
        }


        // GET: Clients/LinkContact/{id}
        [HttpGet("Clients/LinkContact/{id}")]
        public IActionResult LinkContact(int id)
        {
            var client = _context.Clients
                .Include(c => c.ClientContacts)
                .ThenInclude(cc => cc.Contact)
                .FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            // Get all contacts that are not linked to the client
            var availableContacts = _context.Contacts
                .Where(c => !client.ClientContacts.Select(cc => cc.ContactId).Contains(c.Id)) // Check for unlinked contacts
                .ToList();

            var viewModel = new LinkContactViewModel
            {
                Client = client,
                AvailableContacts = availableContacts
            };

            return View(viewModel);
        }


        // POST: Clients/LinkContact/{id}
        [HttpPost]
        public IActionResult LinkContact(int id, int contactId)
        {
            var client = _context.Clients.Find(id);
            var contact = _context.Contacts.Find(contactId);

            if (client == null || contact == null)
            {
                return NotFound();
            }

            client.ClientContacts.Add(new ClientContact { ClientId = id, ContactId = contactId });
            _context.SaveChanges();
            return RedirectToAction(nameof(LinkContact), new { id });
        }

        // POST: Clients/UnlinkContact/{id}
        [HttpPost]
        public IActionResult UnlinkContact(int id, int contactId)
        {
            var clientContact = _context.ClientContacts
                .FirstOrDefault(cc => cc.ClientId == id && cc.ContactId == contactId);

            if (clientContact != null)
            {
                _context.ClientContacts.Remove(clientContact);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(LinkContact), new { id });
        }
    }
}
