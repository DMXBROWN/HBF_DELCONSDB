using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using delconsdb_api.Services;
using Microsoft.AspNetCore.Authorization;
using delconsdb_api.Models;
using Microsoft.AspNetCore.Http;
using delconsdb_api.Models.User;

namespace delconsdb_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeliveryNoteServiceController : ControllerBase
    {
        private readonly IDeliveryNoteService _dnoteservice; 
        
        public DeliveryNoteServiceController(IDeliveryNoteService dnoteservice)
        {
            _dnoteservice = dnoteservice;
         }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(Delivery_Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Delivery_Note>> Delivery()
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var dnotes = _dnoteservice.RetrieveDnotes(userid);

            if (dnotes == null)
            {
                return NotFound();
            }

            return Ok(dnotes);
        }

        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(Dnote_Upcoming), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Dnote_Upcoming>> UpcomingDelivery([FromBody] UserParameter param)
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var dnotes = _dnoteservice.UpcomingDnotes(userid, param);
            if (dnotes == null)
            {
                return NotFound();
            }

            return Ok(dnotes);
        }

        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(Dnote_Deilvery_Details), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Dnote_Deilvery_Details>> DeliveryDetails([FromBody] UserParameter param)
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var dnotes = _dnoteservice.DeliveryDetails(userid, param);
            if (dnotes == null)
            {
                return NotFound();
            }

            return Ok(dnotes);
        }

        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(Dnote_Upcoming), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Dnote_Upcoming>> TrackDelivery([FromBody] UserParameter param)
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var dnotes = _dnoteservice.TrackDelivery(userid, param);
            if (dnotes == null)
            {
                return NotFound();
            }

            return Ok(dnotes);
        }





    }
}
