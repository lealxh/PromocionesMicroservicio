using Autofac;
using Promociones.Domain.Core;
using Promociones.Application;
using Promociones.Infrastructure;

namespace Promociones.Presentation.Api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PromocionesManager>().As<IPromocionesManager>();
            builder.RegisterType<MedioPagoManager>().As<IMedioPagoManager>();
            builder.RegisterType<ProductosManager>().As<IProductosManager>();
            builder.RegisterType<FechaSistema>().As<IDateTime>();
            builder.RegisterType<RequestManager>().As<IRequestsManager>();
            

        }
    }
}
