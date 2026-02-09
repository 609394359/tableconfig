/*
* ==============================================================================
*
* FileName: SysDatabaseFieldRelations.cs
* Created: 2026/1/19 17:02:37
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace TableConfig.Models.Entitys
{
    /// <summary>
    /// 数据库字段关系
    /// </summary>
    [SugarTable("t_sys_database_field_relations")]
    public class SysDatabaseFieldRelations
    {
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true)]
        public long DatabaseId { get; set; }

        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true)]
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
