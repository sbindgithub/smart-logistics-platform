using SmartLogistics.Application.Common;

namespace SmartLogistics.API.Correlation
{
    public sealed class HttpCorrelationContext : ICorrelationContext
    {
        //private const string HeaderName = "X-Correlation-Id";
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public HttpCorrelationContext(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        //public string CorrelationId =>
        //    _httpContextAccessor.HttpContext?.Items[HeaderName]?.ToString()
        //    ?? "unknown";

        private const string HeaderName = "X-Correlation-Id";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpCorrelationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CorrelationId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (context == null) return Guid.NewGuid().ToString();

                if (!context.Request.Headers.TryGetValue(HeaderName, out var cid))
                {
                    cid = Guid.NewGuid().ToString();
                    context.Request.Headers[HeaderName] = cid;
                }

                return cid!;
            }
        }
    }

}
