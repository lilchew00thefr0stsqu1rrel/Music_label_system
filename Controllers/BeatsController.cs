using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Data;
using MvcMusicDistr.Models;

namespace MvcMusicDistr.Controllers
{
    public class BeatsController : Controller
    {
        private readonly MvcMusicDistrContext _context;

        public BeatsController(MvcMusicDistrContext context)
        {
            _context = context;
        }

        // GET: Beats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Beat.ToListAsync());
        }

        // GET: Beats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beat = await _context.Beat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beat == null)
            {
                return NotFound();
            }

            return View(beat);
        }

        // GET: Beats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Beats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Beatmaker,Tonality,BPM,Price,OtherProducers")] Beat beat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beat);
        }

        // GET: Beats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beat = await _context.Beat.FindAsync(id);
            if (beat == null)
            {
                return NotFound();
            }
            return View(beat);
        }

        // POST: Beats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Beatmaker,Tonality,BPM,Price,OtherProducers")] Beat beat)
        {
            if (id != beat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeatExists(beat.Id))
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
            return View(beat);
        }

        // GET: Beats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beat = await _context.Beat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beat == null)
            {
                return NotFound();
            }

            return View(beat);
        }

        // POST: Beats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beat = await _context.Beat.FindAsync(id);
            if (beat != null)
            {
                _context.Beat.Remove(beat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeatExists(int id)
        {
            return _context.Beat.Any(e => e.Id == id);
        }


        // GET: Books/XML
        public ActionResult XML([Bind("Id,Name,Beatmaker,Tonality,BPM,Price")] Beat beat)
        {
            var beat_q = from b in _context.Beat select b;
            beat.Name = beat_q.First().Name;
            beat.Beatmaker = beat_q.First().Beatmaker;
            beat.Tonality = beat_q.First().Tonality;

            List<string> name_l = new List<string>();
            foreach (Beat b in beat_q)
            {
                //beat.Name += ", " + b.Name;
                if (b.Name != null) 
                { 
                name_l.Add(b.Name);
            }       
            }


            // Create the XML data
            var xml0 = new XDocument(
                new XElement("Beats",
                    new XElement("Beat",
                        new XElement("Name", beat.Name),
                        new XElement("Beatmaker", beat.Beatmaker),
                        new XElement("Tonality", beat.Tonality),
                        new XElement("BPM", beat.BPM),
                        new XElement("Price", beat.Price
                    ),
                    new XElement("Beat",
                        new XElement("Name", name_l[0]),
                        new XElement("Beatmaker", beat.Beatmaker),
                        new XElement("Tonality", beat.Tonality),
                        new XElement("BPM", beat.BPM),
                        new XElement("Price", beat.Price)
                    ),
                    new XElement("Beat",
                        new XElement("Name", name_l[1]),
                        new XElement("Beatmaker", beat.Beatmaker),
                        new XElement("Tonality", beat.Tonality),
                        new XElement("BPM", beat.BPM),
                        new XElement("Price", beat.Price)
                    ),
                    new XElement("Beat",
                        new XElement("Name", name_l[2]),
                        new XElement("Beatmaker", beat.Beatmaker),
                        new XElement("Tonality", beat.Tonality),
                        new XElement("BPM", beat.BPM),
                        new XElement("Price", beat.Price)
                    )

                )
               
            ));

            var xml = new XDocument(
                new XElement("Beats"));

            //XDocument xml2= XDocument.Load("Beats.xml");


            foreach (var b in beat_q) { 
                  xml.Root.Add(new XElement("Beat",
                            new XElement("Name", b.Name),
                            new XElement("Beatmaker", b.Beatmaker),
                            new XElement("Tonality", b.Tonality),
                            new XElement("BPM", b.BPM),
                        new XElement("Price", b.Price)));
            }

            var doc = XDocument.Load("Beats.xml");
            var name = doc.Descendants("Name").First();

            xml.Save("Beats.xml");

            // Add a record from context
            //foreach (Beat b in beats) {
            //    xml.Add(new XElement("Beat",
            //        new XElement("Name", b.Name),
            //        new XElement("Beatmaker", b.Beatmaker),
            //        new XElement("Tonality", b.Tonality)
            //       ));                  
            //}

            // Return the XML as a ContentResult with appropriate content type
            return Content(xml.ToString(), "application/xml", Encoding.UTF8);

            
        }

        XmlSerializer serializer = new XmlSerializer(typeof(Beat));


        // 13X
        // GET: api/authors
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_context.Beat.ToList());
        }

       
    }
}
