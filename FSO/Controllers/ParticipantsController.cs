using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FSO.Data;
using FSO.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FSO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ParticipantsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Participants
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Participants.Include(p => p.Event).ToListAsync();

            if (User.IsInRole("User"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // UserId jako string

                // konwersja guidu na stringa
                if (Guid.TryParse(currentUserId, out Guid userGuid))
                {
                    applicationDbContext = applicationDbContext.Where(p => p.UserId == userGuid).ToList();
                }
                else
                {
                    return Forbid();
                }
            }

            var userIds = applicationDbContext.Select(p => p.UserId.ToString()).Distinct();
            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            ViewData["Users"] = users;


            return View(applicationDbContext);
        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants
                .Include(p => p.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participants == null)
            {
                return NotFound();
            }

            return View(participants);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            return Forbid();
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id");
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,UserId,Status")] Participants participants)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", participants.EventId);
            return View(participants);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants.FindAsync(id);
            if (participants == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", participants.EventId);
            return View(participants);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,UserId,Status")] Participants participants)
        {
            return Forbid();
            if (id != participants.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantsExists(participants.Id))
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
            ViewData["EventId"] = new SelectList(_context.Event, "Id", "Id", participants.EventId);
            return View(participants);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return Forbid();
            if (id == null)
            {
                return NotFound();
            }

            var participants = await _context.Participants
                .Include(p => p.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participants == null)
            {
                return NotFound();
            }

            return View(participants);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participants = await _context.Participants.FindAsync(id);
            if (participants != null)
            {
                _context.Participants.Remove(participants);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantsExists(int id)
        {
            return _context.Participants.Any(e => e.Id == id);
        }
    }
}
