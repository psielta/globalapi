using AutoMapper;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GlobalErpData.Repository
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
            CreateMap<UsuarioDto, Usuario>();
            CreateMap<EmpresaDto, Empresa>();
            CreateMap<UsuarioPermissaoDto, UsuarioPermissao>();
            CreateMap<PermissaoDto, Permissao>();
            CreateMap<UsuarioPermissaoDto, UsuarioPermissao>();
            CreateMap<CidadeDto, Cidade>();
            CreateMap<ReferenciaEstoqueDto, ReferenciaEstoque>();
            CreateMap<GrupoEstoqueDto, GrupoEstoque>();
            CreateMap<ProdutoEstoqueDto, ProdutoEstoque>();
            CreateMap<UnidadeMedidaDto, UnidadeMedida>();
            
            CreateMap<ClienteDto, Cliente>();
            CreateMap<Cliente, ClienteDto>();


            CreateMap<FornecedorDto, Fornecedor>();
            CreateMap<CertificadoDto, Certificado>();
            CreateMap<PlanoEstoqueDto, PlanoEstoque>();
            CreateMap<SaldoEstoqueDto, SaldoEstoque>();
            CreateMap<CfopImportacaoDto, CfopImportacao>();
            CreateMap<ImpcabnfeDto,  Impcabnfe>();
            CreateMap<ImpdupnfeDto,  Impdupnfe>();
            CreateMap<ImpitensnfeDto,  Impitensnfe>();
            CreateMap<ImptotalnfeDto, Imptotalnfe>();
            CreateMap<ConfiguracoesUsuarioDto, ConfiguracoesUsuario>();
            CreateMap<ConfiguracoesEmpresaDto, ConfiguracoesEmpresa>();
            CreateMap<ProdutosFornDto, ProdutosForn>();
            CreateMap<EntradaDto, Entrada>();
            CreateMap<TransportadoraDto, Transportadora>();
            CreateMap<ProdutoEntradaDto, ProdutoEntradum>();
            CreateMap<FotosProdutoDto, FotosProduto>();
            CreateMap<FeaturedDto, Featured>();
            CreateMap<SectionDto, Section>();
            CreateMap<SectionItemDto, SectionItem>();
            CreateMap<ProductDetailDto, ProductDetail>();
            CreateMap<ItemDetailDto, ItemDetail>();
            CreateMap<PerfilLojaDto, PerfilLoja>();
            CreateMap<OlderDto,  Older>();
            CreateMap<OlderItemDto, OlderItem>();
            CreateMap<OlderItem, GetOlderItemDto>();
            CreateMap<Older, GetOldersDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OlderItems));
            CreateMap<CategoryDto, Category>();
            CreateMap<ContaCaixaDto, ContaDoCaixa>();
            CreateMap<PlanoCaixaDto, PlanoDeCaixa>();
            CreateMap<HistoricoCaixaDto, HistoricoCaixa>();
            CreateMap<FormaPagtDto, FormaPagt>();
            CreateMap<ContasAPagarDto, ContasAPagar>();
            CreateMap<ImpxmlDto, Impxml>();

        }
    }
}
