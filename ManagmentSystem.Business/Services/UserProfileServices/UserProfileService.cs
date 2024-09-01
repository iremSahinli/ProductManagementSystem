using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Domain.Utilities.Interfaces;
using ManagmentSystem.Infrastructure.Repositories.UserProfileRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;

namespace ManagmentSystem.Business.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IResult> CreateUserAsync(UserProfileDTO userProfileDTO)
        {
            var newUserProfile = userProfileDTO.Adapt<UserProfile>();
            await _userProfileRepository.AddAsync(newUserProfile);
            await _userProfileRepository.SaveChangeAsync();
            return new SuccessResult("Kullanıcı profili başarıyla oluşturuldu.");
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string identityUserId)
        {
            var userProfile = await _userProfileRepository.FindByIdentityUserIdAsync(identityUserId);

            if (userProfile == null)
            {
                return null;
            }

            return userProfile.Adapt<UserProfileDTO>();

        }

        public async Task<UserProfileDTO?> GetUserProfileByIdAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(userId);

            if (userProfile == null)
            {
                return null;
            }

            return userProfile.Adapt<UserProfileDTO>();
        }

        public async Task<IResult> UpdateUserAsync(UserProfileDTO userProfileDTO)
        {
            var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfileDTO.Id);
            if (existingUserProfile == null)
            {
                return new ErrorResult("Kullanıcı profili bulunamadı.");
            }

            existingUserProfile = userProfileDTO.Adapt(existingUserProfile);
            await _userProfileRepository.UpdateAsync(existingUserProfile);
            await _userProfileRepository.SaveChangeAsync();
            return new SuccessResult("Kullanıcı profili başarıyla güncellendi.");
        }

       
    }
}
