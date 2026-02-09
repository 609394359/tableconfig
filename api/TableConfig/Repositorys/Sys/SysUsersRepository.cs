/*
* ==============================================================================
*
* FileName: SysUsersRepository.cs
* Created: 2026/1/20 12:40:35
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Repositorys;

namespace TableConfig.Repositorys.Sys
{
    public class SysUsersRepository : Repository<SysUsers>, ISysUsersRepository
    {
        public SysUsersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
