using System;
using Moq;
using Xunit;

using Microsoft.EntityFrameworkCore;

using Promociones.Infrastructure.Persistence;

using Promociones.Domain.Core;
using Promociones.Domain.Entities;
using Promociones.Presentation.Api.Controllers;
using Promociones.Application;

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class PromocionesTests
    {

        List<Promocion> promociones;
        List<ProductoCategoria> categorias;
        List<MedioPago> mediosPago;

        Mock<IDateTime> mockDateTimeNow;
        Mock<IProductosManager> mockCategorias;
        Mock<IMedioPagoManager> mockMedioPago;

        public PromocionesTests()
        {
          
            promociones = new List<Promocion>()
            {
                new Promocion()
                {
                    Id=1,
                    Activo = true,
                    EntidadFinancieraIds = new List<int> { 1, 2 },
                    MaxCantidadDeCuotas = 36,
                    PorcentajeDecuento = 10,
                    FechaInicio = new DateTime(2018, 01, 01),
                    FechaFin = new DateTime(2018, 12, 31)
                },
                new Promocion()
                {
                    Id=2,
                    Activo = true,
                    EntidadFinancieraIds = new List<int> { 3, 4 },
                    MaxCantidadDeCuotas = 36,
                    PorcentajeDecuento = 20,
                    FechaInicio = new DateTime(2018, 01, 01),
                    FechaFin = new DateTime(2018, 12, 31)
                },
                new Promocion()
                {
                    Id=3,
                    Activo = true,
                    EntidadFinancieraIds = new List<int> { 5, 6 },
                    MaxCantidadDeCuotas = 36,
                    PorcentajeDecuento = 30,
                    FechaInicio = new DateTime(2018, 01, 01),
                    FechaFin = new DateTime(2018, 12, 31)
                }
            };
            categorias = new List<ProductoCategoria>
            {
                new ProductoCategoria() { Id = 1, Descripcion = "Tvs" },
                new ProductoCategoria() { Id = 2, Descripcion = "Heladeras" },
                new ProductoCategoria() { Id = 3, Descripcion = "Lavarropas" },
                new ProductoCategoria() { Id = 4, Descripcion = "Celulares" },
                new ProductoCategoria() { Id = 5, Descripcion = "Notebooks" },
                new ProductoCategoria() { Id = 6, Descripcion = "Gaming" }
            };
            mediosPago = new List<MedioPago>()
            {
                new MedioPago(){ Id=1,Descripcion="MercadoPago"},
                new MedioPago(){ Id=2,Descripcion="Visa Debito"},
                new MedioPago(){ Id=3,Descripcion="Visa"},
                new MedioPago(){ Id=4,Descripcion="MasterCard"},
                new MedioPago(){ Id=5,Descripcion="American Express"}
            };

            mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 01, 01));

            mockCategorias = new Mock<IProductosManager>();
            mockCategorias.Setup(x => x.GetCategorias()).Returns(this.categorias);

            mockMedioPago = new Mock<IMedioPagoManager>();
            mockMedioPago.Setup(x => x.GetMedioPago(It.IsAny<int>())).Returns(mediosPago[0]);



        }
        private PromocionesDbContext GetContextFilled(List<Promocion> data)
        {
            var options = new DbContextOptionsBuilder<PromocionesDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            PromocionesDbContext context = new PromocionesDbContext(options);
            context.Promociones.AddRange(data);
            return context;
           
        }

    }
}

