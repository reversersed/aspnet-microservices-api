using Extensions.HttpExtension.Clients;
using Extensions.HttpExtension.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.HttpExtension
{
    public static class HttpExtension
    {
        public static void UseHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IInternalHttpClient, InternalHttpClient>();
        }
    }
}
