using AutoMapper;
using GlobalAPI_ACBrNFe.Models;
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
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<EmpresaDto, Empresa>().ReverseMap();
            CreateMap<UsuarioPermissaoDto, UsuarioPermissao>().ReverseMap();
            CreateMap<PermissaoDto, Permissao>().ReverseMap();
            CreateMap<CidadeDto, Cidade>().ReverseMap();
            CreateMap<ReferenciaEstoqueDto, ReferenciaEstoque>().ReverseMap();
            CreateMap<GrupoEstoqueDto, GrupoEstoque>().ReverseMap();
            CreateMap<ProdutoEstoqueDto, ProdutoEstoque>().ReverseMap();
            CreateMap<UnidadeMedidaDto, UnidadeMedida>().ReverseMap();
            CreateMap<ClienteDto, Cliente>().ReverseMap();
            CreateMap<FornecedorDto, Fornecedor>().ReverseMap();
            CreateMap<CertificadoDto, Certificado>().ReverseMap();
            CreateMap<PlanoEstoqueDto, PlanoEstoque>().ReverseMap();
            CreateMap<SaldoEstoqueDto, SaldoEstoque>().ReverseMap();
            CreateMap<CfopImportacaoDto, CfopImportacao>().ReverseMap();
            CreateMap<ImpcabnfeDto, Impcabnfe>().ReverseMap();
            CreateMap<ImpdupnfeDto, Impdupnfe>().ReverseMap();
            CreateMap<ImpitensnfeDto, Impitensnfe>().ReverseMap();
            CreateMap<ImptotalnfeDto, Imptotalnfe>().ReverseMap();
            CreateMap<ConfiguracoesUsuarioDto, ConfiguracoesUsuario>().ReverseMap();
            CreateMap<ConfiguracoesEmpresaDto, ConfiguracoesEmpresa>().ReverseMap();
            CreateMap<ProdutosFornDto, ProdutosForn>().ReverseMap();
            CreateMap<EntradaDto, Entrada>().ReverseMap();
            CreateMap<TransportadoraDto, Transportadora>().ReverseMap();
            CreateMap<ProdutoEntradaDto, ProdutoEntradum>().ReverseMap();
            CreateMap<ProdutoEntradaDto, ProdutoEntradaDtoComId>().ReverseMap();
            CreateMap<FotosProdutoDto, FotosProduto>().ReverseMap();
            CreateMap<FeaturedDto, Featured>().ReverseMap();
            CreateMap<SectionDto, Section>().ReverseMap();
            CreateMap<SectionItemDto, SectionItem>().ReverseMap();
            CreateMap<ProductDetailDto, ProductDetail>().ReverseMap();
            CreateMap<ItemDetailDto, ItemDetail>().ReverseMap();
            CreateMap<PerfilLojaDto, PerfilLoja>().ReverseMap();
            CreateMap<OlderDto, Older>().ReverseMap();
            CreateMap<OlderItemDto, OlderItem>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ContaCaixaDto, ContaDoCaixa>().ReverseMap();
            CreateMap<PlanoCaixaDto, PlanoDeCaixa>().ReverseMap();
            CreateMap<HistoricoCaixaDto, HistoricoCaixa>().ReverseMap();
            CreateMap<FormaPagtDto, FormaPagt>().ReverseMap();
            CreateMap<ContasAPagarDto, ContasAPagar>().ReverseMap();
            CreateMap<ImpxmlDto, Impxml>().ReverseMap();
            CreateMap<ContasAReceberDto, ContasAReceber>().ReverseMap();

            // Mapear e configurar propriedades específicas
            CreateMap<Older, GetOldersDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OlderItems));
            CreateMap<OlderItem, GetOlderItemDto>();

            CreateMap<ProdutoEstoque, ProdutoEstoqueDto2>().ReverseMap();

            // Mapeamento entre Amarracao e Amarracao2, incluindo a propriedade produto
            CreateMap<Amarracao, Amarracao2>()
                .ForMember(dest => dest.produto, opt => opt.MapFrom(src => src.produto))
                .ReverseMap();

            CreateMap<Saida, SaidaDto>().ReverseMap();
            CreateMap<ProdutoSaidum, ProdutoSaidumDto>().ReverseMap();
            CreateMap<OrigemCst, OrigemCstDto>().ReverseMap();
            CreateMap<Cst,CstDto>().ReverseMap();
            CreateMap<Csosn, CsosnDto>().ReverseMap();
            CreateMap<Cfop, CfopDto>().ReverseMap();
            CreateMap<Ibpt, IbptDto>().ReverseMap();
            CreateMap<Ncm, NcmDto>().ReverseMap();
            CreateMap<CestNcm,CestNcmDto>().ReverseMap();
            CreateMap<ObsNf, ObsNfDto>().ReverseMap();
            CreateMap<SaidasVolume, SaidasVolumeDto>().ReverseMap();
            CreateMap<Frete, FreteDto>().ReverseMap();

            CreateMap<SaidaNotasDevolucaoDto, SaidaNotasDevolucao>().ReverseMap();
            CreateMap<ProtocoloEstadoNcmDto, ProtocoloEstadoNcm>().ReverseMap();
            CreateMap<CfopCsosnV2Dto, CfopCsosnV2>().ReverseMap();
            CreateMap<NcmProtocoloEstadoDto, NcmProtocoloEstado>().ReverseMap();
            CreateMap<Icm, IcmDto>().ReverseMap();

        }
    }
}
