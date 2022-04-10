using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class AttachmentDTO
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public int ModuleID { get; set; }
        public int CreatorID { get; set; }
        public int AttachmentType { get; set; }
        public int ModuleTypeID { get; set; }
        public int GroupID { get; set; }
    }
}
