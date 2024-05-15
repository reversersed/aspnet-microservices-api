using Extensions.MediatorExtension.Middlewares;
using Extensions.MediatorExtension.Pipelines;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.MediatorExtension
{
    public static class MediatRExtension
    {
        public static void UseMediatR(this IServiceCollection services, System.Type assembly)
        {
            services.AddHttpContextAccessor();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly.Assembly));
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}
