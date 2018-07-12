using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Promociones.Application;
using Promociones.Domain.Core;
using Promociones.Domain.Entities;
using Promociones.Infrastructure.Persistence;
using Promociones.Presentation.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class PromocionesTests
    {

        private List<Promocion> testDataPromociones;
        private List<ProductoCategoria> testDataCategorias;
        private List<MedioPago> testDataMediosPago;
        private PromocionesDbContext context;
        public PromocionesTests()
        {

            
            testDataPromociones = new List<Promocion>()
            {
                new Promocion()
                {
                    Id=1,
                    Activo = true,
                    TipoMedioPagoIds=null,
                    EntidadFinancieraIds = null,
                    MedioPagoIds = new List<int>{1,2,3},
                    ProductoCategoriaIds=new List<int> { 1,2,3,4,5,6},
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 15,
                    FechaInicio = new DateTime(2018, 06, 01),
                    FechaFin = new DateTime(2019, 06, 01)
                },
                new Promocion()
                {

                    Id=2,
                    Activo = true,
                    TipoMedioPagoIds=null,
                    EntidadFinancieraIds = new List<int>{1},
                    MedioPagoIds = null,
                    ProductoCategoriaIds=new List<int> {1,2,3,4,5,6},
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 10,
                    FechaInicio = new DateTime(2018, 06, 01),
                    FechaFin = new DateTime(2019, 06, 01)
                },
                new Promocion()
                {
                    Id=3,
                    Activo = true,
                    TipoMedioPagoIds=new List<int>{1,2},
                    EntidadFinancieraIds = null,
                    MedioPagoIds = null,
                    ProductoCategoriaIds=new List<int> { 1,2,3,4,5,6},
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 20,
                    FechaInicio = new DateTime(2018, 06, 01),
                    FechaFin = new DateTime(2019, 06, 01)
                },
                new Promocion()
                {
                    Id=4,
                    Activo = true,
                    TipoMedioPagoIds=null,
                    EntidadFinancieraIds = null,
                    MedioPagoIds = new List<int>{10},
                    ProductoCategoriaIds=new List<int> { 1,2,3,4,5,6},
                    MaxCantidadDeCuotas = null,
                    PorcentajeDecuento = 25,
                    FechaInicio = new DateTime(2018, 06, 01),
                    FechaFin = new DateTime(2019, 06, 01)
                },
                new Promocion()
                {
                    Id =5,
                    Activo = true,
                    TipoMedioPagoIds= new List<int> {1},
                    EntidadFinancieraIds = new List<int>{1},
                    MedioPagoIds = null,
                    ProductoCategoriaIds=new List<int> { 1,2,3,4,5,6},
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 5,
                    FechaInicio = new DateTime(2018, 06, 01),
                    FechaFin = new DateTime(2019, 06, 01)
                },
                 new Promocion()
                {
                    Id =6,
                    Activo = true,
                    TipoMedioPagoIds= new List<int> {1},
                    EntidadFinancieraIds = new List<int>{1},
                    MedioPagoIds = new List<int>{1},
                    ProductoCategoriaIds=new List<int> {1,2,3,4,5,6},
                    MaxCantidadDeCuotas = 12,
                    PorcentajeDecuento = 5,
                    FechaInicio = new DateTime(2018, 01, 01),
                    FechaFin = new DateTime(2018, 06, 01)
                }
    };
            testDataCategorias = new List<ProductoCategoria>
            {
                new ProductoCategoria() { Id = 1, Descripcion = "Tvs" },
                new ProductoCategoria() { Id = 2, Descripcion = "Heladeras" },
                new ProductoCategoria() { Id = 3, Descripcion = "Lavarropas" },
                new ProductoCategoria() { Id = 4, Descripcion = "Celulares" },
                new ProductoCategoria() { Id = 5, Descripcion = "Notebooks" },
                new ProductoCategoria() { Id = 6, Descripcion = "Gaming" }
            };
            testDataMediosPago = new List<MedioPago>()
            {
                new MedioPago(){ Id=1,Descripcion="MercadoPago"},
                new MedioPago(){ Id=2,Descripcion="Visa Debito"},
                new MedioPago(){ Id=3,Descripcion="Visa"},
                new MedioPago(){ Id=4,Descripcion="MasterCard"},
                new MedioPago(){ Id=5,Descripcion="American Express"}
            };


            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            var options = new DbContextOptionsBuilder<PromocionesDbContext>().UseInMemoryDatabase(databaseName: "Promociones").Options;
            context = new PromocionesDbContext(options);
            context.AddRange(testDataPromociones);
            context.SaveChanges();


        }

        [Fact]
        public async void GetAllPromociones_ShouldReturnAllData()
        {
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));
            var result = await controller.GetAllPromociones();

            OkObjectResult ResultGet = result as OkObjectResult;
            List<Promocion> values = ResultGet.Value as List<Promocion>;
            Assert.Equal(values.Count, testDataPromociones.Count);

            

        }


        [Fact]
        public async void GetPromocionesVigentes_ShouldReturn1Record()
        {

            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.GetPromocionesVigentes();

            OkObjectResult ResultGet = result as OkObjectResult;
            List<Promocion> value = ResultGet.Value as List<Promocion>;
            Assert.True(1 == value.Count);

            

        }

        [Fact]
        public async void GetPromocionesVigentesEnFecha_ShouldReturn1Record()
        {

            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.GetPromocionesVigentesEnFecha(new DateTime(2018, 02, 01));

            OkObjectResult ResultGet = result as OkObjectResult;
            List<Promocion> value = ResultGet.Value as List<Promocion>;
            Assert.True(1 == value.Count);

        }

        [Fact]
        public async void GetPromocionesVigentesEnFecha_ShouldReturn0Records()
        {
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));


            var result = await controller.GetPromocionesVigentesEnFecha(new DateTime(2017, 02, 01));
            OkObjectResult ResultGet = result as OkObjectResult;

            List<Promocion> value = ResultGet.Value as List<Promocion>;
            Assert.True(0 == value.Count);

            
        }

        [Fact]
        public async void GetPromocionesPromocionesPorVenta_ShouldReturn1Record()
        {

            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.GetPromocionesVenta(new QueryPromocionesDTO()
            {
                CantidadDeCuotas = 0,
                EntidadFinancieraId = 0,
                ProductoCategoriaId = 5,
                MedioPagoId = 10,
                TipoMedioPagoId = 0

            });

            OkObjectResult ResultGet = result as OkObjectResult;
            List<Promocion> value = ResultGet.Value as List<Promocion>;
            Assert.True(1 == value.Count);
            

        }

        [Fact]
        public async void ModificarPromociones_ShouldReturnOk()
        {


            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));
            var registroModificado = new List<int> { 1 };
            var dataModificar = new PromocionUpdateDTO()
            {
                Id = 1,
                TipoMedioPagoIds = null,
                EntidadFinancieraIds = null,
                MedioPagoIds = registroModificado,
                ProductoCategoriaIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                MaxCantidadDeCuotas = 12,
                PorcentajeDecuento = 15
            };

            var result = await controller.ModificarPromocion(dataModificar);
            var dataModificada = context.Promociones.SingleOrDefault(x => x.Id == dataModificar.Id);
            Assert.True(result is OkObjectResult);
            Assert.True(registroModificado[0].Equals(dataModificada.MedioPagoIds[0]));
               
        
        }

        [Fact]
        public async void ModificarPromociones_ShouldReturn404NotFound()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018

            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.ModificarPromocion(new PromocionUpdateDTO()
            {
                Id = 10,//Id 10 no existe
                TipoMedioPagoIds = null,
                EntidadFinancieraIds = null,
                MedioPagoIds = new List<int> { 1, 2 },
                ProductoCategoriaIds = new List<int> { 1, 2, 3, 4, 5, 6 },//promocion 7 no valida
                MaxCantidadDeCuotas = 12,
                PorcentajeDecuento = 15,
            });

            Assert.True(result is NotFoundObjectResult);

            
        }

        [Fact]
        public async void ModificarPromociones_ShouldReturnInvalidMedioPago()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018

            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));
            var result = await controller.ModificarPromocion(new PromocionUpdateDTO()
            {
                Id = 1,
                TipoMedioPagoIds = null,
                EntidadFinancieraIds = null,
                MedioPagoIds = new List<int> { 1, 2, 20 },//medio de pago 20 no valida
                ProductoCategoriaIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                MaxCantidadDeCuotas = 12,
                PorcentajeDecuento = 15,
            });

            Assert.True(result is BadRequestObjectResult);

            
        }

        [Fact]
        public async void ModificarPromociones_ShouldReturnInvalidCategoria()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));
            var result = await controller.ModificarPromocion(new PromocionUpdateDTO()
            {
                Id = 1,
                TipoMedioPagoIds = null,
                EntidadFinancieraIds = null,
                MedioPagoIds = new List<int> { 1, 2},
                ProductoCategoriaIds = new List<int> { 1, 2, 3, 4, 5, 6 ,20},
                MaxCantidadDeCuotas = 12,
                PorcentajeDecuento = 15,
            });


            Assert.True(result is BadRequestObjectResult);

            
        }

        [Fact]
        public async void DeletePromociones_ShouldDelete2Records()
        {

            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
          
            int elementosAntes = context.Promociones.Count(x => x.Activo);

            var controller = new PromocionesController(new PromocionesManager(context, null,null,null));
            var result = await controller.DeletePromocion(new PromocionDeleteDTO()
            {
                PromocionesIds = new List<int> { 1, 2, 20 }
            });

            OkObjectResult ResultGet = result as OkObjectResult;

            int elementosEliminados = (int)ResultGet.Value;
            int elementosDespues = context.Promociones.Count(x => x.Activo);
            Assert.True(elementosEliminados == 2);

            Assert.True(elementosAntes - elementosEliminados == elementosDespues);

            

        }

        [Fact]
        public async void DeletePromociones_ShoulReturn404NotFound()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            int elementosAntes = context.Promociones.Count(x => x.Activo);
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.DeletePromocion(new PromocionDeleteDTO()
            {
                PromocionesIds = new List<int> { 10 }
            });


            Assert.True(result is NotFoundObjectResult);

            
        }

        [Fact]
        public async void ValidarVigencia_ShoulReturn404NotFound()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            int elementosAntes = context.Promociones.Count(x => x.Activo);
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 02, 01));


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.GetVigenciaPromocion(20);
          
            Assert.True(result is NotFoundObjectResult);


        }

        [Fact]
        public async void ValidarVigencia_ShoulReturnNoVigente()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            int elementosAntes = context.Promociones.Count(x => x.Activo);
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 08, 01));// fecha moqueada a la segunda mitad del año


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.GetVigenciaPromocion(6);//promocion 6 vigente solo por la primera mitad del año
            OkObjectResult res = result as OkObjectResult;
            bool vigente = (bool)res.Value;
            
            Assert.False(vigente);


        }

        [Fact]
        public async void CrearPromocion_ShoulReturnDuplicidadException()
        {
            //el contexto esta poblado con promociones vigentes este año desde junio a junio del 2018 y una promocion de enero a junio de 2018
            int elementosAntes = context.Promociones.Count(x => x.Activo);
            Mock<ProductosManager> mokPR = new Mock<ProductosManager>();
            mokPR.Setup(x => x.GetCategorias()).Returns(testDataCategorias);

            Mock<MedioPagoManager> mokMP = new Mock<MedioPagoManager>();
            mokMP.Setup(x => x.GetMedioPago(1)).Returns(testDataMediosPago[0]);
            mokMP.Setup(x => x.GetMedioPago(2)).Returns(testDataMediosPago[1]);
            mokMP.Setup(x => x.GetMedioPago(3)).Returns(testDataMediosPago[2]);
            mokMP.Setup(x => x.GetMedioPago(4)).Returns(testDataMediosPago[3]);
            mokMP.Setup(x => x.GetMedioPago(5)).Returns(testDataMediosPago[4]);

            Mock<IDateTime> mockDateTimeNow = new Mock<IDateTime>();
            mockDateTimeNow.Setup(x => x.Now).Returns(new DateTime(2018, 08, 01));// fecha moqueada a la segunda mitad del año


            var controller = new PromocionesController(new PromocionesManager(context, mockDateTimeNow.Object, mokPR.Object, mokMP.Object));

            var result = await controller.CrearPromocion(new PromocionInsertDTO()
            {
                Activo = true,
                TipoMedioPagoIds = new List<int> { 1 },
                EntidadFinancieraIds = new List<int> { 1 },
                MedioPagoIds = new List<int> { 1 },
                ProductoCategoriaIds = new List<int> { 1, 2, 3, 4, 5, 6 },
                MaxCantidadDeCuotas = 12,
                PorcentajeDecuento = 5,
                FechaInicio = new DateTime(2017, 06, 01),
                FechaFin = new DateTime(2018, 06, 01)
            });//promocion 6 vigente solo por la primera mitad del año
          
            Assert.True(result is BadRequestObjectResult);





        }
    }
}

    

