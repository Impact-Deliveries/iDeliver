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
        public long EnrolmentID { get; set; }
        public long DriverID { get; set; }
        public long? DriveCaseID { get; set; }
        public short Status { get; set; } = 0;
        public bool? IsOnline { get; set; }
        public string? Latitude { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? ModifiedDate { get; set; }
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? CreationDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? phone { get; set; } = string.Empty;
        public string? name { get; set; } = string.Empty;
    }
}
