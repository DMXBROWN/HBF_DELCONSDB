using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnapObjects.Data;
using DWNet.Data;
using delconsdb_api.Models;
using delconsdb_api;

namespace delconsdb_api.Services
{
   
    public class SalesOrderService : ISalesOrderService
    {
        private readonly delconsdb_api.DelConsDBDataContext _dataContext;

        public SalesOrderService(delconsdb_api.DelConsDBDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Order> RetrieveOrders(string userid)
        {
            var ds = new DataStore<Order>(_dataContext);
            ds.Retrieve(userid);
           
           for (int i = 0; i < ds.RowCount; i++)
            {
                string orderno = ds.GetItem<string>(i, "Order_No");                
                string customer = ds.GetItem<string>(i, "Customer_Code");
                string siteno = ds.GetItem<string>(i, "Site_No");
                var dt = new DataStore<Order_Detail>(_dataContext);               
                dt.Retrieve(userid, orderno, customer, siteno);
                ds.SetItem(i, "Product", dt.ToList());               
            }
            return ds.ToList();
        }

        
        public List<Order_Enq> RetrieveOrderEnquiry(string userid)
        {
            var ds = new DataStore<Order_Enq>(_dataContext);
            ds.Retrieve(userid);
            for (int i = 0; i < ds.RowCount; i++)
            {
                string orderno = ds.GetItem<string>(i, "Order_No");
                string customer = ds.GetItem<string>(i, "Customer_Code");
                string siteno = ds.GetItem<string>(i, "Site_No");
                var dt = new DataStore<Order_Enq_Detail>(_dataContext);
                dt.Retrieve(userid, orderno, customer, siteno);                
                for (int j = 0; j < dt.RowCount; j++)
                {
                    string itemcode = dt.GetItem<string>(j, "Item_Code");
                    var dl = new DataStore<Order_Enq_Del_Detail>(_dataContext);
                    List<Order_Enq_Del_Detail> delivery = new List<Order_Enq_Del_Detail>();
                    dl.Retrieve(userid, orderno, customer, siteno, itemcode);
                    delivery = dl.ToList();                    
                    dt.SetItem(j,"Delivery_Details", delivery);
                }
                ds.SetItem(i, "Product", dt.ToList());

            }
            return ds.ToList();
        }
    }
}