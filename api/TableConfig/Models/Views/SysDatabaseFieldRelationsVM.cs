/*
* ==============================================================================
*
* FileName: SysDatabaseFieldRelationsVM.cs
* Created: 2026/1/20 16:53:51
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using TableConfig.Models.Entitys;

namespace TableConfig.Models.Views
{
    public class SysDatabaseFieldRelationsVM
    {
        [Display(Name = "数据库主键")]
        [SugarColumn(IsPrimaryKey = true)]
        public long DatabaseId { get; set; }

        [Display(Name = "字段主键")]
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


        [Display(Name = "字段名称")]
        public string FieldName { get; set; }

        [Display(Name = "字段描述")]
        public string FieldDescription { get; set; }


        [Display(Name = "数据库名称")]
        public string DatabaseName { get; set; }

        [Display(Name = "数据库描述")]
        public string DatabaseDescription { get; set; }


        public string DataTypeString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DataType))
                    return "";

                if (Length == null)
                    return DataType;

                if (Precision == null)
                    return $"{DataType}({Length})";

                return $"{DataType}({Length},{Precision})";
            }
        }
    }
}
