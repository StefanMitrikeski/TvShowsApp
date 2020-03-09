using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TvShowsApp.Models;

namespace TvShowsApp.Controllers
{
    [Authorize]
    public class ActorsController : Controller
    {
        private readonly TvShowsContext _context;

        public ActorsController(TvShowsContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index(int Id)
        {
            ViewBag.TvShow_id = Id;
            return View(await _context.Actors.Where(m => m.TvShowID == Id).Include(a => a.TvShow).ToListAsync());
        }   

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actors = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorsID == id);
            if (actors == null)
            {
                return NotFound();
            }

            return View(actors);
        }

        // GET: Actors/Create
        public IActionResult Create(int Id)
        {
            ViewBag.TvShow_id = Id;
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,LastName,Age,ImageUrl,ImdbUrl,TvShowID")] Actors actors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actors);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = actors.TvShowID });

            }
            return View(actors);
        }

        // GET: Actors/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Actors actors = await _context.Actors.FindAsync(id);
            if (actors == null)
            {
                return NotFound();
            }
            return View(actors);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ActorsID,Name,LastName,Age,ImdbUrl,ImageUrl,TvShowID")] Actors actors)
        {
            if (id != actors.ActorsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsExists(actors.ActorsID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = actors.TvShowID });
            }
            return View(actors);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actors = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorsID == id);
            if (actors == null)
            {
                return NotFound();
            }

            return View(actors);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actors = await _context.Actors.FindAsync(id);
            _context.Actors.Remove(actors);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = actors.TvShowID });
        }

        private bool ActorsExists(int id)
        {
            return _context.Actors.Any(e => e.ActorsID == id);
        }

        public async Task<IActionResult> ListaAkteri()
        {
            return View(await _context.Actors.Include(a => a.TvShow).ToListAsync());
        }
    }
}
