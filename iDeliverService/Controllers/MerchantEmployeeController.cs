using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/MerchantEmployee")]
    [ApiController]
    [Authorize]
    public class MerchantEmployeeController : ControllerBase
    {
        private readonly IMerchantEmployeeRepository _repository;

        private readonly IEnrolmentRepository _Erepository;
        private readonly IUserRepository _Urepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public MerchantEmployeeController(IMerchantEmployeeRepository repository, IEnrolmentRepository Erepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IUserRepository Urepository)
        {
            _repository = repository;
            _Erepository = Erepository;
            _Urepository = Urepository;
            _env = env;
        }

        // GET: api/MerchantEmployee
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MerchantEmployee>>> GetAllMerchantEmployees()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }
        [HttpGet("GetBranchEmployees")]
        public async Task<ActionResult<IEnumerable<MerchantEmployee>>> GetBranchEmployees(long MerchantBranchID, bool IsActive = true)
        {
            var result = await _repository.Find(a => a.IsActive == IsActive && a.MerchantBranchId == MerchantBranchID);
            return Ok(result);
        }

        // GET: api/MerchantEmployee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantEmployeeDTO>> GetByEmployeeID(long id)
        {
            var MerchantEmployee = await _repository.GetEmployeeById(id);

            if (MerchantEmployee == null)
            {
                return NotFound();
            }
            return Ok(MerchantEmployee);
        }



        [HttpGet, Route("GetMerchantEmployees")]
        public async Task<ActionResult<MerchantEmployeeDTO>> GetMerchantEmployees([FromQuery] long MerchantBranchID, [FromQuery] bool IsActive)
        {
            try
            {
                var MerchantEmployees = await _repository.GetMerchantEmployeeByBranchId(MerchantBranchID, IsActive);
                return Ok(MerchantEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddEmployee")]
        public async Task<ActionResult<MerchantEmployee>> AddEmployee([FromBody] MerchantEmployeeDTO obj)
        {
            try
            {
                long EmployeeID = 0;
                MerchantEmployee Employee = new MerchantEmployee();
                var Old_Employee = obj.Id != null ? await _repository.GetByID(obj.Id.Value) : null;
                if (Old_Employee != null && Old_Employee.Id > 0)
                {
                    EmployeeID = Old_Employee.Id;
                    Old_Employee.FirstName = obj.FirstName;
                    Old_Employee.LastName = obj.LastName;
                    Old_Employee.NationalNumber = obj.NationalNumber;
                    Old_Employee.MiddleName = obj.MiddleName;
                    Old_Employee.Mobile = obj.Mobile;
                    Old_Employee.Phone = obj.Phone;
                    Old_Employee.MerchantBranchId = obj.MerchantBranchId != null ? obj.MerchantBranchId.Value : 0;
                    Old_Employee.ModifiedDate = DateTime.UtcNow;
                    Old_Employee.IsActive = obj.IsActive;
                    await _repository.Update(Old_Employee);
                }
                else
                {
                    Employee = new MerchantEmployee()
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        NationalNumber = obj.NationalNumber,
                        MiddleName = obj.MiddleName,
                        Mobile = obj.Mobile,
                        Phone = obj.Phone,
                        MerchantBranchId = obj.MerchantBranchId != null ? obj.MerchantBranchId.Value : 0,
                        ModifiedDate = DateTime.UtcNow,
                        IsActive = obj.IsActive,
                        CreationDate = DateTime.UtcNow,
                    };
                    #region User
                    Common.HashKey.CreateMD5Hash("1234", out string hash);
                    var hashed_password = hash;
                   var count=await _Urepository.GetuserCount();
                    User user = new User()
                    {
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        IsActive = true,
                        Username = obj.Mobile!=null?obj.Mobile.ToString(): obj.Phone,
                        Password = hashed_password,
                        ReferenceNumber = DateTime.UtcNow.Year.ToString()+ DateTime.UtcNow.Month.ToString()+( count + 1).ToString()
                    };
                    await _Urepository.Add(user);
                    #endregion
                    #region Enrolment
                    if (user.Id != null && user.Id > 0)
                    {
                        Enrolment enroll = new Enrolment()
                        {
                            UserId = user.Id,
                            RoleId = (int)Roles.MerchantEmployee
                        };
                        await _Erepository.Add(enroll);
                        Employee.EnrolmentId = enroll.Id;
                        if (enroll.Id != null && enroll.Id > 0)
                        {
                            await _repository.Add(Employee);
                        }
                    }
                    #endregion
                }
                return Ok(EmployeeID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private bool MerchantEmployeeExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
