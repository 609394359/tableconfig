/*
* ==============================================================================
*
* FileName: MD5Encode.cs
* Created: 2026/1/5 16:45:28
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using System.Security.Cryptography;
using System.Text;
using System;

namespace TableConfig.Utils
{
    public class MD5Encode
    {
        /// <summary>
        /// 32位 MD5 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string content)
        {
            var md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(content))).Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 16位 MD5 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string content)
        {
            var md5 = MD5.Create();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(content)), 4, 8);
            t2 = t2.Replace("-", string.Empty);
            return t2.ToUpper();
        }



    }
}
