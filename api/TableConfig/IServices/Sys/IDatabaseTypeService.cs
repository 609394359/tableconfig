/*
* ==============================================================================
*
* FileName: DatabaseTypeService.cs
* Created: 2026/1/20 15:11:03
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Models.Views;
using TableConfig.Services;
using TableConfig.Utils;

namespace TableConfig.IServices.Sys
{
    public interface IDatabaseTypeService
    {

        Task<List<SysDatabaseTypes>> GetListAsync();


        Task<Dictionary<long, int>> GetDatabaseRelationsFieldCount();

        Task<SysDatabaseTypes> GetInfo(long id);

        Task<SysDatabaseTypes> Save(SysDatabaseTypesSaveReq parm);

        Task Del(long id);

        Task<List<SysDatabaseFieldRelationsVM>> GetFields(long id);


        Task SaveFieldRelations(SysDatabaseFieldsSaveReq parm);
    }
}
