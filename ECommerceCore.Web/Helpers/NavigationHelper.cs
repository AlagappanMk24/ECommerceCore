using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Web.Helpers
{
    public static class NavigationHelper
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action = null, string area = null)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeController = routeData.Values["controller"]?.ToString();
            var routeAction = routeData.Values["action"]?.ToString();
            var routeArea = routeData.Values["area"]?.ToString();

            var isController = string.Equals(controller, routeController, StringComparison.OrdinalIgnoreCase);
            var isAction = action == null || string.Equals(action, routeAction, StringComparison.OrdinalIgnoreCase);
            var isArea = area == null || string.Equals(area, routeArea, StringComparison.OrdinalIgnoreCase);

            return isController && isAction && isArea ? "active" : "";
        }
    }
}
