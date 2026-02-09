/*
* ==============================================================================
*
* FileName: SysFieldTypesReq.cs
* Created: 2026/1/20 17:54:49
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Req
{
    public class SysFieldTypesSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "字段名称")]
        [Required(ErrorMessage = "字段名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }
    }


    public class SysFieldDatabasesSaveReq
    {
        [Display(Name = "字段类型Id")]
        public long Id { get; set; }

        [Display(Name = "数据库列表")]
        public List<SysFieldDatabasesSaveItemReq> Databases { get; set; } = [];
    }
    public class SysFieldDatabasesSaveItemReq
    {

        [Display(Name = "数据库Id")]
        public long DatabaseId { get; set; }

        [Display(Name = "Sql类型")]
        public string DataType { get; set; }

        [Display(Name = "长度")]
        public int? Length { get; set; }

        [Display(Name = "精度")]
        public int? Precision { get; set; }

        [Display(Name = "默认值")]
        public string DefaultValue { get; set; }
    }
}
