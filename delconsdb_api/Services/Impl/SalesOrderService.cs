using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnapObjects.Data;
using DWNet.Data;
using delconsdb_api.Models;
using delconsdb_api;
using delconsdb_api.Models.User;

namespace delconsdb_api.Services
{
   
    public class SalesOrderService : ISalesOrderService
    {
        private readonly delconsdb_api.DelConsDBDataContext _dataContext;
        
        public SalesOrderService(delconsdb_api.DelConsDBDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public List<Order>  RetrieveOrders(string userid, UserParameter param)
        {
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Order> orders = new List<Order>();
            
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
                             
            string sql = @"Select Order_No = b.order_no, Order_Date = b.order_date, lpo_no = b.lpo_no, lpo_date = b.lpo_date,
                           Site_No = b.site_no,Customer_Code = b.customer_code
                       From  app_user_customer a, sales_order b
                       Where a.user_id=@userid    
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.order_no in (select order_no from delivery_note where company_code = b.company_code  
                       and customer_code = b.customer_code and ((dnote_date >= @datefrom) and (dnote_date <= @dateto)))
                       order by b.order_no
                       ";
            if (!datefrom.HasValue && !dateto.HasValue)
            {
                datefrom = DateTime.Today;
                datefrom = new DateTime(datefrom.Value.Year, datefrom.Value.Month, datefrom.Value.Day, 0, 0, 0);
                sql = @"Select distinct Order_No = b.order_no, Order_Date = b.order_date, Lpo_No =b.Lpo_No, Lpo_Date = b.Lpo_Date,
                           Site_No = b.site_no,Customer_Code = b.customer_code
                       From  app_user_customer a, sales_order b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.order_no in (select order_no from delivery_note where company_code = b.company_code  
                       and customer_code = b.customer_code and ((dnote_date >= @datefrom)))
                       order by b.order_no
                       ";
            }
                       
            var result =  _dataContext.SqlExecutor.Select<Order>(sql, userid, datefrom, dateto);
            
            if (site.Count > 0)
            {
                result = result.Where(c => site.Contains(c.Site_No)).ToList();
            }
            
            foreach (var data in result)
            {
                Order s = new Order();
                s.Order_No = data.Order_No;
                s.Order_Date = data.Order_Date;
                s.Site_No = data.Site_No;
                s.Lpo_No = data.Lpo_No;
                s.Lpo_Date = data.Lpo_Date;
                s.Customer_Code = data.Customer_Code;
                
                string sqldt = @"Select Product = c.item_description,                          
                           Sr_No = c.sr_no, 
                           Item_Code =c.item_code, 
                           Unit =c.unit,
                           Order_Quantity = c.order_quantity, 
                           Del_Quantity =0.00, 
                           Bal_Qty = 0.00
                       From  app_user_customer a, sales_order b, sales_order_detail c 
                       Where a.user_id=@userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code                      
                       and b.order_no=c.order_no
                       and b.order_no=@orderno
                       and b.customer_code=@customer
                       and b.site_no =@site_no";
                       
                       
                string orderno = data.Order_No;
                string customer = data.Customer_Code;
                string siteno = data.Site_No;
               
                var rs = _dataContext.SqlExecutor.Select<Order_Detail>(sqldt, userid, orderno, customer, siteno);
                    
                if (product.Count > 0)
                {
                    rs = rs.Where(c => product.Contains(c.Item_Code) ).ToList();
                    if (rs.Count <= 0) 
                    {
                        continue;
                    }
                }
                List<Order_Detail> items = new List<Order_Detail>();
                foreach (var i in rs) 
                {
                    Order_Detail it = new Order_Detail();
                    decimal? ld_del_qty = 0;
                    string itemcode = i.Item_Code;
                    var sql_qty = @"select quantity =sum(delivery_note_detail.quantity) from delivery_note, delivery_note_detail
                                where delivery_note.company_code = delivery_note_detail.company_code
                                and delivery_note.dnote_no = delivery_note_detail.dnote_no
                                and delivery_note.order_no =:order_no
                                and delivery_note_detail.item_code =:itemcode
                                and (delivery_note.dnote_date>=:datefrom or :datefrom is null)
                                and (delivery_note.dnote_date<=:dateto or :dateto is null)";
                                
                    var res_sql_qty = _dataContext.SqlExecutor.Select<DynamicModel>(sql_qty, orderno, itemcode, datefrom, dateto);
                    if (res_sql_qty.Count == 1)
                    {
                        var ddata = res_sql_qty[0];
                        ld_del_qty = ddata.GetValue<decimal?>("quantity");
                        if(ld_del_qty == null) ld_del_qty = 0;
                    }
                    it.Product = i.Product;               
                    it.Sr_No = i.Sr_No;
                    it.Item_Code = i.Item_Code;
                    it.Unit = i.Unit;
                    it.Order_Quantity = i.Order_Quantity;
                    it.Del_Quantity = ld_del_qty;
                    it.Bal_Qty = i.Order_Quantity - ld_del_qty;
                    items.Add(it);
                }
                
                s.Product = items.ToList();
                orders.Add(s);
            }                       
            
            return orders.ToList();
            
        }
        
        
        public List<Order_Enq> RetrieveOrderEnquiry(string userid, UserParameter param)
        {
            DateTime? datefrom = param.Datefrom;
            DateTime? dateto = param.Dateto;
            List<string> site = new List<string>();
            List<string> product = new List<string>();
            List<Order_Enq> orders = new List<Order_Enq>();
            
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
            
            string sql = @"Select distinct Order_No = b.order_no, Order_Date = b.order_date, Lpo_No =b.Lpo_No, Lpo_Date = b.Lpo_Date,
                           Site_No = b.site_no,Customer_Code = b.customer_code
                       From  app_user_customer a, sales_order b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.order_no in (select order_no from delivery_note where company_code = b.company_code  
                       and customer_code = b.customer_code and ((dnote_date >= @datefrom) and (dnote_date <= @dateto )))
                       order by b.order_no
                       ";
                       
            if (!datefrom.HasValue && !dateto.HasValue)
            {
                datefrom = DateTime.Today;
                datefrom = new DateTime(datefrom.Value.Year, datefrom.Value.Month, datefrom.Value.Day, 0, 0, 0);
                sql = @"Select distinct Order_No = b.order_no, Order_Date = b.order_date, Lpo_No =b.Lpo_No, Lpo_Date = b.Lpo_Date,
                           Site_No = b.site_no,Customer_Code = b.customer_code
                       From  app_user_customer a, sales_order b
                       Where a.user_id=@userid   
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.order_no in (select order_no from delivery_note where company_code = b.company_code  
                       and customer_code = b.customer_code and ((dnote_date >= @datefrom)))
                       order by b.order_no
                       ";     
            }
         
            var result = _dataContext.SqlExecutor.Select<Order_Enq>(sql, userid, datefrom, dateto);
            
            if (site.Count > 0)
            {
                result = result.Where(c => site.Contains(c.Site_No)).ToList();
            }
            
            foreach (var data in result)
            {
                Order_Enq s = new Order_Enq();
                s.Order_No = data.Order_No;
                s.Order_Date = data.Order_Date;
                s.Site_No = data.Site_No;
                s.Customer_Code = data.Customer_Code;
                s.Lpo_Date = data.Lpo_Date;
                s.Lpo_No = data.Lpo_No;
                
                string sqldt = @"Select Product = c.item_description, 
                           Item_Code =c.item_code, 
                           Unit =c.unit,
                           Order_Quantity = c.order_quantity, 
                           Del_Quantity =c.del_quantity, 
                           Bal_Qty = isnull(c.order_quantity,0) - isnull(c.del_quantity,0)
                       From  app_user_customer a, sales_order b, sales_order_detail c
                       Where a.user_id=@userid
                       and a.customer_code = b.customer_code
                       and a.site_no = b.site_no
                       and b.company_code = c.company_code                      
                       and b.order_no=c.order_no
                       and b.order_no=@orderno
                       and b.customer_code=@customer
                       and b.site_no =@site_no";
                       
                string orderno = data.Order_No;
                string customer = data.Customer_Code;
                string siteno = data.Site_No;
                
                var rs = _dataContext.SqlExecutor.Select<Order_Enq_Detail>(sqldt, userid, orderno, customer, siteno);
                
                if (product.Count > 0)
                {
                    rs = rs.Where(c => product.Contains(c.Item_Code)).ToList();
                    if (rs.Count <= 0)
                    {
                        continue;
                    }
                }
                
                List<Order_Enq_Detail> items = new List<Order_Enq_Detail>();
                
                foreach (var i in rs)
                {
                    Order_Enq_Detail it = new Order_Enq_Detail();
                    decimal? ld_del_qty = 0;
                    string itemcode = i.Item_Code;
                    var sql_qty = @"select quantity =sum(delivery_note_detail.quantity) from delivery_note, delivery_note_detail
                                where delivery_note.company_code = delivery_note_detail.company_code
                                and delivery_note.dnote_no = delivery_note_detail.dnote_no
                                and delivery_note.order_no =:order_no
                                and delivery_note_detail.item_code =:itemcode
                                and (delivery_note.dnote_date>=:datefrom or :datefrom is null)
                                and (delivery_note.dnote_date<=:dateto or :dateto is null)";
                                
                    var res_sql_qty = _dataContext.SqlExecutor.Select<DynamicModel>(sql_qty, orderno, itemcode, datefrom, dateto);
                    if (res_sql_qty.Count == 1)
                    {
                        var ddata = res_sql_qty[0];
                        ld_del_qty = ddata.GetValue<decimal?>("quantity");
                        if (ld_del_qty == null) ld_del_qty = 0;
                    }
                    it.Product = i.Product;                 
                    it.Item_Code = i.Item_Code;
                    it.Unit = i.Unit;
                    it.Order_Quantity = i.Order_Quantity;                    
                    it.Del_Quantity = ld_del_qty;
                    it.Bal_Qty = i.Order_Quantity - ld_del_qty;
                    var dl = new DataStore<Pend_Del_Detail>(_dataContext);                    
                    dl.Retrieve(userid, orderno, customer, siteno, i.Item_Code);
                    it.Delivery_Details = dl.ToList();
                    items.Add(it);
                }
                
                s.Product = items.ToList();
                orders.Add(s);
            }
            
            return orders.ToList();
           
        }
    }
}