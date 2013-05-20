
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using FubuMVC.Core.UI.Elements;
using HtmlTags;
using Microsoft.Practices.ServiceLocation;

namespace FubuAspNetTags.Core
{
    public static class FubuAspNetTagExtensions
    {
        public static HtmlTag Input<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();
            return generator.InputFor(expression);
        }

        public static HtmlTag Label<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();
            return generator.LabelFor(expression);
        }

        public static HtmlTag Display<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> expression) where T : class
        {
            var generator = GetGenerator<T>();
            return generator.DisplayFor(expression);
        }

        private static IElementGenerator<T> GetGenerator<T>() where T : class
        {
            var generator = ServiceLocator.Current.GetInstance<IElementGenerator<T>>();
            return generator;
        }
    }
}
