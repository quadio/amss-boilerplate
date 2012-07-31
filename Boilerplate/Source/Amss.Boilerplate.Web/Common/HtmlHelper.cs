namespace Amss.Boilerplate.Web.Common
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using Amss.Boilerplate.Common.Configuration;

    public static class HtmlHelperExtended
    {
        public static MvcHtmlString ApplicationVersion<TModel>(this HtmlHelper<TModel> html)
        {
            var version = typeof(HtmlHelperExtended).Assembly.EffectiveVersion();
            
            return MvcHtmlString.Create(version);
        }

        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, System.Linq.Expressions.Expression<Func<TModel, TValue>> expression)
        {
            var label = html.LabelFor(expression);

            var required = label.ToHtmlString().Replace("<label", @"<label class=""required""");

            return MvcHtmlString.Create(required);
        }

        public static IHtmlString ActiveItem<TModel>(
            this HtmlHelper<TModel> html, 
            string controller,
            string action)
        {
            var currentAction = html.ViewContext.RouteData.GetRequiredString("action");
            var currentController = html.ViewContext.RouteData.GetRequiredString("controller");
            var result = controller == currentController && action == currentAction ? "class=\"active\"" : string.Empty;
            return html.Raw(result);
        }

        public static IHtmlString ActiveController<TModel>(
            this HtmlHelper<TModel> html,
            params string[] controllers)
        {
            Contract.Assert(controllers != null);

            var currentController = html.ViewContext.RouteData.GetRequiredString("controller");
            var result = controllers.Any(c => c.Equals(currentController, StringComparison.InvariantCultureIgnoreCase)) ? "class=\"active\"" : string.Empty;
            return html.Raw(result);
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string controller, string action, object routeValues, string imagePath, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            var imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");
            anchorBuilder.MergeAttribute("href", url.Action(action, controller, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            var anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }
    }
}