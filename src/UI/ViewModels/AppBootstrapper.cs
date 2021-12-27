using RCTProgress.UI.Views;
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

        MainViewModel = resolver.GetService<MainViewModel>();
        Router.Navigate.Execute(MainViewModel).Subscribe();
    }

    public MainViewModel MainViewModel { get; }

    /// <inheritdoc />
    public RoutingState Router { get; }

    private void RegisterParts(IReadonlyDependencyResolver resolver, IMutableDependencyResolver mutableResolver)
    {
        mutableResolver.InitializeSplat();
        mutableResolver.InitializeReactiveUI();

        mutableResolver.RegisterConstant<IScreen>(this);
        mutableResolver.Register<MainViewModel>(() => new MainViewModel(resolver.GetService<IScreen>()));

        mutableResolver.Register<IViewFor<MainViewModel>>(() => new MainView());
        mutableResolver.Register<IViewFor<FileViewModel>>(() => new FileView());
    }
}
