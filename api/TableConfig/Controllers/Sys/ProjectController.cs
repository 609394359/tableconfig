using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.IServices.Sys;
using TableConfig.Models.Res;
using TableConfig.Models;
using TableConfig.Services;
using TableConfig.Services.Sys;
using TableConfig.Models.Entitys;
using System.Linq;
using Mapster;
using TableConfig.Models.Req;
using TableConfig.Enums;
using TableConfig.Utils;
using TableConfig.Attributes;
using TableConfig.Models.Views;

namespace TableConfig.Controllers.Sys
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : BaseController
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;
        private readonly TokenManager _tokenManager;

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService, TokenManager tokenManager)
        {
            _logger = logger;
            _projectService = projectService;
            _tokenManager = tokenManager;
        }


        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [Authorization]
        public async Task<ApiResult<List<SysProjectsRes>>> List()
        {
            var list = await _projectService.GetListAsync();
            var dict1 = await _projectService.GetProjectGroupCount();
            var dict2 = await _projectService.GetProjectTableCount();

            var response = list.OrderBy(m=>m.Name).Select(m =>
            {
                var _m = m.Adapt<SysProjectsRes>();
                _m.GroupCount = dict1.ContainsKey(m.Id) ? dict1[m.Id] : 0;
                _m.TableCount = dict2.ContainsKey(m.Id) ? dict2[m.Id] : 0;
                return _m;
            }).ToList();

            return ToResult(response);
        }

        /// <summary>
        /// 数据库列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Database/List")]
        [Authorization]
        public async Task<ApiResult<List<SysDatabaseTypes>>> DatabaseList()
        {
            var list = await _projectService.GetDatabaseListAsync();

            return ToResult(list);
        }

        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorization]
        public async Task<ApiResult<SysProjects>> Get(long id)
        {
            var info = await _projectService.GetProjectInfo(id);

            if (info == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            return ToResult(info);
        }

        /// <summary>
        /// 项目保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorization]
        public async Task<ApiResult<SysProjects>> Save([FromBody] SysProjectsSaveReq req)
        {
            var info = await _projectService.Save(req);

            return ToResult(info);
        }

        /// <summary>
        /// 项目删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Del")]
        [Authorization]
        public async Task<ApiResult> Del([FromBody] BaseIdReq req)
        {
            await _projectService.Del(req.Id);

            return ToResult("ok");
        }



        /// <summary>
        /// 表列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Table/List")]
        [Authorization]
        public async Task<ApiResult<SysProjectTablesRes>> TableList(long projectId)
        {
            var project = await _projectService.GetProjectInfo(projectId);
            var groupList = await _projectService.GetGroupListAsync(projectId);
            var tableList = await _projectService.GetTableListAsync(projectId);

            var response = new SysProjectTablesRes
            {
                Project = project,
                Groups = groupList.OrderBy(m => m.SortIndex).Select(m =>
                {
                    var _m = m.Adapt<SysTableGroupsRes>();
                    _m.TableCount = tableList.Count(n => n.GroupId == m.Id);
                    return _m;
                }).ToList(),
                Tables = tableList.OrderBy(m=>m.Name).Select(m =>
                {
                    var _m = m.Adapt<SysTablesRes>();
                    _m.GroupName = groupList.FirstOrDefault(n => n.Id == m.GroupId)?.Name;
                    return _m;
                }).ToList()
            };

            return ToResult(response);
        }


        /// <summary>
        /// 分组保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Group/Save")]
        [Authorization]
        public async Task<ApiResult<SysTableGroups>> GroupSave([FromBody] SysTableGroupsSaveReq req)
        {
            var info = await _projectService.TableGroupSave(req);

            return ToResult(info);
        }


        /// <summary>
        /// 分组删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Group/Del")]
        [Authorization]
        public async Task<ApiResult> GroupDel([FromBody] BaseIdReq req)
        {
            await _projectService.TableGroupDel(req.Id);

            return ToResult("ok");
        }



        /// <summary>
        /// 表保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Table/Save")]
        [Authorization]
        public async Task<ApiResult<SysTables>> TableSave([FromBody] SysTablesSaveReq req)
        {
            var info = await _projectService.TableSave(req);

            return ToResult(info);
        }


        /// <summary>
        /// 表复制
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Table/Copy")]
        [Authorization]
        public async Task<ApiResult> TableCopy([FromBody] BaseIdReq req)
        {
            await _projectService.TableCopy(req.Id);

            return ToResult("ok");
        }


        /// <summary>
        /// 表删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Table/Del")]
        [Authorization]
        public async Task<ApiResult> TableDel([FromBody] BaseIdReq req)
        {
            await _projectService.TableDel(req.Id);

            return ToResult("ok");
        }



        /// <summary>
        /// 字段类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("FieldType/List")]
        [Authorization]
        public async Task<ApiResult<List<SysFieldTypesRes>>> FieldTypeList(long databaseId)
        {
            var fields = await _projectService.GetFieldTypeListAsync();
            var relations = await _projectService.GetDatabaseFieldRelationListAsync(databaseId);

            var response = fields.Select(m =>
            {
                var _m = m.Adapt<SysFieldTypesRes>();
                var relation = relations.FirstOrDefault(n => n.FieldId == m.Id);
                _m.DataType = relation?.DataType;
                _m.Length = relation?.Length;
                _m.Precision = relation?.Precision;
                _m.DefaultValue = relation?.DefaultValue;
                return _m;
            }).ToList();

            return ToResult(response);
        }

        /// <summary>
        /// 字段列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Field/List")]
        [Authorization]
        public async Task<ApiResult<List<SysTableFieldsRes>>> FieldList(long databaseId,long tableId)
        {
            var fields = await _projectService.GetTableFieldListAsync(tableId);
            var database = await _projectService.GetDatabaseTypeAsync(databaseId);
            var fieldTypes = await _projectService.GetFieldTypeListAsync();
            var relations = await _projectService.GetDatabaseFieldRelationListAsync(databaseId);

            var response = fields.Select(m =>
            {
                var _m = m.Adapt<SysTableFieldsRes>();
                _m.DatabaseName = database.Name;
                _m.FieldTypeName = fieldTypes.FirstOrDefault(n => n.Id == m.FieldTypeId)?.Name;
                _m.Relation = relations.FirstOrDefault(n => n.FieldId == m.FieldTypeId)?.Adapt<SysDatabaseFieldRelationsVM>();
                return _m;
            }).ToList();

            return ToResult(response);
        }

        /// <summary>
        /// 字段列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Field/Get")]
        [Authorization]
        public async Task<ApiResult<SysTableFields>> FieldGet(long tableFieldId)
        {
            var fields = await _projectService.GetTableFieldAsync(tableFieldId);

            return ToResult(fields);
        }

        /// <summary>
        /// 字段保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Field/Save")]
        [Authorization]
        public async Task<ApiResult<SysTableFields>> FieldSave([FromBody] SysTableFieldsSaveReq req)
        {
            var fields = await _projectService.TableFieldSave(req);

            return ToResult(fields);
        }

        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Field/Sort")]
        [Authorization]
        public async Task<ApiResult> FieldSort([FromBody] SysTableFieldsSortReq req)
        {
            await _projectService.TableFieldSort(req);

            return ToResult("ok");
        }

        /// <summary>
        /// 字段继承
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Field/Copy")]
        [Authorization]
        public async Task<ApiResult> FieldCopy([FromBody] SysTableFieldsCopyReq req)
        {
            await _projectService.TableFieldCopy(req);

            return ToResult("ok");
        }

        /// <summary>
        /// 字段删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Field/Del")]
        [Authorization]
        public async Task<ApiResult> FieldDel([FromBody] BaseIdReq req)
        {
            await _projectService.TableFieldDel(req.Id);

            return ToResult("ok");
        }


        [HttpGet("Sql/Preview")]
        [Authorization]
        public async Task<ApiResult> SqlPreview(long databaseId, long tableId)
        {
            var table = await _projectService.GetTableAsync(tableId);
            var fields = await _projectService.GetTableFieldListAsync(tableId);
            var database = await _projectService.GetDatabaseTypeAsync(databaseId);
            var relations = await _projectService.GetDatabaseFieldRelationListAsync(databaseId);


            var dataFields = fields.Select(m => {
                var relation = relations.First(c => c.FieldId == m.FieldTypeId)?.Adapt<SysDatabaseFieldRelationsVM>();
                return new
                {
                    Name = m.Name,
                    Des = m.Description,
                    IsPrimaryKey= m.IsPrimaryKey,
                    IsIdentity = m.IsIdentity,
                    IsUnique = m.IsUnique,
                    IsNull = m.IsNull,
                    DefaultValue = m.DefaultValue,
                    Length = relation?.Length,
                    Precision = relation?.Precision,
                    DbType = relation?.DataType,
                    DbTypeValue = relation?.DataTypeString,
                };
            }).ToList();

            var data = new
            {
                DatabaseName = database.Name,
                CreateScripts = database.CreateScripts,
                TableName = table.Name,
                TableDescription = table.Description,
                Fields = dataFields,
                PrimaryKeys = dataFields.Where(m => m.IsPrimaryKey).ToList(),
                Identitys = dataFields.Where(m => m.IsIdentity).ToList(),
                Uniques = dataFields.Where(m => m.IsUnique).ToList(),
            };

            return ToResult(data);
        }
    }
}
