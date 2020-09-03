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

            foreach (var i in param.Project)
            {
                site.Add(i.Site_No);
            }

            foreach (var i in param.Product)
            {
                product.Add(i.Item_Code);
            }

            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Customer_Code = b.customer_code,
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
                       and isnull(b.delivery_status,'S') IN ('S','O')
                       and((b.dnote_date >=@datefrom or @datefrom is null) and(b.dnote_date <=@dateto or @dateto is null)                                               )
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

                var dt = new DataStore<Dnote_Upcoming_Det>(_dataContext);
                dt.Retrieve(userid, data.Dnote_No);
                List<Dnote_Upcoming_Det> det = new List<Dnote_Upcoming_Det>();
                det = dt.ToList();
                if (product.Count > 0)
                {
                    
                    if (det.Count <= 0)
                    {
                        continue;
                    }
                }
                dn.Product = det.ToList();
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

            foreach (var i in param.Project)
            {
                site.Add(i.Site_No);
            }

            foreach (var i in param.Product)
            {
                product.Add(i.Item_Code);
            }

            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Customer_Code = b.customer_code,
                            Vehicle_No=b.Vehicle_No,
                            Driver_Name = b.Driver_Name,                            
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
                       and isnull(b.delivery_status,'S') ='C'
                       and((b.dnote_date >=@datefrom or @datefrom is null) and(b.dnote_date <=@dateto or @dateto is null)                                               )
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

                var dt = new DataStore<Dnote_Deilvery_Details_Det>(_dataContext);
                dt.Retrieve(userid, data.Dnote_No);
                List<Dnote_Deilvery_Details_Det> det = new List<Dnote_Deilvery_Details_Det>();
                det = dt.ToList();
                if (product.Count > 0)
                {

                    if (det.Count <= 0)
                    {
                        continue;
                    }
                }
                dn.Product = det.ToList();
                dnotes.Add(dn);
            }
            return dnotes.ToList();



            //var ds = new DataStore<Dnote_Deilvery_Details>(_dataContext);

            //ds.Retrieve(userid);
            //for (int i = 0; i < ds.RowCount; i++)
            //{
            //    string dnote = ds.GetItem<string>(i, "Dnote_No");
            //    var dt = new DataStore<Dnote_Deilvery_Details_Det>(_dataContext);
            //    dt.Retrieve(userid, dnote);
            //    List<Dnote_Deilvery_Details_Det> det = new List<Dnote_Deilvery_Details_Det>();
            //    det = dt.ToList();
            //    ds.SetItem(i, "Product", det.ToList());

            //}
            //return ds.ToList();
        }

        public List<Dnote_Upcoming> TrackDelivery(string userid, UserParameter param)
        {
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Dnote_Upcoming> dnotes = new List<Dnote_Upcoming>();

            foreach (var i in param.Project)
            {
                site.Add(i.Site_No);
            }

            foreach (var i in param.Product)
            {
                product.Add(i.Item_Code);
            }

            string sql = @"Select distinct Order_No = b.order_no, 
                            Order_Date = (SELECT TOP 1 ORDER_DATE FROM SALES_ORDER WHERE COMPANY_CODE = b.COMPANY_CODE
                                          AND ORDER_NO = b.ORDER_NO),
                            Dnote_No = b.dnote_no,
                            Dnote_Date=b.dnote_date,                            
                            Site_No = b.site_no,
                            Customer_Code = b.customer_code,
                            Vehicle_No=b.Vehicle_No,
                            Driver_Name = b.Driver_Name,                            
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
                       and isnull(b.delivery_status,'S') ='C'
                       and((b.dnote_date >=@datefrom or @datefrom is null) and(b.dnote_date <=@dateto or @dateto is null)                                               )
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

                var dt = new DataStore<Dnote_Upcoming_Det>(_dataContext);
                dt.Retrieve(userid, data.Dnote_No);
                List<Dnote_Upcoming_Det> det = new List<Dnote_Upcoming_Det>();
                det = dt.ToList();
                dn.Product = det.ToList();
                dnotes.Add(dn);
            }
            return dnotes.ToList();
            //var ds = new DataStore<Dnote_Track_Delivery>(_dataContext);

            //ds.Retrieve(userid);
            //for (int i = 0; i < ds.RowCount; i++)
            //{
            //    string dnote = ds.GetItem<string>(i, "Dnote_No");
            //    var dt = new DataStore<Dnote_Upcoming_Det>(_dataContext);
            //    dt.Retrieve(userid, dnote);
            //    List<Dnote_Upcoming_Det> det = new List<Dnote_Upcoming_Det>();
            //    det = dt.ToList();
            //    ds.SetItem(i, "Product", det.ToList());

            //}
            //return ds.ToList();
        }



    }
}
