/*
* ==============================================================================
*
* FileName: SysDatabases.cs
* Created: 2026/1/19 16:37:43
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
    /// 数据库类型
    /// </summary>
    [SugarTable("t_sys_database_types")]
    public class SysDatabaseTypes : IDeleted
    {
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "版本")]
        public string Version { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "创建脚本")]
        public string CreateScripts { get; set; }


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
