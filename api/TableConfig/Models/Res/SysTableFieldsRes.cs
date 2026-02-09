/*
* ==============================================================================
*
* FileName: SysTableFieldsRes.cs
* Created: 2026/1/21 15:12:52
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.ComponentModel.DataAnnotations;
using TableConfig.Models.Entitys;
using TableConfig.Models.Views;

namespace TableConfig.Models.Res
{
    public class SysTableFieldsRes: SysTableFields
    {
        public string DatabaseName { get; set;}
        public string FieldTypeName { get; set;}

        public SysDatabaseFieldRelationsVM Relation { get; set;}
    }
}
