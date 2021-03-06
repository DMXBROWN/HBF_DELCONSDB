using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using delconsdb_api.Services;

namespace delconsdb_api.Controllers
{
    

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {      
        protected string GetUserId()
        {          
            return this.User.Claims.First(i => i.Type == "Name").Value;
        }
    }
}
