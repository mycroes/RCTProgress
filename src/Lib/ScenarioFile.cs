namespace RCTProgress.Lib;

public class ScenarioFile
{
    public ScenarioFile(List<Scenario> scenarios, byte numScenarios, byte[] megaParkHash, byte flag, ushort css1TimeRef, uint checksum, long scenarioFileSize)
    {
        Scenarios = scenarios;
        MegaParkHash = megaParkHash;
        Flag = flag;
        Css1TimeRef = css1TimeRef;
        Checksum = checksum;
        NumScenarios = numScenarios;
        ScenarioFileSize = scenarioFileSize;
    }

    public List<Scenario> Scenarios { get; }

    public byte NumScenarios { get; }

    public byte[] MegaParkHash { get; }

    public byte Flag { get; set; }

    public ushort Css1TimeRef { get; set; }

    public uint Checksum { get; set; }

    public long ScenarioFileSize { get; set; }
}
