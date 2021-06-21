using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPlusSportsAPI.Services
{
    public class CosmosDBServiceOptions
    {
        public string DBUri { get; set; }
        public string DBKey { get; set; }
        public string DBName { get; set; }

        public string DBCollection { get; set; }
    }
}
