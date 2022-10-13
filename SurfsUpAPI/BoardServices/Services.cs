using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SurfsUpAPI.Model;

namespace SurfsUpAPI.BoardServices
{
    public class Services
    {

        private readonly APIContext _context;
        public Services(APIContext context)
        {
            _context = context;
        }

    }
}
