/*
* ==============================================================================
*
* FileName: SysDatabaseFieldRelationsRepository.cs
* Created: 2026/1/20 15:20:24
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Models.Views;

namespace TableConfig.Repositorys.Sys
{
    public class SysDatabaseFieldRelationsRepository : Repository<SysDatabaseFieldRelations>, ISysDatabaseFieldRelationsRepository
    {
        public SysDatabaseFieldRelationsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public async Task<Dictionary<long, int>> GetDatabaseRelationsFieldCount()
        {
            var list = await _db.Queryable<SysDatabaseFieldRelations>().GroupBy(m => m.DatabaseId).Select(m => new
            {
                DatabaseId = m.DatabaseId,
                Count = SqlFunc.AggregateCount(m.DatabaseId)
            }).MergeTable().ToListAsync();

            return list.ToDictionary(m => m.DatabaseId, m => m.Count);
        }

        public async Task<Dictionary<long, int>> GetFieldRelationsDatabaseCount()
        {
            var list = await _db.Queryable<SysDatabaseFieldRelations>().GroupBy(m => m.FieldId).Select(m => new
            {
                FieldId = m.FieldId,
                Count = SqlFunc.AggregateCount(m.FieldId)
            }).MergeTable().ToListAsync();

            return list.ToDictionary(m => m.FieldId, m => m.Count);
        }

        public async Task<List<SysDatabaseFieldRelationsVM>> GetRelations()
        {
            var list = await _db.Queryable<SysDatabaseFieldRelations, SysFieldTypes,SysDatabaseTypes>((a, b, c) => new object[] {
                JoinType.Inner,a.FieldId == b.Id
                ,JoinType.Inner,a.DatabaseId == c.Id
            })
            .Select((a, b, c) => new SysDatabaseFieldRelationsVM() { DatabaseId = c.Id,DatabaseName = c.Name,DatabaseDescription =  c.Description, FieldName = b.Name, FieldDescription = b.Description }, true)
            .MergeTable().OrderBy(m => m.FieldName).ToListAsync();

            return list;
        }

        public async Task<List<SysDatabaseFieldRelationsVM>> GetDatabaseFields(long databaseId)
        {
            var list = await _db.Queryable<SysDatabaseFieldRelations, SysFieldTypes>((a, b) => new object[] {
                JoinType.Right,a.FieldId == b.Id
            }).Where((a, b) => a.DatabaseId == databaseId)
            .Select((a, b) => new SysDatabaseFieldRelationsVM() { DatabaseId = databaseId, FieldName = b.Name, FieldDescription = b.Description }, true)
            .MergeTable().OrderBy(m => m.FieldName).ToListAsync();

            return list;
        }
        public async Task<List<SysDatabaseFieldRelationsVM>> GetFieldDatabases(long fieldId)
        {
            var list = await _db.Queryable<SysDatabaseFieldRelations, SysDatabaseTypes>((a, b) => new object[] {
                JoinType.Right,a.DatabaseId == b.Id
            }).Where((a, b) => a.FieldId == fieldId)
            .Select((a, b) => new SysDatabaseFieldRelationsVM() { FieldId = fieldId, DatabaseName = b.Name, DatabaseDescription = b.Description }, true)
            .MergeTable().OrderBy(m => m.DatabaseName).ToListAsync();

            return list;
        }
    }
}
