namespace AutofacPOC.Services
{
    public class DefaultService : ITenantService
    {
        public string invoke()
        {
            return "You invoked the default service!";
        }
    }
}
