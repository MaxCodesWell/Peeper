using Terminal.Gui;
using Peeper.Services;
using Peeper.Widgets;

namespace Peeper.UI;

public class MainWindow : Window {
    public CpuWidget cpuWidget;
    public MemoryWidget memoryWidget;
    public GpuWidget gpuWidget;
    public NetWidget netWidget;

    public MainWindow() {
        Title = "Peeper System Monitor";
        X = 0;
        Y = 1; // Leave Room For a Menu Bar
        Width = Dim.Fill();
        Height = Dim.Fill();

        cpuWidget = new() {
            X = 1,
            Y = 2,
            Width = 40,
            Height = 6
        };

        gpuWidget = new() {
            X = 1,
            Y = cpuWidget.Frame.Bottom + 1,
            Width = 40,
            Height = 6
        };

        memoryWidget = new() {
            X = 1,
            Y = gpuWidget.Frame.Bottom + 1,
            Width = 40,
            Height = 5
        };

        netWidget = new() {
            X = 1,
            Y = memoryWidget.Frame.Bottom + 1,
            Width = 40,
            Height = 7
        };
        Add(cpuWidget, gpuWidget, memoryWidget, netWidget); 
    }

    public override bool ProcessKey(KeyEvent keyEvent) {
        if (keyEvent.Key is Key.q || keyEvent.Key is Key.Q || keyEvent.Key is (Key.CtrlMask | Key.C)) {
            Application.RequestStop();
            return true;
        }
        else return base.ProcessKey(keyEvent);
    }
}