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
    public interface ISalesOrderService
    {
        List<order> RetrieveOrders(string userid);
    }
}
