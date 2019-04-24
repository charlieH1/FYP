using System.Data.Entity;
using TonerManagement.Handlers;
using TonerManagement.Handlers.Interface;
using TonerManagement.Repository;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets;
using TonerManagement.Toolsets.Interface;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TonerManagement.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TonerManagement.App_Start.NinjectWebCommon), "Stop")]

namespace TonerManagement.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using TonerManagement.Models;

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
            //db bindings
            TonerManagementEntities db = new TonerManagementEntities();
            kernel.Bind<DbContext>().ToConstant(db);
            //repo bindings
            kernel.Bind<IUserRepo>().To<UserRepo>();
            kernel.Bind<ITonerPrinterRepo>().To<TonerPrinterRepo>();
            kernel.Bind<IPrinterRepo>().To<PrinterRepo>();
            kernel.Bind<ICustomerRepo>().To<CustomerRepo>();
            kernel.Bind<IStockLocationRepo>().To<StockLocationRepo>();
            kernel.Bind<IStockLocationTonerRepo>().To<StockLocationTonerRepo>();
            kernel.Bind<ITonerRepo>().To<TonerRepo>();
            //handler bindings
            kernel.Bind<IRegistrationHandler>().To<RegistrationHandler>();
            kernel.Bind<ILoginHandler>().To<LoginHandler>();
            kernel.Bind<ICustomerHandler>().To<CustomerHandler>();
            kernel.Bind<IUserHandler>().To<UserHandler>();
            kernel.Bind<IPrinterTonerHandler>().To<PrinterTonerHandler>();
            kernel.Bind<IStockLocationHandler>().To<StockLocationHandler>();
            kernel.Bind<IDevicesHandler>().To<DevicesHandler>();
            kernel.Bind<IStockLocationTonerHandler>().To<StockLocationTonerHandler>();
            //toolset bindings
            kernel.Bind<ICoverageToolset>().To<CoverageToolset>();
        }        
    }
}