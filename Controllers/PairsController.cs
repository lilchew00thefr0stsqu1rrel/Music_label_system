using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MvcMusicDistr.Data;
using MvcMusicDistr.Models;
using NuGet.Packaging;


namespace MvcMusicDistr.Controllers
{
    public class PairsController : Controller
    {
        private readonly MvcMusicDistrContext _context;

        public PairsController(MvcMusicDistrContext context)
        {
            _context = context;
        }

        // GET: Pairs
        public async Task<IActionResult> Index(string sortOrder, 
            string currentFilter, 
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GaitSortParm"] = sortOrder == "Gait" ? "gait_desc" : "Gait";
            //ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var mvcMusicDistrContext = _context.Pair.Include(p => p.Gait);
            //
            var pairs = from p in _context.Pair select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                pairs = pairs.Where(p => p.Gait.Name.Contains(searchString)
                                       || p.Name.Contains(searchString)
                                       || p.Id == int.Parse(searchString));
            }

            switch (sortOrder)
            {
                case "name":
                    pairs = pairs.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    pairs = pairs.OrderByDescending(p => p.Name);
                    break;
                case "gait_desc":
                    pairs = pairs.OrderByDescending(p => p.GaitID);
                    break;
                default:
                    pairs = pairs.OrderBy(p => p.GaitID);
                    break;
            }
            //
            int pageSize = 4;
            return View(await PaginatedList<Pair>.CreateAsync(pairs
                .Include(p => p.Gait).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Pairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pair = await _context.Pair
                .Include(p => p.Gait)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pair == null)
            {
                return NotFound();
            }

            return View(pair);
        }

        // GET: Pairs/Create
        public IActionResult Create()
        {
            PopulateGaitsDropDownList();
            ViewData["GaitID"] = new SelectList(_context.Gait, "Id", "Name");
            return View();
        }

        // POST: Pairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OnGroundLeft,OnGroundRight,GaitID")] Pair pair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pair);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GaitID"] = new SelectList(_context.Gait, "Id", "Name", pair.GaitID);
            PopulateGaitsDropDownList(pair.GaitID); // 25 I 2025
            return View(pair);
        }

        // GET: Pairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pair = await _context.Pair.FindAsync(id);
            if (pair == null)
            {
                return NotFound();
            }
            ViewData["GaitID"] = new SelectList(_context.Gait, "Id", "Name", pair.GaitID);
            PopulateGaitsDropDownList(pair.GaitID);
            return View(pair);
        }

        // POST: Pairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OnGroundLeft,OnGroundRight,GaitID")] Pair pair)
        {
            if (id != pair.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PairExists(pair.Id))
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
            ViewData["GaitID"] = new SelectList(_context.Gait, "Id", "Name", pair.GaitID);
            return View(pair);
        }

        //25012025
        private void PopulateGaitsDropDownList(object selectedGait = null)
        {
            var gaitsQuery = from g in _context.Gait
                                   orderby g.Name
                                   select g;
            ViewBag.DepartmentID = new SelectList(gaitsQuery.AsNoTracking(), "GaitID", "Name", selectedGait);
        }

        public void UpdatePairsInGait()
        {

            ModelState.Remove("OnGroundLeft");
            ModelState.Remove("OnGroundRight");

        }


        // GET: Pairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pair = await _context.Pair
                .Include(p => p.Gait)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pair == null)
            {
                return NotFound();
            }

            return View(pair);
        }

        // POST: Pairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pair = await _context.Pair.FindAsync(id);
            if (pair != null)
            {
                _context.Pair.Remove(pair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PairExists(int id)
        {
            return _context.Pair.Any(e => e.Id == id);
        }

    }
}
