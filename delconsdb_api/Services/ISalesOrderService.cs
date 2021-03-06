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
    public interface ISalesOrderService
    {
        List<Order> RetrieveOrders(string userid, UserParameter param);
        List<Order_Enq> RetrieveOrderEnquiry(string userid, UserParameter param);
    }
}
