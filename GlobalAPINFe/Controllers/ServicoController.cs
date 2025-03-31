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

namespace GlobalAPINFe.Controllers
{
    public class ServicoController : GenericPagedController<Servico, long, ServicoDto>
    {
        private readonly GlobalErpFiscalBaseContext globalErpFiscalBaseContext;
        public ServicoController(IQueryRepository<Servico, long, ServicoDto> repo, ILogger<GenericPagedController<Servico, long, ServicoDto>> logger, GlobalErpFiscalBaseContext context) : base(repo, logger)
        {
            this.globalErpFiscalBaseContext = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Servico>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Servico>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Servico), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Servico>> GetEntity(long id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Servico), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Servico>> Create([FromBody] ServicoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                Servico? added = await repo.CreateAsync(dto);
                if (added == null)
                {
                    return BadRequest("Failed to create the entity.");
                }
                else
                {
                    added = await repo.RetrieveAsyncAsNoTracking(added.GetId());
                    if (added == null)
                    {
                        return BadRequest("Failed to create the entity.");
                    }
                    return CreatedAtAction( // 201 Created
                      nameof(GetEntity),
                      new { id = added.GetId() },
                      added);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating an entity.");
                return StatusCode(500, $"An error occurred while creating the entity: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Servico>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Servico>>> CreateBulk([FromBody] IEnumerable<ServicoDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Servico), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Servico>> Update(long id, [FromBody] ServicoDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }

        [HttpGet("GetServicosPorUnity", Name = nameof(GetServicosPorUnity))]
        [ProducesResponseType(typeof(PagedResponse<Servico>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Servico>>> GetServicosPorUnity(
            int unity, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] int? departamentoId = null,
            [FromQuery] string? departamentoNome = null)
        {
            try
            {
                var query = ((ServicoRepository)repo).GetServicosPorUnity(unity);
                if ((!string.IsNullOrEmpty(departamentoNome)) || departamentoId.HasValue)
                {
                    string SQL = $@"
                        SELECT * FROM servicos s WHERE
                        (1=1)
                        AND s.unity = {unity}
                        {((!string.IsNullOrEmpty(departamentoNome)) ? $" AND Upper(Trim(s.nm_servico)) LIKE '%{departamentoNome.ToUpper().Trim()}%' " : "" )}
                        {(departamentoId.HasValue ? $" AND s.id_departamento = {departamentoId} " : "")}
                        ORDER BY s.id
                    ";

                    query = this.globalErpFiscalBaseContext.Servicos.FromSqlRaw(SQL).Include(x => x.IdDepartamentoNavigation).AsQueryable();
                }

                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<Servico>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpPost("PostWithOsTabelaPreco")]
        [ProducesResponseType(typeof(Servico), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Servico>> PostWithOsTabelaPreco([FromBody] ServicoDto dto)
        {
            try
            {
                var novoServico = new Servico
                {
                    Unity = dto.Unity,
                    IdDepartamento = dto.IdDepartamento,
                    PagaComissao = dto.PagaComissao,
                    ValorUnitario = dto.ValorUnitario,
                    NmServico = dto.NmServico
                };

                foreach (var osTabelaPrecoDto in dto.OsTabelaPrecos)
                {
                    var novoOsTabela = new OsTabelaPreco
                    {
                        CdServicoNavigation = novoServico,
                        IdTabelaPreco = osTabelaPrecoDto.IdTabelaPreco,
                        PrecoVenda = osTabelaPrecoDto.PrecoVenda,
                        DtUltAlteracao = osTabelaPrecoDto.DtUltAlteracao,
                        Descricao = osTabelaPrecoDto.Descricao,
                        Unity = dto.Unity
                    };

                    novoServico.OsTabelaPrecos.Add(novoOsTabela);
                }

                this.globalErpFiscalBaseContext.Servicos.Add(novoServico);
                await this.globalErpFiscalBaseContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEntity), new { id = novoServico.Id }, novoServico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro no PostWithOsTabelaPreco");
                return StatusCode(500, $"Erro ao criar Servico com TabelaPrecos: {ex.Message}");
            }
        }


        [HttpPost("PutWithOsTabelaPreco")]
        [ProducesResponseType(typeof(Servico), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Servico>> PutWithOsTabelaPreco([FromBody] ServicoPutDto dto)
        {
            try
            {
                var servicoExistente = await globalErpFiscalBaseContext.Servicos
                    .Include(s => s.OsTabelaPrecos)
                    .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (servicoExistente == null)
                {
                    return NotFound($"Servico id={dto.Id} não encontrado");
                }

                servicoExistente.Unity = dto.Unity;
                servicoExistente.IdDepartamento = dto.IdDepartamento;
                servicoExistente.PagaComissao = dto.PagaComissao;
                servicoExistente.ValorUnitario = dto.ValorUnitario;
                servicoExistente.NmServico = dto.NmServico;

                var dictTabelaPrecosExistentes = servicoExistente.OsTabelaPrecos
                    .ToDictionary(x => x.Id, x => x);

                foreach (var itemDto in dto.OsTabelaPrecos)
                {
                    if (itemDto.Id.HasValue && dictTabelaPrecosExistentes.ContainsKey(itemDto.Id.Value))
                    {
                        // a) Atualizar item existente
                        var registroBanco = dictTabelaPrecosExistentes[itemDto.Id.Value];
                        registroBanco.IdTabelaPreco = itemDto.IdTabelaPreco;
                        registroBanco.PrecoVenda = itemDto.PrecoVenda;
                        registroBanco.DtUltAlteracao = itemDto.DtUltAlteracao;
                        registroBanco.Descricao = itemDto.Descricao;

                        registroBanco.Unity = dto.Unity;
                        registroBanco.CdServico = servicoExistente.Id;

                        dictTabelaPrecosExistentes.Remove(itemDto.Id.Value);
                    }
                    else
                    {
                        var novoOsTabela = new OsTabelaPreco
                        {
                            IdTabelaPreco = itemDto.IdTabelaPreco,
                            PrecoVenda = itemDto.PrecoVenda,
                            DtUltAlteracao = itemDto.DtUltAlteracao,
                            Descricao = itemDto.Descricao,

                            CdServicoNavigation = servicoExistente,
                            Unity = dto.Unity
                        };

                        servicoExistente.OsTabelaPrecos.Add(novoOsTabela);
                    }
                }

                foreach (var itemNaoEnviado in dictTabelaPrecosExistentes.Values)
                {
                    globalErpFiscalBaseContext.OsTabelaPrecos.Remove(itemNaoEnviado);
                }

                await globalErpFiscalBaseContext.SaveChangesAsync();

                return Ok(servicoExistente);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro no PutWithOsTabelaPreco");
                return StatusCode(500, $"Erro ao atualizar Servico c/ TabelaPrecos: {ex.Message}");
            }
        }

    }
}
