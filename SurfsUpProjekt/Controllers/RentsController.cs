﻿using Microsoft.AspNetCore.Http;
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

        // GET: Boards
        public async Task<IActionResult> Index(string sortOrder,
                                                string currentFilter,
                                                string search,
                                                int? pageNumber,
                                                string type)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }

            var boards = from m in _context.Board
                         select m;

            boards = boards.Include(r => r.Rent);

            await boards.Where(board => board.Rent != null && board.Rent.EndRent < DateTime.Now).ForEachAsync(board => board.Rent = null);
            {
                await _context.SaveChangesAsync();
            }

            boards = boards.Where(board => board.Rent == null);


            /* Filtering the boards by the search string and then sorting them by the type. */
            if (!String.IsNullOrEmpty(search))
                boards = from b in boards where b.Name.ToLower()!.Contains(search.ToLower()) select b;

            if (!String.IsNullOrEmpty(type))
            {
                //PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(Board)).Find("Length", true);
                //var test2 = from b in _context.Board.ToList() orderby prop.GetValue(b) select b; //THIS WORKS
                //ReturnedList = (from b in _context.Board.ToList() orderby prop.GetValue(b) select b).ToList(); ///Works too
                boards = type.ToLower() switch
                {
                    "name" => from b in boards orderby b.Name select b,
                    "length" => from b in boards orderby b.Length select b,
                    "thickness" => from b in boards orderby b.Thickness select b,
                    "volume" => from b in boards orderby b.Volume select b,
                    "type" => from b in boards orderby b.Type select b,
                    "price" => from b in boards orderby b.Price select b,
                    "equipment" => from b in boards orderby b.Equipment select b,
                    _ => from b in boards orderby b.Name select b,
                };
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
    }

}
