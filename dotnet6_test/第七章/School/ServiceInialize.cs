using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zack.Commons;

namespace School
{
    public class ServiceInitialize : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<SchoolService>();
        }
    }
}
