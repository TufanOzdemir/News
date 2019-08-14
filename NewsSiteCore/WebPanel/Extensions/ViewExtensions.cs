using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebPanel.Extensions
{
    public static class ViewExtensions
    {
        public static async Task<string> ToString(this PartialViewResult partialView, ActionContext actionContext)
        {
            using (var writer = new StringWriter())
            {
                var services = actionContext.HttpContext.RequestServices;
                var executor = services.GetRequiredService<PartialViewResultExecutor>();
                var view = executor.FindView(actionContext, partialView).View;
                var viewContext = new ViewContext(actionContext, view, partialView.ViewData, partialView.TempData, writer, new HtmlHelperOptions());
                await view.RenderAsync(viewContext);
                return writer.ToString();
            }
        }
    }
}