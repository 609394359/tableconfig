/*
* ==============================================================================
*
* FileName: SysTablesRepository.cs
* Created: 2026/1/20 15:17:22
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System;
using System.Linq;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.Models.Entitys;

namespace TableConfig.Repositorys.Sys
{
    public class SysTablesRepository : Repository<SysTables>, ISysTablesRepository
    {
        public SysTablesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public async Task<Dictionary<long, int>> GetProjectTableCount()
        {
            var list = await _db.Queryable<SysTables>().GroupBy(m => m.ProjectId).Select(m => new
            {
                ProjectId = m.ProjectId,
                Count = SqlFunc.AggregateCount(m.ProjectId)
            }).MergeTable().ToListAsync();

            return list.ToDictionary(m => m.ProjectId, m => m.Count);
        }
    }
}
