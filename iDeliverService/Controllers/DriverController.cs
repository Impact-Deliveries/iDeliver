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

        // GET: api/Drivers
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

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(long id)
        {
            var Driver = await _repository.GetByID(id);

            if (Driver == null)
            {
                return NotFound();
            }

            return Ok(Driver);
        }


        // Post: api/Drivers/UploadDrivers
        [HttpPost("AddOrEditDriver")]
        public async Task<ActionResult<Driver>> AddOrEditDriver([FromBody] DriverDTO obj)
        {
            try
            {
                int DriverID = 0;
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
                if (user.Id != null && user.Id > 0)
                {
                    Enrolment enroll = new Enrolment()
                    {
                        UserId = user.Id,
                        RoleId = (int)Roles.driver
                    };
                    await _Erepository.Add(enroll);
                    if (enroll.Id != null && enroll.Id > 0)
                    {
                        Driver driver = new Driver()
                        {
                            EnrolmentId = enroll.Id,
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
                            IsActive = false,
                            IsHaveProblem = obj.isHaveProblem,
                            Reason = obj.reason,
                        };
                        await _repository.Add(driver);
                        if (driver.Id != null && driver.Id > 0)
                        {
                            DriverID = driver.Id;
                            DriverDetail Detail = new DriverDetail()
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
                            await _Drepository.Add(Detail);
                            if (obj.selecteddays != null && obj.selecteddays.Count > 0)
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
                        }
                    }
                }
                return Ok(DriverID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Drivers/GetDrivers
        [HttpGet, Route("GetDrivers")]
        public async Task<ActionResult<Driver>> GetDrivers([FromQuery]NgTableParam<NgDriverTableFilter> request)
        {
            try
            {

                #region Varaibles
                NgTableResult<Driver> results = new NgTableResult<Driver>();
                int total = 0;
                var page_index = request.page == 0 ? request.page : request.page - 1;
                var page_skips = page_index * request.count;
                NgDriverTableFilter objects = request.objects;
                bool isactive = request.objects!=null?(objects.IsActive == 0 ? false : true):false;
                #endregion

                #region return results
                var drivers = await _repository.GetDrivers(objects, page_skips, request.count);
                results = new NgTableResult<Driver>()
                {
                    results = drivers.Drivers,
                    total = drivers.Total
                };
                return Ok(results);
                #endregion
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
