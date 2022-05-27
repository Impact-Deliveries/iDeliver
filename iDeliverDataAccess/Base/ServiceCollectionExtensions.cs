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
            services.AddScoped<IDriverCaseRepository, DriverCaseRepository>();
            services.AddScoped<IDriverDetailsRepository, DriverDetailRepository>();
            services.AddScoped<IDriverSchaduleRepository, DriverSchaduleRepository>();
            services.AddScoped<IEnrolmentRepository, EnrolmentRepository>();
            services.AddScoped<IOrganizationRepository,OrganizationRepository >();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IMerchantBranchRepository, MerchantBranchRepository>();
            services.AddScoped<IMerchantEmployeeRepository, MerchantEmployeeRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IMerchantDeliveryPriceRepository, MerchantDeliveryPriceRepository>();
        }
    }
}
