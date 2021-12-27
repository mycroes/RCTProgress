using System.Reactive.Linq;

namespace RCTProgress.UI.ViewModels;

public class MainViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly ReactiveCommand<Unit, string> _openFileCommand;

    private readonly ReactiveCommand<Unit, Unit> _saveFileCommand;

    public MainViewModel(IScreen screen)
    {
        HostScreen = screen;

        SelectFile = new Interaction<string, string>();

        _openFileCommand = ReactiveCommand.CreateFromObservable(() => SelectFile.Handle("Select your CSS0.DAT"));
        var openImpl = ReactiveCommand.CreateFromTask<string>(OpenFile);
        _openFileCommand.Subscribe(s => openImpl.Execute(s));

        _saveFileCommand = ReactiveCommand.CreateFromTask(
            () => SaveFile(File!.FileName), this.WhenAnyValue(x => x.File).Select(f => f is {}));
    }

    private FileViewModel? _file;
    public FileViewModel? File
    {
        get => _file;
        set => this.RaiseAndSetIfChanged(ref _file, value);
    }

    public IScreen HostScreen { get; }

    public ICommand OpenFileCommand => _openFileCommand;

    public ICommand SaveFileCommand => _saveFileCommand;

    public Interaction<string, string> SelectFile { get; }

    public string UrlPathSegment => string.Empty;

    private async Task OpenFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return;

        var reader = new ScenarioFileReader();
        var file = await reader.ReadAsync(fileName);

        File = new FileViewModel(fileName, file);
    }

    private async Task SaveFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return;
        if (File == null) return;

        var writer = new ScenarioFileWriter();
        
        await writer.WriteAsync(fileName, MapToFile(File));
    }

    private ScenarioFile MapToFile(FileViewModel viewModel)
    {
        return new ScenarioFile(viewModel.Scenarios.Select(MapToFile).ToList(), viewModel.NumScenarios, viewModel.MegaParkHash,  viewModel.Flag,
            viewModel.Css1TimeRef, viewModel.Checksum, viewModel.ScenarioFileSize);
    }

    private Scenario MapToFile(ScenarioViewModel viewModel)
    {
        return new Scenario
        {
            Name = viewModel.Name,
            FileName = viewModel.FileName,
            Winner = viewModel.Winner,
            CompanyValue = viewModel.CompanyValue,
            Available = viewModel.Available
        };
    }
}