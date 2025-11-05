using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;

public class AdminCheckMiddleware
{
    private readonly RequestDelegate _next;

    public AdminCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, GreenShopContext db)
    {
        var path = context.Request.Path.Value;

        // Chỉ check những url /Products/... hoặc /Categories/...
        if (path.StartsWith("/Products", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/Categories", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/Orders", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/Accounts", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/Dashboard", StringComparison.OrdinalIgnoreCase))
        {
            var token = context.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/Shop/Product");
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (!Guid.TryParse(idClaim, out Guid userId))
            {
                context.Response.Redirect("/Shop/Product");
                return;
            }

            var user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.Role == null || !user.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Redirect("/Shop/Product");
                return;
            }
        }

        // Nếu ok → next middleware
        await _next(context);
    }
}
