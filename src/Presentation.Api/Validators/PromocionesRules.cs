using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Promociones.Domain.Entities;
using Promociones.Domain.Core;
using Promociones.Application;
using Promociones.Infrastructure;

namespace Promociones.Presentation.Api
{


 public class PromocionUpdateDTORules : AbstractValidator<PromocionUpdateDTO>
{
        public PromocionUpdateDTORules()
        {
            RuleFor(m => m.Id).GreaterThan(0).WithMessage("Id invalido"); ;
            RuleFor(m => m.PorcentajeDecuento).GreaterThan(0).LessThanOrEqualTo(100).WithMessage("Porcentaje invalido");   
         
        }
    }


 public class PromocionInsertDTORules : AbstractValidator<PromocionInsertDTO>
 {
        public PromocionInsertDTORules()
        {
            RuleFor(m => m.PorcentajeDecuento).GreaterThan(0).LessThanOrEqualTo(100).WithMessage("Porcentaje invalido");
        }
}

}
