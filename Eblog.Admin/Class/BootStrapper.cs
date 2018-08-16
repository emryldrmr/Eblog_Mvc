using Autofac;
using Autofac.Integration.Mvc;
using Eblog.Core.Infrastructure;
using Eblog.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eblog.Admin.Class
{
    public class BootStrapper
    {
        // Boot aşamasında çalışacak

        public static void RunConfig()
        {
            BuildAutoFac();
        }

        private static void BuildAutoFac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<KullaniciRepository>().As<IKullaniciRepository>();
            builder.RegisterType<KategoriRepository>().As<IKategoriRepository>();
            builder.RegisterType<EtiketRepository>().As<IEtiketRepository>();
            builder.RegisterType<MakaleRepository>().As<IMakaleRepository>();
            builder.RegisterType<YetkiRepository>().As<IYetkiRepository>();
            builder.RegisterType<YorumRepository>().As<IYorumRepository>();
            builder.RegisterType<BizeUlasinRepository>().As<IBizeUlasinRepository>();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}