using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenStore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenStore.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserRepository _user;

        public UsersController(IApplicationUserRepository user)
        {
            _user = user;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult Get(string type, string query = null)
        {
            if (type.Equals("email") && query != null)
            {
                var users = _user.List(u => u.Email.ToLower().Contains(query.ToLower()));

                return Ok(users);
            }
            return Ok();
        }
    }
}
