using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<PaginatedResponseDTO<UserResponseDTO>> GetAllUsers(PaginationRequestDTO requestDTO);
        Task<UserResponseDTO> GetUserById(Guid id);
        Task<UserResponseDTO> CreateUser(CreateUserRequestDTO user);
        Task<UserResponseDTO> UpdateUser(Guid id, UpdateUserRequestDTO user);
        Task<bool> DeleteUser(Guid id);
        Task<string> LoginUser(LoginRequestDTO loginRequest);
        Task<UserResponseDTO> ChangePassword(ChangePasswordRequestDTO request);

        Task<string> AuthenticateGoogleUserAsync(GoogleRequestDTO googleRequestDTO);

        Task RequestRegistrationOtpAsync(CreateUserRequestDTO createUserRequest);

        Task RequestPasswordResetOtpAsync(string email);
        Task<UserResponseDTO> VerifyOtpAndRegisterAsync(string email, string otpCode);
    }
}
