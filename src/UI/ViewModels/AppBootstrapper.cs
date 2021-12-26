using RCTProgress.UI.Views;
using ReactiveUI;
using Splat;

namespace RCTProgress.UI.ViewModels;

public class AppBootstrapper : ReactiveObject, IScreen
{
    public AppBootstrapper(IReadonlyDependencyResolver? resolver = null, IMutableDependencyResolver? mutableResolver = null, RoutingState? routingState = null)
    {
        Router = routingState ?? new();
        resolver ??= Locator.Current;
        mutableResolver ??= Locator.CurrentMutable;

        RegisterParts(resolver, mutableResolver);

        Router.Navigate.Execute(resolver.GetService<MainViewModel>()).Subscribe();
    }

    /// <inheritdoc />
    public RoutingState Router { get; }

    private void RegisterParts(IReadonlyDependencyResolver resolver, IMutableDependencyResolver mutableResolver)
    {
        mutableResolver.InitializeSplat();
        mutableResolver.InitializeReactiveUI();

        mutableResolver.RegisterConstant<IScreen>(this);
        mutableResolver.Register<MainViewModel>(() => new MainViewModel(resolver.GetService<IScreen>()));

        mutableResolver.Register<IViewFor<MainViewModel>>(() => new MainView());
    }
}
