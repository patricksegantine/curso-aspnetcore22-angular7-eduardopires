using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Eventos.IO.Infra.CrossCutting.AspNetFilters
{
    public class GlobalActionLoggerFilter : IActionFilter
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostingEnvironment _env;

        public GlobalActionLoggerFilter(
            ILogger<GlobalExceptionHandlingFilter> logger,
            IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_env.IsDevelopment())
            {
                var data = new
                {
                    Version = "v1.0",
                    User = context.HttpContext.User.Identity.Name,
                    IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    HostName = context.HttpContext.Request.Host.ToString(),
                    AreaAccessed = context.HttpContext.Request.GetDisplayUrl(),
                    Action = context.ActionDescriptor.DisplayName,
                    TimeStamp = DateTime.Now
                };

                _logger.LogInformation(1, "Log de Auditoria :" + data.ToString());
            }

            if (_env.IsProduction())
            {
                //ElmahioAPI
                //_logger.LogError();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new System.NotImplementedException();
        }


    }
}
