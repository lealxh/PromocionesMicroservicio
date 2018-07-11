using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promociones.Domain.Core;

namespace Promociones.Presentation.Api
{
    public class ValidatorPropertyMedioPago: PropertyValidator
    {
        IMedioPagoManager _mpmanager;
        public ValidatorPropertyMedioPago(IMedioPagoManager mpmanager) : base("El medio de pago es invalido")
        {
            this._mpmanager = mpmanager;

        }
        
        protected override bool IsValid(PropertyValidatorContext context)
        {

            if (context.PropertyValue != null)
            {
                List<int> Ids = context.PropertyValue as List<int>;
                foreach (int Id in Ids)
                {
                    var medio = _mpmanager.GetMedioPago(Id);

                    if (medio == null)
                        return false;
                }
            }
            return true;
        }
    }

}
