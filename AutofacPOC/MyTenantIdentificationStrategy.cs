using Autofac.Multitenant;
using System.IdentityModel.Tokens.Jwt;

namespace AutofacPOC
{
    public class MyTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private IHttpContextAccessor __httpContextAccessor;

        public MyTenantIdentificationStrategy(IHttpContextAccessor httpContextAccessor)
        {
            __httpContextAccessor = httpContextAccessor;
        }

        public bool TryIdentifyTenant(out object tenantId)
        {
            tenantId = null;
            try
            {
                var context = __httpContextAccessor.HttpContext;
                DecodeBearerToken(context);
                if (context != null && context.Request != null)
                {
                    // casting to string is necessary for some reason... otherwise the value doesn't stick
                    tenantId = (string)context.Request.Query["tenant"];
                }
            }
            catch (Exception)
            {
                // Happens at app startup in IIS 7.0
            }
            
            return tenantId != null;
        }

        public void DecodeBearerToken(HttpContext context)
        {
            if (context == null)
            {
                return;
            }
            var token = context.Request.Headers.Authorization.First().Split("Bearer ")[1];
            var handler = new JwtSecurityTokenHandler();
            var json = (JwtSecurityToken)handler.ReadToken(token);
            var email = json.Claims.Where(c => c.Type == "emails");
        }
    }
}
