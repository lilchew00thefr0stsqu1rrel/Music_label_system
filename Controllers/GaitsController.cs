using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Data;
using MvcMusicDistr.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;


namespace MvcMusicDistr.Controllers
{
    public class GaitsController : Controller
    {
        private readonly MvcMusicDistrContext _context;

        public GaitsController(MvcMusicDistrContext context)
        {
            _context = context;
        }

        // GET: Gaits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gait.ToListAsync());
        }

        // GET: Gaits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gait = await _context.Gait
                .Include(g => g.Pairs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            //For sorted 
            var prs = gait.Pairs.OrderByDescending(p => p.Name).ToList();
            gait.Pairs = prs;

            //ViewData["LinkToPair"] = 

            if (gait == null)
            {
                return NotFound();
            }

            return View(gait);
        }

        // GET: Gaits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gaits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PairID")] Gait gait)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gait);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gait);
        }

        // GET: Gaits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gait = await _context.Gait.FindAsync(id);
            if (gait == null)
            {
                return NotFound();
            }

            //26 01 2025
            // Инициирование списка пар ног
            gait.Pairs = new List<Pair> { };
            // Очистка, чтоб пары шли в правильном порядке при изменении состава пар данного аллюра
            gait.Pairs.Clear();
            // В отсортированном списке пар (кортежей) поиск относящихся к данному аллюру 
            foreach (Pair p in _context.Pair.OrderByDescending(p=>p.Name))
            {
                if (p.Gait == gait && !gait.Pairs.Contains(p))
                {
                    gait.Pairs.Add(p);
                }
            }
            //gait.Pairs.OrderByDescending(p => p.Name).ToList();
            
            return View(gait);
        }

        // POST: Gaits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PairID,Pairs")] Gait gait)
        {
            if (id != gait.Id)
            {
                return NotFound();
            }


            string Nm = Request.Form["PairName"];
            string[] Nme = Nm.Split(new char[] { ',' });

            string Lf = Request.Form["Left"];
            string[] Lft = Lf.Split(new char[] { ',' });

            string Rg = Request.Form["Right"];
            string[] Rgt = Rg.Split(new char[] { ',' });




            ///gait.Pairs = new List<Pair>();  //очищ.!!
            
            // Пары данного аллюра.
            var gp = from p in _context.Pair where p.GaitID == gait.Id select p;
            
            // Из запроса - в список (поле объекта Аллюр - Gait)
            gait.Pairs = gp.Cast<Pair>().ToList();

            // Имеющиеся названия пар (кодовые)
            var nms = from p in _context.Pair select p.Name;
            // Имеющиеся идентификаторы аллюров
            var gt = from p in _context.Pair select p.GaitID;
            int length = 0;
            List<string> pairNames = new List<string>();
            foreach (Pair p in gait.Pairs)
            {
             
            //    if (!pairNames.Contains(p.Name)) {  //без повт.!!
            //    gait.Pairs.Add(new Pair()
            //    {
            //        Gait = p.Gait,
            //        GaitID = p.GaitID,
            //        //Id = p.Id,
            //        Name = p.Name,
            //        OnGroundLeft = p.OnGroundLeft,
            //        OnGroundRight = p.OnGroundRight
            //    });
                pairNames.Add(p.Name);
                length++;
            //    }
            }

            //int length = gait.Pairs.Count();
            

            int i = 0;
            foreach (Pair pair1 in gait.Pairs)
            {
                pair1.Gait = gait;
                pair1.GaitID = id;
                pair1.Name = Nme[i];
                pair1.OnGroundLeft = int.Parse(Lft[i]);
                pair1.OnGroundRight = int.Parse(Rgt[i]);
                i++;
                
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(gait);

                    foreach (Pair pair1 in gait.Pairs)
                    {
                        //_context.Update(pair1); //27 I 2025
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaitExists(gait.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

               
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Edit", new { id = gait.Id, Pairs = gait.Pairs });
            }
            return View(gait);
        }

        

        

        // GET: Gaits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gait = await _context.Gait
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gait == null)
            {
                return NotFound();
            }

            return View(gait);
        }

        // POST: Gaits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gait = await _context.Gait.FindAsync(id);
            if (gait != null)
            {
                _context.Gait.Remove(gait);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GaitExists(int id)
        {
            return _context.Gait.Any(e => e.Id == id);
        }
    }
}
