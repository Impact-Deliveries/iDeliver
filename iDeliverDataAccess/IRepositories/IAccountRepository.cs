using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Base;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.EntityFrameworkCore;

namespace iDeliverDataAccess.Repositories
{
    public interface IAccountRepository
    {
        Task<UserDTO?> Login(string username, int? roleID);
        Task<List<Account>> Accounts(long UserID);
    }
}