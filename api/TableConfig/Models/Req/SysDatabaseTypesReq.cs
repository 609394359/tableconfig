/*
* ==============================================================================
*
* FileName: SysDatabaseTypesReq.cs
* Created: 2026/1/20 15:47:31
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace TableConfig.Models.Req
{
    public class SysDatabaseTypesSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "数据库名称")]
        [Required(ErrorMessage = "数据库名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "版本")]
        [Required(ErrorMessage = "版本不能为空")]
        public string Version { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建脚本")]
        public string CreateScripts { get; set; }

    }


    public class SysDatabaseFieldsSaveReq
    {
        [Display(Name = "数据库类型Id")]
        public long Id { get; set; }

        [Display(Name = "字段列表")]
        public List<SysDatabaseFieldsSaveItemReq> Fields { get; set; } = [];
    }
    public class SysDatabaseFieldsSaveItemReq
    {

        [Display(Name = "字段Id")]
        public long FieldId { get; set; }

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
