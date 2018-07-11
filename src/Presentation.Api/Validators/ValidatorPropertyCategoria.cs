using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Promociones.Domain.Core;
using Promociones.Domain.Entities;

namespace Promociones.Presentation.Api
{
    public class ValidatorPropertyCategoria : PropertyValidator
    {
        IProductosManager _prmanager;
        public ValidatorPropertyCategoria(IProductosManager prmanager) : base("La categoria del producto es invalida")
        {
            this._prmanager = prmanager;

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue != null)
            {
                List<int> Ids = context.PropertyValue as List<int>;

                var categorias = _prmanager.GetCategorias();


                foreach (int Id in Ids)
                {
                    if (!categorias.Any(x => x.Id == Id))
                        return false;
                }
            }
            return true;
        }
    }
}
