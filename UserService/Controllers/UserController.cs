using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Models;
using UserService.Settings;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UsersSettings _settings;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,
        IOptions<UsersSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var rng = new Random();


            int index = rng.Next(_settings.Users.Count());
            Thread.Sleep(3000);
            return Ok(_settings.Users.ElementAt(index));
        }

        [HttpGet("{id:int}")]
        public ActionResult<User> GetUserById([Required][FromRoute] int id)
        {
            User user = _settings.Users.SingleOrDefault(u => u.Id == id);

            //To simulate computation time 
            Thread.Sleep(3000);
            
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
