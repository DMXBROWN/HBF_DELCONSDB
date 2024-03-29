using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;
using delconsdb_api.Models.User;
using DWNet.Data;

namespace delconsdb_api.Services.Impl 
{
    public class DeliveryNoteService : IDeliveryNoteService
    {
        //Testing by Arjun on 02-08-2021
        private readonly delconsdb_api.DelConsDBDataContext _dataContext;
        
        public DeliveryNoteService(delconsdb_api.DelConsDBDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public List<Delivery_Note> RetrieveDnotes(string userid)
        {
            var ds = new DataStore<Delivery_Note>(_dataContext);
            
            ds.Retrieve(userid);
            return ds.ToList();
        }
        
        public List<Dnote_Upcoming> UpcomingDnotes(string userid, UserParameter param)
        {
            
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Dnote_Upcoming> dnotes = new List<Dnote_Upcoming>();
            
            if (dateto.HasValue)
            {
                dateto = new DateTime(dateto.Value.Year, dateto.Value.Month, dateto.Value.Day, 23, 59, 59);
            }
            
            foreach (var i in param.Project)
            {
                if(!String.IsNullOrEmpty(i.Site_No))
                    site.Add(i.Site_No);
            }
            
            foreach (var i in param.Product)
            {
                if (!String.IsNullOrEmpty(i.Item_Code))
                    product.Add(i.Item_Code);
            }
            //just testing
            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Vehicle_No=b.Vehicle_No,
                            Gps_Id =b.Gps_Id,
                            Driver_Name = b.Driver_Name,
                            Track_Api_User_Name = (SELECT TOP 1 GPS_INTERFACE_ID FROM GPS_INTERFACE),
                            Track_Api_Password =(SELECT TOP 1 GPS_INTERFACE_PWD FROM GPS_INTERFACE),
                            St_Longitude= b.St_Longitude,
                            St_Latitude=b.St_Latitude,
                            En_Longitude=b.En_Longitude,
                            En_Latitude=b.En_Latitude,
                            Plate_No=b.Plate_No,
                            Status =b.Delivery_Status,
                            Lpo_No = (SELECT TOP 1 Lpo_No FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Lpo_Date = (SELECT TOP 1 Lpo_Date FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO)
                       From  app_user_customer a, delivery_note b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and isnull(b.delivery_status,'S') IN ('S','O')
                       ";
            //and((b.dnote_date >= @datefrom or @datefrom is null) and(b.dnote_date <= @dateto or @dateto is null))
            
            var result = _dataContext.SqlExecutor.Select<Dnote_Upcoming>(sql, userid, datefrom, dateto);
            
            if (site.Count > 0)
            {
                result = result.Where(c => site.Contains(c.Site_No)).ToList();
            }
            
            foreach (var data in result)
            {
                Dnote_Upcoming dn = new Dnote_Upcoming();
                dn.Dnote_No = data.Dnote_No;
                dn.Order_Date = data.Order_Date;
                dn.Order_No = data.Order_No;
                dn.Dnote_Date = data.Dnote_Date;
                dn.Site_No = data.Site_No;
                dn.Vehicle_No = data.Vehicle_No;
                dn.Gps_Id = data.Gps_Id;
                dn.Driver_Name = data.Driver_Name;
                dn.Track_Api_User_Name = data.Track_Api_User_Name;
                dn.Track_Api_Password = data.Track_Api_Password;
                dn.St_Longitude = data.St_Longitude;
                dn.St_Latitude = data.St_Latitude;
                dn.En_Longitude = data.En_Longitude;
                dn.En_Latitude = data.En_Latitude;
                dn.Plate_No = data.Plate_No;
                dn.Status = data.Status;
                dn.Lpo_No = data.Lpo_No;
                dn.Lpo_Date = data.Lpo_Date;
                
                string sqldt = @"Select Product = c.item_description,                            
                           Sr_No = c.sr_no, 
                           Item_Code =c.item_code, 
                           Unit =c.unit,
                           Order_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                            FROM SALES_ORDER_DETAIL 
                                            WHERE COMPANY_CODE = b.COMPANY_CODE 
                                            AND ORDER_NO = b.ORDER_NO 
                                            AND ITEM_CODE =c.ITEM_CODE ), 
                           Del_Qty =c.quantity, 
                           Bal_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                       FROM SALES_ORDER_DETAIL 
                                       WHERE COMPANY_CODE = b.COMPANY_CODE 
                                       AND ORDER_NO = b.ORDER_NO 
                                       AND ITEM_CODE =c.ITEM_CODE )  -  c.QUANTITY
                       From  app_user_customer a, delivery_note b, delivery_note_detail c
                       Where a.user_id=@userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code                      
                       and b.dnote_no=c.dnote_no
                       and b.order_no=@orderno
                       and b.dnote_no=@dnoteno
                       and b.site_no =@siteno";
                       
                string orderno = data.Order_No;
                string dnoteno = data.Dnote_No;
                string siteno = data.Site_No;
                
                var rs = _dataContext.SqlExecutor.Select<Dnote_Upcoming_Det>(sqldt, userid, orderno, dnoteno, siteno);
                
                if (product.Count > 0)
                {
                    rs = rs.Where(c => product.Contains(c.Item_Code)).ToList();
                    if (rs.Count <= 0)
                    {
                        continue;
                    }
                }
                List<Dnote_Upcoming_Det> items = new List<Dnote_Upcoming_Det>();
                
                foreach (var i in rs)
                {
                    Dnote_Upcoming_Det it = new Dnote_Upcoming_Det();
                    it.Product = i.Product;
                    //it.Lpo_No = i.Lpo_No;
                    //it.Lpo_Date = i.Lpo_Date;
                    it.Sr_No = i.Sr_No;
                    it.Item_Code = i.Item_Code;
                    it.Unit = i.Unit;
                    it.Order_Qty = i.Order_Qty;
                    it.Del_Qty = i.Del_Qty;
                    it.Bal_Qty = i.Bal_Qty;
                    items.Add(it);
                }
                dn.Product = items.ToList();
                dnotes.Add(dn);
            }
            return dnotes.ToList();
        }
        
        public List<Dnote_Deilvery_Details> DeliveryDetails(string userid, UserParameter param)
        {
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Dnote_Deilvery_Details> dnotes = new List<Dnote_Deilvery_Details>();
            
            if (dateto.HasValue)
            {
                dateto = new DateTime(dateto.Value.Year, dateto.Value.Month, dateto.Value.Day, 23, 59, 59);
            }
            
            foreach (var i in param.Project)
            {
                if (!String.IsNullOrEmpty(i.Site_No))
                    site.Add(i.Site_No);
            }
            
            foreach (var i in param.Product)
            {
                if (!String.IsNullOrEmpty(i.Item_Code))
                    product.Add(i.Item_Code);
            }
            
            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Vehicle_No=b.Vehicle_No,
                            Driver_Name = b.Driver_Name,                            
                            St_Longitude= b.St_Longitude,
                            St_Latitude=b.St_Latitude,
                            En_Longitude=b.En_Longitude,
                            En_Latitude=b.En_Latitude,
                            Plate_No=b.Plate_No,
                            Status =b.Delivery_Status,
                            Lpo_No = (SELECT TOP 1 Lpo_No FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Lpo_Date = (SELECT TOP 1 Lpo_Date FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO)
                       From  app_user_customer a, delivery_note b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and isnull(b.delivery_status,'S') ='C'
                       and((b.dnote_date >=@datefrom or @datefrom is null) and(b.dnote_date <=@dateto or @dateto is null))
                       ";
                       
            var result = _dataContext.SqlExecutor.Select<Dnote_Deilvery_Details>(sql, userid, datefrom, dateto);
            
            if (site.Count > 0)
            {
                result = result.Where(c => site.Contains(c.Site_No)).ToList();
            }
            
            
            foreach (var data in result)
            {
                Dnote_Deilvery_Details dn = new Dnote_Deilvery_Details();
                dn.Dnote_No = data.Dnote_No;
                dn.Order_Date = data.Order_Date;
                dn.Order_No = data.Order_No;
                dn.Dnote_Date = data.Dnote_Date;
                dn.Site_No = data.Site_No;
                dn.Vehicle_No = data.Vehicle_No;
                dn.Driver_Name = data.Driver_Name;             
                dn.St_Longitude = data.St_Longitude;
                dn.St_Latitude = data.St_Latitude;
                dn.En_Longitude = data.En_Longitude;
                dn.En_Latitude = data.En_Latitude;
                dn.Plate_No = data.Plate_No;
                dn.Status = data.Status;
                dn.Lpo_Date = data.Lpo_Date;
                dn.Lpo_No = data.Lpo_No;
                
                string sqldt = @"Select Product = c.item_description, 
                           Sr_No = c.sr_no, 
                           Item_Code =c.item_code, 
                           Unit =c.unit,
                           Order_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                            FROM SALES_ORDER_DETAIL 
                                            WHERE COMPANY_CODE = b.COMPANY_CODE 
                                            AND ORDER_NO = b.ORDER_NO 
                                            AND ITEM_CODE =c.ITEM_CODE ), 
                           Del_Qty =c.quantity, 
                           Bal_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                       FROM SALES_ORDER_DETAIL 
                                       WHERE COMPANY_CODE = b.COMPANY_CODE 
                                       AND ORDER_NO = b.ORDER_NO 
                                       AND ITEM_CODE =c.ITEM_CODE )  -  c.QUANTITY
                       From  app_user_customer a, delivery_note b, delivery_note_detail c
                       Where a.user_id=@userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code                      
                       and b.dnote_no=c.dnote_no
                       and b.order_no=@orderno
                       and b.dnote_no=@dnoteno
                       and b.site_no =@siteno";
                       
                string orderno = data.Order_No;
                string dnoteno = data.Dnote_No;
                string siteno = data.Site_No;
                
                var rs = _dataContext.SqlExecutor.Select<Dnote_Deilvery_Details_Det>(sqldt, userid, orderno, dnoteno, siteno);
                
                if (product.Count > 0)
                {
                    rs = rs.Where(c => product.Contains(c.Item_Code)).ToList();
                    if (rs.Count <= 0)
                    {
                        continue;
                    }
                }
                List<Dnote_Deilvery_Details_Det> items = new List<Dnote_Deilvery_Details_Det>();
                
                foreach (var i in rs)
                {
                    Dnote_Deilvery_Details_Det it = new Dnote_Deilvery_Details_Det();
                    it.Product = i.Product;
                    it.Sr_No = i.Sr_No;
                    it.Item_Code = i.Item_Code;
                    it.Unit = i.Unit;
                    it.Order_Qty = i.Order_Qty;
                    it.Del_Qty = i.Del_Qty;
                    it.Bal_Qty = i.Bal_Qty;
                    items.Add(it);
                }
                dn.Product = items.ToList();
                dnotes.Add(dn);
            }
            return dnotes.ToList();
        }
        
        public List<Dnote_Upcoming> TrackDelivery(string userid, UserParameter param)
        {
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Dnote_Upcoming> dnotes = new List<Dnote_Upcoming>();
            
            if (dateto.HasValue)
            {
                dateto = new DateTime(dateto.Value.Year, dateto.Value.Month, dateto.Value.Day, 23, 59, 59);
            }
            
            foreach (var i in param.Project)
            {
                if (!String.IsNullOrEmpty(i.Site_No))
                    site.Add(i.Site_No);
            }
            
            foreach (var i in param.Product)
            {
                if (!String.IsNullOrEmpty(i.Item_Code))
                    product.Add(i.Item_Code);
            }
            
            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                             Lpo_No = (SELECT TOP 1 Lpo_No FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Lpo_Date = (SELECT TOP 1 Lpo_Date FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Vehicle_No=b.Vehicle_No,
                            Gps_Id =b.Gps_Id,
                            Driver_Name = b.Driver_Name,
                            Track_Api_User_Name = (SELECT TOP 1 GPS_INTERFACE_ID FROM GPS_INTERFACE),
                            Track_Api_Password =(SELECT TOP 1 GPS_INTERFACE_PWD FROM GPS_INTERFACE),
                            St_Longitude= b.St_Longitude,
                            St_Latitude=b.St_Latitude,
                            En_Longitude=b.En_Longitude,
                            En_Latitude=b.En_Latitude,
                            Plate_No=b.Plate_No,
                            Status =b.Delivery_Status                           
                       From  app_user_customer a, delivery_note b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and isnull(b.delivery_status,'S') IN ('O')
                       and((b.dnote_date >=@datefrom or @datefrom is null) and(b.dnote_date <=@dateto or @dateto is null))
                       ";
                       
            var result = _dataContext.SqlExecutor.Select<Dnote_Upcoming>(sql, userid, datefrom, dateto);
            
            if (site.Count > 0)
            {
                result = result.Where(c => site.Contains(c.Site_No)).ToList();
            }
            
            foreach (var data in result)
            {
                Dnote_Upcoming dn = new Dnote_Upcoming();
                dn.Dnote_No = data.Dnote_No;
                dn.Order_Date = data.Order_Date;
                dn.Order_No = data.Order_No;
                dn.Dnote_Date = data.Dnote_Date;
                dn.Site_No = data.Site_No;
                dn.Vehicle_No = data.Vehicle_No;
                dn.Gps_Id = data.Gps_Id;
                dn.Driver_Name = data.Driver_Name;
                dn.Track_Api_User_Name = data.Track_Api_User_Name;
                dn.Track_Api_Password = data.Track_Api_Password;
                dn.St_Longitude = data.St_Longitude;
                dn.St_Latitude = data.St_Latitude;
                dn.En_Longitude = data.En_Longitude;
                dn.En_Latitude = data.En_Latitude;
                dn.Plate_No = data.Plate_No;
                dn.Status = data.Status;
                dn.Lpo_Date = data.Lpo_Date;
                dn.Lpo_No = data.Lpo_No;
                
                string sqldt = @"Select Product = c.item_description,                           
                           Sr_No = c.sr_no, 
                           Item_Code =c.item_code, 
                           Unit =c.unit,
                           Order_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                            FROM SALES_ORDER_DETAIL 
                                            WHERE COMPANY_CODE = b.COMPANY_CODE 
                                            AND ORDER_NO = b.ORDER_NO 
                                            AND ITEM_CODE =c.ITEM_CODE ), 
                           Del_Qty =c.quantity, 
                           Bal_Qty = (SELECT SUM(ORDER_QUANTITY) 
                                       FROM SALES_ORDER_DETAIL 
                                       WHERE COMPANY_CODE = b.COMPANY_CODE 
                                       AND ORDER_NO = b.ORDER_NO 
                                       AND ITEM_CODE =c.ITEM_CODE )  -  c.QUANTITY
                       From  app_user_customer a, delivery_note b, delivery_note_detail c
                       Where a.user_id=@userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code                      
                       and b.dnote_no=c.dnote_no
                       and b.order_no=@orderno
                       and b.dnote_no=@dnoteno
                       and b.site_no =@siteno";
                       
                string orderno = data.Order_No;
                string dnoteno = data.Dnote_No;
                string siteno = data.Site_No;
                
                var rs = _dataContext.SqlExecutor.Select<Dnote_Upcoming_Det>(sqldt, userid, orderno, dnoteno, siteno);
                
                if (product.Count > 0)
                {
                    rs = rs.Where(c => product.Contains(c.Item_Code)).ToList();
                    if (rs.Count <= 0)
                    {
                        continue;
                    }
                }
                List<Dnote_Upcoming_Det> items = new List<Dnote_Upcoming_Det>();
                
                foreach (var i in rs)
                {
                    Dnote_Upcoming_Det it = new Dnote_Upcoming_Det();
                    it.Product = i.Product;
                    //it.Lpo_No = i.Lpo_No;
                    //it.Lpo_Date = i.Lpo_Date;
                    it.Sr_No = i.Sr_No;
                    it.Item_Code = i.Item_Code;
                    it.Unit = i.Unit;
                    it.Order_Qty = i.Order_Qty;
                    it.Del_Qty = i.Del_Qty;
                    it.Bal_Qty = i.Bal_Qty;
                    items.Add(it);
                }
                dn.Product = items.ToList();
                dnotes.Add(dn);
            }
            return dnotes.ToList();
            
        }
        
        
        
    }
}
