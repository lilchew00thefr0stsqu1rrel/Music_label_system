using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvcMusicDistr.Data;
using MvcMusicDistr.Migrations;
using MvcMusicDistr.Models;

namespace MvcMusicDistr.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MvcMusicDistrContext _context;

        public ArtistsController(MvcMusicDistrContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index(string artistSubstance, string searchString)
        {
            if (_context.Artist == null)
            {
                return Problem("Entity set 'MvcMusicDistrContext.Movie'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> substanceQuery = from m in _context.Artist
                                                orderby m.Substance
                                                select m.Substance;

            var artists = from a in _context.Artist
                          select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(s => s.Nickname!.ToUpper().Contains(searchString.ToUpper())
                || s.Animal!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(artistSubstance))
            {
                artists = artists.Where(x => x.Substance == artistSubstance);
            }

            var ArtistSubstanceVM = new ArtistSubstanceViewModel
            {
                Substances = new SelectList(await substanceQuery.Distinct().ToListAsync()),
                Artists = await artists.ToListAsync()
            };
            return View(ArtistSubstanceVM);
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nickname,Animal,Substance,Reason")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ViewData["Message1"] = "Field Nickname should be unique";
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // Message if not unique nickname (constraint)
        public void WarnUnique()
        {
            ViewData["Message1"] = "Field Nickname should be unique";
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nickname,Animal,Substance,Reason")] Artist artist)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            // Перехват.
            _context.Update(artist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //return BadRequest();
                //return RedirectToAction(nameof(Index));
                ViewData["Message1"] = "Field Nickname should be unique";
                return View();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
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
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artist.FindAsync(id);
            if (artist != null)
            {
                _context.Artist.Remove(artist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.Id == id);
        }

        public ActionResult XML([Bind("Id,Nickname,Animal,Substance,Reason")] Artist artist)
        {// Create the XML data
            var xml = new XDocument(
            new XElement("Artists"));

            var artist_q = from a in _context.Artist select a;

            foreach (var a in artist_q)
            {
                xml.Root.Add(new XElement("Artist",
                          new XElement("Id", a.Id),
                          new XElement("Nickname", a.Nickname),
                          new XElement("Animal", a.Animal),
                          new XElement("Substance", a.Substance),
                          new XElement("Reason", a.Reason)));
            }
            var doc = XDocument.Load("Artists.xml");
            //var Id = doc.Descendants("Id").First();

            xml.Save("Artists.xml");
            return Content(xml.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
