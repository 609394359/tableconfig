using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Models.Views;

namespace TableConfig.IServices.Sys
{
    public interface IFieldTypeService
    {
        Task<List<SysFieldTypes>> GetListAsync();

        Task<Dictionary<long, int>> GetRelationsDatabaseCount();

        Task<List<SysDatabaseFieldRelationsVM>> GetRelations();

        Task<SysFieldTypes> GetInfo(long id);

        Task<SysFieldTypes> Save(SysFieldTypesSaveReq parm);

        Task Del(long id);

        Task<List<SysDatabaseFieldRelationsVM>> GetDatabases(long id);

        Task SaveDatabaseRelations(SysFieldDatabasesSaveReq parm);
    }
}
