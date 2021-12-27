namespace RCTProgress.UI.ViewModels;

public class FileViewModel
{
    private readonly ScenarioFile _file;

    public FileViewModel(string fileName, ScenarioFile file)
    {
        _file = file;

        FileName = fileName;

        Flag = file.Flag;
        Css1TimeRef = file.Css1TimeRef;
        Checksum = file.Checksum;
        ScenarioFileSize = file.ScenarioFileSize;

        MegaParkHash = file.MegaParkHash;

        NumScenarios = file.NumScenarios;

        Scenarios = file.Scenarios.Select(sc => new ScenarioViewModel(sc)).ToList();
    }

    public string FileName { get; }

    public List<ScenarioViewModel> Scenarios { get; }

    public byte NumScenarios { get; }

    public byte[] MegaParkHash { get; }

    public byte Flag { get; set; }
    public ushort Css1TimeRef { get; set; }
    public uint Checksum { get; set; }
    public long ScenarioFileSize { get; set; }
}
