using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SurfsUpAPI.REPO;
using SurfsUpAPI.Model;
using System.Security.Claims;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly APIContext _context;

        public BoardsController(APIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Board>> GetAllBoards()
        {
            return _context.Board.OrderBy(a => a.Name).ToList();
        }
        //[HttpGet, Route(]
        //public async Task<ActionResult> GetBoard(int id)
        //{

        //    return Ok( _context.Board.Find(id));
        //}
        [HttpPost, Route("Rent/{id}")]
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
            return Ok(rent);
        }
        private bool BoardExists(int id)
        {
            return _context.Board.Any(e => e.Id == id);
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
