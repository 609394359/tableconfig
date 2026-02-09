/*
* ==============================================================================
*
* FileName: SysTableFieldsRepository.cs
* Created: 2026/1/20 15:18:51
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Threading.Tasks;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.Models.Entitys;

namespace TableConfig.Repositorys.Sys
{
    public class SysTableFieldsRepository : Repository<SysTableFields>, ISysTableFieldsRepository
    {
        public SysTableFieldsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
