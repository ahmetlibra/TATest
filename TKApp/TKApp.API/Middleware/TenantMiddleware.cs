using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using TKApp.Shared.Interfaces;

namespace TKApp.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            // Try to get tenant ID from the request
            var tenantId = GetTenantIdFromRequest(context);
            
            // If tenant ID is found in the request, set it in the tenant provider
            if (tenantId.HasValue)
            {
                tenantProvider.SetTenantId(tenantId.Value);
            }
            
            // Call the next middleware in the pipeline
            await _next(context);
        }

        private int? GetTenantIdFromRequest(HttpContext context)
        {
            // 1. Try to get from route values (e.g., /api/tenants/123/...)
            if (context.Request.RouteValues.TryGetValue("tenantId", out var tenantIdValue) && 
                int.TryParse(tenantIdValue?.ToString(), out var tenantId))
            {
                return tenantId;
            }

            // 2. Try to get from query string (e.g., ?tenantId=123)
            if (context.Request.Query.TryGetValue("tenantId", out var queryValue) && 
                int.TryParse(queryValue.FirstOrDefault(), out var queryTenantId))
            {
                return queryTenantId;
            }

            // 3. Try to get from headers (e.g., X-Tenant-Id: 123)
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue) && 
                int.TryParse(headerValue.FirstOrDefault(), out var headerTenantId))
            {
                return headerTenantId;
            }

            // 4. Try to get from JWT claim (handled by JWT middleware)
            // The claim will be available in the User property of the HttpContext
            
            return null;
        }
    }
}
