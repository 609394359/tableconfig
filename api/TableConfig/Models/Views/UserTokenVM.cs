/*
* ==============================================================================
*
* FileName: UserTokenVM.cs
* Created: 2026/1/8 9:48:23
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System;

namespace TableConfig.Models.Views
{
    public class UserTokenVM
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
