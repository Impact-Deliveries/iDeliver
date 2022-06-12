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
    public class DriverDTO
    {
        public long? DriverID { get; set; }
        public long OrganizationID { get; set; }
        public string username { get; set; } = string.Empty;
        public string nationalNumber { get; set; } = string.Empty;
        public string? firstname { get; set; }
        public string? middlename { get; set; }
        public string? lastname { get; set; }
        public string? address { get; set; }
        public bool? IsActive { get; set; }
        public string? phone { get; set; }
        public string mobile { get; set; } = string.Empty;
        
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime birthday { get; set; }

        public short? SocialStatus { get; set; }
        public bool isHaveProblem { get; set; }
        public string? reason { get; set; }
        public int WorkTime { get; set; }
        public DateTime fromTime { get; set; }
        public DateTime toTime { get; set; }

        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime startJob { get; set; }
        public string? college { get; set; }
        public string? university { get; set; }
        public string? major { get; set; }
        public string? graduationyear { get; set; }
        public string? estimate { get; set; }
        public string? advancedStudies { get; set; }
        public List<int>? selecteddays { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public decimal? DeliveryPercent { get; set; }
    }

    public class DriverTableDTO
    {
        public List<Driver> Drivers { get; set; }
        public int Total { get; set; }
    }
}
