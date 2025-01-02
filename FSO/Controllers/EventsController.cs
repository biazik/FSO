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
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;

namespace FSO.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            // Dodana funkcjonalność do sortowania po tagach
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userIdGuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Pobranie punktacji użytkownika
            var userScores = await _context.Participants
                .Where(p => p.UserId == userIdGuid)
                .ToListAsync();

            var tagScores = userScores
                .Join(
                    _context.EventTag,
                    participant => participant.EventId,
                    eventTag => eventTag.EventId,
                    (participant, eventTag) => new { eventTag.TagId, participant.Status }
                )
                .GroupBy(et => et.TagId)
                .Select(group => new
                {
                    TagId = group.Key,
                    Score = group.Sum(g => g.Status)
                })
                .ToDictionary(t => t.TagId, t => t.Score);

            var events = await _context.Event
                .Include(e => e.EventType)
                .Include(e => e.Location)
                .Where(e => e.End >= DateTime.Now)
                .ToListAsync();

            var rankedEvents = events
                .AsEnumerable()
                .Select(e => new
                {
                    Event = e,
                    Score = _context.EventTag
                        .Where(et => et.EventId == e.Id)
                        .AsEnumerable()
                        .Sum(et => tagScores.ContainsKey(et.TagId) ? tagScores[et.TagId] : 0)
                })
                .OrderByDescending(e => e.Score)
                .Select(e => e.Event)
                .ToList();

            return View(rankedEvents);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var event_ = await _context.Event
                .Include(e => e.EventType)
                .Include(f => f.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null || event_ == null)
            {
                TempData["ErrorMessage"] = "Podano nieprawidłowy identyfikator wydarzenia";
                return RedirectToAction("Index", "Home");
            }
            var location = _context.Location.FirstOrDefault(l => l.Id == event_.LocationId);
            Debug.WriteLine($"Lat: {location.lat}, Lon: {location.lon}");

            if (location == null)
            {
                return NotFound();
            }

            ViewData["Lat"] = location.lat;
            ViewData["Lon"] = location.lon;

            return View(event_);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["EventTypeId"] = new SelectList(_context.EventType, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Name");
            var viewModel = new EventView
            {
                AvailableTags = _context.Tag.ToList() // Pobieranie dostępnych tagów
            };

            return View(viewModel);
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventView model)
        {
            var userIdString = _userManager.GetUserId(User);
            var @event = new Event
            {
                Name = model.Event.Name,
                Description = model.Event.Description,
                isPublic = model.Event.isPublic,
                EventTypeId = model.Event.EventTypeId,
                LocationId = model.Event.LocationId,
                Start = model.Event.Start,
                End = model.Event.End,
                CreatorId = Guid.Parse(userIdString)
            };

            if (model.Event.Start >= model.Event.End)
            {
                TempData["ErrorMessage"] = "Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.";
            }

            if ((model.Event.End - model.Event.Start).TotalDays > 7)
            {
                TempData["ErrorMessage"] = "Wydarzenie nie może trwać dłużej niż 7 dni.";
            }

            if (model.SelectedTagIds == null || !model.SelectedTagIds.Any())
            {
                TempData["ErrorMessage"] = "Musisz wybrać przynajmniej jeden tag.";
            }

            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                foreach (var tagId in model.SelectedTagIds)
                {
                    var eventTag = new EventTag
                    {
                        EventId = @event.Id,
                        TagId = tagId
                    };

                    _context.Add(eventTag);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            model.AvailableTags = _context.Tag.ToList();
            ViewData["EventTypeId"] = new SelectList(_context.EventType, "Id", "Name", model.Event.EventTypeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Name", model.Event.LocationId);
            return View(model);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["EventTypeId"] = new SelectList(_context.EventType, "Id", "Id", @event.EventTypeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Id", @event.LocationId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,EventTypeId,LocationId,Start,End,CreatorId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            ViewData["EventTypeId"] = new SelectList(_context.EventType, "Id", "Id", @event.EventTypeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Id", @event.LocationId);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.EventType)
                .Include(f => f.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event != null)
            {
                _context.Event.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReactToEvent(int eventId, int status) // Dodawanie reakcji do eventów
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdString, out var userId);// konwersja, bo id jest przechowywane jako string, a nie guid
            await AddOrUpdateParticipant(eventId, userId, status);
            TempData["ConfirmationMessage"] = "Dodano reakcję!";
            return RedirectToAction(nameof(Index));
        }

        private async Task AddOrUpdateParticipant(int eventId, Guid userId, int status)
        {
            var participant = _context.Participants
                .FirstOrDefault(p => p.EventId == eventId && p.UserId == userId);

            if (participant == null)
            {
                participant = new Participants
                {
                    EventId = eventId,
                    UserId = userId,
                    Status = status
                };
                _context.Participants.Add(participant);
            }
            else //update zamiast dodawania wartości x razy
            {
                participant.Status = status;
            }

            await _context.SaveChangesAsync();
        }

    private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
