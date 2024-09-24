using AutoMapper;
using BusinessLayer;
using DataAccessLayer.Database;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Repositories;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebApi.App_Start.NinjectWebCommon), "Stop")]

namespace WebApi.App_Start
{
    using BusinessLayer.Model.Interfaces;
    using BusinessLayer.Services;
    using DataAccessLayer;
    using Microsoft.Extensions.Logging;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.WebApi.DependencyResolver;
    using System;
    using System.Web;
    using System.Web.Http;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            kernel.Bind<ILoggerFactory>().ToConstant(loggerFactory).InSingletonScope();

            kernel.Bind(typeof(ILogger<>)).ToMethod(context =>
            {
                var factory = context.Kernel.Get<ILoggerFactory>();
                var genericType = context.Request.Service.GetGenericArguments()[0];
                var loggerType = typeof(Logger<>).MakeGenericType(genericType);
                return Activator.CreateInstance(loggerType, factory);
            }).InSingletonScope();

            kernel.Bind<IMapper>().ToMethod(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<RepositoryProfile>();
                    cfg.AddProfile<BusinessProfile>();
                    cfg.AddProfile<AppServicesProfile>();
                    cfg.ConstructServicesUsing(t => kernel.Get(t));
                });
                return config.CreateMapper();
            }).InSingletonScope();

            kernel.Bind<ICompanyService>().To<CompanyService>();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            kernel.Bind<IEmployeeService>().To<EmployeeService>();
            kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
            kernel.Bind(typeof(IDbWrapper<>)).To(typeof(InMemoryDatabase<>));
        }
    }
}