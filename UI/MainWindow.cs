using Terminal.Gui;
using Peeper.Services;

namespace Peeper.UI;

public class MainWindow : Window {
    public CpuWidget CpuWidget;
    public MemoryWidget MemoryWidget;
    public GpuWidget GpuWidget;

    public MainWindow() {
        Title = "Peeper System Monitor";
        X = 0;
        Y = 1; // Leave Room For a Menu Bar
        Width = Dim.Fill();
        Height = Dim.Fill();

        CpuWidget = new() {
            X = 1,
            Y = 2,
            Width = 40,
            Height = 4
        };

        GpuWidget = new() {
            X = 1,
            Y = CpuWidget.Frame.Bottom + 1,
            Width = 40,
            Height = 5
        };

        MemoryWidget = new() {
            X = 1,
            Y = GpuWidget.Frame.Bottom + 1,
            Width = 40,
            Height = 4
        };
        Add(CpuWidget, GpuWidget, MemoryWidget);
    }

    public override bool ProcessKey(KeyEvent keyEvent) {
        if (keyEvent.Key is Key.q || keyEvent.Key is Key.Q || keyEvent.Key is (Key.CtrlMask | Key.C)) {
            Application.RequestStop();
            return true;
        }
        else return base.ProcessKey(keyEvent);
    }
}