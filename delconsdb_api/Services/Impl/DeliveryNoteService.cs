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

        public List<Dnote_Upcoming> UpcomingDnotes(string userid)
        {
            var ds = new DataStore<Dnote_Upcoming>(_dataContext);

            ds.Retrieve(userid);
            for (int i = 0; i < ds.RowCount; i++)
            {
                string dnote = ds.GetItem<string>(i, "Dnote_No");
                var dt = new DataStore<Dnote_Upcoming_Det>(_dataContext);
                dt.Retrieve(userid, dnote);
                List<Dnote_Upcoming_Det> det = new List<Dnote_Upcoming_Det>();
                det = dt.ToList();
                ds.SetItem(i, "Product", det.ToList());

            }
            return ds.ToList();
        }

        public List<Dnote_Deilvery_Details> DeliveryDetails(string userid)
        {
            var ds = new DataStore<Dnote_Deilvery_Details>(_dataContext);

            ds.Retrieve(userid);
            for (int i = 0; i < ds.RowCount; i++)
            {
                string dnote = ds.GetItem<string>(i, "Dnote_No");
                var dt = new DataStore<Dnote_Deilvery_Details_Det>(_dataContext);
                dt.Retrieve(userid, dnote);
                List<Dnote_Deilvery_Details_Det> det = new List<Dnote_Deilvery_Details_Det>();
                det = dt.ToList();
                ds.SetItem(i, "Product", det.ToList());

            }
            return ds.ToList();
        }

        

    }
}
