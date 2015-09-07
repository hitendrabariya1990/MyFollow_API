using System.Web.Http;

namespace MyFollow
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
               name: "ProductOwner",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
               name: "EndUser",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
