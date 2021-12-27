namespace RCTProgress.UI.ViewModels;

public class FileViewModel
{
    private readonly ScenarioFile _file;

    public FileViewModel(ScenarioFile file)
    {
        _file = file;

        Flag = file.Flag;
        Css1TimeRef = file.Css1TimeRef;
        Checksum = file.Checksum;
        ScenarioFileSize = file.ScenarioFileSize;

        Scenarios = file.Scenarios.Select(sc => new ScenarioViewModel(sc)).ToList();
    }

    public List<ScenarioViewModel> Scenarios { get; }

    public byte Flag { get; set; }
    public ushort Css1TimeRef { get; set; }
    public uint Checksum { get; set; }
    public long ScenarioFileSize { get; set; }
}
