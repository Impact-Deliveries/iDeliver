using IDeliverObjects.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class NgOrderTable
    {
        public int? merchantID { get; set; }
        public int? status { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? fromdate { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? toDate { get; set; }
        public int? driverID { get; set; }
        public int? merchantBranchID { get; set; }


    }
}
