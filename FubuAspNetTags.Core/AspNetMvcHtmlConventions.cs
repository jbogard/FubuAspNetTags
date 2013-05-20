using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Elements;
using HtmlTags;

namespace FubuAspNetTags.Core
{
    public class DefaultAspNetMvcHtmlConventions : HtmlConventionRegistry
    {
        public DefaultAspNetMvcHtmlConventions()
        {
            Editors.IfPropertyIs<bool>().BuildBy(req =>
            {
                var check = new CheckboxTag(req.Value<bool>()).Attr("value", req.StringValue());
                var hidden = new HiddenTag().Attr("name", req.ElementId).Attr("value", false);
                return check.Append(hidden);
            });

            Editors.IfPropertyIs<IEnumerable<SelectListItem>>().BuildBy(req =>
            {
                var list = req.Value<IEnumerable<SelectListItem>>();
                var drop = new SelectTag();
                foreach (var item in list)
                {
                    drop.Add("option").Attr("value", item.Value).Attr("selected", item.Selected).Text(item.Text);
                }
                return drop;
            });

            Editors.Always.ModifyWith(AddElementName);

            //Editors.Always.BuildBy(TagActionExpression.BuildTextbox);
            //Displays.Always.BuildBy(req => new HtmlTag("span").Text(req.StringValue()));
            //Labels.Always.BuildBy(req => new HtmlTag("span").Text(req.Accessor.Name));
        }

        private static Regex idRegex = new Regex(@"[\.\[\]]");

        public static void AddElementName(ElementRequest request)
        {
            if (request.OriginalTag.IsInputElement())
            {
                //request.CurrentTag.Attr("name", request.ElementId);
                request.CurrentTag.Attr("id", idRegex.Replace(request.ElementId, "_"));
            }
        }
    }
}