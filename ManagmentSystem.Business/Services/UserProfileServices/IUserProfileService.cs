using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Business.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        Task<IResult> CreateUserAsync(UserProfileDTO userProfileDTO);
        Task<IResult> UpdateUserAsync(UserProfileDTO userProfileDTO);
        Task<UserProfileDTO?> GetUserProfileAsync(string identityUserId); 

        Task<UserProfileDTO?> GetUserProfileByIdAsync(Guid userId);
        Task<List<UserDTO>> GetAllUserProfilesAsync();


    }
}
