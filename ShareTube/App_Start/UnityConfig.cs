using System;
using ShareTube.Models;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ShareTube.Hubs;
using ShareTube.Data;
using ShareTube.Models.Tracking;
using ShareTube.Infrastructure;

namespace ShareTube.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IShareTubeClientContract, ShareTubeClientContract>();
            container.RegisterType<ICookieHelper, CookieHelper>();
            container.RegisterType<ITrackingService, TrackingService>();

			container.RegisterType<IShareTubeDataContext, ShareTubeDataContext>();
			container.RegisterType<IShareTubeService, ShareTubeService>();
			//container.RegisterType<IShareTubeRepository, ShareTubeMemoryRepository>();


			// NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
			// container.LoadConfiguration();

			// TODO: Register your types here
			// container.RegisterType<IProductRepository, ProductRepository>();
			//container.RegisterType<IShareTubeRoomRepository, ShareTubeRoomRepository>();
			//container.RegisterType<IUserStore<ApplicationUser>, UserManager<ApplicationUser>>();

			//container.RegisterType(typeof(UserManager<>),
			//new InjectionConstructor(typeof(IUserStore<>)));
			//container.RegisterType<Microsoft.AspNet.Identity.IUser>(new InjectionFactory(c => c.Resolve<Microsoft.AspNet.Identity.IUser>()));
			//container.RegisterType(typeof(IUserStore<>), typeof(UserStore<>));
			//container.RegisterType<IdentityUser, ApplicationUser>(new ContainerControlledLifetimeManager());
			//container.RegisterType<DbContext, ApplicationDbContext>(new ContainerControlledLifetimeManager());
		}
    }
}
