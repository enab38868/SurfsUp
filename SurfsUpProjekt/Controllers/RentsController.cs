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
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

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
            if (_context.Rent != null)
            {
                foreach (var board in _context.Rent)
                {
                    if (DateTime.Now > board.EndRent)
                    {
                        DeleteUserRentedBoardViaDateTime(board.BoardId);
                    }
                }
            }

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
                         where s.Premium == false
                         select s;
            
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewData["CurrentFilter"] = searchString;
                boards = from s in _context.Board
                             where s.IsRented == false
                             select s;
            }

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

        [Authorize(Roles = "User,Administrator")]
        public async Task<IActionResult> RentOut(int? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var rent = new Rent();
            return View(rent);
        }

        [Authorize(Roles = "User,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentOut(int id, string userID, [Bind(include: "StartRent,EndRent,RowVersionRent")] Rent rent)
        {
            if (!BoardExists(id))
            {
                return NotFound();
            }

            int tmpID = id;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            userID = claims.Value;
            rent.UserID = userID;

            rent.BoardId = id;

            if (rent.StartRent > rent.EndRent)
            {
                ModelState.AddModelError("StartRent", "Start date must be before end date");
            }
            else
            {
                if (rent.UserID != null && rent.BoardId != 0) //TODO vi vil gerne have modelstate.isvalid, men vi kan ikke få det til at fungere
                {
                    try
                    {
                        Board board = FindBoard(id);
                        board.IsRented = true;
                        board.UserID = userID;

                        _context.Update(board);
                        await _context.SaveChangesAsync();

                        _context.Add(rent);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(UserIndex));

                    }
                    catch (SqlException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Board is already rented bitch");
                    }
                    catch (DbUpdateException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Someone was faster than you bitch");
                    }
                }
            }
            return View(rent);
        }

        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
        }

        // Deletes User and return all boards in there rented table.
        public static void DeleteUserRentedBoard(string user)
        {
            string connectionString = "Server=10.56.8.36;Database=PEDB10;User Id=PE-10;Password=OPENDB_10;Trusted_Connection=False;MultipleActiveResultSets=true";
            //TODO fix, så connectionstring ikke ligger her?
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
        public static void DeleteUserRentedBoardViaDateTime(int boardId)
        {
            string connectionString = "Server=10.56.8.36;Database=PEDB10;User Id=PE-10;Password=OPENDB_10;Trusted_Connection=False;MultipleActiveResultSets=true";
            //TODO fix, så connectionstring ikke ligger her?
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteBoardDateTime", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@BoardId";
                paramId.Value = boardId;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Board FindBoard(int id)
        {
            Board tmpBoard = new();
            foreach (var board in _context.Board)
            {
                if (id == board.Id)
                {
                    tmpBoard = board;
                    break; //TODO <- Test 
                }
            }
            return tmpBoard;
        }


    }
}
