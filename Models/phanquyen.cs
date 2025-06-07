using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class phanquyen : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Kiểm tra có phải controller trong area Admin hay không
        var area = context.RouteData.Values["area"]?.ToString();
        if (area == "Admin")
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                context.Result = new RedirectToActionResult("index", "Login", new { area = "" });
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Không làm gì sau khi action chạy
    }
}
