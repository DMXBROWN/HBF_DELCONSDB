
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
            var sql = @"Select Top 1 user_name, phone_no, user_image,customer_name
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
                det.FirstOrDefault().Customer_Name = data.GetValue<string>("customer_name");
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
        
        public List<UserProject> GetUserProject(string userid)
        
        {
            var sql = @"Select distinct site_no
                       From  app_user_customer
                       Where user_id=:userid";
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql, userid);
            List<UserProject> det = new List<UserProject>();
            
           
            for (int i = 0; i < result.Count; i++)
            {
                var data = result[i];
                UserProject s = new UserProject();
                s.Site_No = data.GetValue<string>("site_no");
                det.Add(s);                
            }
            return det.ToList();
        }
        
        public List<UserProduct> GetUserProduct(string userid, string flag)
        
        {
            string sql;
            
            sql = "";
            //Sales Order
            if (flag =="SO" || flag == "OE")
            {
                sql = @"Select distinct item_code = c.item_code, item_description = c.item_description
                       From  app_user_customer a, sales_order b, sales_order_detail c
                       Where a.user_id=:userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code
                       and b.order_no=c.order_no";
            }
            
            //Track Delivery
            if (flag == "TD")
            {
                sql = @"Select distinct item_code = c.item_code, item_description = c.item_description
                       From  app_user_customer a, delivery_note b, delivery_note_detail c
                       Where a.user_id=:userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code
                       and b.dnote_no=c.dnote_no
                       and b.delivery_status='O'";
            }
            
            //Track Delivery
            if (flag == "DD")
            {
                sql = @"Select distinct item_code = c.item_code, item_description = c.item_description
                       From  app_user_customer a, delivery_note b, delivery_note_detail c
                       Where a.user_id=:userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code
                       and b.dnote_no=c.dnote_no
                       and b.delivery_status='C'";
            }
            
            
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql, userid);
            
            List<UserProduct> det = new List<UserProduct>();
            
            
            for (int i = 0; i < result.Count; i++)
            {
                var data = result[i];
                UserProduct s = new UserProduct();
                s.Item_Code = data.GetValue<string>("item_code");
                s.Item_Description = data.GetValue<string>("item_description");
                det.Add(s);
            }
            return det.ToList();
        }
        
        public String GetProjectURL()
        
        {
            string sql = @"Select company_project_link
                       From  company";
            string url;
            url = "";
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql);
            if (result != null)
            {
                var data = result[0];
                url = data.GetValue<string>("company_project_link");
            }
            return url;
        }
        
        public String GetProductURL()
        
        {
            string sql = @"Select company_product_link
                       From  company";
            string url;
            url = "";
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql);
            if (result != null)
            {
                var data = result[0];
                url = data.GetValue<string>("company_product_link");
            }
            return url;
        }
        
        public String GetDownloadURL()
        
        {
            string sql = @"Select company_download_link
                       From  company";
            string url;
            url = "";
            var result = _dataContext.SqlExecutor.Select<DynamicModel>(sql);
            if (result != null)
            {
                var data = result[0];
                url = data.GetValue<string>("company_download_link");
            }
            return url;
        }
        
        public int InsertApproval(Approval_Test ap_test)
        
        {
            var ds_approval = new DataStore("Approval_Test", _dataContext);
            
            ds_approval.AddRow(ap_test);
        
            return ds_approval.Update();
        }
        
    }
}


