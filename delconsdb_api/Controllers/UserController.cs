using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using delconsdb_api.Services;
using Microsoft.AspNetCore.Http;
using delconsdb_api.Models;

namespace delconsdb_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]    
    public class UserController : BaseController
    {
        private readonly IUserService _iuserservice;

        public UserController(IUserService iuserservice)
        {
            _iuserservice = iuserservice;
        }

        //GET api/User/Retrieve
        [AllowAnonymous]
        [HttpPost]       
        public IActionResult Login([FromBody] D_User user)
        {
            try
            {              
                var result = _iuserservice.LogIn(user.User_Id, user.Passwd);

                if (result == null || result == String.Empty) 
                {
                     return BadRequest(new { message = "User name or password is incorrect" });
                }

                return Ok(result);
            }
                  catch (Exception ex)
                  {
                      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                  }
        }

    }
}
