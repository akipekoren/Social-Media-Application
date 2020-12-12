using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserModel>> Register(RegisterModel model)
        {

            if (await UserExists(model.Username))
            {
                return BadRequest("Username is already exist!");
            }


            using var hmac = new HMACSHA512();

            var user  = new User
            {
                UserName = model.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password)),
                PasswordSalt = hmac.Key
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserModel{

                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }


        [HttpPost("login")]

        public async Task <ActionResult<UserModel>> Login(LoginModel model)
        {
            var user = await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == model.Username);

            if (user == null)
                return Unauthorized("Username is not valid!");


           using var hmac = new HMACSHA512(user.PasswordSalt);
           
           var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));

           for (int i=0 ; i< computedHash.Length; i++)
           {
               if (computedHash[i] != user.PasswordHash[i])
               {
                   return Unauthorized("Invalid Password");
               }
           }

            return new UserModel{

                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

    




        private async Task<bool> UserExists(string username)
        {
            var flag = await _context.Users.AnyAsync(name => name.UserName == username.ToLower());

            return flag;
        }
    }
}