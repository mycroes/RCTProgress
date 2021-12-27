namespace RCTProgress.UI.Views;

public partial class FileView
{
    public FileView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.Flag, v => v.Flag.Content, value => value.ToString("X2")));
            d(this.OneWayBind(ViewModel, vm => vm.Css1TimeRef, v => v.Css1TimeRef.Content, value => value.ToString("X4")));
            d(this.OneWayBind(ViewModel, vm => vm.Checksum, v => v.Checksum.Content, value => value.ToString("X8")));
            d(this.OneWayBind(ViewModel, vm => vm.ScenarioFileSize, v => v.ScenarioFileSize.Content));
            d(this.OneWayBind(ViewModel, vm => vm.MegaParkHash, v => v.MegaParkHash.Content, value => string.Join("-", value.Select(v => v.ToString("X2")))));
            d(this.OneWayBind(ViewModel, vm => vm.NumScenarios, v => v.NumScenarios.Content));
            d(this.OneWayBind(ViewModel, vm => vm.Scenarios, v => v.Scenarios.ItemsSource));
        });
    }
}