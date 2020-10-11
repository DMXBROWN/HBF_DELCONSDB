using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace delconsdb_api.Models.User
{
    public class UserParameter
    {
        public DateTime? Datefrom { get; set; }
        public DateTime? Dateto { get; set; }
        public List<UserProject> Project { get; set; }
        public List<UserProduct> Product { get; set; }
    }
}

