[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BulbaCourses.Podcasts.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BulbaCourses.Podcasts.Web.App_Start.NinjectWebCommon), "Stop")]

namespace BulbaCourses.Podcasts.Web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using BulbaCourses.Podcasts.Web.Infrastructure;
    using BulbaCourses.Podcasts.Logic.Infrastracture;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.WebApi;
    using Ninject.Web.Common.WebHost;
    using BulbaCourses.Podcasts.Logic.Infrastructure;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
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

                var ninjectResolver = new NinjectDependencyResolver(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;
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
            kernel.Load<LogicLoadModule>();
            kernel.Load<MapperLoadModule>();
        }        
    }
}