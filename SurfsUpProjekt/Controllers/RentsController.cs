using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt.Data;
using SurfsUpProjekt.Models;
using SurfsUpProjekt.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Versioning;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using static SurfsUpProjekt.Core.ConstantsRole;

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
                         where s.IsRented == false
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
        
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RentOut(int? id)
        {

            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var rent = new Rent();
            return View(rent);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, string UserID, [Bind(include: "StartRent,EndRent")] Rent rent)
        {
            if (!BoardExists(id))
            {
                return NotFound();
            }
            int tmpID = id;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            UserID = claims.Value;
            rent.UserID = UserID;

            rent.BoardId = id;
            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            else
            {
                if (rent.UserID != null && rent.BoardId != 0) //vi vil gerne have modelstate.isvalid, men vi kan ikke få det til at fungere
                {
                    try
                    {
                        Board board = FindBoard(id); //TODO Spørg Simon hvordan man kan lave det her smartere
                        board.IsRented = true;
                        board.UserID = UserID;

                        _context.Update(board);
                        await _context.SaveChangesAsync();

                        _context.Add(rent);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return RedirectToAction(nameof(UserIndex));
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

        // Deletes User and return all boards in there rented table.

        public static void DeleteUserRentedBoard(string user)
        {
            string connectionString = "Server=10.56.8.36;Database=PEDB10;User Id=PE-10;Password=OPENDB_10;Trusted_Connection=False;MultipleActiveResultSets=true";



            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteRentedBoard", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@UserID";
                paramId.Value = user;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();
            }


        }
        private Board FindBoard(int id)
        {
            Board tmpBoard = new ();
            foreach (var board in _context.Board)
            {
                if (id == board.Id)
                {
                    tmpBoard = board;
                    break; // <- Test 
                }
            }
            return tmpBoard;
        }
    }

}
