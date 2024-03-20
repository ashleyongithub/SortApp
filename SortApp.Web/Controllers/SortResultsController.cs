using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SortApp.Web.Data;
using SortApp.Web.Models;

namespace SortApp.Web.Controllers
{
    public class SortResultsController(SortAppWebContext context, ILogger<SortResultsController> logger) : Controller
    {
        private readonly SortAppWebContext _context = context;
        private readonly ILogger<SortResultsController> _logger = logger;

        // GET: SortResults
        public async Task<IActionResult> Index()
        {
            return View(await _context.SortResult.ToListAsync());
        }

        public IActionResult ExportToJson()
        {
            return Json(_context.SortResult.ToList());
        }

        // GET: SortResults/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SortResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numbers,Order")] SortResult sortResult)
        {

            if (!ModelState.IsValid)
            {
                return View(sortResult);
            }

            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                sortResult.Numbers = SortNumbers(sortResult.Numbers, sortResult.Order);

                watch.Stop();
                sortResult.TimeTaken = watch.Elapsed.TotalMilliseconds.ToString();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Numbers", "Error. Please provide a comma-separated list of integers.");
                return View(sortResult);
            }

            await SaveToDb(sortResult);

            return RedirectToAction(nameof(Index));
        }

        private async Task SaveToDb(SortResult sortResult)
        {
            try
            {
                _context.Add(sortResult);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
            }
        }

        private string SortNumbers(string numbersStr, string order)
        {
            try
            {
                int[] numbersArr = numbersStr.Split(",").Select(i => Convert.ToInt32(i)).ToArray();

                if (order == "Ascending")
                {
                    Array.Sort(numbersArr);
                }
                else if (order == "Descending")
                {
                    Array.Reverse(numbersArr);
                }

                return string.Join(", ", numbersArr);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message}");
                throw;
            }
        }
    }
}