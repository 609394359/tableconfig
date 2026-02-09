/*
* ==============================================================================
*
* FileName: BaseReq.cs
* Created: 2026/1/20 15:43:54
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Collections.Generic;

namespace TableConfig.Models.Req
{
    public class BaseIdReq
    {
        public long Id { get; set; }
    }
    public class BaseIdsReq
    {
        public List<long> Ids { get; set; }
    }
}
