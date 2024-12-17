using Jeez.Workflow.API.Dtos;
using Jeez.Workflow.API.Models;
using JeezFoundation.Dapper;

namespace Jeez.Workflow.API.Repositories.IRepository
{
    public interface IUserRepository : IDapperRepository<User>
    {
        Task<List<User>> UserGetListPageAsync(UserGetListPageDto userGetListDto);

        Task<int> UserGetListCountPageAsync(UserGetListPageDto userGetListDto);
    }
}
