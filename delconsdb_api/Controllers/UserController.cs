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
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<String> ProjectURL()
        {
            try
            {
                var result = _iuserservice.GetProjectURL();
                
                if (result == null || result == String.Empty)
                {
                    return NotFound();
                }
                
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<String> ProductURL()
        {
            try
            {
                var result = _iuserservice.GetProductURL();
                
                if (result == null || result == String.Empty)
                {
                    return NotFound();
                }
                
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<String> DownloadURL()
        {
            try
            {
                var result = _iuserservice.GetDownloadURL();
                
                if (result == null || result == String.Empty)
                {
                    return NotFound();
                }
                
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        
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
        
        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]         
        public ActionResult<int> Approval()
        {          
            
            int insertedRow;
            
            try
            {
                Approval_Test app_test = new Approval_Test();
                DateTime dt = DateTime.Now;
                app_test.Company_Code = 1;
                app_test.Module = "PO";
                app_test.Reportsys = "PO-T-211B";
                app_test.Name = "Approval Email test";
                app_test.Parameters ="Test Email";
                app_test.Priority = 1;
                app_test.Created_Date = dt;
                app_test.Created_By = "AKA";
                app_test.User_Id = "MAS";
                app_test.Status = 0;
                app_test.App_Key = "MAT_REQUISITION";
                app_test.Subject = "Your request for approval is in process..";
                
                insertedRow = _iuserservice.InsertApproval(app_test);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
            if (insertedRow <= 0)
            {
                return Conflict();
            }
            else
            {
                return Ok(insertedRow);
            }
        }
        
    }
}
