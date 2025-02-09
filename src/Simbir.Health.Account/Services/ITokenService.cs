﻿using Simbir.Health.Account.Services.DTO;

namespace Simbir.Health.Account.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(int userId);
        Task<string> GenerateRefreshTokenAsync(int userId);
        Task<AccessAndRefreshTokenDTO> GenerateTokensAsync(int userId);
        Task<TokenValidationResultDTO> ValidateAccessToken(string token);
        Task<AccessAndRefreshTokenDTO> RefreshTokensAsync(string refreshToken);
    }
}
