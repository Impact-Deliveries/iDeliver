using IDeliverObjects.Common;
using IDeliverObjects.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class DriverCaseDTO
    {
        public long? DriveCaseID { get; set; }
        public short? Status { get; set; }
        public bool? IsOnline { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? ModifiedDate { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? phone { get; set; }
        public string? name { get; set; }



    }

   
}
