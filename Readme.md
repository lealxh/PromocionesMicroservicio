# Promociones

## Consigna ##
Se requiere el desarollo de un microservicio que gestione, almacene y proporcione las promociones de la venta de productos de toda la compañía. El servicio será consumido por la venta en sucursal y online.

Se deben proveer las siguientes funcionalidades a traves de una API Rest:
- Ver lista de promociones.
- Ver lista de promociones vigentes.
- Ver lista de promociones vigentes en x fecha.
	- Parámetros:
    	- Fecha
- Ver lista de promociones vigentes para una venta: 
	- Parámetros:
		- Id medio de pago
		- Id tipo de medio de pago 
		- Id entidad financiera
		- cantidad de cuotas
		- Id categoría de producto.
- Modificar una promoción.
	- Parámetros:
		- Id promoción
		- Ids medios de pago
		- Ids tipos de medio de pago
		- Ids entidades financieras
		- Ids categorías de productos
		- Cantidad de cuotas
		- Porcentaje de descuento
- Eliminar una o varias promociones. (Borrado lógico)
	- Parámetros:
    	- Ids de promociones
- Validar si cierta promoción se encuentra vigente.
	- Parámetros:
    	- Id de promoción

En el Solution encontrará la arquitectura de la aplicación ya armada.

### Validaciones de negocio ###
- Duplicidad. (acoplamiento de fechas, ejemplo: %20 en efectivo del 01/01/2018 al 15/01/2018 y %30 en efectivo del 10/01/2018 al 20/01/2018)
- El medio de pago debe ser válido. (proporcionado por el servicio de medios de pago)
- Las categorías de los productos deben ser validas. (proporcionado por el servicio de productos)

## Modelo de datos ##
### Promocion ###
```C#
int[] TipoMedioPagoId //Representa el Id del Tipo de Medio de pago en la que aplica la promoción. Ejemplo: Visa , Amex, Efectivo (esta información esta guardada en el microservicio de Medio de Pago)

int[] EntidadFinancieraId // Representa el Id de la entidad Finaciera en la que aplica la promoción. Ejemplo: Banco Galicia, Banco Rio  (esta información esta guardada en el microservicio de Medio de Pago)

int[] MedioPagoId //Representa el Id del Medio de pago en la que aplica la promoción. Ejemplo: Tarjeta Visa Galicia Gold, Tarjeta Amex Frances Platinium, Efectivo Pesos, Efectivo Dollar (esta información esta guardada en el microservicio de Medio de Pago)

int? MaxCantidadDeCuotas //Representa la cantidad maxima de cuotas en la que aplica la promoción.

int[] ProductoCategoriaIds //Representa los Ids de las Categorias de los Items en la cual aplica la promoción (Estas categorias estan en el Microservicio de Productos)

decimal? PorcentajeDecuento: //Representa el porcentaje de descuento a Aplicar en el Producto

DateTime FechaInicio

DateTime FechaFin

Bool Activo

DateTime FechaCreacion

DateTime? FechaModificacion
```

## Ejemplos ##
- 15% de descuento para Banco Galicia Visa Platinium(Id: 1) y Banco Galicia Amex Platinium(Id: 2), pagando hasta en 12  cuotas entre el 01/06/2018 hasta 01/06/2019:
```json
{
	TipoMedioPagoId: null,
	EntidadFinancieraId: null,
	MedioPagoId: [1,2],
	MaxCantidadDeCuotas:12,
	PorcentajeDecuento: 15,
	FechaInicio: 01/06/2018,
	FechaFin: 01/06/2019,
	Active: true
}
```

- 10% de descuento para Todas las tarjetas del Banco Rio (EntidadFinancieraId: 1), pagando hasta en 12 cuotas entre el 01/06/2018 hasta 01/06/2019:
```json
{
	TipoMedioPagoId: null,
	EntidadFinancieraId: [1],
	MedioPagoId: null,
	MaxCantidadDeCuotas:12,
	PorcentajeDecuento: 10,
	FechaInicio: 01/06/2018,
	FechaFin: 01/06/2019,
	Active: true
}
```

- 20% de descuento para Todas las tarjetas Visa (TipoMedioPagoId: 1), Mastercard (TipoMedioPagoId: 2), pagando hasta en 12 cuotas entre el 01/06/2018 hasta 01/06/2019:
```json
{
	TipoMedioPagoId: [1,2],
	EntidadFinancieraId: null,
	MedioPagoId: null,
	MaxCantidadDeCuotas:12,
	PorcentajeDecuento: 20,
	FechaInicio: 01/06/2018,
	FechaFin: 01/06/2019,
	Active: true
}
```

- 25% de descuento para el pago en Efectivo en Pesos (MedioPagoId: 10) entre el 01/06/2018 hasta 01/06/2019:
```json
{
	TipoMedioPagoId: null,
	EntidadFinancieraId: null,
	MedioPagoId: [10],
	MaxCantidadDeCuotas:null,
	PorcentajeDecuento: 25,
	FechaInicio: 01/06/2018,
	FechaFin: 01/06/2019,
	Active: true
}
```

- 5% de descuento para el pago con todas las tarjetas Visa del Banco Rio, pagando hasta en 12 cuotas entre el 01/06/2018 hasta 01/06/2019:
```json
{
	TipoMedioPagoId: [1]
	EntidadFinancieraId: [1],
	MedioPagoId: null,
	MaxCantidadDeCuotas:12,
	PorcentajeDecuento: 5,
	FechaInicio: 01/06/2018,
	FechaFin: 01/06/2019,
	Active: true
}
```

## Especificiación Técnica ##

Obligatorios
- Framework: .Net Core >= 2.0 
- Base de datos: SQL Server/PostgreSQL
- Inyección de dependencias: Autofac
- Comunicación entre micro-servicios: Rest
- Utilizar mocks para dependencias de micro-servicios externos
- Tener en cuenta buenas prácticas
- Tener en cuenta convenciones de API Rest

Puntos extra (no obligatorios): 
- Utilizar Async/Await    
- Utilizar MongoDb
- Utilizar Swagger
- Utilizar una cache de Redis
- Se debe poder levantar con docker-compose
- Unit Tests
	- XUnit
	- Autofixture
	- Moq
- Versionado de la API

## Convenciones opcionales ##
- Es deseable que los setters de las properties de las entidades sean privados.
- Es deseable que en `domain.core` no haya ninguna implementación técnica, solo lógica de negocio.

## Servicios con los que se comunica ##

### Medios de pago ###

URL: mediodepago/{id}

Recibe: Id de medio de pago por Route

Devuelve:

```json
HttpStatusCode: 200
{
    Id: int
    Descripcion: string
}
```

HttpStatusCode: 404 --> si el medio de pago no existe o se encuentra inactivo

### Productos ###

URL: producto/categorias

Devuelve:

```json
HttpStatusCode: 200
[{
    Id: int
    Descripcion: string
}]
```

## Observaciones ##
- Si considera necesario modificar el micro-servicio de productos o el de medios de pago, haga la modificación en los Mocks y modifique el Readme con lo/s método/s nuevos
- Cualquier duda con la consigna o inconviente puede comunicarse por mail con Gustavo Galperin a gustavo.galperin@fravega.com.ar o Diego Malchinsky a diego.malchinsky@fravega.com.ar