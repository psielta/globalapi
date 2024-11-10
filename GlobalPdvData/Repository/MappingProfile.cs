using AutoMapper;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPdvData.Repository
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*
               // Example mapping configuration
               CreateMap<MyDto, MyEntity>();
               // Add other mappings here
             */

            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<EmpresaDto, Empresa>().ReverseMap();
            CreateMap<UsuarioPermissaoDto, UsuarioPermissao>().ReverseMap();
            CreateMap<PermissaoDto, Permissao>().ReverseMap();
            CreateMap<CidadeDto, Cidade>().ReverseMap();
        }
    }
}
