/*
* ==============================================================================
*
* FileName: ISysTablesRepository.cs
* Created: 2026/1/20 15:13:06
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;

namespace TableConfig.IRepositorys.Sys
{
    public interface ISysTablesRepository : IRepository<SysTables>
    {

        Task<Dictionary<long, int>> GetProjectTableCount();
    }
}
