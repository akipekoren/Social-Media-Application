using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly DataContext _context;

        private readonly ITokenService _tokenService;

        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserModel>> Register(RegisterModel model)
        {

            if (await UserExists(model.Username))
            {
                return BadRequest("Username is already exist!");
            }


            var user = _mapper.Map<User>(model);


            using var hmac = new HMACSHA512();

       
                user.UserName = model.Username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                user.PasswordSalt = hmac.Key;
                
          

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserModel{

                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
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
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

    




        private async Task<bool> UserExists(string username)
        {
            var flag = await _context.Users.AnyAsync(name => name.UserName == username.ToLower());

            return flag;
        }
    }
}