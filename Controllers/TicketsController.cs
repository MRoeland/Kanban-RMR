using Kanban_RMR.Data;
using Kanban_RMR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;

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
        return View(await _context.Tickets.ToListAsync());
    }

    // GET: Tickets2
    public async Task<IActionResult> Index2()
    {
        // Fetch the data from the "Tickets" table and group it by "Status"
        var ticketsGroupedByStatus = _context.Tickets
            .GroupBy(t => t.Status)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Fetch the data from the "Statuses" table
        var statuses = _context.Statuses.ToList();
        // Pass the data to the view
        ViewBag.Statuses = new SelectList(statuses, "Id", "Description");
        ViewBag.StatusCnt = statuses.Count;

        // Pass the grouped data to the view
        ViewBag.TicketsGroupedByStatus = ticketsGroupedByStatus;

        return View();
        }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Tickets
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
        // Fetch the data from the "TicketTypes" table
        var types = _context.TicketTypes.ToList();
        // Pass the data to the view
        ViewBag.Types = new SelectList(types, "Id", "Description");

        // Fetch the data from the "Statuses" table
        var statuses = _context.Statuses.ToList();
        // Pass the data to the view
        ViewBag.Statuses = new SelectList(statuses, "Id", "Description");

        // Fetch the data from the "Priorities" table
        var priorities = _context.Priorities.ToList();
        // Pass the data to the view
        ViewBag.Priorities = new SelectList(priorities, "Id", "Description");

        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        KanbanUser user = _context.Users.Where(a => a.Id == userid).Single();
        ViewBag.CreatedBy = userid;
        ViewBag.Customer = user.Customer;
        ViewBag.CreatedOn = DateTime.Now;

        return View();
    }

    // POST: Tickets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Type,Project,Status,Priority,Customer,CreatedBy,CreatedOn")] Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        // Fetch the Customer name from the "Customers" table
        Customer customer = _context.Customers.Where(a => a.Id == ticket.Customer).Single();//_context.Customers.FindAsync(ticket.Customer);
        ViewBag.CustomerName = customer?.Name;

        KanbanUser user = _context.Users.Where(a => a.Id == ticket.CreatedBy).Single();//_context.Users.FindAsync(ticket.CreatedBy);
        ViewBag.CreatedBy = user?.Name;

        // Fetch the data from the "TicketTypes" table
        var types = _context.TicketTypes.ToList();
        // Pass the data to the view
        ViewBag.Types = new SelectList(types, "Id", "Description");

        // Fetch the data from the "Statuses" table
        var statuses = _context.Statuses.ToList();
        // Pass the data to the view
        ViewBag.Statuses = new SelectList(statuses, "Id", "Description");

        // Fetch the data from the "Priorities" table
        var priorities = _context.Priorities.ToList();
        // Pass the data to the view
        ViewBag.Priorities = new SelectList(priorities, "Id", "Description");

        return View(ticket);
    }

    // POST: Tickets/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Type,Project,Status,Priority,Customer,CreatedBy,CreatedOn")] Ticket ticket)
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
            return RedirectToAction(nameof(Index));
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TicketExists(int id)
    {
        return _context.Tickets.Any(e => e.Id == id);
    }
}