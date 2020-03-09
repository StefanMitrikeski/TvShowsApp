using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvShowsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TvShowsApp.Controllers
{
    [Authorize]
    public class TvShowsController : Controller
    {
        private readonly TvShowsContext _context;

        public TvShowsController(TvShowsContext context)
        {
            _context = context;
        }

        // GET: TvShows

        public async Task<IActionResult> Index()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
            return View(await _context.TvShow.ToListAsync());
        }

        /*public async Task<IActionResult> Searching(string searchString)
        {
            var model = from m in _context.TvShow select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Title.Contains(searchString));
            }
            return View(await _context.model.ToListAsync());
        }*/

        // GET: TvShows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvShow = await _context.TvShow
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tvShow == null)
            {
                return NotFound();
            }

            return View(tvShow);
        }

        // GET: TvShows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TvShows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TvShowID,Title,Genre,Rating,ImdbUrl,ImageUrl")] TvShow tvShow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tvShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tvShow);
        }

        // GET: TvShows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvShow = await _context.TvShow.FindAsync(id);
            if (tvShow == null)
            {
                return NotFound();
            }
            return View(tvShow);
        }

        // POST: TvShows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Genre,Rating,ImdbUrl,ImageUrl")] TvShow tvShow)
        {
            if (id != tvShow.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tvShow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvShowExists(tvShow.ID))
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
            return View(tvShow);
        }

        // GET: TvShows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tvShow = await _context.TvShow
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tvShow == null)
            {
                return NotFound();
            }

            return View(tvShow);
        }

        // POST: TvShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tvShow = await _context.TvShow.FindAsync(id);
            _context.TvShow.Remove(tvShow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TvShowExists(int id)
        {
            return _context.TvShow.Any(e => e.ID == id);
        }
        public async Task<IActionResult> Movies()
        {
            return View(await _context.TvShow.ToListAsync());
        }

    }
}
