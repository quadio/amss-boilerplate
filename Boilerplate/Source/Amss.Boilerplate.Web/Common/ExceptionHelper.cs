namespace Amss.Boilerplate.Web.Common
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Mvc;

    using Amss.Boilerplate.Business.Exceptions;

    internal static class ExceptionHelper
    {
        public static void FillFrom(this ModelStateDictionary modelStateDictionary, BusinessValidationException exception)
        {
            Contract.Assert(exception != null);
            Contract.Assert(modelStateDictionary != null);
            var pairs = (from info in exception.Errors
                         let property = !string.IsNullOrEmpty(info.PropertyName) ? info.PropertyName : info.ErrorCode
                         let index = property.LastIndexOf('.')
                         let key = index > 0 && index < property.Length ? property.Substring(index + 1) : property
                         select new { key, info.ErrorMessage }).ToList();

            foreach (var p in pairs.Where(i => !string.IsNullOrEmpty(i.key)))
            {
                // try to match business property name and model property name
                modelStateDictionary.AddModelError(p.key, p.ErrorMessage);
            }

            var messages = pairs.Where(i => string.IsNullOrEmpty(i.key)).Select(i => i.ErrorMessage).ToArray();
            if (messages.Length > 0)
            {
                modelStateDictionary.AddModelError(string.Empty, string.Join("\r\n", messages));
            }
        }
    }
}