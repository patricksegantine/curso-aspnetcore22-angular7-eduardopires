using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Eventos.IO.Services.Api.Configurations
{
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        /// <summary>
        /// Aplica em todos os Controllers a conveção da aplicação
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel =
                            AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelector = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelector.Any())
                {
                    foreach (var selectorModel in unmatchedSelector)
                    {
                        selectorModel.AttributeRouteModel = _centralPrefix;
                    }
                }
            }
        }
    }

    public static class MvcRouteExtensions
    {

        public static void AddApiVersioning(this IServiceCollection services, string routeUrl)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.Configure<MvcOptions>(options =>
            {
                options.UseCentralRoutePrefix(new RouteAttribute(routeUrl));
            });
        }


        public static void UseCentralRoutePrefix(this MvcOptions mvcOptions, IRouteTemplateProvider routeAttibute)
        {
            mvcOptions.Conventions.Insert(0, new RouteConvention(routeAttibute));
        }
    }
}
