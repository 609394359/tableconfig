/*
* ==============================================================================
*
* FileName: Tip.cs
* Created: 2026/1/1 17:08:27
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System;
using TableConfig.Attributes;
using TableConfig.Enums;

namespace TableConfig.Utils
{
    public class Tip : Exception
    {

        public Tip(string message) : base(message)
        {
            this.Code = (int)StatusCodeType.Error;
        }
        public Tip(StatusCodeType code, string message) : base(message)
        {
            this.Code = (int)code;
        }
        public Tip(StatusCodeType code) : base(code.ToText())
        {
            this.Code = (int)code;
        }

        public Tip(int code, string message) : base(message)
        {
            Code = code;
        }
        public int Code { get; set; }
    }
}
