using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using TKApp.Shared.Interfaces;

namespace TKApp.Shared.Services
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int? _tenantId;
        private int? _currentUserId;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetTenantId()
        {
            if (_tenantId.HasValue)
                return _tenantId;

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            // Try to get tenant ID from route or query string
            if (httpContext.Request.RouteValues.TryGetValue("tenantId", out var tenantIdValue) && 
                int.TryParse(tenantIdValue?.ToString(), out var tenantId))
            {
                _tenantId = tenantId;
                return _tenantId;
            }

            // Try to get from query string
            if (httpContext.Request.Query.TryGetValue("tenantId", out var queryValue) && 
                int.TryParse(queryValue.ToString(), out var queryTenantId))
            {
                _tenantId = queryTenantId;
                return _tenantId;
            }

            // Try to get from JWT claim
            var tenantClaim = httpContext.User?.FindFirst("tenantId")?.Value;
            if (!string.IsNullOrEmpty(tenantClaim) && int.TryParse(tenantClaim, out var claimTenantId))
            {
                _tenantId = claimTenantId;
                return _tenantId;
            }

            return null;
        }

        public int? GetCurrentUserId()
        {
            if (_currentUserId.HasValue)
                return _currentUserId;

            var httpContext = _httpContextAccessor.HttpContext;
            var userIdClaim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                _currentUserId = userId;
                return _currentUserId;
            }

            return null;
        }

        public void SetTenantId(int? tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
