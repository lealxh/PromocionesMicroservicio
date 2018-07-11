using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Promociones.Domain.Entities;
using Promociones.Domain.Core;

namespace Promociones.Presentation.Api
{

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<PromocionInsertDTO, Promocion>().
                ForMember(x => x.Id, opt => opt.Ignore()).
                ForMember(x => x.FechaCreacion, opt => opt.Ignore()).
                ForMember(x => x.FechaModificacion, opt => opt.Ignore());

            CreateMap<PromocionUpdateDTO, Promocion>().
                ForMember(x => x.Id, opt => opt.Ignore());
          
        }
    }
}
