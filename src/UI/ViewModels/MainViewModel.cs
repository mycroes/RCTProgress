namespace RCTProgress.UI.ViewModels;

public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly ReactiveCommand<Unit, string> _openFileCommand;

    public MainViewModel(IScreen screen)
    {
        HostScreen = screen;

        SelectFile = new Interaction<string, string>();

        _openFileCommand = ReactiveCommand.CreateFromObservable(() => SelectFile.Handle("Select your CSS0.DAT"));
        var openImpl = ReactiveCommand.CreateFromTask<string>(OpenFile);
        _openFileCommand.Subscribe(s => openImpl.Execute(s));
    }

    private FileViewModel? _file;
    public FileViewModel? File
    {
        get => _file;
        set => this.RaiseAndSetIfChanged(ref _file, value);
    }

    public IScreen HostScreen { get; }

    public ICommand OpenFileCommand => _openFileCommand;

    public Interaction<string, string> SelectFile { get; }

    public string UrlPathSegment => string.Empty;

    private async Task OpenFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return;

        var reader = new ScenarioFileReader();
        var file = await reader.ReadAsync(fileName);

        File = new FileViewModel(file);
    }
}