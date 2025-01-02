using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FSO.Data;
using FSO.Models;
using Microsoft.AspNetCore.Authorization;

namespace FSO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventTags
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventTag.Include(e => e.Event).Include(e => e.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var eventTag = await _context.EventTag
                .Include(e => e.Event)
                .Include(e => e.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventTag == null)
            {
                return NotFound();
            }

            return View(eventTag);
        }

        // GET: EventTags/Create
        public IActionResult Create()
        {
            return Forbid();
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id");
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Id");
            return View();
        }

        // POST: EventTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,TagId")] EventTag eventTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", eventTag.EventId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Id", eventTag.TagId);
            return View(eventTag);
        }

        // GET: EventTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var eventTag = await _context.EventTag.FindAsync(id);
            if (eventTag == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", eventTag.EventId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Id", eventTag.TagId);
            return View(eventTag);
        }

        // POST: EventTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,TagId")] EventTag eventTag)
        {
            if (id != eventTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventTagExists(eventTag.Id))
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
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", eventTag.EventId);
            ViewData["TagId"] = new SelectList(_context.Tag, "Id", "Id", eventTag.TagId);
            return View(eventTag);
        }

        // GET: EventTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var eventTag = await _context.EventTag
                .Include(e => e.Event)
                .Include(e => e.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventTag == null)
            {
                return NotFound();
            }

            return View(eventTag);
        }

        // POST: EventTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventTag = await _context.EventTag.FindAsync(id);
            if (eventTag != null)
            {
                _context.EventTag.Remove(eventTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventTagExists(int id)
        {
            return _context.EventTag.Any(e => e.Id == id);
        }
    }
}
