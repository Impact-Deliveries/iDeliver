using iDeliverDataAccess.Base;
using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using iDeliverService.Common;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using iDeliverService.Common.Service;

namespace iDeliverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly JwtAuthentication _JwtAuthentication;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthenticationController(IAccountRepository repository, IConfiguration configuration, IUserService userService)
        {
            _repository = repository;
            _configuration = configuration;
            _JwtAuthentication = new JwtAuthentication(_configuration);
            _userService = userService;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            UserDTO? user = await _repository.Login(request.Username, request.Role);

            if (user is null)
                return NotFound("User not found.");

            if (user is not null && !Common.HashKey.VerifyMD5Hash(request.Password, user.Password))
                return BadRequest("Wrong password.");

            string token = _JwtAuthentication.CreateToken(user);
            return Ok(new
            {
                token
            });
        }

        [HttpGet("loggedin"), Authorize]
        public async Task<ActionResult<Profile>> loggedin()
        {
            Profile profile = new Profile();
            profile = _userService.Profile();
            profile.Accounts = await _repository.Accounts(profile.UserID);
            return Ok(profile);
        }
    }
}
