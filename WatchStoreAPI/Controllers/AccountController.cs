//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using WatchStoreAPI.Models.DTO;
//using WebAPIdemo.DTO;

//namespace WatchStoreAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly UserManager<appUser> _userManger;
//        private readonly IConfiguration _config;

//        public AccountController(UserManager<appUser> userManger,IConfiguration config)
//        {
//            _userManger = userManger;
//            _config = config;
//        }
//        [HttpPost("Register")]
//        public async Task<IActionResult> register(RegisterDTO userDto)
//        {
//            if (ModelState.IsValid)
//            {
//                appUser user = new appUser();
//                user.UserName = userDto.userName;
//                user.Email = userDto.Email;
//                IdentityResult result = await _userManger.CreateAsync(user, userDto.password);
//                if (result.Succeeded)
//                {
//                    return Ok("Account Add Success");
//                }
//                return BadRequest(result.Errors.FirstOrDefault());
//            }
//            return BadRequest(ModelState);
//        }
//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(loginDTO userDto)
//        {
//            if (ModelState.IsValid)
//            {
//                appUser user = await _userManger.FindByNameAsync(userDto.userName);
//                if (user != null)
//                {
//                    bool found = await _userManger.CheckPasswordAsync(user, userDto.password);
//                    if (found)
//                    {
//                        //Create Claims
//                        var Myclaims = new List<Claim>();
//                        Myclaims.Add(new Claim(ClaimTypes.Name, userDto.userName));
//                        Myclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
//                        Myclaims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

//                        //get roles
//                        var roles = await _userManger.GetRolesAsync(user);
//                        foreach (var role in roles)
//                        {
//                            Myclaims.Add(new Claim(ClaimTypes.Role, role));
//                        }
//                        //create SingingCredentials
//                        SecurityKey myKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
//                        SigningCredentials singingCred = new SigningCredentials(myKey, SecurityAlgorithms.HmacSha256);

//                        //Init Token
//                        JwtSecurityToken myToken = new JwtSecurityToken(
//                            issuer: _config["JWT:validIssure"],
//                            audience: _config["JWT:validAudience"],
//                            expires: DateTime.Now.AddHours(1),
//                            claims: Myclaims,
//                            signingCredentials: singingCred
//                            );
//                        return Ok(new
//                        {
//                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
//                            expiration = myToken.ValidTo
//                        });

//                    }
//                }

//            }
//            return Unauthorized();
//        }
//    }
//}
