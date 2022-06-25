using iDeliverDataAccess.Repositories;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrolmentDeviceController : ControllerBase
    {
        private readonly IEnrolmentDeviceRepository _repository;

        public EnrolmentDeviceController(IEnrolmentDeviceRepository repository)
        {
            _repository = repository;
        }

        // GET: api/EnrolmentDevice
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EnrolmentDevice>>> GetAll()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }

        // GET: api/EnrolmentDevice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrolmentDevice>> GetByID(long id)
        {
            var EnrolmentDevice = await _repository.GetByID(id);

            if (EnrolmentDevice is null)
            {
                return NotFound();
            }

            return Ok(EnrolmentDevice);
        }

        [HttpPost]
        public async Task<ActionResult<EnrolmentDevice>> PostEnrolmentDevice(EnrolmentDevice row)
        {
            EnrolmentDevice? enrolmentDevice = await _repository.FindRow(w => w.IsDeleted == false &&
            w.EnrolmentId == row.EnrolmentId && w.DeviceType == row.DeviceType);

            if (enrolmentDevice is not null && EnrolmentDeviceExists(enrolmentDevice.Id))
            {

                enrolmentDevice.ModifiedDate = DateTime.UtcNow;
                enrolmentDevice.DeviceId = row.DeviceId;
                enrolmentDevice.DeviceName = row.DeviceName;
                enrolmentDevice.DeviceToken = row.DeviceToken;


                await _repository.Update(enrolmentDevice);
                return Ok(enrolmentDevice);
            }
            else
            {
                row.IsDeleted = false;
                row.CreationDate = DateTime.UtcNow;
                row.ModifiedDate = DateTime.UtcNow;
                await _repository.Add(row);
                return CreatedAtAction("GetByID", new { id = row.Id }, row);
            }
        }


        private bool EnrolmentDeviceExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }
    }
}
