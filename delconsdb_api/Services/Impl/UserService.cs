
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using delconsdb_api.Models;
using delconsdb_api.Models.User;
using DWNet.Data;
using Microsoft.IdentityModel.Tokens;
using SnapObjects.Data;

namespace delconsdb_api.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly delconsdb_api.DelConsDBDataContext _dataContext;

        public string _userid;

        public UserService(delconsdb_api.DelConsDBDataContext dataContext)
        {
            _dataContext = dataContext;
        }

      
        public string LogIn(string userid, string password)        
        
        {
                     
            var users = GetUsers();

            foreach (var user in users) 
            {
                if (user.User_Id == userid && user.Passwd == password)
                {
                    _userid = userid;
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(Startup.SECRET);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.User_Id),
                            new Claim(ClaimTypes.Role, user.Role)
                        }),

                        Expires = DateTime.UtcNow.AddMinutes(5),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)                        
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                  
                    return user.Token;
                }
                }

           return string.Empty;
        }

        public List<D_Gps_Interface> GetTrackId()

        {
            IDataStore ds = new DataStore("D_Gps_Interface", _dataContext);
            ds.Retrieve();
            return ds.AsEnumerable<D_Gps_Interface>().ToList();

        }

        public List<UserDetails> GetUserDetails(string userid)
        {
            var sql = @"Select Top 1 user_name, phone_no, user_image
                       From  app_user
                       Where user_id=:userid";
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql, userid);
            List<UserDetails> det = new List<UserDetails>()
            {
                 new UserDetails {Profile_Pic = "", Phone_No = "", User_Name= "" }
              };
              
            if (result.Count > 0) { 
                var data = result[0];    
                det.FirstOrDefault().User_Name = data.GetValue<string>("user_name");
                det.FirstOrDefault().Phone_No = data.GetValue<string>("phone_no");
                det.FirstOrDefault().Profile_Pic = data.GetValue<string>("user_image");
             }
            return det.ToList();           

        }


        private List<D_User> GetUsers() 
        {
            IDataStore ds = new DataStore("D_User", _dataContext);        
            ds.Retrieve();
            return ds.AsEnumerable<D_User>().ToList();
        }
        
        public string GetUserId() 
        {
            return _userid;
        }

        public String UploadPhoto(string userid, string file)

        {
            Byte[] bytes = File.ReadAllBytes(file);
            String ret = Convert.ToBase64String(bytes);
            return ret;
        }
    }
}


