namespace WebApplication1.Logger
{
    using System.Security.Claims;

    using Serilog;

    public class HttpContextInfo
    {
        public string IpAddress { get; set; }
        public string Host { get; set; }
        public string Protocol { get; set; }
        public string Scheme { get; set; }
    }

    public static class Enricher
    {
        internal static void HttpRequestEnricher(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var httpContextInfo = new HttpContextInfo
            {
                Protocol = httpContext.Request.Protocol,
                Scheme = httpContext.Request.Scheme,
                IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
                Host = httpContext.Request.Host.ToString(),
            };

            diagnosticContext.Set("HttpContext", httpContextInfo, true);
        }
    }

}
