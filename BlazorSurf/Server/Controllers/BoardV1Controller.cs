using BlazorSurf.Server.Data;
using BlazorSurf.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BlazorSurf.Server.Controllers
{
    [Route("api/Boards")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BoardV1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BoardV1Controller(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet, Route("GetAllBoards")]
        public async Task<IEnumerable<Board>> GetAllBoards()
        {
            return _context.Boards.OrderBy(a => a.Name).ToList();
        }
        public async Task<IEnumerable<Rent>> GetAllRents()
        {
            return _context.Rents;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBoard(int id)
        {
            return Ok(_context.Boards.Find(id));
        }

        [HttpPost("Rent/{id}")]
        public async Task<IActionResult> RentOut(int id, [FromBody] Rent rent)
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
                if (rent.UserID != null && rent.BoardId != 0) //TODO vi vil gerne have modelstate.isvalid, men vi kan ikke få det til at fungere
                {
                    try
                    {
                        Board board = FindBoard(id);
                        board.IsRented = true;
                        board.UserID = rent.UserID;

                        _context.Update(board);
                        await _context.SaveChangesAsync();

                        _context.Add(rent);
                        await _context.SaveChangesAsync();
                    }
                    catch (SqlException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Board is already rented");
                    }
                    catch (DbUpdateException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Someone was faster than you");
                    }
                }
            }
            return Ok(rent);
        }

        private bool BoardExists(int id)
        {
            return _context.Boards.Any(e => e.Id == id);
        }
        private Board FindBoard(int id)
        {
            Board tmpBoard = new();
            foreach (var board in _context.Boards)
            {
                if (id == board.Id)
                {
                    tmpBoard = board;
                    break; //TODO <- Test 
                }
            }
            return tmpBoard;
        }

        [HttpPost, Route("Create")] 
        public async Task<IActionResult> Create([FromBody] Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }

    }
}
