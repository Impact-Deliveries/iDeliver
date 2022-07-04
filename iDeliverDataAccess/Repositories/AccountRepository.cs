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
    internal class AccountRepository : IAccountRepository
    {
        private readonly IDeliverDbContext _context;

        public AccountRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO?> Login(string username, int? roleID)
        {

            var query = await (from user in _context.Users
                               join enrolment in _context.Enrolments on user.Id equals enrolment.UserId
                               where user.Username == username && user.IsActive == true &&
                               (roleID.HasValue ? enrolment.RoleId == roleID : 1 == 1)
                               select new UserDTO
                               {
                                   UserID = user.Id,
                                   Username = user.Username,
                                   Password = user.Password
                               }).FirstOrDefaultAsync();

            if (query is not null)
            {
                var enrolments = await (from enrolment in _context.Enrolments
                                        where enrolment.UserId == query.UserID
                                        select enrolment).ToListAsync();


                foreach (var item in enrolments)
                {
                    var role = (IDeliverObjects.Enum.Roles)item.RoleId;
                    query.Roles.Add(role.ToString());
                }
            }

            return query;
        }

        public async Task<List<Account>> Accounts(long UserID)
        {
            List<Enrolment> enrolments = await (from enrolment in _context.Enrolments
                                                where enrolment.UserId == UserID
                                                select enrolment).ToListAsync();

            List<Account> accounts = new List<Account>();

            foreach (Enrolment item in enrolments)
            {
                Account? account = new Account();
                switch (item.RoleId)
                {
                    case 1:
                        // organization employee
                        account = await (from employee in _context.OrganizationEmployees
                                         join organization in _context.Organizations on employee.OrganizationId equals organization.Id
                                         where employee.EnrolmentId == item.Id && employee.IsActive == true
                                         select new Account()
                                         {
                                             EnrolmentID = employee.EnrolmentId,
                                             FirstName = employee.FirstName,
                                             LastName = employee.LastName,
                                             EmployeeID = employee.Id,
                                             MiddleName = employee.MiddleName,
                                             Organization = new OrganizationDTO()
                                             {
                                                 Id = employee.Organization.Id,
                                                 Name = employee.Organization.Name,
                                                 Timezone = employee.Organization.Timezone
                                             },
                                             Role = ((IDeliverObjects.Enum.Roles)employee.Enrolment.RoleId).ToString(),
                                             ProfilePicture = employee.ProfilePicture,
                                             Status = (short)IDeliverObjects.Enum.DriverCaseStatus.available,
                                             IsOnline = true
                                         }).FirstOrDefaultAsync();

                        if (account is not null)
                        {
                            accounts.Add(account);

                            #region account device                            
                            var accountDevice = await (from device in _context.EnrolmentDevices
                                                       where device.EnrolmentId == item.Id
                                                       select device).FirstOrDefaultAsync();

                            if (accountDevice is not null)
                                account.Device = accountDevice;
                            #endregion
                        }
                        break;
                    case 2:
                    // marchent employee
                    case 3:
                        // driver
                        account = await (from driver in _context.Drivers
                                         where driver.EnrolmentId == item.Id && driver.IsActive == true
                                         select new Account()
                                         {
                                             EnrolmentID = driver.EnrolmentId,
                                             EmployeeID = driver.Id,
                                             FirstName = driver.FirstName,
                                             LastName = driver.LastName,
                                             MiddleName = driver.SecondName,
                                             Role = ((IDeliverObjects.Enum.Roles)driver.Enrolment.RoleId).ToString(),
                                             IsOnline = false,
                                             Status = (short)IDeliverObjects.Enum.DriverCaseStatus.unavailable,
                                             Organization = new OrganizationDTO()
                                             {
                                                 Id = driver.Organization.Id,
                                                 Name = driver.Organization.Name,
                                                 Timezone = driver.Organization.Timezone
                                             },
                                         }).FirstOrDefaultAsync();

                        if (account is not null)
                        {
                            accounts.Add(account);

                            #region account case
                            var accountCase = await (from driverCase in _context.DriverCases
                                                     where driverCase.DriverId == account.EmployeeID
                                                     select driverCase).FirstOrDefaultAsync();

                            if (accountCase is not null)
                            {
                                account.IsOnline = accountCase.IsOnline;
                                account.Status = accountCase.Status;
                            } 
                            #endregion

                            #region account device
                            var accountDevice = await (from device in _context.EnrolmentDevices
                                                       where device.EnrolmentId == item.Id
                                                       select device).FirstOrDefaultAsync();

                            if (accountDevice is not null)
                                account.Device = accountDevice;
                            #endregion
                        };
                        break;
                }
            }

            return accounts;
        }
    }
}
