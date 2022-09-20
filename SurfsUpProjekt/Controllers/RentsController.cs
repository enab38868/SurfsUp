using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt.Data;
using SurfsUpProjekt.Models;

namespace SurfsUpProjekt.Controllers
{
    public class RentsController : Controller
    {
        private readonly SurfsUpProjektContext _context;

        public RentsController(SurfsUpProjektContext context)
        {
            _context = context;
        }

        // GET: RentsController/UserIndex
        public async Task<IActionResult> UserIndex(
                string sortOrder,
                string currentFilter,
                string searchString,
                int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var boards = from s in _context.Board
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    boards = boards.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    boards = boards.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    boards = boards.OrderByDescending(s => s.Price);
                    break;
                default:
                    boards = boards.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 4;
            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: RentsController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.Id == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        public async Task<IActionResult> RentOut(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var rent = new Rent();
            return View(rent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, [Bind(include: "StartRent,EndRent")] Rent rent)
        {
            if (!BoardExists(id))
            {
                return NotFound();
            }

            rent.BoardId = id;
            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(rent);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(rent);
        }
        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }
        //[HttpPost, ActionName("Rent")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RentConfirmed(int id)
        //{
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(UserIndex));
        //}
    }

}
