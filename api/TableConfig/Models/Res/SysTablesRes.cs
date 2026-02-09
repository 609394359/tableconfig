/*
* ==============================================================================
*
* FileName: SysTablesRes.cs
* Created: 2026/1/21 14:39:54
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;
using TableConfig.Models.Entitys;

namespace TableConfig.Models.Res
{
    public class SysTablesRes : SysTables
    {
        public string GroupName { get; set;}
    }

    public class SysTableGroupsRes : SysTableGroups
    {
        public int TableCount { get; set; }
    }

    public class SysProjectTablesRes
    {
        public SysProjects Project { get; set; }
        public List<SysTableGroupsRes> Groups { get; set; }
        public List<SysTablesRes> Tables { get; set; }
    }
}
