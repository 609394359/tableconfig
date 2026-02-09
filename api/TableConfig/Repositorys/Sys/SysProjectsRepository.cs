/*
* ==============================================================================
*
* FileName: SysProjectsRepository.cs
* Created: 2026/1/20 15:19:16
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
    public class SysProjectsRepository : Repository<SysProjects>, ISysProjectsRepository
    {
        public SysProjectsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
