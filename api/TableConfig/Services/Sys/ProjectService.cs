/*
* ==============================================================================
*
* FileName: ProjectService.cs
* Created: 2026/1/21 9:49:08
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.IRepositorys.Sys;
using TableConfig.IRepositorys;
using TableConfig.IServices.Sys;
using System.Threading.Tasks;
using System.Collections.Generic;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using Mapster;
using SqlSugar;
using System;
using TableConfig.Enums;
using TableConfig.Utils;
using SqlSugar.Extensions;
using System.Linq;
using TableConfig.Repositorys.Sys;

namespace TableConfig.Services.Sys
{
    public class ProjectService : IProjectService
    {
        private readonly ISysProjectsRepository _projectsRepository;
        private readonly ISysTablesRepository _tablesRepository;
        private readonly ISysTableGroupsRepository _tableGroupsRepository;
        private readonly ISysTableFieldsRepository _tableFieldsRepository;

        private readonly ISysDatabaseTypesRepository _databaseTypesRepository;
        private readonly ISysDatabaseFieldRelationsRepository _relationsRepository;
        private readonly ISysFieldTypesRepository _fieldTypesRepository;
        private readonly TokenManager _tokenManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(ISysProjectsRepository projectsRepository, ISysTablesRepository tablesRepository, ISysTableGroupsRepository tableGroupsRepository, ISysTableFieldsRepository tableFieldsRepository, ISysDatabaseTypesRepository databaseTypesRepository, ISysDatabaseFieldRelationsRepository relationsRepository, ISysFieldTypesRepository fieldTypesRepository, TokenManager tokenManager, IUnitOfWork unitOfWork)
        {
            _projectsRepository = projectsRepository;
            _tablesRepository = tablesRepository;
            _tableGroupsRepository = tableGroupsRepository;
            _tableFieldsRepository = tableFieldsRepository;
            _databaseTypesRepository = databaseTypesRepository;
            _relationsRepository = relationsRepository;
            _fieldTypesRepository = fieldTypesRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SysProjects>> GetListAsync()
        {
            return await _projectsRepository.GetAllAsync();
        }

        public async Task<Dictionary<long, int>> GetProjectTableCount()
        {
            return await _tablesRepository.GetProjectTableCount();
        }

        public async Task<Dictionary<long, int>> GetProjectGroupCount()
        {
            return await _tableGroupsRepository.GetProjectGroupCount();
        }

        public async Task<SysProjects> GetProjectInfo(long id)
        {
            var info = await _projectsRepository.GetByIdAsync(id);
            if (info != null)
            {
                SysDatabaseTypes databaseType = await _databaseTypesRepository.GetByIdAsync(info.DatabaseId);
                info.DatabaseId = databaseType.Id;
                info.DatabaseName = databaseType.Name;
            }

            return info;
        }

        public async Task<SysProjects> Save(SysProjectsSaveReq parm)
        {
            SysDatabaseTypes databaseType = parm.DatabaseId == null ? null : await _databaseTypesRepository.GetByIdAsync(parm.DatabaseId);
            if(databaseType == null)
                throw new Tip(StatusCodeType.Error, "数据库类型不存在");

            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysProjects>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.DatabaseId = databaseType.Id;
                info.DatabaseName = databaseType.Name;
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                await _projectsRepository.InsertAsync(info);

                return info;
            }
            else
            {

                var info = await _projectsRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.DatabaseId = databaseType.Id;
                info.DatabaseName = databaseType.Name;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _projectsRepository.UpdateAsync(info);

                return info;
            }
        }

        public async Task Del(long id)
        {
            try
            {
                _unitOfWork.BeginTran();

                await _projectsRepository.DeleteAsync(id);

                await _tableGroupsRepository.DeleteAsync(m => m.ProjectId == id);
                await _tablesRepository.DeleteAsync(m => m.ProjectId == id);
                await _tableFieldsRepository.DeleteAsync(m => m.ProjectId == id);

                _unitOfWork.CommitTran();


            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }


        public async Task<List<SysDatabaseTypes>> GetDatabaseListAsync()
        {
            return (await _databaseTypesRepository.GetAllAsync()).OrderBy(m=>m.Name).ToList();
        }




        public async Task<List<SysTableGroups>> GetGroupListAsync(long projectId)
        {
            return await _tableGroupsRepository.GetListAsync(m => m.ProjectId == projectId);
        }


        public async Task<SysTableGroups> TableGroupSave(SysTableGroupsSaveReq parm)
        {
            SysProjects project = await _projectsRepository.GetByIdAsync(parm.ProjectId);
            if (project == null)
                throw new Tip(StatusCodeType.Error, "项目不存在");

            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysTableGroups>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                await _tableGroupsRepository.InsertAsync(info);

                return info;
            }
            else
            {

                var info = await _tableGroupsRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _tableGroupsRepository.UpdateAsync(info);

                return info;
            }
        }

        public async Task TableGroupDel(long id)
        {
            try
            {
                _unitOfWork.BeginTran();

                await _tableGroupsRepository.DeleteAsync(id);

                _unitOfWork.CommitTran();


            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }



        public async Task<List<SysTables>> GetTableListAsync(long projectId)
        {
            return await _tablesRepository.GetListAsync(m => m.ProjectId == projectId);
        }


        public async Task<SysTables> GetTableAsync(long tableId)
        {
            return await _tablesRepository.GetByIdAsync(tableId);
        }

        public async Task<SysTables> TableSave(SysTablesSaveReq parm)
        {
            SysProjects project = await _projectsRepository.GetByIdAsync(parm.ProjectId);
            if (project == null)
                throw new Tip(StatusCodeType.Error, "项目不存在");

            SysTableGroups group = await _tableGroupsRepository.GetByIdAsync(parm.GroupId);
            if (project == null)
                throw new Tip(StatusCodeType.Error, "分组不存在");

            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysTables>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                await _tablesRepository.InsertAsync(info);

                return info;
            }
            else
            {

                var info = await _tablesRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.FieldCount = await _tableFieldsRepository.GetCountAsync(m => m.TableId == info.Id);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _tablesRepository.UpdateAsync(info);

                return info;
            }
        }

        public async Task TableDel(long id)
        {
            try
            {
                _unitOfWork.BeginTran();

                await _tablesRepository.DeleteAsync(id);

                await _tableFieldsRepository.DeleteAsync(m => m.TableId == id);

                _unitOfWork.CommitTran();


            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }

        public async Task TableCopy(long id)
        {
            var tokenInfo = _tokenManager.TokenInfo;

            var oldInfo = await _tablesRepository.GetByIdAsync(id);
            if (oldInfo == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            var oldFields = await _tableFieldsRepository.GetListAsync(m => m.TableId == oldInfo.Id);

            var info = oldInfo.Adapt<SysTables>();
            info.Id = SnowFlakeSingle.instance.NextId();
            info.Name = info.Name + "_copy";
            info.CreateId = tokenInfo.Id;
            info.CreateName = tokenInfo.Name;
            info.CreateTime = DateTime.Now;
            info.UpdateId = tokenInfo.Id;
            info.UpdateName = tokenInfo.Name;
            info.UpdateTime = DateTime.Now;

            var fields = oldFields.Select(m =>
            {
                var _m = m.Adapt<SysTableFields>();
                _m.Id = SnowFlakeSingle.instance.NextId();
                _m.TableId = info.Id;
                _m.CreateId = tokenInfo.Id;
                _m.CreateName = tokenInfo.Name;
                _m.CreateTime = DateTime.Now;
                _m.UpdateId = tokenInfo.Id;
                _m.UpdateName = tokenInfo.Name;
                _m.UpdateTime = DateTime.Now;
                return _m;
            }).ToList();

            info.FieldCount = fields.Count();

            try
            {
                _unitOfWork.BeginTran();

                await _tablesRepository.InsertAsync(info);

                await _tableFieldsRepository.InsertAsync(fields);

                _unitOfWork.CommitTran();

            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }



        public async Task<List<SysFieldTypes>> GetFieldTypeListAsync()
        {
            return (await _fieldTypesRepository.GetAllAsync()).OrderBy(m=>m.Name).ToList();
        }

        public async Task<SysDatabaseTypes> GetDatabaseTypeAsync(long databaseId)
        {
            return await _databaseTypesRepository.GetByIdAsync(databaseId);
        }

        public async Task<List<SysDatabaseFieldRelations>> GetDatabaseFieldRelationListAsync(long databaseId)
        {
            return await _relationsRepository.GetListAsync(m=>m.DatabaseId == databaseId);
        }

        public async Task<List<SysTableFields>> GetTableFieldListAsync(long tableId)
        {
            return await _tableFieldsRepository.GetListAsync(m => m.TableId == tableId, m => m.SortIndex, OrderByType.Asc);
        }
        
        public async Task<SysTableFields> GetTableFieldAsync(long tableFieldId)
        {
            return await _tableFieldsRepository.GetByIdAsync(tableFieldId);
        }

        public async Task<SysTableFields> TableFieldSave(SysTableFieldsSaveReq parm)
        {
            var table = await _tablesRepository.GetByIdAsync(parm.TableId);
            if (table == null)
                throw new Tip(StatusCodeType.Error, "表不存在");

            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysTableFields>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.ProjectId = table.ProjectId;
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                info.SortIndex = await _tableFieldsRepository.GetMaxAsync(m => m.TableId == info.TableId, m => m.SortIndex);
                info.SortIndex = info.SortIndex + 1;

                try
                {
                    _unitOfWork.BeginTran();

                    await _tableFieldsRepository.InsertAsync(info);

                    var count = await _tableFieldsRepository.GetCountAsync(m => m.TableId == info.TableId);

                    await _tablesRepository.UpdateAsync(m => m.Id == info.TableId, m => new SysTables
                    {
                        FieldCount = count
                    });

                    _unitOfWork.CommitTran();

                }
                catch
                {
                    _unitOfWork.RollbackTran();
                    throw;
                }

                return info;
            }
            else
            {

                var info = await _tableFieldsRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                try
                {
                    _unitOfWork.BeginTran();

                    await _tableFieldsRepository.UpdateAsync(info);

                    var count = await _tableFieldsRepository.GetCountAsync(m => m.TableId == info.TableId);

                    await _tablesRepository.UpdateAsync(m => m.Id == info.TableId, m => new SysTables
                    {
                        FieldCount = count
                    });

                    _unitOfWork.CommitTran();
                }
                catch
                {
                    _unitOfWork.RollbackTran();
                    throw;
                }

                return info;
            }
        }

        public async Task TableFieldSort(SysTableFieldsSortReq parm)
        {
            List<SysTableFields> newFields = new List<SysTableFields>();
            for (var i = 0; i < parm.Ids.Count; i++)
            {
                newFields.Add(new SysTableFields { Id = parm.Ids[i], SortIndex = i + 1 });
            }

            await _tableFieldsRepository.UpdateAsync(newFields, m => m.SortIndex);
        }


        public async Task TableFieldCopy(SysTableFieldsCopyReq parm)
        {
            var table = await _tablesRepository.GetByIdAsync(parm.TableId);
            if (table == null)
                throw new Tip(StatusCodeType.Error, "表不存在");

            var tokenInfo = _tokenManager.TokenInfo;

            var fields = await _tableFieldsRepository.GetListAsync(m=>parm.Ids.Contains(m.Id));

            var sortIndex = await _tableFieldsRepository.GetMaxAsync(m => m.TableId == parm.TableId, m => m.SortIndex);
            sortIndex++;

            var newFields = fields.OrderBy(m => m.SortIndex).Select(m=> { 
                var _m = m.Adapt<SysTableFields>();
                _m.Id = SnowFlakeSingle.instance.NextId();
                _m.ProjectId = table.ProjectId;
                _m.TableId = table.Id;
                _m.CreateId = tokenInfo.Id;
                _m.CreateName = tokenInfo.Name;
                _m.CreateTime = DateTime.Now;
                _m.UpdateId = tokenInfo.Id;
                _m.UpdateName = tokenInfo.Name;
                _m.UpdateTime = DateTime.Now;
                _m.SortIndex = sortIndex++;
                return _m;
            }).ToList();

            await _tableFieldsRepository.InsertAsync(newFields);

            var count = await _tableFieldsRepository.GetCountAsync(m => m.TableId == table.Id);

            await _tablesRepository.UpdateAsync(m => m.Id == table.Id, m => new SysTables
            {
                FieldCount = count
            });
        }

        public async Task TableFieldDel(long tableFieldId)
        {
            var info = await _tableFieldsRepository.GetByIdAsync(tableFieldId);
            if (info == null)
                return;
            try
            {
                _unitOfWork.BeginTran();

                await _tableFieldsRepository.DeleteAsync(tableFieldId);

                var count = await _tableFieldsRepository.GetCountAsync(m => m.TableId == info.TableId);

                await _tablesRepository.UpdateAsync(m => m.Id == info.TableId, m => new SysTables
                {
                    FieldCount = count
                });

                _unitOfWork.CommitTran();

            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }
    }
}
