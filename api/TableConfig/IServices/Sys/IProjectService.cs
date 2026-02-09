using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;

namespace TableConfig.IServices.Sys
{
    public interface IProjectService
    {
        Task<List<SysProjects>> GetListAsync();
        Task<Dictionary<long, int>> GetProjectTableCount();
        Task<Dictionary<long, int>> GetProjectGroupCount();
        Task<SysProjects> GetProjectInfo(long id);
        Task<SysProjects> Save(SysProjectsSaveReq parm);
        Task Del(long id);
        Task<List<SysDatabaseTypes>> GetDatabaseListAsync();

        Task<List<SysTableGroups>> GetGroupListAsync(long projectId);
        Task<SysTableGroups> TableGroupSave(SysTableGroupsSaveReq parm);
        Task TableGroupDel(long id);



        Task<List<SysTables>> GetTableListAsync(long projectId);
        Task<SysTables> GetTableAsync(long tableId);
        Task<SysTables> TableSave(SysTablesSaveReq parm);
        Task TableDel(long id);
        Task TableCopy(long id);




        Task<List<SysFieldTypes>> GetFieldTypeListAsync();

        Task<SysDatabaseTypes> GetDatabaseTypeAsync(long databaseId);

        Task<List<SysDatabaseFieldRelations>> GetDatabaseFieldRelationListAsync(long databaseId);
        Task<List<SysTableFields>> GetTableFieldListAsync(long tableId);
        Task<SysTableFields> GetTableFieldAsync(long tableFieldId);
        Task<SysTableFields> TableFieldSave(SysTableFieldsSaveReq parm);
        Task TableFieldSort(SysTableFieldsSortReq parm);

        Task TableFieldCopy(SysTableFieldsCopyReq parm);
        Task TableFieldDel(long tableFieldId);
    }
}
