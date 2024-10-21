using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Simbir.Health.Account.Models.DTO;
using System.Text.Json;

namespace Simbir.Health.Hospital.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class ApiAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string _role;

    public ApiAuthorizeAttribute(string role = null)
    {
        _role = role;
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var accessTokenHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(accessTokenHeader) || !accessTokenHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var accessToken = accessTokenHeader.Replace("Bearer ", "");

        var url = $"https://localhost:7136/api/Authentication/Validate?accessToken={accessToken}";

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var validationResult = JsonSerializer.Deserialize<ValidationTokenResult>(jsonString);

                if (validationResult != null && validationResult.IsValid)
                {
                    if (!string.IsNullOrEmpty(_role) && !validationResult.Roles.Contains(_role))
                    {
                        context.Result = new ForbidResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Error = "Ошибка валидации токена"
                });
            }
        }
        catch (Exception ex)
        {
            context.Result = new ObjectResult(new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Error = "Сервис авторизации недоступен",
                Details = ex.Message
            });
        }
    }
}