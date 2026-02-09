/*
* ==============================================================================
*
* FileName: SysProjectsRes.cs
* Created: 2026/1/21 11:23:01
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Models.Entitys;

namespace TableConfig.Models.Res
{
    public class SysProjectsRes: SysProjects
    {
        public int TableCount { get; set; }
        public int GroupCount { get; set; }
    }
}
