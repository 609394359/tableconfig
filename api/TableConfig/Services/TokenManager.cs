/*
* ==============================================================================
*
* FileName: TokenManager.cs
* Created: 2026/1/8 11:32:33
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Models.Views;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using NetTaste;

namespace TableConfig.Services
{
    public class TokenManager
    {
        private readonly CacheContext _cache;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenManager(CacheContext cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token => _httpContextAccessor.HttpContext.Request.Headers["Token"].ToString();

        public UserTokenVM TokenInfo => _cache.Get<UserTokenVM>(CacheContext.GroupToken, Token);

        public string SetToken(UserTokenVM userToken)
        {
            _cache.Set(CacheContext.GroupToken, userToken.Token, userToken, TimeSpan.FromHours(2), false);
            return userToken.Token;
        }
        public void Remove(string token)
        {
            _cache.Remove(CacheContext.GroupToken, token);
        }
        public void Remove(List<long> userIds)
        {
            var values = _cache.GetValues<UserTokenVM>(CacheContext.GroupToken).ToList();

            foreach (var item in values)
            {
                if (!userIds.Contains(item.Id)) continue;
                _cache.Remove(CacheContext.GroupToken, item.Token);
            }
        }
        public void Remove()
        {
            _cache.Remove(CacheContext.GroupToken, this.Token);
        }
        public bool IsLogin()
        {
            return TokenInfo != null;
        }
    }
}
