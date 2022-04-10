using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDeliverObjects.DTO
{
    public class NgTableParam<T> where T : class
    {
        public int page { get; set; }
        public int count { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public T? objects { get; set; }
        public T? filter { get; set; }
        public T? sorting { get; set; }
    }

    public class NgTableResult<T> where T : class
    {
        public int total { get; set; }

        public List<T> results { get; set; }
    }

    public class NgTableFilter
    {
        public int StudentID { get; set; }
        public int StudentRegistrationID { get; set; }
        public string FromDate { get; set; }
        public string HomeworkTitle { get; set; }
        public string ToDate { get; set; }
        public int SchoolDivisionGradeSubjectID { get; set; }
        public int StatusID { get; set; }
        public int HomeworkTypeID { get; set; }
        public int SemesterID { get; set; }

    }

    #region Driver
    public class NgDriverTableFilter
    {
        public int DriverID { get; set; }
        public int IsActive { get; set; }
        public string DriverName { get; set; }
        public int Mobile { get; set; }
    }
    #endregion

}
