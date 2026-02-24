using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Data;
using MvcMusicDistr.Models;

using MvcMusicDistr.Models.DistribModels;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Linq;

namespace MvcMusicDistr.Controllers
{
    public class DistributionsController : Controller
    {
        private readonly MvcMusicDistrContext _context;

        public DistributionsController(MvcMusicDistrContext context)
        {
            _context = context;
        }

        // GET: Distributions
        public async Task<IActionResult> Index()

         
        {
            decimal TotalValue = 0;
            foreach (Distribution d in _context.Distribution) { TotalValue += d.Value; }
            ViewData["TotalValue"] = TotalValue;

            ThisMonth();

            return View(await _context.Distribution.ToListAsync());

            
        }

        // GET: Distributions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distribution
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distribution == null)
            {
                return NotFound();
            }

            return View(distribution);
        }

        // GET: Distributions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Distributions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Performer,ReleaseType,Value")] Distribution distribution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distribution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(distribution);
        }

        // GET: Distributions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distribution.FindAsync(id);
            if (distribution == null)
            {
                return NotFound();
            }
            return View(distribution);
        }

        // POST: Distributions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Performer,ReleaseType,Value")] Distribution distribution)
        {
            if (id != distribution.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distribution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistributionExists(distribution.Id))
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
            return View(distribution);
        }

        // GET: Distributions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distribution = await _context.Distribution
                .FirstOrDefaultAsync(m => m.Id == id);
            if (distribution == null)
            {
                return NotFound();
            }

            return View(distribution);
        }

        // POST: Distributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distribution = await _context.Distribution.FindAsync(id);
            if (distribution != null)
            {
                _context.Distribution.Remove(distribution);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistributionExists(int id)
        {
            return _context.Distribution.Any(e => e.Id == id);
        }

        public void ThisMonth()
        {
            IQueryable<DistributionDateGroup> data =
                from distribution in _context.Distribution
                group distribution by distribution.Date.Month into dateGroup
                select new DistributionDateGroup()
                {
                    Month = dateGroup.Key,
                    TotalValue = dateGroup.Sum(x => x.Value)
                };
                
            foreach (var monthGroup in data)
            {
                if (monthGroup.Month == DateTime.Now.Month)
                {
                    ViewData["TotalValueMonth"] = monthGroup.TotalValue;
                }
            }

            //return View(await data.AsNoTracking().ToListAsync());
        }

        public decimal UserMonth(int Month)
        {
            IQueryable<DistributionDateGroup> monthGroup = from distribution in _context.Distribution
                                    group distribution by distribution.Date.Month into dateGroup
                                    select new DistributionDateGroup()
                                    {
                                        Month = dateGroup.Key,
                                        TotalValue = dateGroup.Sum(x => x.Value)
                                    };
            decimal totalValue = 0;
            foreach (var v in monthGroup)
            {
                totalValue += v.TotalValue;
            }
            return totalValue;
        }

        // GET: 
        public async Task<IActionResult> Report()


        {
            return View(await _context.Distribution.ToListAsync());
        }

        public ActionResult XML([Bind("Id,Date,Performer,ReleaseType,Value")] Distribution distribution)
        {// Create the XML data
            var xml = new XDocument(
            new XElement("Distributions"));

            var distr_q = from d in _context.Distribution select d;

            foreach (var d in distr_q)
            {
                xml.Root.Add(new XElement("Distribution",
                          new XElement("Id", d.Id),
                          new XElement("Date", d.Date),
                          new XElement("Performer", d.Performer),
                          new XElement("ReleaseType", d.ReleaseType),
                          new XElement("Value", d.Value)));
            }
            var doc = XDocument.Load("Distributions.xml");
            //var Id = doc.Descendants("Id").First();

            xml.Save("Distributions.xml");
            return Content(xml.ToString(), "application/xml", Encoding.UTF8);
        }



    }
}
