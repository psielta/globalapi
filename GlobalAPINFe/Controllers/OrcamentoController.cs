using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlobalErpData.Data;
using GlobalLib.Dto;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;
using Serilog;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoController : GenericPagedController<OrcamentoCab, Guid, OrcamentoCabDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly ILogger<OrcamentoController> _logger;
        public OrcamentoController(IQueryRepository<OrcamentoCab, Guid, OrcamentoCabDto> repo, ILogger<GenericPagedController<OrcamentoCab, Guid, OrcamentoCabDto>> logger, GlobalErpFiscalBaseContext context, ILogger<OrcamentoController> l) : base(repo, logger)
        {
            _context = context;
            _logger = l;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<OrcamentoCab>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<OrcamentoCab>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoCab>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrcamentoCab), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OrcamentoCab>> Create([FromBody] OrcamentoCabDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<OrcamentoCab>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<OrcamentoCab>>> CreateBulk([FromBody] IEnumerable<OrcamentoCabDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoCab>> Update(Guid id, [FromBody] OrcamentoCabDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }

        [HttpPut("PutWithItensAndServices")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrcamentoCab>> PutWithItensAndServices([FromBody] OrcamentoPutDto dto)
        {
            try
            {
                var orcCabExistente = await _context.OrcamentoCabs
                    .Include(o => o.OrcamentoItens)
                    .Include(o => o.OrcamentoServicos)
                    .FirstOrDefaultAsync(o => o.Id == dto.Id);

                if (orcCabExistente == null)
                    return NotFound($"OrcamentoCab id={dto.Id} não encontrado");

                orcCabExistente.Unity = dto.Unity;
                orcCabExistente.Empresa = dto.Empresa;
                orcCabExistente.IdCliente = dto.IdCliente;
                orcCabExistente.Gerado = dto.Gerado;
                orcCabExistente.IdFuncionario = dto.IdFuncionario;
                orcCabExistente.PercentualComissao = dto.PercentualComissao;
                orcCabExistente.ValorProdutos = dto.ValorProdutos;
                orcCabExistente.ValorAcrescimo = dto.ValorAcrescimo;
                orcCabExistente.ValorDesconto = dto.ValorDesconto;
                orcCabExistente.ValorComissao = dto.ValorComissao;
                orcCabExistente.ValorServicos = dto.ValorServicos;
                orcCabExistente.ValorTotal = dto.ValorTotal;
                orcCabExistente.CdPlano = dto.CdPlano;

                var dictItensBanco = orcCabExistente.OrcamentoItens
                    .ToDictionary(x => x.Id, x => x);

                foreach (var itemDto in dto.OrcamentoItens)
                {
                    if (itemDto.Id.HasValue && dictItensBanco.ContainsKey(itemDto.Id.Value))
                    {
                        var itemBanco = dictItensBanco[itemDto.Id.Value];
                        itemBanco.Unity = itemDto.Unity;
                        itemBanco.Empresa = itemDto.Empresa;
                        itemBanco.IdCliente = itemDto.IdCliente;
                        itemBanco.Gerado = itemDto.Gerado;
                        itemBanco.IdFuncionario = itemDto.IdFuncionario;
                        itemBanco.PercentualComissao = itemDto.PercentualComissao;
                        itemBanco.IdProduto = itemDto.IdProduto;
                        itemBanco.Qtde = itemDto.Qtde;
                        itemBanco.ValorUnitario = itemDto.ValorUnitario;
                        itemBanco.ValorAcrescimo = itemDto.ValorAcrescimo;
                        itemBanco.ValorDesconto = itemDto.ValorDesconto;
                        itemBanco.ValorComissao = itemDto.ValorComissao;
                        itemBanco.ValorTotal = itemDto.ValorTotal;
                        itemBanco.SequenciaCab = itemDto.SequenciaCab;
                        itemBanco.CdPlano = itemDto.CdPlano;

                        dictItensBanco.Remove(itemDto.Id.Value);
                    }
                    else
                    {
                        // Criar novo item
                        var novoItem = new OrcamentoIten
                        {
                            Id = Guid.NewGuid(),
                            Unity = itemDto.Unity,
                            Empresa = itemDto.Empresa,
                            IdCliente = itemDto.IdCliente,
                            Gerado = itemDto.Gerado,
                            IdFuncionario = itemDto.IdFuncionario,
                            PercentualComissao = itemDto.PercentualComissao,
                            IdProduto = itemDto.IdProduto,
                            Qtde = itemDto.Qtde,
                            ValorUnitario = itemDto.ValorUnitario,
                            ValorAcrescimo = itemDto.ValorAcrescimo,
                            ValorDesconto = itemDto.ValorDesconto,
                            ValorComissao = itemDto.ValorComissao,
                            ValorTotal = itemDto.ValorTotal,
                            IdCab = orcCabExistente.Id,
                            SequenciaCab = itemDto.SequenciaCab,
                            CdPlano = itemDto.CdPlano,

                            OrcamentoCab = orcCabExistente
                        };

                        orcCabExistente.OrcamentoItens.Add(novoItem);
                    }
                }

                foreach (var itemBanco in dictItensBanco.Values)
                {
                    _context.OrcamentoItens.Remove(itemBanco);
                }

                var dictServicosBanco = orcCabExistente.OrcamentoServicos
                    .ToDictionary(x => x.Id, x => x);

                foreach (var svcDto in dto.OrcamentoServicos)
                {
                    if (svcDto.Id.HasValue && dictServicosBanco.ContainsKey(svcDto.Id.Value))
                    {
                        // Atualizar
                        var svcBanco = dictServicosBanco[svcDto.Id.Value];
                        svcBanco.Unity = svcDto.Unity;
                        svcBanco.Empresa = svcDto.Empresa;
                        svcBanco.IdCliente = svcDto.IdCliente;
                        svcBanco.Gerado = svcDto.Gerado;
                        svcBanco.IdFuncionario = svcDto.IdFuncionario;
                        svcBanco.PercentualComissao = svcDto.PercentualComissao;
                        svcBanco.CdMecanico = svcDto.CdMecanico;
                        svcBanco.CdMecanico2 = svcDto.CdMecanico2;
                        svcBanco.TxtRelatoCliente = svcDto.TxtRelatoCliente;
                        svcBanco.TxtDivergencia = svcDto.TxtDivergencia;
                        svcBanco.TxtAvalTecnico = svcDto.TxtAvalTecnico;
                        svcBanco.Lado = svcDto.Lado;
                        svcBanco.IdServico = svcDto.IdServico;
                        svcBanco.Qtde = svcDto.Qtde;
                        svcBanco.ValorUnitario = svcDto.ValorUnitario;
                        svcBanco.ValorAcrescimo = svcDto.ValorAcrescimo;
                        svcBanco.ValorDesconto = svcDto.ValorDesconto;
                        svcBanco.ValorComissao = svcDto.ValorComissao;
                        svcBanco.ValorTotal = svcDto.ValorTotal;
                        svcBanco.SequenciaCab = svcDto.SequenciaCab;

                        // Remove do dicionario
                        dictServicosBanco.Remove(svcDto.Id.Value);
                    }
                    else
                    {
                        // Criar novo
                        var novoServico = new OrcamentoServico
                        {
                            Id = Guid.NewGuid(),
                            Unity = svcDto.Unity,
                            Empresa = svcDto.Empresa,
                            IdCliente = svcDto.IdCliente,
                            Gerado = svcDto.Gerado,
                            IdFuncionario = svcDto.IdFuncionario,
                            PercentualComissao = svcDto.PercentualComissao,
                            CdMecanico = svcDto.CdMecanico,
                            CdMecanico2 = svcDto.CdMecanico2,
                            TxtRelatoCliente = svcDto.TxtRelatoCliente,
                            TxtDivergencia = svcDto.TxtDivergencia,
                            TxtAvalTecnico = svcDto.TxtAvalTecnico,
                            Lado = svcDto.Lado,
                            IdServico = svcDto.IdServico,
                            Qtde = svcDto.Qtde,
                            ValorUnitario = svcDto.ValorUnitario,
                            ValorAcrescimo = svcDto.ValorAcrescimo,
                            ValorDesconto = svcDto.ValorDesconto,
                            ValorComissao = svcDto.ValorComissao,
                            ValorTotal = svcDto.ValorTotal,
                            IdCab = orcCabExistente.Id,
                            SequenciaCab = svcDto.SequenciaCab,

                            OrcamentoCab = orcCabExistente
                        };

                        orcCabExistente.OrcamentoServicos.Add(novoServico);
                    }
                }

                // Remover do banco os serviços que sobraram no dicionário (opcional, se esta for a regra)
                foreach (var svcBanco in dictServicosBanco.Values)
                {
                    _context.OrcamentoServicos.Remove(svcBanco);
                }

                // 5) Salvar
                await _context.SaveChangesAsync();
                // await transaction.CommitAsync();

                return Ok(orcCabExistente);
            }
            catch (Exception ex)
            {
                // await transaction.RollbackAsync();
                _logger.LogError(ex, "Erro ao atualizar Orcamento com itens/serviços");
                return StatusCode(500, $"Erro ao atualizar Orcamento: {ex.Message}");
            }
        }


        [HttpPost("PostWithItensAndServices")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrcamentoCab>> PostWithItensAndServices([FromBody] OrcamentoPostDto dto)
        {
            try
            {
                // 1) Criar o OrcamentoCab
                var novoOrcCab = new OrcamentoCab
                {
                    Id = Guid.NewGuid(),

                    Unity = dto.Unity,
                    Empresa = dto.Empresa,
                    IdCliente = dto.IdCliente,
                    Gerado = dto.Gerado,
                    IdFuncionario = dto.IdFuncionario,
                    PercentualComissao = dto.PercentualComissao,
                    ValorProdutos = dto.ValorProdutos,
                    ValorAcrescimo = dto.ValorAcrescimo,
                    ValorDesconto = dto.ValorDesconto,
                    ValorComissao = dto.ValorComissao,
                    ValorServicos = dto.ValorServicos,
                    ValorTotal = dto.ValorTotal,
                    CdPlano = dto.CdPlano
                };

                // 2) Criar os OrcamentoItens
                foreach (var itemDto in dto.OrcamentoItens)
                {
                    var novoItem = new OrcamentoIten
                    {
                        Id = Guid.NewGuid(),
                        // Sequencia = ??? se houver rotina de sequencia
                        Unity = itemDto.Unity,
                        Empresa = itemDto.Empresa,
                        IdCliente = itemDto.IdCliente,
                        Gerado = itemDto.Gerado,
                        IdFuncionario = itemDto.IdFuncionario,
                        PercentualComissao = itemDto.PercentualComissao,
                        IdProduto = itemDto.IdProduto,
                        Qtde = itemDto.Qtde,
                        ValorUnitario = itemDto.ValorUnitario,
                        ValorAcrescimo = itemDto.ValorAcrescimo,
                        ValorDesconto = itemDto.ValorDesconto,
                        ValorComissao = itemDto.ValorComissao,
                        ValorTotal = itemDto.ValorTotal,
                        IdCab = novoOrcCab.Id, // chave estrangeira p/ o Cab
                        SequenciaCab = itemDto.SequenciaCab,
                        CdPlano = itemDto.CdPlano
                    };

                    // associar via navegação (opcional, mas costuma ser bom)
                    novoItem.OrcamentoCab = novoOrcCab;

                    // adicionar na coleção (opcional)
                    novoOrcCab.OrcamentoItens.Add(novoItem);
                }

                // 3) Criar os OrcamentoServicos
                foreach (var svcDto in dto.OrcamentoServicos)
                {
                    var novoServico = new OrcamentoServico
                    {
                        Id = Guid.NewGuid(),
                        // Sequencia = ??? se houver rotina de sequencia
                        Unity = svcDto.Unity,
                        Empresa = svcDto.Empresa,
                        IdCliente = svcDto.IdCliente,
                        Gerado = svcDto.Gerado,
                        IdFuncionario = svcDto.IdFuncionario,
                        PercentualComissao = svcDto.PercentualComissao,
                        CdMecanico = svcDto.CdMecanico,
                        CdMecanico2 = svcDto.CdMecanico2,
                        TxtRelatoCliente = svcDto.TxtRelatoCliente,
                        TxtDivergencia = svcDto.TxtDivergencia,
                        TxtAvalTecnico = svcDto.TxtAvalTecnico,
                        Lado = svcDto.Lado,
                        IdServico = svcDto.IdServico,
                        Qtde = svcDto.Qtde,
                        ValorUnitario = svcDto.ValorUnitario,
                        ValorAcrescimo = svcDto.ValorAcrescimo,
                        ValorDesconto = svcDto.ValorDesconto,
                        ValorComissao = svcDto.ValorComissao,
                        ValorTotal = svcDto.ValorTotal,
                        IdCab = novoOrcCab.Id,
                        SequenciaCab = svcDto.SequenciaCab
                    };

                    // associar via navegação
                    novoServico.OrcamentoCab = novoOrcCab;

                    // adicionar na coleção
                    novoOrcCab.OrcamentoServicos.Add(novoServico);
                }

                _context.OrcamentoCabs.Add(novoOrcCab);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEntity), new { id = novoOrcCab.Id }, novoOrcCab);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar Orcamento com itens e serviços");
                return StatusCode(500, $"Erro ao criar Orcamento: {ex.Message}");
            }
        }

    }
}
