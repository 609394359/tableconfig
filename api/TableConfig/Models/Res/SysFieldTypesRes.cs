/*
* ==============================================================================
*
* FileName: SysFieldTypesRes.cs
* Created: 2026/1/20 17:52:08
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TableConfig.Models.Entitys;
using TableConfig.Models.Views;

namespace TableConfig.Models.Res
{
    public class SysFieldTypesRes: SysFieldTypes
    {

        public List<SysDatabaseFieldRelationsVM> Relations { get; set; }

        public int DatabaseCount { get; set; }



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
