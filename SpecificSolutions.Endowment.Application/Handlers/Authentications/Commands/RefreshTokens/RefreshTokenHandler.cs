using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshToken;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Application.Models.Identity;
using SpecificSolutions.Endowment.Application.Models.Identity.Entities;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshTokens
{
    public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenHandler(ITokenService tokenService, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<EndowmentResponse<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            //todo: check if the token is not valid or expired reomve all the refresh tokens for the user

            //var userId = _tokenService.GetUserIdFromToken(request.Token);

            //if (userId == null)
            //    return Response.FailureResponse<RefreshTokenResponse>("Invalid token");

            //var user = await _userManager.FindByIdAsync(userId);

            //if (user == null)
            //    return Response.FailureResponse<RefreshTokenResponse>("User not found");

            Models.Identity.Entities.RefreshToken? refreshToken = await _userManager.Users.Where(u => u.IdentityUserTokens.Any(u => u.Token.Equals(request.RefreshToken))).Select(u => u.IdentityUserTokens.FirstOrDefault()).FirstOrDefaultAsync();

            //Models.Identity.RefreshToken? refreshToken = tokensss.IdentityUserTokens.FirstOrDefault(u => u.Token.Equals(request.RefreshToken));

            //var refreshToken = tokensss.IdentityUserTokens.FirstOrDefault(u => u.Token.Equals(request.RefreshToken));
            if (refreshToken is null)
            {
                return Response.FailureResponse<RefreshTokenResponse>("Invalid refresh token");
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                await _unitOfWork.RefreshTokens.RemoveByUserIdAsync(new Guid(refreshToken.UserId).ToString());
                return Response.FailureResponse<RefreshTokenResponse>("Invalid refresh token");
            }

            var newRefreshToken = _tokenService.GenerateRefreshTokenAsync();
            var newExpiryDate = DateTime.Now.AddHours(1);

            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());

            var token = await _tokenService.GenerateTokenAsync(user!);

            refreshToken.UpdateToken(newRefreshToken, newExpiryDate);

            await _unitOfWork.CompleteAsync(cancellationToken);

            //ToDo remove this line
            //await _userManager.SetAuthenticationTokenAsync(user!, "CustomProvider", "JwtRefreshToken", request.RefreshToken);

            var response = new RefreshTokenResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token
            };

            return Response.Responsee(response);

        }

        //public async Task<EndowmentResponse<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        //{
        //    var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        //    if (refreshToken == null)
        //    {
        //        return Response.FailureResponse<RefreshTokenResponse>("Invalid refresh token");
        //    }

        //    if (refreshToken != null || refreshToken.ExpiryDate < DateTime.Now)
        //    {
        //        await _unitOfWork.RefreshTokenRepository.RemoveByUserIdAsync(refreshToken.UserId);
        //        return Response.FailureResponse<RefreshTokenResponse>("Invalid refresh token");
        //    }

        //    var newRefreshToken = _tokenService.GenerateRefreshTokenAsync();
        //    var newExpiryDate = DateTime.Now.AddHours(1);

        //    refreshToken.UpdateToken(newRefreshToken, newExpiryDate);



        //    //var token = await _tokenService.GenerateTokenAsync(user: refreshToken.);

        //    refreshToken.UpdateToken(_tokenService.GenerateRefreshTokenAsync(), DateTime.Now.AddHours(1));

        //    await _unitOfWork.CompleteAsync();

        //    var response = new RefreshTokenResponse
        //    {
        //        Token = "token",
        //        RefreshToken = refreshToken.Token
        //    };

        //    return Response.Responsee(response );
        //}

    }
}

//namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshTokens
//{
//    public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
//    {
//        private readonly ITokenService _tokenService;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public RefreshTokenHandler(ITokenService tokenService, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
//        {
//            _tokenService = tokenService;
//            _unitOfWork = unitOfWork;
//            _userManager = userManager;
//        }

//        public async Task<EndowmentResponse<RefreshTokenResponse>> Handle(
//            RefreshTokenCommand request,
//            CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Validate the expired JWT token first
//                var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken);
//                if (principal == null)
//                {
//                    return Response.FailureResponse<RefreshTokenResponse>("Invalid token");
//                }

//                var userId = principal.FindFirst("UserId")?.Value;
//                if (string.IsNullOrEmpty(userId))
//                {
//                    return Response.FailureResponse<RefreshTokenResponse>("Invalid token claims");
//                }

//                // Find the refresh token in database
//                var refreshToken = await _unitOfWork.RefreshTokenRepository
//                    .GetByTokenAsync(request.RefreshToken);

//                if (refreshToken == null)
//                {
//                    return Response.FailureResponse<RefreshTokenResponse>("Invalid refresh token");
//                }

//                // Validate refresh token belongs to the same user
//                if (refreshToken.UserId.ToString() != userId)
//                {
//                    return Response.FailureResponse<RefreshTokenResponse>("Token mismatch");
//                }

//                // Check if refresh token is expired
//                if (refreshToken.ExpiryDate < DateTime.UtcNow)
//                {
//                    // Clean up all refresh tokens for this user if token is expired
//                    await _unitOfWork.RefreshTokenRepository.RemoveByUserIdAsync(userId);
//                    await _unitOfWork.CompleteAsync(cancellationToken);
//                    return Response.FailureResponse<RefreshTokenResponse>("Refresh token expired");
//                }

//                // Get user from database
//                var user = await _userManager.FindByIdAsync(userId);
//                if (user == null)
//                {
//                    return Response.FailureResponse<RefreshTokenResponse>("User not found");
//                }

//                // Generate new tokens
//                var newJwtToken = await _tokenService.GenerateTokenAsync(user);
//                var newRefreshTokenValue = _tokenService.GenerateRefreshTokenAsync();
//                var newExpiryDate = DateTime.UtcNow.AddHours(1);

//                // Update the refresh token
//                refreshToken.UpdateToken(newRefreshTokenValue, newExpiryDate);
//                await _unitOfWork.CompleteAsync(cancellationToken);

//                // Create response
//                var response = new RefreshTokenResponse
//                {
//                    Token = newJwtToken,
//                    RefreshToken = newRefreshTokenValue
//                };

//                return Response.Responsee(response);
//            }
//            catch (SecurityTokenException ex)
//            {
//                return Response.FailureResponse<RefreshTokenResponse>($"Token validation failed: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                // Log the exception here
//                return Response.FailureResponse<RefreshTokenResponse>($"An error occurred: {ex.Message}");
//            }
//        }
//    }
//}
