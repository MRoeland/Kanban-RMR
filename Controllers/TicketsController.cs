using Azure.Core;
using Kanban_RMR.Data;
using Kanban_RMR.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Xml.Linq;


public class TicketsController : Controller
{
    private readonly KanbanDbContext _context;
    private static Microsoft.Extensions.Primitives.StringValues referer;

    public TicketsController(KanbanDbContext context)
    {
        _context = context;
    }

    // GET: Tickets
    public async Task<IActionResult> Index()
    {
        var userid = GetUserId();
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
        var userid = GetUserId();
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
            .Include(x => x.Comments.Where(c => c.Deleted == false))
            .ThenInclude(c => c.KanbanUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin,employee,user")]
    // GET: Tickets/Create
    public IActionResult Create()
    {
        //get redirect url where we came from (Index or Index2)
        referer = Request.Headers.Referer;

        var userid = GetUserId();
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

            await AddReward("CreatedTicket");

            // If redirect supplied, then do it, otherwise use a default
            if (!String.IsNullOrEmpty(referer))
                return Redirect(referer.ToString());
            else
                return RedirectToAction(nameof(Index2));
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin,employee,user")]
    // GET: Tickets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //get redirect url where we came from (Index or Index2)
        referer = Request.Headers.Referer;

        var userid = GetUserId();
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
            // If redirect supplied, then do it, otherwise use a default
            if (!String.IsNullOrEmpty(referer))
                return Redirect(referer.ToString());
            else
                return RedirectToAction(nameof(Index2));
        }
        return View(ticket);
    }

    [Authorize(Roles = "admin,employee")]
    // GET: Tickets/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        //get redirect url where we came from (Index or Index2)
        referer = Request.Headers.Referer;

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
    [Authorize(Roles = "admin,employee")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        //ticket ophalen
        var ticket = await _context.Tickets.FindAsync(id);
        //_context.Tickets.Remove(ticket);  // niet fysiek deleten maar enkel logische delete
        ticket.Deleted = true;
        _context.Update(ticket);
        await _context.SaveChangesAsync();

        // If redirect supplied, then do it, otherwise use a default
        if (!String.IsNullOrEmpty(referer))
            return Redirect(referer.ToString());
        else
            return RedirectToAction(nameof(Index));
    }

    private bool TicketExists(int id)
    {
        return _context.Tickets.Any(e => e.Id == id);
    }

    // POST: Tickets/Switch
    [HttpPost, ActionName("Switch")]
    [Authorize(Roles = "admin,employee,user")]
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
    [Authorize(Roles = "admin,employee")]
    public async Task<IActionResult> UpdateStatus(int id, int newstatus)
    {
        if(User.IsInRole("admin") || User.IsInRole("employee"))
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

            if (newstatus == 5)
                await AddReward("MoveToDone");

            return RedirectToAction(nameof(Index2));
        }
        else {
            return Json(new { success = false, responseText = "update status not allowed" });
        }
    }

    // Action to save a comment
    [HttpPost, ActionName("SaveComment")]
    [Authorize(Roles = "admin,employee,user")]
    public async Task<IActionResult> SaveComment(int ticketId, string commentText)
    {
        Comment newComment = null;
        if (commentText != null)
        {
            newComment = new Comment
            {
                TicketId = ticketId,
                CreatedBy = GetUserId(),
                Description = commentText,
                CreatedOn = DateTime.Now
            };
            _context.Add(newComment);
            await _context.SaveChangesAsync();

            await AddReward("AddedComment");
            return Json(new { success = true }); // Return a success message to the client if needed
        }
        return Json(new { success = false, responseText = "Comment is empty" });
    }

    // Action to update a comment
    [HttpPost, ActionName("UpdateComment")]
    [Authorize(Roles = "admin,employee,user")]
    public async Task<IActionResult> UpdateComment(int ticketId, int commentId, string commentText)
    {
        var commentToUpdate = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.TicketId == ticketId);
        if (commentToUpdate != null)
        {
            commentToUpdate.Description = commentText;
            commentToUpdate.CreatedOn = DateTime.Now;

            _context.Update(commentToUpdate);
            await _context.SaveChangesAsync();
            return Json(new { success = true }); // Return a success message to the client if needed
        }
        return Json(new { success = false, responseText = "Comment not found for update" });
    }

    // Action to delete a comment
    [HttpPost, ActionName("DeleteComment")]
    public async Task<IActionResult> DeleteComment(int ticketId, int commentId)
    {
        var commentToDelete = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.TicketId == ticketId);
        if (commentToDelete != null)
        {
            commentToDelete.Deleted = true;
            _context.Update(commentToDelete);
            await _context.SaveChangesAsync();
            return Json(new { success = true }); // Return a success message to the client if needed
        }
        return Json(new { success = false, responseText = "Comment not found for delete" });
    }

    [HttpPost]
    public async Task<IActionResult> LikeComment(int ticketId, int commentId)
    {
        // Implement logic to find and update the comment's like count
        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.TicketId == ticketId);
        if (comment != null)
        {
            var currentUserId = GetUserId();
            if (comment.CreatedBy != currentUserId)
            {
                comment.Likes++;
                _context.Update(comment);
                await _context.SaveChangesAsync();
                await AddReward("LikedComment", comment.CreatedBy);
            }
        }

        return Json(new { likes = comment.Likes });
    }

    [HttpPost]
    public async Task<IActionResult> DislikeComment(int ticketId, int commentId)
    {
        // Implement logic to find and update the comment's dislike count
        var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId && c.TicketId == ticketId);
        if (comment != null)
        {
            var currentUserId = GetUserId();
            if (comment.CreatedBy != currentUserId)
            {
                comment.Dislikes++;
                _context.Update(comment);
                await _context.SaveChangesAsync();
                await AddReward("DislikedComment", comment.CreatedBy);
            }
        }

        return Json(new { dislikes = comment.Dislikes });
    }

    private async Task<bool> AddReward(string action, string userId = null)
    {
        Reward reward = _context.Rewards.Where(r => r.Action == action && r.Enabled == true && r.Deleted == false).Single();
        if (reward != null)
        {
            var usertoreward = userId;
            if (usertoreward == null)
              usertoreward = GetUserId();
            KanbanUser user = _context.Users.Where(a => a.Id == usertoreward).Single();

            user.Points = user.Points + reward.Points;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        return true;
    }

    private string GetUserId()
    {
        //return id of user currently logged in
        var userid = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userid;
    }
}