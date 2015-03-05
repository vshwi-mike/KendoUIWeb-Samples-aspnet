using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace KendoUIWeb.Controllers
{
    public class SourceFileController : Controller
    {
        public ActionResult AsText(string path){
            if (string.IsNullOrWhiteSpace(path)) {
                return HttpNotFound();
            }
            if (!path.StartsWith("/")) {
                path = "/" + path;
            }
            path = path.ToLower();
            if (path.EndsWith(".config") || path.EndsWith(".dll") || path.EndsWith(".pdb")) {
                return HttpNotFound();
            }

            try {
                string filename = Server.MapPath(path);

                if (!System.IO.File.Exists(filename)) {
                    return HttpNotFound();
                }

                var result = new FilePathResult(filename, "text/plain"); ;
                return result;

            } catch (Exception) {
                return HttpNotFound();
            }
        }
    }
}