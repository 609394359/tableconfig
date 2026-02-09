/*
* ==============================================================================
*
* FileName: SysTableFields.cs
* Created: 2026/1/20 9:43:27
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System;

namespace TableConfig.Models.Entitys
{
    /// <summary>
    /// 表字段
    /// </summary>
    [SugarTable("t_sys_table_fields")]
    public class SysTableFields : IDeleted
    {
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [Display(Name = "项目Id")]
        public long ProjectId { get; set; }

        [Display(Name = "表Id")]
        public long TableId { get; set; }

        [Display(Name = "表字段类型Id")]
        public long FieldTypeId { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
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

        [Display(Name = "排序")]
        public int SortIndex { get; set; }


        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "创建用户Id")]
        public long CreateId { get; set; }

        [Display(Name = "创建用户名称")]
        public string CreateName { get; set; }

        [Display(Name = "修改时间")]
        public DateTime UpdateTime { get; set; }

        [Display(Name = "修改用户Id")]
        public long UpdateId { get; set; }

        [Display(Name = "修改用户名称")]
        public string UpdateName { get; set; }

        [Display(Name = "是否删除", Order = 0)]
        public bool IsDeleted { get; set; }
    }
}
