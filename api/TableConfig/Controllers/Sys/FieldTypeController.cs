using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Models.Res;
using TableConfig.Models.Views;
using TableConfig.Models;
using TableConfig.Services;
using TableConfig.Utils;
using Mapster;
using TableConfig.Attributes;

namespace TableConfig.Controllers.Sys
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldTypeController : BaseController
    {
        private readonly ILogger<FieldTypeController> _logger;
        private readonly IFieldTypeService _fieldTypeService;
        private readonly TokenManager _tokenManager;

        public FieldTypeController(ILogger<FieldTypeController> logger, IFieldTypeService fieldTypeService, TokenManager tokenManager)
        {
            _logger = logger;
            _fieldTypeService = fieldTypeService;
            _tokenManager = tokenManager;
        }


        /// <summary>
        /// 字段类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [Authorization]
        public async Task<ApiResult<List<SysFieldTypesRes>>> List()
        {
            var list = await _fieldTypeService.GetListAsync();
            var relations = await _fieldTypeService.GetRelations();

            var response = list.OrderBy(m => m.Name).Select(m =>
            {
                var _m = m.Adapt<SysFieldTypesRes>();
                _m.Relations = relations.Where(c=>c.FieldId == m.Id).OrderBy(m=>m.DatabaseName).ToList();
                return _m;
            }).ToList();

            return ToResult(response);
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorization]
        public async Task<ApiResult<SysFieldTypes>> Get(long id)
        {
            var info = await _fieldTypeService.GetInfo(id);

            if (info == null)
                throw new Tip(StatusCodeType.Error, "数据不存在");

            return ToResult(info);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorization]
        public async Task<ApiResult<SysFieldTypes>> Save([FromBody] SysFieldTypesSaveReq req)
        {
            var info = await _fieldTypeService.Save(req);

            return ToResult(info);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Del")]
        [Authorization]
        public async Task<ApiResult> Del([FromBody] BaseIdReq req)
        {
            await _fieldTypeService.Del(req.Id);

            return ToResult("ok");
        }


        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Database/List")]
        [Authorization]
        public async Task<ApiResult<List<SysDatabaseFieldRelationsVM>>> DatabaseList(long id)
        {
            var list = await _fieldTypeService.GetDatabases(id);

            return ToResult(list);
        }


        /// <summary>
        /// 保存数据库列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Database/Save")]
        [Authorization]
        public async Task<ApiResult> FieldSave([FromBody] SysFieldDatabasesSaveReq req)
        {
            await _fieldTypeService.SaveDatabaseRelations(req);

            return ToResult("ok");
        }
    }
}
