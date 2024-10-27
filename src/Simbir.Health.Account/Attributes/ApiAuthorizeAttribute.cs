using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Simbir.Health.Account.Services;

namespace Simbir.Health.Account.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ApiAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly List<string> _roles;

        public ApiAuthorizeAttribute(string? role)
        {
            _roles = role?.Split(',').ToList() ?? [];
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var tokenService = context.HttpContext.RequestServices.GetService<ITokenService>();

            if (tokenService == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var accessTokenHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(accessTokenHeader) || !accessTokenHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var userInfo = await tokenService.ValidateAccessToken(accessToken);

            if (userInfo != null && userInfo.IsValid)
            {
                if (_roles.Count != 0 && !_roles.Any(r => userInfo.Roles.Contains(r)))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
