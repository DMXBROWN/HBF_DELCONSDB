using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;

namespace delconsdb_api.Services
{
    public interface IUserService
    {
        string LogIn(string userid, string password);
        string GetUserId();
        List<D_Gps_Interface> GetTrackId();
    }
}
