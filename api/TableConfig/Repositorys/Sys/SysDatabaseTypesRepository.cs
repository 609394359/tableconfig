/*
* ==============================================================================
*
* FileName: SysDatabaseTypesRepository.cs
* Created: 2026/1/20 15:20:03
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
    public class SysDatabaseTypesRepository : Repository<SysDatabaseTypes>, ISysDatabaseTypesRepository
    {
        public SysDatabaseTypesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
