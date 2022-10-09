using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurfsUpProjekt;
using SurfsUpProjekt.Data;
using SurfsUpProjekt.Models;
using System.Net;

namespace SurfsUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        //private readonly SurfsUpProjektContext _context;


        public BoardsController()
        {
           
        }

        [HttpGet("/UserIndex")]
        public async Task<IEnumerable<Board>> GetAllBoards()
        {
            List<Board> boardslist = new List<Board>();//Services(mappe). fks Boardserverice 
            string cncstring = ("Server = 10.56.8.36; Database = PEDB10; User Id = PE - 10; Password = OPENDB_10; Trusted_Connection = False; MultipleActiveResultSets = true"); // REPO mappe //husk at registere i Program.cs
            using (SqlConnection sqlcon = new SqlConnection(cncstring))
            {
                //boardslist = _context.Board.OrderBy(a => a.Name).ToList(); // den ´forventer Ienumerable som retur nu
               
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
