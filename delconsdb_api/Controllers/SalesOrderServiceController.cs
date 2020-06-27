using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWNet.Data;
using Microsoft.AspNetCore.Http;
using delconsdb_api.Services;
using delconsdb_api.Models;
using delconsdb_api;
using Microsoft.AspNetCore.Authorization;

namespace delconsdb_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SalesOrderServiceController : BaseController
    {
        private readonly ISalesOrderService _salesorderservice;
        
        public SalesOrderServiceController(ISalesOrderService salesorderservice)
        {
            _salesorderservice = salesorderservice;
         }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        [ProducesResponseType(typeof(order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<order> >Orders()
        {
            var currentUser = HttpContext.User;
            string userid = currentUser.Identity.Name;
            var orders = _salesorderservice.RetrieveOrders(userid);
            
            if (orders==null)
            {
                return NotFound();
            }
            
            return Ok(orders);
        }

      
    }
}
