namespace TKApp.Shared.Interfaces
{
    public interface ITenantProvider
    {
        int? GetTenantId();
        int? GetCurrentUserId();
        void SetTenantId(int? tenantId);
    }
}
