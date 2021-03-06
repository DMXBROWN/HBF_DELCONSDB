using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;
using delconsdb_api.Models.User;

namespace delconsdb_api.Services
{
    
    public interface IDeliveryNoteService
    {
        List<Delivery_Note> RetrieveDnotes(string userid);
        List<Dnote_Upcoming> UpcomingDnotes(string userid, UserParameter param);
        List<Dnote_Deilvery_Details> DeliveryDetails(string userid, UserParameter param);
        List<Dnote_Upcoming> TrackDelivery(string userid, UserParameter param);
    }
}
