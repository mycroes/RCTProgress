namespace RCTProgress.UI.ViewModels;

public class ScenarioViewModel
{
    private readonly Scenario _scenario;

    public ScenarioViewModel(Scenario scenario)
    {
        _scenario = scenario;

        Name = scenario.Name;
        FileName = scenario.FileName;
        CompanyValue = scenario.CompanyValue;
        Winner = scenario.Winner;
        Available = scenario.Available;
    }

    public string? Name { get; set; }

    public string? FileName { get; set; }

    public int CompanyValue { get; set; }

    public string? Winner { get; set; }

    public bool Available { get; set; }
}