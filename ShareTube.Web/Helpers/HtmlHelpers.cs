using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ShareTube.Helpers
{
    public static class HtmlHelperExtensions
    {
        //public static string MyCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlLabelAttributes = null, object htmlCheckBoxAttributes = null)
        //{
        //    var checkbox = htmlHelper.CheckBoxFor(expression, htmlCheckBoxAttributes);

        //    var labelTag = new TagBuilder("label");
        //    var checkboxName = ExpressionHelper.GetExpressionText(expression);
        //    labelTag.AddCssClass("checkbox");
        //    labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
        //    labelTag.
        //    labelTag.InnerHtml = checkbox.ToString() + LabelHelper(ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), checkboxName);

        //    labelTag.ToString();
        //}

        //private static MvcHtmlString LabelHelper(ModelMetadata metadata, string fieldName)
        //{
        //    string labelText;
        //    var displayName = metadata.DisplayName;

        //    if (displayName == null)
        //    {
        //        var propertyName = metadata.PropertyName;

        //        labelText = propertyName ?? fieldName.Split(new[] { '.' }).Last();
        //    }
        //    else
        //    {
        //        labelText = displayName;
        //    }

        //    if (string.IsNullOrEmpty(labelText))
        //    {
        //        return MvcHtmlString.Empty;
        //    }

        //    return new MvcHtmlString(labelText);
        //}
    }
}