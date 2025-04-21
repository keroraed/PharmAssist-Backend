using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PharmAssist.Core.Entities.Email;
using PharmAssist.Core.Entities.Identity;
using PharmAssist.Core.Entities.OTP;
using PharmAssist.Core.Services;
using PharmAssist.DTOs;
using PharmAssist.Errors;
using PharmAssist.Extensions;
using PharmAssist.Repository.Services;
using System.Security.Claims;


namespace PharmAssist.Controllers
{
	public class AccountsController : APIBaseController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
        private readonly OtpService _otpService;

        public AccountsController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService emailService,
            OtpService otpService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _otpService = otpService;
        }

        //public AccountsController()
        //{
        //}


        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "This email is already in use"));
<<<<<<< HEAD

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            await _otpService.SendOtpAsync(user.Email); // <--- sends + stores OTP

            var ReturnedUser = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }

=======

			var user = new AppUser()
			{
				DisplayName = model.Name,
				Email = model.Email,
				UserName = model.Email.Split('@')[0],
				EmailConfirmed = false
			};
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            await _otpService.SendOtpAsync(user.Email); // <--- sends + stores OTP
            
            return Ok(new
			{
				success= true,
				Message= "User registered successfully. Please check your email for the OTP.",
			});
        }

>>>>>>> 0741810 (Forgot password)

        [HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new ApiResponse(401));

			if(!user.EmailConfirmed)
			{
				return BadRequest(new ApiResponse(400, "Email not confirmed. Please check your email for the OTP."));
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});
		}

		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return BadRequest(new ApiResponse(400, "User not found."));

			// Generate and send OTP
			await _otpService.SendOtpAsync(user.Email);

			return Ok(new { success = true, message = "OTP sent to your email for password reset." });
		}

		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { success = false, message = "Invalid input data." });
			}

			var (isValid, errorMessage) = await _otpService.VerifyOtpAsync(model.Email, model.Otp);
			if (!isValid)
			{
				return BadRequest(new { success = false, message = errorMessage });
			}

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return BadRequest(new { success = false, message = "User not found." });
			}

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
			if (!resetResult.Succeeded)
			{
				var errors = resetResult.Errors.Select(e => e.Description).ToList();
				return BadRequest(new { success = false, message = "Password reset failed.", errors });
			}

			return Ok(new { success = true, message = "Password reset successful." });
		}

		[Authorize]
		[HttpGet("GetCurrentUser")]
		public async Task<ActionResult<UserDTO>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);
			var returnedUser = new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};
			return Ok(returnedUser);
		}


		[Authorize]
		[HttpGet("CurrentUserAddress")]
		public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
		{
			var user = await _userManager.FindUserWithAddressAsync(User);
			var mappedAddress = _mapper.Map<Address, AddressDTO>(user.Address);
			return Ok(mappedAddress);
		}


		[Authorize]
		[HttpPut("Address")]
		public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO updatedAddress)
		{
			var user = await _userManager.FindUserWithAddressAsync(User);
			if (user is null) return Unauthorized(new ApiResponse(401));
			var address = _mapper.Map<AddressDTO, Address>(updatedAddress);
			address.Id = user.Address.Id; 
			user.Address = address;
			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(updatedAddress);
		}


		[HttpGet("emailExists")]
		 public async Task<ActionResult<bool>> CheckEmailExists(string email)
		 {
			return await _userManager.FindByEmailAsync(email) is not null;
		 }
	}
}
