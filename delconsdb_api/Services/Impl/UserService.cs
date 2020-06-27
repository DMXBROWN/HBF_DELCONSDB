
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using delconsdb_api.Models;
using DWNet.Data;
using Microsoft.IdentityModel.Tokens;

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

            //if (!users.Select(s => s.User_Id == userid && s.Passwd == password).FirstOrDefault()) 
            //{
            //    return string.Empty;
            //}

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
    }
}


