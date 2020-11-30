using DBLayer;
using GameEngine.Contracts;
using GameEngine.Utilities;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace WebApiService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IGameRepository, GameRepository>(new InjectionConstructor(new GameEntities()));
            
            container.RegisterType<IStateHelper, StateHelper>();
            container.RegisterType<IGameEngine, GameEngine.Contracts.GameEngine>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}