using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //WebAPIからの戻り値の形式をJSONのみに限定。
            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            //Employeeテーブルなどの自己結合でJSONシリアライズ時に循環参照エラーになるのを防ぐ。
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //別ドメインからでも呼べる様にCORS(Cross-Origin Requests)を使用。
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors); 

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
