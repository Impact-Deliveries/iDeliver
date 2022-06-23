using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Driver")]
    [ApiController]
    [Authorize]
    public class DriverController : ControllerBase
    {
        private readonly IDriverRepository _repository;
        private readonly IDriverDetailsRepository _Drepository;
        private readonly IDriverSchaduleRepository _Srepository;
        private readonly IEnrolmentRepository _Erepository;
        private readonly IUserRepository _Urepository;

        public DriverController(IDriverRepository repository, IDriverDetailsRepository Drepository,
            IDriverSchaduleRepository Srepository, IEnrolmentRepository Erepository, IUserRepository Urepository)
        {
            _repository = repository;
            _Drepository = Drepository;
            _Srepository = Srepository;
            _Erepository = Erepository;
            _Urepository = Urepository;
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

        [HttpPost("AddDriver")]
        public async Task<ActionResult<Driver>> AddDriver([FromBody] DriverDTO obj)
        {
            try
            {
                // bool isExistUsername = _Urepository.IsExists(w => w.Username.ToLower() == obj.username);
                // if (isExistUsername) return BadRequest("username is exist");

                // bool isExistNationalNumber = _repository.IsExists(w => w.NationalNumber == obj.nationalNumber);
                // if (isExistNationalNumber) return BadRequest("national number is exist");

                long DriverID = 0;
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
                    Old_driver.Phone = obj.phone;
                    Old_driver.NationalNumber = obj.nationalNumber;
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
                        old_details.AvancedStudies = obj.advancedStudies;
                        old_details.CreationDate = DateTime.UtcNow;
                        old_details.IsDeleted = false;
                        old_details.DeliveryPercent = obj.DeliveryPercent;
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
                        Phone = obj.phone,
                        NationalNumber = obj.nationalNumber,
                        Birthday = obj.birthday,
                        SocialStatus = obj.SocialStatus,
                        CreationDate = DateTime.UtcNow,
                        IsDeleted = false,
                        IsActive = obj.IsActive,
                        IsHaveProblem = obj.isHaveProblem,
                        Reason = (obj.isHaveProblem != null && obj.isHaveProblem == true) ? obj.reason : "",
                    };
                    #region User
                    Common.HashKey.CreateMD5Hash("1234", out string hash);
                    var hashed_password = hash;
                    var count = await _Urepository.GetuserCount();
                    User user = new User()
                    {
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        IsActive = true,
                        Username = obj.username,
                        Password = hashed_password,
                        ReferenceNumber = DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + (count + 1).ToString()
                    };
                    await _Urepository.Add(user);
                    #endregion
                    #region Enrolment
                    if (user.Id != null && user.Id > 0)
                    {
                        Enrolment enroll = new Enrolment()
                        {
                            UserId = user.Id,
                            RoleId = (int)IDeliverObjects.Enum.Roles.driver
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
                                    AvancedStudies = obj.advancedStudies,
                                    CreationDate = DateTime.UtcNow,
                                    IsDeleted = false,
                                    DeliveryPercent = obj.DeliveryPercent
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


        //[HttpPost("AddDriver")]
        //public async Task<ActionResult<Driver>> AddDriver([FromBody] DriverDTO obj)
        //{
        //    try
        //    {
        //        // validation
        //        bool isExistUsername = _Urepository.IsExists(w => w.Username.ToLower() == obj.username);
        //        if (isExistUsername) return BadRequest("username is exist");

        //        bool isExistNationalNumber = _repository.IsExists(w => w.NationalNumber == obj.nationalNumber);
        //        if (isExistNationalNumber) return BadRequest("national number is exist");

        //        // Add user
        //        Common.HashKey.CreateMD5Hash("1234", out string hash);
        //        var hashed_password = hash;
        //        var count = await _Urepository.GetuserCount();
        //        User user = new User()
        //        {
        //            CreationDate = DateTime.UtcNow,
        //            ModifiedDate = DateTime.UtcNow,
        //            IsActive = true,
        //            Username = obj.username,
        //            Password = hashed_password,
        //            ReferenceNumber = DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + (count + 1).ToString()
        //        };
        //        await _Urepository.Add(user);

        //        // Add enrollment
        //        Enrolment enroll = new Enrolment()
        //        {
        //            UserId = user.Id,
        //            RoleId = (int)Roles.driver
        //        };
        //        await _Erepository.Add(enroll);

        //        // Add driver
        //        Driver driver = new Driver()
        //        {
        //            EnrolmentId = enroll.Id,
        //            FirstName = obj.firstname,
        //            SecondName = obj.middlename,
        //            LastName = obj.lastname,
        //            Address = obj.address,
        //            Mobile = obj.mobile,
        //            Phone = obj.phone,
        //            Birthday = obj.birthday,
        //            SocialStatus = obj.SocialStatus,
        //            ModifiedDate = DateTime.UtcNow,
        //            CreationDate = DateTime.UtcNow,
        //            IsDeleted = false,
        //            IsActive = obj.IsActive,
        //            IsHaveProblem = obj.isHaveProblem,
        //            Reason = obj.reason,
        //            OrganizationId = obj.OrganizationID,
        //            NationalNumber = obj.nationalNumber,
        //            IsOnline = false
        //        };
        //        await _repository.Add(driver);

        //        // Add driver detail
        //        DriverDetail Detail = new DriverDetail()
        //        {
        //            DriverId = driver.Id,
        //            JobTime = obj.WorkTime,
        //            FromTime = obj.fromTime,
        //            ToTime = obj.toTime,
        //            StartJob = obj.startJob,
        //            College = obj.college,
        //            University = obj.university,
        //            Major = obj.major,
        //            GraduationYear = obj.graduationyear,
        //            Estimate = obj.estimate,
        //            AvancedStudies = obj.advancedStudies,
        //            CreationDate = DateTime.UtcNow,
        //            ModifiedDate = DateTime.UtcNow,
        //            IsDeleted = false,
        //        };
        //        await _Drepository.Add(Detail);

        //        // Add driver work schedule
        //        if (obj.selecteddays is not null && obj.selecteddays.Count > 0 && obj.WorkTime == 2)
        //        {
        //            foreach (var item in obj.selecteddays)
        //            {
        //                DriverSchadule schadule = new DriverSchadule()
        //                {
        //                    CreationDate = DateTime.UtcNow,
        //                    DriverId = driver.Id,
        //                    IsDeleted = false,
        //                    DayId = item
        //                };
        //                await _Srepository.Add(schadule);
        //            }
        //        }

        //        return Ok(driver.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost("ChangeDriverStatus")]
        public async Task<ActionResult<Driver>> ChangeDriverStatus([FromQuery] long DriverID)
        {
            try
            {
                var Driver = await _repository.GetByID(DriverID);

                if (Driver == null)
                {
                    return NotFound();
                }
                Driver.IsActive = !Driver.IsActive;
                await _repository.Update(Driver);
                return Ok(Driver);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet, Route("GetDrivers")]
        public async Task<ActionResult<Driver>> GetDrivers([FromQuery] bool? IsActive, string? DriverName)
        {
            try
            {
                var drivers = await _repository.GetAllDrivers(IsActive, DriverName);
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


        [HttpPost, Route("DeleteDriver")]
        public async Task<ActionResult> DeleteDriver([FromBody] long DriverID)
        {

            try
            {
                Driver driver = await _repository.GetByID(DriverID);
                if (driver == null) return BadRequest("Driver Not Found");

                driver.IsDeleted = true;
                await _repository.Update(driver);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPost, Route("ActiveDriver")]
        public async Task<ActionResult> ActiveDriver([FromBody] long DriverID)
        {

            try
            {
                Driver driver = await _repository.GetByID(DriverID);
                if (driver == null) return BadRequest("Driver Not Found");

                driver.IsActive = !driver.IsActive;
                await _repository.Update(driver);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
