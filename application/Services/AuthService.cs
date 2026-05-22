using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectTaskManagement.DTOs.Auth;
using ProjectTaskManagement.Exceptions;
using ProjectTaskManagement.Models;
using ProjectTaskManagement.Persistence;

namespace ProjectTaskManagement.Services;

public class AuthService : ServiceBase, IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(
        IApplicationDbContext context,
        ITokenService tokenService,
        ILogger<AuthService> logger)
        : base(logger)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request) =>
        ExecuteAsync(async () =>
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                throw new AppException("Email is already registered.", HttpErrors.Conflict);
            }

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreateAuthResponse(user);
        });

    public Task<AuthResponseDto> LoginAsync(LoginRequestDto request) =>
        ExecuteAsync(async () =>
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new AppException("Invalid email or password.", HttpErrors.Unauthorized);
            }

            return CreateAuthResponse(user);
        });

    private AuthResponseDto CreateAuthResponse(User user) =>
        new()
        {
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };
}
