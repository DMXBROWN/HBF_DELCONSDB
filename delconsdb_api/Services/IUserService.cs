using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;
using delconsdb_api.Models.User;

namespace delconsdb_api.Services
{
    public interface IUserService
    {
        string LogIn(string userid, string password);
        string GetUserId();
        List<D_Gps_Interface> GetTrackId();
        List<UserDetails> GetUserDetails(string userid);
        string UploadPhoto(string userid, string file);
        List<UserProject> GetUserProject(string userid);
        List<UserProduct> GetUserProduct(string userid, string flag);
        string GetProjectURL();
        string GetProductURL();
        string GetDownloadURL();
        int InsertApproval(Approval_Test ap_test);
    }
}
