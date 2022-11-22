using BlazorSurf.Client;
using BlazorSurf.Server.Data;
using BlazorSurf.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorSurf.Server.Controllers
{
    public class RentServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Rent> _rent;
     
        public RentServiceController(ApplicationDbContext context, DbSet<Rent> rent)
        {
            _context = context;
            _rent = rent;
        }
        public async Task<IActionResult> UserIndex(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            if (_context.Rents != null)
            {
                foreach (var board in _context.Rents)
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

            // Default sorting without premium boards
            var boards = from s in _context.Boards
                         where s.IsRented == false
                         where s.Premium == false
                         select s;

            // If you are logged in it shows all the boards
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewData["CurrentFilter"] = searchString;
                boards = from s in _context.Boards
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

            string URL; // TODO Api light version her gør ikke noget
            if (User.Identity.IsAuthenticated)
            {
                URL = "https://localhost:7217/api/Boards?api-version=1.0";
            }
            else
            {
                URL = "https://localhost:7217/api/Boards?api-version=2.0";
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
    }
}
