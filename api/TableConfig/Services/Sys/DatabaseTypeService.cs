/*
* ==============================================================================
*
* FileName: DatabaseTypeService.cs
* Created: 2026/1/20 15:11:20
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Mapster;
using SqlSugar;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Models.Views;
using TableConfig.Utils;

namespace TableConfig.Services.Sys
{
    public class DatabaseTypeService : IDatabaseTypeService
    {
        private readonly ISysDatabaseTypesRepository _databaseTypesRepository;
        private readonly ISysDatabaseFieldRelationsRepository _relationsRepository;
        private readonly ISysFieldTypesRepository _fieldTypesRepository;
        private readonly ISysProjectsRepository _projectsRepository;
        private readonly TokenManager _tokenManager;
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseTypeService(ISysDatabaseTypesRepository databaseTypesRepository, ISysDatabaseFieldRelationsRepository relationsRepository, ISysFieldTypesRepository fieldTypesRepository, ISysProjectsRepository projectsRepository, TokenManager tokenManager, IUnitOfWork unitOfWork)
        {
            _databaseTypesRepository = databaseTypesRepository;
            _relationsRepository = relationsRepository;
            _fieldTypesRepository = fieldTypesRepository;
            _projectsRepository = projectsRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SysDatabaseTypes>> GetListAsync()
        {
            var list = await _databaseTypesRepository.GetAllAsync();
            return list;
        }


        public async Task<Dictionary<long, int>> GetDatabaseRelationsFieldCount()
        {
            return await _relationsRepository.GetDatabaseRelationsFieldCount();
        }


        public async Task<SysDatabaseTypes> GetInfo(long id)
        {
            var info = await _databaseTypesRepository.GetByIdAsync(id);
            return info;
        }


        public async Task<SysDatabaseTypes> Save(SysDatabaseTypesSaveReq parm)
        {
            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysDatabaseTypes>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                await _databaseTypesRepository.InsertAsync(info);

                return info;
            }
            else
            {

                var info = await _databaseTypesRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _databaseTypesRepository.UpdateAsync(info);

                return info;
            }
        }


        public async Task Del(long id)
        {
            if (await _projectsRepository.AnyAsync(m => m.DatabaseId == id))
            {
                throw new Tip(StatusCodeType.Error, "有项目在使用改数据库类型，不能删除");
            }
            try
            {
                _unitOfWork.BeginTran();

                await _databaseTypesRepository.DeleteAsync(id);

                await _relationsRepository.DeleteAsync(m => m.DatabaseId == id);

                _unitOfWork.CommitTran();


            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }

        }


        public async Task<List<SysDatabaseFieldRelationsVM>> GetFields(long id)
        {

            var list = await _fieldTypesRepository.GetAllAsync();
            var relations = await _relationsRepository.GetListAsync(m => m.DatabaseId == id);

            var query = from m in list
                        join b in relations on m.Id equals b.FieldId into c
                        from d in c.DefaultIfEmpty()
                        orderby m.Name
                        select new SysDatabaseFieldRelationsVM
                        {
                            FieldId = m.Id,
                            FieldName = m.Name,
                            FieldDescription = m.Description,
                            DataType = d?.DataType,
                            Length = d?.Length,
                            Precision = d?.Precision,
                            DefaultValue = d?.DefaultValue,
                        };


            return query.ToList();


            //return await _relationsRepository.GetDatabaseFields(id);
        }


        public async Task SaveFieldRelations(SysDatabaseFieldsSaveReq parm)
        {
            var tokenInfo = _tokenManager.TokenInfo;
            var info = await _databaseTypesRepository.GetByIdAsync(parm.Id);
            if (info == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            var allfields = await _fieldTypesRepository.GetAllAsync();


            var relations = parm.Fields.Where(m=>!string.IsNullOrWhiteSpace(m.DataType)).Where(m => allfields.Any(c => c.Id == m.FieldId)).Select(m =>
            {
                var _m = m.Adapt<SysDatabaseFieldRelations>();
                _m.FieldId = m.FieldId;
                _m.DatabaseId = info.Id;
                return _m;
            }).ToList();

            await _relationsRepository.StorageableExecuteCommandAsync(relations, m => new { m.FieldId, m.DatabaseId });

        }
    }
}
