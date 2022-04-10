using IDeliverObjects.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class DriverDTO
    {
        public string? firstname { get; set; }
        public string? middlename { get; set; }
        public string? lastname { get; set; }
        public string? address { get; set; }
        public Nullable<int> mobile2 { get; set; }
        public int mobile { get; set; }
        public DateTime birthday { get; set; }
        public int SocialStatus { get; set; }
        public bool isHaveProblem { get; set; }
        public string? reason { get; set; }
        public int WorkTime { get; set; }
        public DateTime fromTime { get; set; }
        public DateTime toTime { get; set; }
        public DateTime startJob { get; set; }
        public string? college{ get; set; }
        public string? university{ get; set; }
        public string? major{ get; set; }
        public string? graduationyear{ get; set; }
        public string? estimate{ get; set; }
        public string? avancedstudies{ get; set; }
        public List<int>? selecteddays { get; set; }
    }

    public class DriverTableDTO {
        public List<Driver> Drivers { get; set; }
        public int Total { get; set; }
    }
}
