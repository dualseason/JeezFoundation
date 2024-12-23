using Consul;
using JeezFoundation.Core.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace JeezFoundation.Consul
{
    /// <summary>
    /// Consul 中间件扩展
    /// </summary>
    public static class ConsulExtension
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="checkOptions"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceRegistration(this IApplicationBuilder app, ServiceCheckOptions checkOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var lifetime = app.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;

            var serviceOptions = app.ApplicationServices.GetService(typeof(IOptions<ServiceDiscoveryOptions>)) as IOptions<ServiceDiscoveryOptions>;
            var consul = app.ApplicationServices.GetService(typeof(IConsulClient)) as IConsulClient;

            lifetime.ApplicationStarted.Register(() =>
            {
                OnStart(app, serviceOptions.Value, consul, lifetime, checkOptions);
            });
            lifetime.ApplicationStopped.Register(() =>
            {
                OnStop(app, serviceOptions.Value, consul, lifetime);
            });

            return app;
        }
        private static void OnStop(IApplicationBuilder app, ServiceDiscoveryOptions serviceOptions, IConsulClient consul, IApplicationLifetime lifetime)
        {
            var serviceId = $"{serviceOptions.Service.Name}_{serviceOptions.Service.Address}:{serviceOptions.Service.Port}";

            consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();

        }

        private static void OnStart(IApplicationBuilder app, ServiceDiscoveryOptions serviceOptions, IConsulClient consul, IApplicationLifetime lifetime, ServiceCheckOptions checkOptions)
        {

            var serviceId = $"{serviceOptions.Service.Name}_{serviceOptions.Service.Address}:{serviceOptions.Service.Port}";
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                Interval = TimeSpan.FromSeconds(serviceOptions.Service.Interval),
                HTTP = $"http://{serviceOptions.Service.Address}:{serviceOptions.Service.Port}/{checkOptions.HealthCheckUrl}"
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                Address = serviceOptions.Service.Address,
                ID = serviceId,
                Name = serviceOptions.Service.Name,
                Port = serviceOptions.Service.Port
            };

            consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));
            return services;
        }
    }
}
