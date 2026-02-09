using System.Collections.Generic;
using System.Threading.Tasks;
using TableConfig.Models.Entitys;
using TableConfig.Models.Req;

namespace TableConfig.IServices.Sys
{
    public interface IUserService
    {
        Task<List<SysUsers>> GetUserListAsync();

        Task<SysUsers> GetInfoAsync(long id);

        Task<SysUsers> Save(SysUsersSaveReq parm);

        Task Del(List<long> ids);

        Task Reset();
    }
}
