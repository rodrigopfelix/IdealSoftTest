using IdealSoftTestServer.Application.DTOs.Login;
using IdealSoftTestServer.Application.Interfaces;
using IdealSoftTestServer.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdealSoftTestServer.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: true);

            if (!result.Succeeded)
                return Unauthorized();

            var token = await _tokenService.GenerateToken(user);

            return Ok(new LoginResponseDto { AccessToken = token });
        }

        [Authorize]
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok("Access token valid");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("checkadmin")]
        public IActionResult AdminOnly()
        {
            return Ok("Admin authorized");
        }
    }
}
