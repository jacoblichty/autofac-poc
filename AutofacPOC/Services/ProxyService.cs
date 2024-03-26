namespace AutofacPOC.Services
{
    public class ProxyService : IProxy
    {
        public ITenantService TenantService;

        public ProxyService(ITenantService tenantService) {
            TenantService = tenantService;
        }

        public string invoke()
        {
            return TenantService.invoke();
        }
    }
}
