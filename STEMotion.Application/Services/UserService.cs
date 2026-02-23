using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IJWTService _jwtService;
        private readonly IOtpService _otpService;
        #region cto
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService, IJWTService jWTService, IOtpService otpService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
            _jwtService = jWTService;
            _otpService = otpService;
        }
        #endregion cto
        public async Task<string> AuthenticateGoogleUserAsync(GoogleRequestDTO googleRequestDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(googleRequestDTO.Email, false);
            if (user == null)
            {
                var roleName = googleRequestDTO.RoleName?.ToLower() ?? "student";
                var role = await _unitOfWork.RoleRepository.GetRoleByNameAsync(roleName);
                if (role == null)
                {
                    throw new NotFoundException("Role", roleName);
                }
                var newUser = _mapper.Map<User>(googleRequestDTO);
                newUser.RoleId = role.Id;
                newUser.Password = _passwordService.HashPasswords(Guid.NewGuid().ToString());

                await _unitOfWork.UserRepository.CreateAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(newUser.Email, false);
            }
            var token = _jwtService.GenerateToken(user.UserId,user.Email, user.Role.Name);
            return token;
        }

        public async Task<UserResponseDTO> ChangePassword(ChangePasswordRequestDTO request)
        {
            var (isOtpValid, _) = await _otpService.VerifyOtpAsync(request.Email, request.OtpCode);
            if (!isOtpValid)
            {
                throw new BadRequestException("Mã OTP không chính xác hoặc đã hết hạn!");
            }
            var user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(request.Email, true);
            if (user == null)
            {
                throw new NotFoundException("User", request.Email);
            }
            user.Password = _passwordService.HashPasswords(request.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateUser(CreateUserRequestDTO user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(user.Email, false);
            if (existingUser != null)
            {
                throw new AlreadyExistsException("User", user.Email);
            }
            var role = await _unitOfWork.RoleRepository.GetRoleByNameAsync(user.RoleName);
            if (role == null)
            {
                throw new NotFoundException("Role", user.RoleName);
            }
            var entites = _mapper.Map<User>(user);
            entites.RoleId = role.Id;
            entites.Status = "Active";
            entites.Password = _passwordService.HashPasswords(user.Password);
            var newUser = await _unitOfWork.UserRepository.CreateAsync(entites);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<UserResponseDTO>(newUser);
            response.RoleName = role.Name;
            return response;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User", id);

            }
            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        public async Task<PaginatedResponseDTO<UserResponseDTO>> GetAllUsers(PaginationRequestDTO requestDTO)
        {
            var (items, total) = await _unitOfWork.UserRepository.GetPagedAsync(requestDTO.PageNumber, requestDTO.PageSize);
            var response = _mapper.Map<IEnumerable<UserResponseDTO>>(items);
            return new PaginatedResponseDTO<UserResponseDTO> 
            {
                Items = response,
                PageSize = requestDTO.PageSize,
                PageNumber = requestDTO.PageNumber,
                TotalCount = total
            };
        }

        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User", id);
            }

            var response = _mapper.Map<UserResponseDTO>(user);
            response.RoleName = (await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId))?.Name;
            return response;
        }

        public async Task<string> LoginUser(LoginRequestDTO loginRequest)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(loginRequest.Email, false);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            if (user.Status != "Active")
            {
                throw new ForbiddenException("User account is inactive");
            }
            var checkpassword = _passwordService.VerifyPassword(loginRequest.Password, user.Password);

            if (!checkpassword)
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            //var response = _mapper.Map<UserResponseDTO>(user);
            var response = _jwtService.GenerateToken(user.UserId,loginRequest.Email, user.Role.Name);

            return response;
        }

        public async Task RequestPasswordResetOtpAsync(string email)
        {

            var user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(email, false);
            if (user == null)
            {
                throw new NotFoundException("User", email);
            }

            await _otpService.SendOtpAsync(email, (user.FirstName + " " + user.LastName));
        }

        public async Task RequestRegistrationOtpAsync(CreateUserRequestDTO createUserRequest)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailWithRoleAsync(createUserRequest.Email, false);
            if (user != null)
            {
                throw new AlreadyExistsException("User", createUserRequest.Email);
            }

            var role = await _unitOfWork.RoleRepository.GetRoleByNameAsync(createUserRequest.RoleName);
            if (role == null)
            {
                throw new NotFoundException("Role", createUserRequest.RoleName);
            }

            await _otpService.SendOtpAsync(createUserRequest.Email, null, createUserRequest);
        }

        public async Task<UserResponseDTO> UpdateUser(Guid id, UpdateUserRequestDTO userDTO)
        {
            var users = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (users == null)
            {
                throw new NotFoundException("User", id);
            }

            _mapper.Map(userDTO, users);

            _unitOfWork.UserRepository.Update(users);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<UserResponseDTO>(users);
            response.RoleName = (await _unitOfWork.RoleRepository.GetByIdAsync(users.RoleId))?.Name;
            return response;
        }
        public async Task<UserResponseDTO> VerifyOtpAndRegisterAsync(string email, string otpCode)
        {
            {
                var (isValid, registrationJson) = await _otpService.VerifyOtpAsync(email, otpCode);
                if (!isValid || string.IsNullOrEmpty(registrationJson))
                    throw new BadRequestException("Mã OTP không chính xác hoặc đã hết hạn.");

                var request = JsonSerializer.Deserialize<CreateUserRequestDTO>(registrationJson);
                if (request is null)
                    throw new ValidationException(new Dictionary<string, string[]>
        {
            { "RegistrationData", new[] { "Dữ liệu đăng ký không hợp lệ." } }
        });
                return await CreateUser(request);

            }
        }
    }
}

