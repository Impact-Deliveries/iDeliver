using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Repositories;

namespace iDeliverDataAccess.Base
{
    public static class ServiceCollectionExtensions
    {
        public static void RepositroyScoped(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IDriverDetailsRepository, DriverDetailRepository>();
            services.AddScoped<IDriverSchaduleRepository, DriverSchaduleRepository>();
            services.AddScoped<IEnrolmentRepository, EnrolmentRepository>();
        }
    }
}
