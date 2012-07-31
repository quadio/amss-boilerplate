namespace Amss.Boilerplate.Web.Common.Binders
{
    using System;
    using System.Web.Mvc;

    using Mvc.JQuery.Datatables;

    public class DataTablesModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var obj = new DataTablesParam();
            var request = controllerContext.HttpContext.Request.Params;

            obj.iDisplayStart = Convert.ToInt32(request["iDisplayStart"]);
            obj.iDisplayLength = Convert.ToInt32(request["iDisplayLength"]);
            obj.iColumns = Convert.ToInt32(request["iColumns"]);
            obj.sSearch = request["sSearch"];
            obj.bEscapeRegex = Convert.ToBoolean(request["bEscapeRegex"]);
            obj.iSortingCols = Convert.ToInt32(request["iSortingCols"]);
            obj.sEcho = int.Parse(request["sEcho"]);

            for (int i = 0; i < obj.iColumns; i++)
            {
                obj.bSortable.Add(Convert.ToBoolean(request["bSortable_" + i]));
                obj.bSearchable.Add(Convert.ToBoolean(request["bSearchable_" + i]));
                obj.sSearchColumns.Add(request["sSearch_" + i]);
                obj.bEscapeRegexColumns.Add(Convert.ToBoolean(request["bEscapeRegex_" + i]));
                obj.iSortCol.Add(Convert.ToInt32(request["iSortCol_" + i]));
                obj.sSortDir.Add(request["sSortDir_" + i]);
            }

            return obj;
        }
    }
}