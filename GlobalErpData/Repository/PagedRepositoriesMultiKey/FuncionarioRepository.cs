using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class FuncionarioRepository : GenericPagedRepositoryMultiKey<Funcionario, GlobalErpFiscalBaseContext, int, int, FuncionarioDto>
    {
        public FuncionarioRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Funcionario, GlobalErpFiscalBaseContext, int, int, FuncionarioDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<List<Funcionario>> GetFuncionariosByUnity(int unity)
        {
            return await db.Funcionarios.Where(f => f.Unity == unity).ToListAsync();
        }

        public IQueryable<Funcionario> GetQueryableFuncionariosByUnity(int unity)
        {
            return db.Funcionarios.Where(f => f.Unity == unity);
        }
    }
}
