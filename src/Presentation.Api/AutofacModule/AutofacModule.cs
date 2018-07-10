using Autofac;
using Promociones.Domain.Core;
using Promociones.Application;

namespace Promociones.Presentation.Api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PromocionesManager>().As<IPromocionesManager>();
            builder.RegisterType<FechaSistema>().As<IDateTime>();

        }
    }
}
