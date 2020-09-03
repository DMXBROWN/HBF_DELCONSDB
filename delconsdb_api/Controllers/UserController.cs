using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using delconsdb_api.Services;
using Microsoft.AspNetCore.Http;
using delconsdb_api.Models;
using System.Collections.Generic;
using delconsdb_api.Models.User;

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

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult GetProjects()
        //{
        //    try
        //    {
        //        var result = _iuserservice.LogIn(user.User_Id, user.Passwd);

        //        if (result == null || result == String.Empty)
        //        {
        //            return BadRequest(new { message = "User name or password is incorrect" });
        //        }


        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}


        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(D_Gps_Interface), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<D_Gps_Interface> >TrackId()
        {
            
            var trackid = _iuserservice.GetTrackId();

            if (trackid == null)
            {
                return NotFound();
            }

            return Ok(trackid);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(UserDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserDetails>> UserDetails()
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var users = _iuserservice.GetUserDetails(userid);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }


        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public string UpdatePhoto( string file)
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var users = _iuserservice.UploadPhoto(userid, file);

            if (users == null)
            {
                return "";
            }

            return users;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(UserProject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserProject>> GetProject()
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var proj = _iuserservice.GetUserProject(userid);

            if (proj == null)
            {
                return NotFound();
            }

            return Ok(proj);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(UserProduct), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<UserProduct>> GetUserProduct(string flag)
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var proj = _iuserservice.GetUserProduct(userid, flag);

            if (proj == null)
            {
                return NotFound();
            }

            return Ok(proj);
        }

    }
}
