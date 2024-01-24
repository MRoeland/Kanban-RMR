using Microsoft.AspNetCore.Mvc;
using Kanban_RMR.Models;
using Kanban_RMR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kanban_RMR.Controllers
{
    [Authorize(Roles = "admin,employee")]
    public class ProjectController : Controller
    {
        private readonly KanbanDbContext _context;

        public ProjectController(KanbanDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects
                .Where(x => x.Deleted == false)
                .Include(x => x.Customer)
                .AsNoTracking()
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //project ophalen
            var project = await _context.Projects
                .AsNoTracking()
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public IActionResult Create()
        {
            // Fetch the data from the "Customers" table
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            // Pass the data to the view
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CustomerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //project ophalen
            var project = await _context.Projects
                .AsNoTracking()
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            // Fetch the data from the "Customers" table
            var customers = _context.Customers.Where(x => x.Deleted == false).ToList();
            // Pass the data to the view
            ViewBag.Customers = new SelectList(customers, "Id", "Name");

            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CustomerId")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //project ophalen
            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //project ophalen
            var project = await _context.Projects.FindAsync(id);
            //_context.Projects.Remove(project);  // niet fysiek deleten maar enkel logische delete
            project.Deleted = true;
            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
