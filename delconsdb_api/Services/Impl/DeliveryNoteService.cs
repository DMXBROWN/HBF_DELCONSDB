using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;
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
    }
}
