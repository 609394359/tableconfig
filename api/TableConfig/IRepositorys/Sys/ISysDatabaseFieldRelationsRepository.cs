using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;
using TableConfig.Models.Views;

namespace TableConfig.IRepositorys.Sys
{
    public interface ISysDatabaseFieldRelationsRepository : IRepository<SysDatabaseFieldRelations>
    {
        Task<Dictionary<long, int>> GetDatabaseRelationsFieldCount();

        Task<Dictionary<long, int>> GetFieldRelationsDatabaseCount();
        Task<List<SysDatabaseFieldRelationsVM>> GetRelations();
        Task<List<SysDatabaseFieldRelationsVM>> GetDatabaseFields(long databaseId);

        Task<List<SysDatabaseFieldRelationsVM>> GetFieldDatabases(long fieldId);
    }
}
