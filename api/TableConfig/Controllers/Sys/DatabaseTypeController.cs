using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using TableConfig.IServices.Sys;
using TableConfig.Models.Req;
using TableConfig.Models.Views;
using TableConfig.Models;
using TableConfig.Services;
using TableConfig.Services.Sys;
using System.Linq;
using Mapster;
using TableConfig.Models.Res;
using System.Collections.Generic;
using TableConfig.Models.Entitys;
using TableConfig.Enums;
using TableConfig.Utils;
using TableConfig.Attributes;

namespace TableConfig.Controllers.Sys
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTypeController : BaseController
    {
        private readonly ILogger<DatabaseTypeController> _logger;
        private readonly IDatabaseTypeService _databaseTypeService;
        private readonly TokenManager _tokenManager;

        public DatabaseTypeController(ILogger<DatabaseTypeController> logger, IDatabaseTypeService databaseTypeService, TokenManager tokenManager)
        {
            _logger = logger;
            _databaseTypeService = databaseTypeService;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// 数据库类型列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("List")]
        [Authorization]
        public async Task<ApiResult<List<SysDatabaseTypesRes>>> List()
        {
            var list = await _databaseTypeService.GetListAsync();
            var dict = await _databaseTypeService.GetDatabaseRelationsFieldCount();

            var response = list.Select(m =>
            {
                var _m = m.Adapt<SysDatabaseTypesRes>();
                _m.FieldCount = dict.ContainsKey(m.Id) ? dict[m.Id] : 0;
                return _m;
            }).ToList();

            return ToResult(response);
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorization]
        public async Task<ApiResult<SysDatabaseTypes>> Get(long id)
        {
            var info = await _databaseTypeService.GetInfo(id);

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
        public async Task<ApiResult<SysDatabaseTypes>> Save([FromBody] SysDatabaseTypesSaveReq req)
        {
            var info = await _databaseTypeService.Save(req);

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
            await _databaseTypeService.Del(req.Id);

            return ToResult("ok");
        }


        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Field/List")]
        [Authorization]
        public async Task<ApiResult<List<SysDatabaseFieldRelationsVM>>> FieldList(long id)
        {
            var list = await _databaseTypeService.GetFields(id);

            return ToResult(list);
        }


        /// <summary>
        /// 保存字段列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Field/Save")]
        [Authorization]
        public async Task<ApiResult> FieldSave([FromBody] SysDatabaseFieldsSaveReq req)
        {
            await _databaseTypeService.SaveFieldRelations(req);

            return ToResult("ok");
        }

    }
}
