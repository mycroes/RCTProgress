using Microsoft.Win32;
using ReactiveUI;

namespace RCTProgress.UI.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        this.WhenActivated(d => {
            d(ViewModel.SelectFile.RegisterHandler(SelectFile));
            d(this.BindCommand(ViewModel, vm => vm.OpenFileCommand, v => v.OpenMenuItem));
        });
    }

    private void SelectFile(InteractionContext<string, string> context)
    {
        var dialog = new OpenFileDialog { Title = context.Input };

        context.SetOutput(dialog.ShowDialog() != true ? null : dialog.FileName);
    }
}