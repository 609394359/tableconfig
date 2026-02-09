/*
* ==============================================================================
*
* FileName: FieldTypeService.cs
* Created: 2026/1/20 17:49:02
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.IRepositorys.Sys;
using TableConfig.IRepositorys;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using SqlSugar;
using System;
using System.Linq;
using TableConfig.Enums;
using TableConfig.Models.Req;
using TableConfig.Utils;
using Mapster;
using TableConfig.Models.Views;

namespace TableConfig.Services.Sys
{
    public class FieldTypeService: IFieldTypeService
    {
        private readonly ISysDatabaseTypesRepository _databaseTypesRepository;
        private readonly ISysDatabaseFieldRelationsRepository _relationsRepository;
        private readonly ISysFieldTypesRepository _fieldTypesRepository;
        private readonly TokenManager _tokenManager;
        private readonly IUnitOfWork _unitOfWork;

        public FieldTypeService(ISysDatabaseTypesRepository databaseTypesRepository, ISysDatabaseFieldRelationsRepository relationsRepository, ISysFieldTypesRepository fieldTypesRepository, TokenManager tokenManager, IUnitOfWork unitOfWork)
        {
            _databaseTypesRepository = databaseTypesRepository;
            _relationsRepository = relationsRepository;
            _fieldTypesRepository = fieldTypesRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SysFieldTypes>> GetListAsync()
        {
            var list = await _fieldTypesRepository.GetAllAsync();
            return list;
        }


        public async Task<Dictionary<long, int>> GetRelationsDatabaseCount()
        {
            return await _relationsRepository.GetFieldRelationsDatabaseCount();
        }


        public async Task<List<SysDatabaseFieldRelationsVM>> GetRelations()
        {
            return await _relationsRepository.GetRelations();
        }



        public async Task<SysFieldTypes> GetInfo(long id)
        {
            var info = await _fieldTypesRepository.GetByIdAsync(id);
            return info;
        }

        public async Task<SysFieldTypes> Save(SysFieldTypesSaveReq parm)
        {
            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                parm.Id = 0;
                var info = parm.Adapt<SysFieldTypes>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;
                await _fieldTypesRepository.InsertAsync(info);

                return info;
            }
            else
            {

                var info = await _fieldTypesRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                parm.Adapt(info);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _fieldTypesRepository.UpdateAsync(info);

                return info;
            }
        }


        public async Task Del(long id)
        {
            try
            {
                _unitOfWork.BeginTran();

                await _fieldTypesRepository.DeleteAsync(id);

                await _relationsRepository.DeleteAsync(m => m.FieldId == id);

                _unitOfWork.CommitTran();


            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }

        }


        public async Task<List<SysDatabaseFieldRelationsVM>> GetDatabases(long id)
        {
            var list = await _databaseTypesRepository.GetAllAsync();
            var relations = await _relationsRepository.GetListAsync(m => m.FieldId == id);

            var query = from m in list
                        join b in relations on m.Id equals b.DatabaseId into c
                        from d in c.DefaultIfEmpty()
                        orderby m.Name
                        select new SysDatabaseFieldRelationsVM
                        {
                            DatabaseId = m.Id,
                            DatabaseName = m.Name,
                            DatabaseDescription = m.Description,
                            DataType = d?.DataType,
                            Length = d?.Length,
                            Precision = d?.Precision,
                            DefaultValue = d?.DefaultValue,
                        };


            return query.ToList();

            //return await _relationsRepository.GetFieldDatabases(id);
        }


        public async Task SaveDatabaseRelations(SysFieldDatabasesSaveReq parm)
        {
            var tokenInfo = _tokenManager.TokenInfo;
            var info = await _fieldTypesRepository.GetByIdAsync(parm.Id);
            if (info == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            var alldatabases = await _databaseTypesRepository.GetAllAsync();


            var relations = parm.Databases.Where(m => !string.IsNullOrWhiteSpace(m.DataType)).Where(m => alldatabases.Any(c => c.Id == m.DatabaseId)).Select(m => {
                var _m = m.Adapt<SysDatabaseFieldRelations>();
                _m.DatabaseId = m.DatabaseId;
                _m.FieldId = info.Id;
                return _m;
            }).ToList();

            await _relationsRepository.StorageableExecuteCommandAsync(relations, m => new { m.FieldId, m.DatabaseId });

        }
    }
}
