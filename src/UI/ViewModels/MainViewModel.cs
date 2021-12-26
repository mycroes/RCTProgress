using System.Windows.Input;
using ReactiveUI;

namespace RCTProgress.UI.ViewModels;

public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    public MainViewModel(IScreen screen)
    {
        HostScreen = screen;

        SelectFile = new Interaction<string, string>();

        OpenFileCommand = ReactiveCommand.CreateFromObservable(() => SelectFile.Handle("Select your CSS0.DAT"));
    }

    public IScreen HostScreen { get; }

    public ICommand OpenFileCommand { get; }
    public Interaction<string, string> SelectFile { get; }

    public string UrlPathSegment => string.Empty;

    private void OpenFile()
    {
        
    }
}