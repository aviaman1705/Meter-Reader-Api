

using AutoMapper;
using MeterReaderAPI.DTO.User;
using MeterReaderAPI.Entities;
using MeterReaderAPI.Helpers;
using MeterReaderAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MeterReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IWebHostEnvironment __webHostEnvironment;
        private readonly IFileStorageService _fileStorageService;
        private IMapper _mapper;
        private readonly ILogger<AccountsController> _logger;
        private string container = "users";

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository repository,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AccountsController> logger,
            IWebHostEnvironment webHostEnvironment,
            IFileStorageService fileStorageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            __webHostEnvironment = webHostEnvironment;
            _fileStorageService = fileStorageService;
        }


        [HttpGet("GetUserDetails")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetails()
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.Headers["Authorization"]))
                {
                    string token = Request.Headers["Authorization"].ToString().Replace("bearer ", string.Empty);
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);

                    KeyValuePair<string, object> payload = jwtSecurityToken.Payload.FirstOrDefault(x => x.Key.ToLower() == "email");
                    string currentUserEmail = payload.Value.ToString();

                    var user = _mapper.Map<UserDetailsDTO>(await _repository.GetUserByEmail(currentUserEmail));

                    if (user == null)
                    {
                        return BadRequest("יוזר לא קיים במערכת");
                    }

                    return Ok(user);
                }
                else
                {
                    return BadRequest("יוזר לא קיים במערכת");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }


        [HttpPost("Create")]
        public async Task<ActionResult<AuthenticationResponse>> Create([FromBody] RegisterDTO register)
        {
            try
            {
                var user = new ApplicationUser { UserName = register.UserName, Email = register.Email };

                // בדיקה אם מייל קיים במערכת
                var emailExist = await _repository.GetUserByEmail(register.Email);
                if (emailExist != null)
                {
                    return BadRequest($"{register.Email} כבר קיים במערכת.");
                }

                //בדיקה אם שם משתמש קיים במערכת
                var usernameExist = await _repository.GetUserByName(register.UserName);
                if (usernameExist != null)
                {
                    return BadRequest($"{register.UserName} כבר קיים במערכת.");
                }

                // יצירת משתמש חדש
                var result = await _repository.Create(user, register.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Creating ${register.Email} as new user");
                    return Ok();
                }
                else
                {
                    _logger.LogError(result.Errors.ToString());
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            try
            {
                _logger.LogInformation($"Creating {userCredentials.Email} trying to log in");

                // step one retreive user by email
                var user = await _repository.GetUserByEmail(userCredentials.Email);

                //step two check if user exsist
                if (user != null)
                {
                    var loginProcess = await _repository.Login(user.UserName, userCredentials.Password);
                    if (loginProcess.Succeeded)
                    {
                        _logger.LogInformation($"{userCredentials.Email} is logged in");

                        var currentUser = _mapper.Map<LoginDTO>(user);
                        return await BuildToken(currentUser);
                    }
                    else
                    {
                        return BadRequest("מייל או סיסמא אינם נכונים");
                    }
                }
                else
                {
                    return BadRequest("יוזר לא קיים במערכת");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("UpdateUserDetails")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserDetails([FromForm] UserDetailsDTO userDetails)
        {
            try
            {
                _logger.LogInformation($"Updating {userDetails.Email} user");

                ApplicationUser user = await _repository.GetUserByEmail(userDetails.Email);
                if (user != null)
                {
                    user.UserName = userDetails.UserName;
                    user.Email = userDetails.Email;
                    user.PhoneNumber = userDetails.Phone;

                    if (userDetails.ImageFile != null)
                    {
                        //FileUpload fileUpload = new FileUpload(__webHostEnvironment, HttpContext);
                        user.Image = await _fileStorageService.SaveFile(container,userDetails.ImageFile);
                    }

                    var updateOperation = await _repository.Update(user);
                    if (updateOperation.Succeeded)
                    {
                        UserDetailsDTO updatedUser = _mapper.Map<UserDetailsDTO>(user);
                        return updatedUser;
                    }
                    else
                    {
                        _logger.LogError(updateOperation.Errors.ToString());
                        return BadRequest(updateOperation.Errors);
                    }
                }
                else
                {
                    return BadRequest("יוזר לא קיים");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        private async Task<AuthenticationResponse> BuildToken(LoginDTO userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",userCredentials.Email),
                new Claim("username",userCredentials.UserName)
            };

            var user = await _userManager.FindByEmailAsync(userCredentials.Email);

            var claimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
