using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt.Core;
using SurfsUpProjekt.Data;
using SurfsUpProjekt.Models;
using static SurfsUpProjekt.Core.ConstantsRole;

namespace SurfsUpProjekt.Controllers
{
    //[Authorize(Roles = $"{ConstantsRole.Roles.Administrator}")]
    public class BoardsController : Controller
    {
        private readonly SurfsUpProjektContext _context;

        public BoardsController(SurfsUpProjektContext context)
        {
            _context = context;
        }

        // GET: Boards
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(
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

        // GET: Boards/Details/5
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

        // GET: Boards/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,Image")] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]                        
        public async Task<IActionResult> Edit([Bind("Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,Image, RowVersion")] Board board)
        {
            if (board.Id == null)
            {
                return NotFound();
            }
            // Trying to locate version?
            var boardToUpdate = await _context.Board.FirstOrDefaultAsync(m => m.Id == board.Id);

            if (boardToUpdate == null)
            {
                Board deletedBoard = new Board();
                await TryUpdateModelAsync(deletedBoard);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The Board was deleted by another user.");
                return View(deletedBoard);
            }
            _context.Entry(boardToUpdate).Property("RowVersion").OriginalValue = board.RowVersion;

            if (await TryUpdateModelAsync<Board>(
                boardToUpdate,
                "",
                s => s.Name, s => s.Length, s => s.Width, s => s.Thickness, s => s.Volume, s => s.Type, s => s.Price, s => s.Equipment, s => s.Image))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Board)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The Board was deleted by another user.");
                    }
                    else  //"Id,Name,Length,Width,Thickness,Volume,Type,Price,Equipment,Image"
                    {
                        var databaseValues = (Board)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }
                        if (databaseValues.Length != clientValues.Length)
                        {
                            ModelState.AddModelError("Length", $"Current value: {databaseValues.Length}");
                        }
                        if (databaseValues.Width != clientValues.Width)
                        {
                            ModelState.AddModelError("Width", $"Current value: {databaseValues.Width}");
                        }
                        if (databaseValues.Thickness != clientValues.Thickness)
                        {
                            ModelState.AddModelError("Thickness", $"Current value: {databaseValues.Thickness}");
                        }
                        if (databaseValues.Volume != clientValues.Volume)
                        {
                            ModelState.AddModelError("Volume", $"Current value: {databaseValues.Volume}");
                        }
                        if (databaseValues.Type != clientValues.Type)
                        {
                            ModelState.AddModelError("Type", $"Current value: {databaseValues.Type}");
                        }
                        if (databaseValues.Price != clientValues.Price)
                        {
                            ModelState.AddModelError("Price", $"Current value: {databaseValues.Price}");
                        }
                        if (databaseValues.Equipment != clientValues.Equipment)
                        {
                            ModelState.AddModelError("Equipment", $"Current value: {databaseValues.Equipment}");
                        }
                        if (databaseValues.Image != clientValues.Image)
                        {
                            ModelState.AddModelError("Image", $"Current value: {databaseValues.Image}");
                        }


                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                           + "was modified by another user after you got the original value. The "
                           + "edit operation was canceled and the current values in the database "
                           + "have been displayed. If you still want to edit this record, click "
                           + "the Save button again. Otherwise click the Back to List hyperlink.");
                        boardToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            return View(board);
        }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(boardToUpdate);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BoardExists(boardToUpdate.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //             throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //   return View(boardToUpdate);
        //}
    
        


        // GET: Boards/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Boards/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpProjektContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(int id)
        {
          return (_context.Board?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost, ActionName("Rent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentConfirmed(int id)
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserIndex));
        }
    }
}
