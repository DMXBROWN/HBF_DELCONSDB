using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using delconsdb_api.Models;

namespace delconsdb_api.Services
{
    
    public interface IDeliveryNoteService
    {
        List<Delivery_Note>RetrieveDnotes(string userid);
    }
}
