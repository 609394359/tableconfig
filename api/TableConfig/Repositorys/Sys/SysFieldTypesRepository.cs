/*
* ==============================================================================
*
* FileName: SysFieldTypesRepository.cs
* Created: 2026/1/20 15:19:40
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.Models.Entitys;

namespace TableConfig.Repositorys.Sys
{
    public class SysFieldTypesRepository : Repository<SysFieldTypes>, ISysFieldTypesRepository
    {
        public SysFieldTypesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
