using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuCore;
using FubuCore.Binding.InMemory;
using FubuCore.Binding.Values;
using FubuMVC.Core.Http.AspNet;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.UI.Templates;
using HtmlTags.Conventions;
using Microsoft.Practices.ServiceLocation;
using StructureMap.Configuration.DSL;
using IServiceLocator = Microsoft.Practices.ServiceLocation.IServiceLocator;

namespace FubuAspNetTags.Core.StructureMap
{
    public class AspNetMvcFubuRegistry : Registry
    {
        public AspNetMvcFubuRegistry(params HtmlConventionRegistry[] registries)
        {
            var htmlConventionLibrary = new HtmlConventionLibrary();
            registries.Select(r => r.Library).Each(library => htmlConventionLibrary.Import(library));
            htmlConventionLibrary.Import(new DefaultAspNetMvcHtmlConventions().Library);
            htmlConventionLibrary.Import(new DefaultHtmlConventions().Library);

            Scan(x =>
            {
                x.AssemblyContainingType<IFubuRequest>();
                x.AssemblyContainingType<ITypeResolver>();
                x.AssemblyContainingType<ITagGeneratorFactory>();
                x.AssemblyContainingType<FubuMVC.StructureMap.StructureMapFubuRegistry>();
                x.WithDefaultConventions();
                x.LookForRegistries();
            });
            //For<IFubuRequest>().Use<FubuRequest>();
            //For<ITypeResolver>().Use<TypeResolver>();
            //For<ITagGeneratorFactory>().Use<TagGeneratorFactory>();

            For<IValueSource>().AddInstances(c =>
            {
                c.Type<RequestPropertyValueSource>();
            });
            For<ITagRequestActivator>().AddInstances(c =>
            {
                c.Type<ElementRequestActivator>();
                c.Type<ServiceLocatorTagRequestActivator>();
            });
            For<HttpRequestBase>().HybridHttpOrThreadLocalScoped().Use(() => new HttpRequestWrapper(HttpContext.Current.Request));
            For<IBindingLogger>().Use<NulloBindingLogger>();
            For<ITypeResolverStrategy>().Use<TypeResolver.DefaultStrategy>();
            For<IElementNamingConvention>().Use<AspNetMvcElementNamingConvention>();
            For<IElementNamingConvention>().Use<DefaultElementNamingConvention>();
            For<HtmlConventionLibrary>().Use(htmlConventionLibrary);
            For(typeof(ITagGenerator<>)).Use(typeof(TagGenerator<>));
            For(typeof(IElementGenerator<>)).Use(typeof(ElementGenerator<>));
            For<IServiceLocator>().Use(() => ServiceLocator.Current);
            For<ITemplateWriter>().Use<TemplateWriter>();
        }
       
    }
}
