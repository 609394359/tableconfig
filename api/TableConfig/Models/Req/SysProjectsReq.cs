/*
* ==============================================================================
*
* FileName: SysProjectsReq.cs
* Created: 2026/1/21 11:40:43
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
    public class SysProjectsSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "项目名称")]
        [Required(ErrorMessage = "项目名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "项目描述")]
        [Required(ErrorMessage = "项目描述不能为空")]
        public string Description { get; set; }

        [Display(Name = "默认数据库id")]
        public long? DatabaseId { get; set; }
    }

    public class SysTableGroupsSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "项目Id")]
        public long ProjectId { get; set; }

        [Display(Name = "分组名称")]
        [Required(ErrorMessage = "分组名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "分组描述")]
        [Required(ErrorMessage = "分组描述不能为空")]
        public string Description { get; set; }

        [Display(Name = "排序")]
        public int SortIndex { get; set; }
    }

    public class SysTablesSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "项目Id")]
        public long ProjectId { get; set; }

        [Display(Name = "分组Id")]
        public long GroupId { get; set; }

        [Display(Name = "表名称")]
        [Required(ErrorMessage = "表名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "表描述")]
        [Required(ErrorMessage = "表描述不能为空")]
        public string Description { get; set; }
    }

    public class SysTableFieldsSaveReq
    {
        [Display(Name = "主键")]
        public long? Id { get; set; }

        [Display(Name = "表Id")]
        public long TableId { get; set; }

        [Display(Name = "表字段类型Id")]
        public long FieldTypeId { get; set; }

        [Display(Name = "字段名称")]
        [Required(ErrorMessage = "字段名称不能为空")]
        public string Name { get; set; }

        [Display(Name = "字段描述")]
        [Required(ErrorMessage = "字段描述不能为空")]
        public string Description { get; set; }

        [Display(Name = "默认值")]
        public string DefaultValue { get; set; }

        [Display(Name = "主键")]
        public bool IsPrimaryKey { get; set; }

        [Display(Name = "自增")]
        public bool IsIdentity { get; set; }

        [Display(Name = "唯一")]
        public bool IsUnique { get; set; }

        [Display(Name = "可空")]
        public bool IsNull { get; set; }
    }

    public class SysTableFieldsSortReq
    {

        [Display(Name = "表Id")]
        public long TableId { get; set; }

        [Display(Name = "字段ids")]
        public List<long> Ids { get; set; }
    }
    public class SysTableFieldsCopyReq
    {

        [Display(Name = "表Id")]
        public long TableId { get; set; }

        [Display(Name = "字段ids")]
        public List<long> Ids { get; set; }
    }
}
