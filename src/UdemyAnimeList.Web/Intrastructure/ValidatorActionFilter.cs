using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text.Json;

namespace UdemyAnimeList.Web.Infrastructure
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (!controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.Method == "GET")
                {
                    filterContext.Result = new BadRequestResult();
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 400;
                    filterContext.Result = new ContentResult
                    {
                        Content = JsonSerializer.Serialize(controller.ModelState),
                        ContentType = "application/json"
                    };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
