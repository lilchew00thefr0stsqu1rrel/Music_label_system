using Microsoft.AspNetCore.Mvc;
using MvcMusicDistr.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MvcMusicDistr.Data;
using MvcMusicDistr.Models.GaitViewModels;
using Microsoft.Extensions.Logging;

namespace MvcMusicDistr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MvcMusicDistrContext _context;

        public HomeController(ILogger<HomeController> logger, MvcMusicDistrContext context)
        {
            _logger = logger;
            _context = context;  //~

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> About()
        {
            IQueryable<GaitGroup> data =
                from pair in _context.Pair
                group pair by pair.GaitID into gaitGroup
                select new GaitGroup()
                {
                    GaitID = gaitGroup.Key,
                    PairCount = gaitGroup.Count()
                };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}
