using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SurfsUpAPI.REPO;
using SurfsUpAPI.Model;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly APIContext _context;
        //private readonly BoardREPO _boardREPO;

        public BoardsController(/*BoardREPO boardREPO,*/ APIContext context)
        {
            //_boardREPO = boardREPO;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Board>> GetAllBoards()
        {
            List<Board> boardslist = new List<Board>();//Services(mappe). fks Boardserverice 
            //string cncstring = _boardREPO.ConString(); // REPO mappe //husk at registere i Program.cs
            //using (SqlConnection sqlcon = new SqlConnection(cncstring))
            {
                boardslist = _context.Board.OrderBy(a => a.Name).ToList(); // den ´forventer Ienumerable som retur nu
               
                return boardslist;

            }
            //return Ok(await _context.Board.ToArrayAsync());
        }
        //[HttpGet, Route("/Rents/Details/{id}")]
        //public async Task<ActionResult> GetBoard(int id)
        //{
        //    //var board = _context.Board.Find(id);
        //    return Ok(board);
        //}
    }
}
