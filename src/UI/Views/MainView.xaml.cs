using Microsoft.Win32;

namespace RCTProgress.UI.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            d(ViewModel!.SelectFile.RegisterHandler(SelectFile));
            d(this.BindCommand(ViewModel, vm => vm.OpenFileCommand, v => v.OpenMenuItem));
            d(this.BindCommand(ViewModel, vm => vm.SaveFileCommand, v => v.SaveMenuItem));
            d(this.OneWayBind(ViewModel, vm => vm.File, v => v.File.ViewModel));
        });
    }

    private void SelectFile(InteractionContext<string, string> context)
    {
        var dialog = new OpenFileDialog
        {
            Title = context.Input,
            FileName = "CSS0",
            DefaultExt = ".DAT",
            Filter = "RollerCoaster Tycoon scenario data (CSS0.DAT)|CSS0.DAT"
        };

        context.SetOutput(dialog.ShowDialog() != true ? null : dialog.FileName);
    }
}