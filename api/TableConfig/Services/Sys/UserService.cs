/*
* ==============================================================================
*
* FileName: UserService.cs
* Created: 2026/1/21 16:25:46
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Enums;
using TableConfig.IRepositorys;
using TableConfig.IRepositorys.Sys;
using TableConfig.IServices.Sys;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;
using TableConfig.Utils;

namespace TableConfig.Services.Sys
{
    public class UserService : IUserService
    {
        private readonly ISysUsersRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly IUnitOfWork _unitOfWork;


        public UserService(ISysUsersRepository userRepository, TokenManager tokenManager, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _tokenManager = tokenManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<SysUsers>> GetUserListAsync()
        {
            return await _userRepository.GetAllAsync();
        }


        public async Task<SysUsers> GetInfoAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<SysUsers> Save(SysUsersSaveReq parm)
        {
            var tokenInfo = _tokenManager.TokenInfo;
            if (parm.Id == null || parm.Id == 0)
            {
                if (await _userRepository.AnyAsync(m => m.UserCode == parm.UserCode))
                    throw new Tip(StatusCodeType.Error, "账号已存在");

                parm.Id = 0;
                var info = parm.Adapt<SysUsers>();
                info.Id = SnowFlakeSingle.instance.NextId();
                info.CreateId = tokenInfo.Id;
                info.CreateName = tokenInfo.Name;
                info.CreateTime = DateTime.Now;
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;


                info.Password = PasswordUtil.CreateDbPassword(info.Id, parm.Password);

                await _userRepository.InsertAsync(info);

                return info;
            }
            else
            {
                if (await _userRepository.AnyAsync(m => m.UserCode == parm.UserCode && m.Id!=parm.Id.Value))
                    throw new Tip(StatusCodeType.Error, "账号已存在");

                var info = await _userRepository.GetByIdAsync(parm.Id);
                if (info == null)
                    throw new Tip(StatusCodeType.Error, "数据不存在");

                if (!string.IsNullOrWhiteSpace(parm.Password))
                {
                    parm.Password = PasswordUtil.CreateDbPassword(info.Id, parm.Password);
                }
                else {
                    parm.Password = info.Password;
                }

                parm.Adapt(info);
                info.UpdateId = tokenInfo.Id;
                info.UpdateName = tokenInfo.Name;
                info.UpdateTime = DateTime.Now;

                await _userRepository.UpdateAsync(info);

                return info;
            }
        }

        public async Task Del(List<long> ids)
        {
            await _userRepository.DeleteAsync(ids);

            _tokenManager.Remove(ids);
        }


        public async Task Reset()
        {
            var user = new SysUsers
            {
                Id = 1111111111111111111L,
                UserCode = "admin",
                UserName = "系统管理员",
                Password = "123456",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            user.CreateId = user.Id;
            user.CreateName = user.UserName;
            user.UpdateId = user.Id;
            user.UpdateName = user.UserName;
            user.Password = PasswordUtil.CreateDbPassword(user.Id, user.Password);

            if (await _userRepository.AnyAsync(m => m.UserCode == user.UserCode)) 
                return;

            await _userRepository.InsertAsync(user);
        }
    }
}
