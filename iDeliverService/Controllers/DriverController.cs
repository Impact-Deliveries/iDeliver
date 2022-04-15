using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _repository;
        private readonly IDriverDetailsRepository _Drepository;
        private readonly IDriverSchaduleRepository _Srepository;
        private readonly IEnrolmentRepository _Erepository;
        private readonly IUserRepository _Urepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public DriverController(IDriverRepository repository, IDriverDetailsRepository Drepository,
            IDriverSchaduleRepository Srepository, IEnrolmentRepository Erepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IUserRepository Urepository)
        {
            _repository = repository;
            _Drepository = Drepository;
            _Srepository = Srepository;
            _Erepository = Erepository;
            _Urepository = Urepository;
            _env = env;
        }

        // GET: api/Driver
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetAllDrivers()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }
        [HttpGet("GetActiveOrNotDrivers")]
        public async Task<ActionResult<IEnumerable<Driver>>> GetActiveDrivers(bool IsActive = true)
        {
            var result = await _repository.Find(a => a.IsActive == IsActive);
            return Ok(result);
        }

        // GET: api/Driver/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(long id)
        {
            var Driver = await _repository.GetDriverById(id);

            if (Driver == null)
            {
                return NotFound();
            }
            return Ok(Driver);
        }
        // Post: api/Driver/UploadDrivers
        [HttpPost("AddOrEditDriver")]
        public async Task<ActionResult<Driver>> AddOrEditDriver([FromBody] DriverDTO obj)
        {
            try
            {
                int DriverID = 0;
                Driver driver = new Driver();
                DriverDetail Detail = new DriverDetail();
                var Old_driver = obj.DriverID != null ? await _repository.GetByID(obj.DriverID.Value) : null;
                if (Old_driver != null && Old_driver.Id > 0)
                {
                    DriverID = Old_driver.Id;
                    Old_driver.FirstName = obj.firstname;
                    Old_driver.SecondName = obj.middlename;
                    Old_driver.LastName = obj.lastname;
                    Old_driver.Address = obj.address;
                    Old_driver.Mobile = obj.mobile;
                    Old_driver.Mobile2 = obj.mobile2;
                    Old_driver.Birthday = obj.birthday;
                    Old_driver.SocialStatus = obj.SocialStatus;
                    Old_driver.CreationDate = DateTime.UtcNow;
                    Old_driver.IsActive = obj.IsActive;
                    Old_driver.IsHaveProblem = obj.isHaveProblem;
                    Old_driver.Reason = obj.isHaveProblem != null && obj.isHaveProblem == true ? obj.reason : "";

                    await _repository.Update(Old_driver);
                    DriverDetail old_details = await _Drepository.GetByDeiverID(DriverID);
                    if (old_details != null)
                    {
                        Detail.DriverId = Old_driver.Id;
                        //old_details.DriverId = driver.Id;
                        old_details.JobTime = obj.WorkTime;
                        old_details.FromTime = obj.fromTime;
                        old_details.ToTime = obj.toTime;
                        old_details.StartJob = obj.startJob;
                        old_details.College = obj.college;
                        old_details.University = obj.university;
                        old_details.Major = obj.major;
                        old_details.GraduationYear = obj.graduationyear;
                        old_details.Estimate = obj.estimate;
                        old_details.AvancedStudies = obj.avancedstudies;
                        old_details.CreationDate = DateTime.UtcNow;
                        old_details.IsDeleted = false;
                        await _Drepository.Update(old_details);
                    }
                }
                else
                {
                    driver = new Driver()
                    {
                        //  EnrolmentId = enroll.Id,
                        FirstName = obj.firstname,
                        SecondName = obj.middlename,
                        LastName = obj.lastname,
                        Address = obj.address,
                        Mobile = obj.mobile,
                        Mobile2 = obj.mobile2,
                        Birthday = obj.birthday,
                        SocialStatus = obj.SocialStatus,
                        CreationDate = DateTime.UtcNow,
                        IsDeleted = false,
                        IsActive = obj.IsActive,
                        IsHaveProblem = obj.isHaveProblem,
                        Reason = obj.isHaveProblem != null && obj.isHaveProblem == true ? obj.reason : "",
                    };
                    #region User
                    Common.HashKey.CreateMD5Hash("1234", out string hash);
                    var hashed_password = hash;
                    User user = new User()
                    {
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        IsActive = true,
                        Username = obj.mobile.ToString(),
                        Password = hashed_password,
                        ReferenceNumber = ""
                    };
                    await _Urepository.Add(user);
                    #endregion
                    #region Enrolment
                    if (user.Id != null && user.Id > 0)
                    {
                        Enrolment enroll = new Enrolment()
                        {
                            UserId = user.Id,
                            RoleId = (int)Roles.driver
                        }; 
                        await _Erepository.Add(enroll);
                        driver.EnrolmentId = enroll.Id;
                        if (enroll.Id != null && enroll.Id > 0)
                        {

                            await _repository.Add(driver);
                            if (driver.Id != null && driver.Id > 0)
                            {
                                Detail = new DriverDetail()
                                {
                                    DriverId = driver.Id,
                                    JobTime = obj.WorkTime,
                                    FromTime = obj.fromTime,
                                    ToTime = obj.toTime,
                                    StartJob = obj.startJob,
                                    College = obj.college,
                                    University = obj.university,
                                    Major = obj.major,
                                    GraduationYear = obj.graduationyear,
                                    Estimate = obj.estimate,
                                    AvancedStudies = obj.avancedstudies,
                                    CreationDate = DateTime.UtcNow,
                                    IsDeleted = false,

                                };
                                DriverID = driver.Id;
                                await _Drepository.Add(Detail);

                            }
                        }
                    }
                    #endregion
                }
                #region DriverSchadule
                List<DriverSchadule> selected = await _Srepository.GetByDriverID(DriverID);
                if (selected != null && selected.Count > 0)
                {
                    await _Srepository.DeleteScheduleByDriverID(selected);
                }
                if (obj.selecteddays != null && obj.selecteddays.Count > 0 && obj.WorkTime == 2)
                {
                    foreach (var item in obj.selecteddays)
                    {
                        DriverSchadule schadule = new DriverSchadule()
                        {
                            CreationDate = DateTime.UtcNow,
                            DriverId = driver.Id,
                            IsDeleted = false,
                            DayId = item
                        };
                        await _Srepository.Add(schadule);
                    }
                }
                #endregion

                return Ok(DriverID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Driver/GetDrivers
        //[HttpGet, Route("GetDrivers")]
        //public async Task<ActionResult<Driver>> GetDrivers([FromQuery] NgTableParam<NgDriverTableFilter> request)
        //{
        //    try
        //    {

        //        #region Varaibles
        //        NgTableResult<Driver> results = new NgTableResult<Driver>();
        //        int total = 0;
        //        var page_index = request.page == 0 ? request.page : request.page - 1;
        //        var page_skips = page_index * request.count;
        //        NgDriverTableFilter objects = request.objects;
        //        bool isactive = request.objects != null ? (objects.IsActive == 0 ? false : true) : false;
        //        #endregion

        //        #region return results
        //        var drivers = await _repository.GetDrivers(objects, page_skips, request.count);
        //        results = new NgTableResult<Driver>()
        //        {
        //            results = drivers.Drivers,
        //            total = drivers.Total
        //        };
        //        return Ok(results);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet, Route("GetDrivers")]
        public async Task<ActionResult<Driver>> GetDrivers([FromQuery] bool? IsActive ,string? DriverName)
        {
            try
            { 
                var drivers = await _repository.GetAllDrivers(IsActive,  DriverName);
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool DriverExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
