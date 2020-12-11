using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(User user);

         Task<bool> SaveAllChangesAsync();

         Task<IEnumerable<User>> GetUserAsync();

         Task <User> GetUserByIdAsync(int id);

         Task<User> GetUserByUsernameAsync(string username);

         Task<IEnumerable<MemberDto>> GetMembersAsync();

         Task<MemberDto> GetMemberAsync(string username);
    }
}