namespace RCTProgress.Lib;

public class ScenarioFile
{
    public ScenarioFile(List<Scenario> scenarios, byte flag, ushort css1TimeRef, uint checksum, long scenarioFileSize)
    {
        Scenarios = scenarios;
        Flag = flag;
        Css1TimeRef = css1TimeRef;
        Checksum = checksum;
        ScenarioFileSize = scenarioFileSize;
    }

    public List<Scenario> Scenarios { get; }

    public byte Flag { get; set; }

    public ushort Css1TimeRef { get; set; }

    public uint Checksum { get; set; }

    public long ScenarioFileSize { get; set; }
}
