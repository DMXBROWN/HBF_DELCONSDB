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

        public List<order> RetrieveOrders(string userid)
        {
            var ds = new DataStore<order>(_dataContext);
            ds.Retrieve(userid);
            
            //var orders= ds.AsEnumerable<order>().ToList();

            //var orders = ds.AsEnumerable<order>().Where(s => s.Sales_Order_Customer_Code == customer).ToList();
            return ds.ToList();
          }
    }
}