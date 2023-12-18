using Kanban_RMR.Data;
using Kanban_RMR.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

public class TicketsController : Controller
{
    private readonly KanbanDbContext _context;

    public TicketsController(KanbanDbContext context)
    {
        _context = context;
    }

    // GET: Tickets
    public async Task<IActionResult> Index()
    {
        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        KanbanUser user = _context.Users.Where(a => a.Id == userid).Single();

        if (user.CustomerId == 1)
        {
            return View(await _context.Tickets
                .Where(x => x.Deleted == false)
                .Include(x => x.Status).Include(x => x.Priority).Include(x => x.Type)
                .Include(x => x.Customer).Include(x => x.Project).Include(x => x.KanbanUser)
                .AsNoTracking()
                .ToListAsync());
        }
        else
        {
            return View(await _context.Tickets
                .Where(x => x.Deleted == false && x.CustomerId == user.CustomerId)
                .Include(x => x.Status).Include(x => x.Priority).Include(x => x.Type)
                .Include(x => x.Customer).Include(x => x.Project).Include(x => x.KanbanUser)
                .OrderBy(x => x.Index)
                .AsNoTracking()
                .ToListAsync());
        }
    }

    // GET: Tickets2
    public async Task<IActionResult> Index2(string searchString)
    {
        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        KanbanUser user = _context.Users.Where(a => a.Id == userid).Single();

        ViewData["CurrentFilter"] = searchString;

        // Fetch the data from the "Statuses" table
        var statuses = _context.Statuses.ToList();
        // Pass the data to the view
        ViewBag.Statuses = new SelectList(statuses, "Id", "Description");
        ViewBag.StatusCnt = statuses.Count;

        if (user.CustomerId == 1) // 'intern' -> tickets tonen voor alle customers
        {
            // Fetch the data from the "Tickets" table and group it by "Status"
            if (!string.IsNullOrEmpty(searchString))  // filter is actief
            {
                var ticketsGroupedByStatus = _context.Tickets
                    .Include(x => x.Customer)
                    .Where(x => x.Deleted == false && x.Customer.Name.Contains(searchString))
                    .GroupBy(t => t.StatusId)
                    .ToDictionary(g => g.Key, g => g.ToList().OrderBy(x => x.Customer.Name).ThenBy(x => x.Index));
                // Pass the grouped data to the view
                ViewBag.TicketsGroupedByStatus = ticketsGroupedByStatus;
            }
            else   // filter is niet actief
            {
                var ticketsGroupedByStatus = _context.Tickets
                    .Include(x => x.Customer)
                    .Where(x => x.Deleted == false)
                    .GroupBy(t => t.StatusId)
                    .ToDictionary(g => g.Key, g => g.ToList().OrderBy(x => x.Customer.Name).ThenBy(x => x.Index));
                // Pass the grouped data to the view
                ViewBag.TicketsGroupedByStatus = ticketsGroupedByStatus;
            }
        }
        else  // niet 'intern' -> enkel tickets tonen voor deze customer
        {
            // Fetch the data from the "Tickets" table and group it by "Status"
            var ticketsGroupedByStatus = _context.Tickets
                .Where(x => x.Deleted == false && x.CustomerId == user.CustomerId)
                .GroupBy(t => t.StatusId)
                .ToDictionary(g => g.Key, g => g.ToList().OrderBy(x => x.Index));
            // Pass the grouped data to the view
            ViewBag.TicketsGroupedByStatus = ticketsGroupedByStatus;
        }

        //return RedirectToAction(nameof(Index2));
        return View();
    }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        //ticket ophalen
        var ticket =  await _context.Tickets
            .Include(x => x.Status).Include(x => x.Priority).Include(x => x.Type)
            .Include(x => x.Customer).Include(x => x.Project).Include(x => x.KanbanUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin,user")]
    // GET: Tickets/Create
    public IActionResult Create()
    {
        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        KanbanUser user = _context.Users.Where(a => a.Id == userid).Single();

        // Fetch the data from the "TicketTypes" table
        var types = _context.TicketTypes.Where(x => x.Deleted == false).ToList();
        // Pass the data to the view
        ViewBag.Types = new SelectList(types, "Id", "Description");

        if (user.CustomerId == 1)  // 'intern' -> alle projecten weergeven
        {
            // Fetch the data from the "Projects" table
            var projects = _context.Projects.Where(x => x.Deleted == false).ToList();
            // Pass the data to the view
            ViewBag.Projects = new SelectList(projects, "Id", "Name");
        }
        else  // niet 'intern' -> alleen projecten voor deze customer weergeven
        {
            // Fetch the data from the "Projects" table
            var projects = _context.Projects.Where(x => x.Deleted == false && x.CustomerId == user.CustomerId).ToList();
            // Pass the data to the view
            ViewBag.Projects = new SelectList(projects, "Id", "Name");
        }

        // Fetch the data from the "Priorities" table
        var priorities = _context.Priorities.Where(x => x.Deleted == false).ToList();
        // Pass the data to the view
        ViewBag.Priorities = new SelectList(priorities, "Id", "Description");

        //update Index - aan einde toevoegen
        var idx = _context.Tickets.Where(x => x.Deleted == false && x.CustomerId == user.CustomerId).Max(x => x.Index);
        ViewBag.Index = idx + 1;
        // readonly data meegeven via ViewBag
        ViewBag.CreatedBy = userid;
        ViewBag.CustomerId = user.CustomerId;
        ViewBag.CreatedOn = DateTime.Now;

        return View();
    }

    // POST: Tickets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,TypeId,ProjectId,StatusId,PriorityId,Index,CustomerId,CreatedBy,CreatedOn")] Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index2));
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin,user")]
    // GET: Tickets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        KanbanUser user = _context.Users.Where(a => a.Id == userid).Single();

        //ticket ophalen
        var ticket = await _context.Tickets
            .Include(x => x.Status).Include(x => x.Priority).Include(x => x.Type)
            .Include(x => x.Customer).Include(x => x.Project).Include(x => x.KanbanUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }

        ViewBag.CustomerName = ticket.Customer.Name;
        ViewBag.CreatedBy = ticket.KanbanUser.Name;

        // Fetch the data from the "TicketTypes" table
        var types = _context.TicketTypes.Where(x => x.Deleted == false).ToList();
        // Pass the data to the view
        ViewBag.Types = new SelectList(types, "Id", "Description");

        if (user.CustomerId == 1)  // 'intern' -> alle projecten weergeven
        {
            // Fetch the data from the "Projects" table
            var projects = _context.Projects.Where(x => x.Deleted == false).ToList();
            // Pass the data to the view
            ViewBag.Projects = new SelectList(projects, "Id", "Name");
        }
        else  // niet 'intern' -> alleen projecten voor deze customer weergeven
        {
            // Fetch the data from the "Projects" table
            var projects = _context.Projects.Where(x => x.Deleted == false && x.CustomerId == user.CustomerId).ToList();
            // Pass the data to the view
            ViewBag.Projects = new SelectList(projects, "Id", "Name");
        }

        // Fetch the data from the "Statuses" table
        var statuses = _context.Statuses.Where(x => x.Deleted == false).ToList();
        // Pass the data to the view
        ViewBag.Statuses = new SelectList(statuses, "Id", "Description");
        ViewBag.Status = ticket.Status.Description;

        // Fetch the data from the "Priorities" table
        var priorities = _context.Priorities.Where(x => x.Deleted == false).ToList();
        // Pass the data to the view
        ViewBag.Priorities = new SelectList(priorities, "Id", "Description");

        return View(ticket);
    }

    // POST: Tickets/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TypeId,ProjectId,StatusId,PriorityId,Index,CustomerId,CreatedBy,CreatedOn")] Ticket ticket)
    {
        if (id != ticket.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index2));
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin")]
    // GET: Tickets/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        //ticket ophalen
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    // POST: Tickets/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        //ticket ophalen
        var ticket = await _context.Tickets.FindAsync(id);
        //_context.Tickets.Remove(ticket);  // niet fysiek deleten maar enkel logische delete
        ticket.Deleted = true;
        _context.Update(ticket);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TicketExists(int id)
    {
        return _context.Tickets.Any(e => e.Id == id);
    }

    // POST: Tickets/Switch
    [HttpPost, ActionName("Switch")]
    [Authorize(Roles = "admin,user")]
    public async Task<IActionResult> Switch(int oldid, int newid)
    {
        if (oldid== newid)
        {
            return Json(new { success = false, responseText = "switch obsolete" });
            //return View();
        }

        //Drag-ticket ophalen
        Ticket ticketToMove = _context.Tickets.Where(x => x.Id == oldid).Single();

        //lijst ophalen van alle tickets voor deze CustomerId, gesorteerd op Index
        // OPGELET: List heeft een ZERO-based index!
        List<Ticket> tickets = _context.Tickets
            .Where(x => x.Deleted == false && x.CustomerId == ticketToMove.CustomerId)
            .OrderBy(x => x.Index).ToList();

        //Nieuwe Index bepalen voor het drag-ticket
        int oldidx = (int)ticketToMove.Index;
        int newidx = oldidx;
        if (newid == 0) // dit betekent dat we het drag-ticket naar de laatste positie draggen
        {
            newidx = tickets.Max(x => x.Index).Value + 1;
        }
        else
        {
            Ticket ticket = tickets.Where(x => x.Id == newid).Single();
            //enkel newidx accepteren als drop-ticket van zelfde customer is
            if (ticket.CustomerId == ticketToMove.CustomerId)
                newidx = (int)ticket.Index;
        }

        // oldidx is zelfde als newidx -> geen updates nodig
        if (oldidx == newidx) {
            return Json(new { success = false, responseText = "switch obsolete or invalid" });
        }

        // alle indexen updaten in onze lijst
        // OPGELET: List heeft een ZERO-based index!
        //          Onze volgorde-index is 1-based
        //          Daarom: altijd tickets[i - 1] gebruiken ipv tickets[i]
        if (oldidx < newidx)
        {
            for (int i = oldidx+1; i <= newidx; i++)
            {
                if (i <= tickets.Count)
                {
                    tickets[i - 1].Index = tickets[i - 1].Index - 1;
                }
                else
                    newidx = tickets.Count;
            }
            tickets[oldidx-1].Index = newidx;
        }
        else
        {
            for (int i = newidx; i < oldidx; i++)
            {
                if (i < tickets.Count)
                {
                    tickets[i-1].Index = tickets[i-1].Index+1;
                }
                else
                    newidx = tickets.Count;
            }
            tickets[oldidx-1].Index = newidx;
        }

        //update de tickets in de tickets tabel
        foreach(Ticket ticket in tickets)
        {
            _context.Update(ticket);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index2));
    }

    // POST: Tickets/UpdateStatus/5
    [HttpPost, ActionName("UpdateStatus")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateStatus(int id, int newstatus)
    {
        if (User.IsInRole("admin"))
        {
            //Status ophalen van ticket dat gedragged is
            Ticket ticketToMove = _context.Tickets.Where(x => x.Id == id).Single();
            int oldstatus = ticketToMove.StatusId;
            if (oldstatus == newstatus)
                return View();

            //Status updaten van ticket dat gedragged is
            ticketToMove.StatusId = newstatus;
            ticketToMove.Status = _context.Statuses.Where(x => x.Id == newstatus).Single();
            _context.Update(ticketToMove);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index2));
        }
        else {
            return Json(new { success = false, responseText = "update status not allowed" });
        }
    }
}