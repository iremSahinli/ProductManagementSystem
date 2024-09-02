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
using Microsoft.EntityFrameworkCore;
using ManagmentSystem.Infrastructure.AppContext;

namespace ManagmentSystem.Business.Services.UserProfileServices
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly AppDbContext _context;

        public UserProfileService(IUserProfileRepository userProfileRepository, AppDbContext context)
        {
            _userProfileRepository = userProfileRepository;
            _context = context;
        }

        public async Task<IResult> CreateUserAsync(UserProfileDTO userProfileDTO)
        {
            var newUserProfile = userProfileDTO.Adapt<UserProfile>();
            await _userProfileRepository.AddAsync(newUserProfile);
            await _userProfileRepository.SaveChangeAsync();
            return new SuccessResult("Kullanıcı profili başarıyla oluşturuldu.");
        }

        public async Task<List<UserDTO>> GetAllUserProfilesAsync()
        {
            var profiles = await _userProfileRepository.GetAllAsync();
            var userProfilesWithEmail = new List<UserDTO>();

            foreach (var profile in profiles)
            {
                var email = await _context.Users
                                          .Where(u => u.Id == profile.IdentityUserId)
                                          .Select(u => u.Email)
                                          .FirstOrDefaultAsync();
                var userProfileDTO = profile.Adapt<UserDTO>();
                userProfileDTO.Mail = email;
                userProfilesWithEmail.Add(userProfileDTO);
            }

            return userProfilesWithEmail;
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
