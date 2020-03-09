using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TvShowsApp.Models;
using System.ComponentModel.DataAnnotations;
using TvShowsApp;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;

namespace TvShowsApp.Controllers
{
    [Authorize]
    public class RentedMoviesController : Controller
    {
        private readonly TvShowsContext _context;

        public RentedMoviesController(TvShowsContext context)
        {
            _context = context;
        }

        // GET: RentedMovies
        public async Task<IActionResult> Index()
        {
            var tvShowsContext = _context.RentedMovies.Include(r => r.TvShow);
            return View(await tvShowsContext.ToListAsync());
        }

        // GET: RentedMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovies = await _context.RentedMovies
                .Include(r => r.TvShow)
                .FirstOrDefaultAsync(m => m.RentedMoviesId == id);
            if (rentedMovies == null)
            {
                return NotFound();
            }

            return View(rentedMovies);
        }

        // GET: RentedMovies/Create
        public IActionResult Create(int id)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name");
            ViewData["TvShowId"] = new SelectList(_context.TvShow.Where(x => !_context.RentedMovies.Select(y => y.TvShowId).Contains(x.ID)), "ID", "Title");
            ViewBag.TvShowId = id;
            return View();
        }

        // POST: RentedMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentedMovieId,UserId,TvShowId")] RentedMovies rentedMovies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentedMovies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Name", rentedMovies.UserId);
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "ID", "ImageUrl", rentedMovies.TvShowId);
            return View(rentedMovies);
        }

        // GET: RentedMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovies = await _context.RentedMovies.FindAsync(id);
            if (rentedMovies == null)
            {
                return NotFound();
            }
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "ID", "ImageUrl", rentedMovies.TvShowId);
            return View(rentedMovies);
        }

        // POST: RentedMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentedMovieId,UserId,TvShowId")] RentedMovies rentedMovies)
        {
            if (id != rentedMovies.RentedMoviesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentedMovies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentedMoviesExists(rentedMovies.RentedMoviesId))
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
            ViewData["TvShowId"] = new SelectList(_context.TvShow, "ID", "ImageUrl", rentedMovies.TvShowId);
            return View(rentedMovies);
        }

        // GET: RentedMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentedMovies = await _context.RentedMovies
                .Include(r => r.TvShow)
                .FirstOrDefaultAsync(m => m.RentedMoviesId == id);
            if (rentedMovies == null)
            {
                return NotFound();
            }

            return View(rentedMovies);
        }

        // POST: RentedMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentedMovies = await _context.RentedMovies.FindAsync(id);
            _context.RentedMovies.Remove(rentedMovies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentedMoviesExists(int id)
        {
            return _context.RentedMovies.Any(e => e.RentedMoviesId == id);
        }

        [HttpPost]
        public void Rent(int TvShowId, int UserId)
        {
            RentedMovies rentedMovies = new RentedMovies();
            rentedMovies.TvShowId = TvShowId;
            rentedMovies.UserId = UserId;
            rentedMovies.RentalDate = DateTime.Now;
            _context.Add(rentedMovies);
            _context.SaveChanges();


            TvShow TvShow = _context.TvShow.FirstOrDefault(x => x.ID == TvShowId);
            TvShow.Available = true;
            _context.Update(TvShow);
            _context.SaveChanges();
            //return RedirectToAction("Index","TvShows");
        }

        [HttpPost]
        public void Return(int TvShowId, int UserId)
        {
            RentedMovies rentedMovies = _context.RentedMovies.FirstOrDefault(x => x.ReturnDate == null && x.TvShowId == TvShowId);
            rentedMovies.ReturnDate = DateTime.Now;
            _context.Update(rentedMovies);
            _context.SaveChangesAsync();

            TvShow TvShow = _context.TvShow.FirstOrDefault(x => x.ID == TvShowId);
            TvShow.Available = false;
            _context.Update(TvShow);
            _context.SaveChangesAsync();
            //return RedirectToAction("Index", "TvShows");
        }

        //-------------EXCEL-----Task-----BELOW-------------------
        [HttpGet]
        public IActionResult ExportExcelRented()
        {



            string[] col_names = new string[]
            {
                "Name",
                "Title",
                "Date Rented",
                "Date Returned",
                "Today"
            };
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("final");

                for (int i = 0; i < col_names.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Style.Font.Size = 14;
                    worksheet.Cells[1, i + 1].Value = col_names[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 243, 214));
                }
                int row = 2;
                foreach (RentedMovies item in _context.RentedMovies.Include(c => c.User).Include(a => a.TvShow).ToList())
                {
                    if (item.ReturnDate != null)
                    {}
                    else
                    {
                        TimeSpan validDate = DateTime.Now.AddDays(7).Subtract(item.RentalDate);
                        if (validDate > TimeSpan.FromDays(7))
                        {
                            for (int col = 1; col <= col_names.Length; col++)
                            {
                                worksheet.Cells[row, col].Style.Font.Size = 12;
                                worksheet.Cells[row, col].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick); worksheet.Cells[row, col].Style.Font.Size = 12;
                            }
                            worksheet.Cells[row, 1].Value = item.User.Name;
                            worksheet.Cells[row, 2].Value = item.TvShow.Title;
                            worksheet.Cells[row, 3].Value = item.RentalDate.ToString();
                            worksheet.Cells[row, 4].Value = item.ReturnDate.ToString();
                            worksheet.Cells[row, 5].Value = DateTime.Now.AddDays(7).ToString();

                            if (row % 2 == 0)
                            {
                                for (int col = 1; col <= col_names.Length; col++)
                                {
                                    worksheet.Cells[row, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(154, 211, 157));
                                }
                            }
                        }
                        row++;
                    }
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                result = package.GetAsByteArray();
            }
            return File(result, "application/vnd.ms-excel", "test.xlsx");
        }

        //---------------- Sand Mail -----------------------------

        [HttpGet]
        public IActionResult SandMail()
        {
            foreach (RentedMovies item in _context.RentedMovies.Include(c => c.User).Include(a => a.TvShow).ToList())
            {
                TimeSpan validDate = DateTime.Now.AddDays(7).Subtract(item.RentalDate);
                
                if (item.ReturnDate != null)
                {
                }
                else
                {
                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress("Stefan", "stefanmitrikeski1994@gmail.com"));

                    message.To.Add(new MailboxAddress(item.User.Name, "stefanmitrikeski1994@gmail.com"));

                    message.Subject = "Hello Geniuss";

                    message.Body = new TextPart("plain")
                    {
                        Text = item.User.Name + " You are the winer of the grand prize for Geniusses " + item.TvShow.Title
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("stefanmitrikeski1994@gmail.com", "mi34ence994stefan");

                        client.Send(message);
                        client.Disconnect(true);
                        // client.Dispose();
                    }
                }
            }
            return RedirectToAction("Index", "RentedMovies");
        }
    }
}
   

     
