#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IDeliverObjects.Objects;
using iDeliverDataAccess;
using iDeliverDataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace iDeliverService.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET: api/users
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var User = await _repository.GetByID(id);

            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }

        // PUT: api/user/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(long id, User User)
        {
            if (id != User.Id)
            {
                return BadRequest();
            }

            try
            {
                Common.HashKey.CreateMD5Hash(User.Password, out string hash);
                User.Password = hash;

                await _repository.Update(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUsers(User User)
        {
            // Create password hash
            Common.HashKey.CreateMD5Hash(User.Password, out string hash);
            User.Password = hash;

            // Create reference number (currentyear + month + random number)
            DateTime UtcNow = DateTime.UtcNow;
            User last = await _repository.GetLastRow();
            int random = 1;
            if (last != null)
                random = Convert.ToInt32(last.ReferenceNumber.Substring(6, last.ReferenceNumber.Length - 6)) + 1;

            User.ReferenceNumber = UtcNow.Year.ToString("0000") + UtcNow.Month.ToString("00") + random.ToString("0000");

            // Add creation and modified date
            User.ModifiedDate = DateTime.UtcNow;
            User.CreationDate = DateTime.UtcNow;

            await _repository.Add(User);

            // string Message = $"({User.Username}) was addad by sysadmin in {DateTime.UtcNow.ToLongTimeString()}";
            //_logger.LogInformation(Message);

            return CreatedAtAction("GetUser", new { id = User.Id }, User);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(long id)
        {
            var User = await _repository.GetByID(id);
            if (User == null)
            {
                return NotFound();
            }

            await _repository.Delete(User);

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }
    }
}
