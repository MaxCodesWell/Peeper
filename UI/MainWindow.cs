using Terminal.Gui;
using Peeper.Services;

namespace Peeper.UI;

public class MainWindow : Window {
    public CpuWidget CpuWidget;
    public MemoryWidget MemoryWidget;

    public MainWindow() {
        Title = "Peeper System Monitor";
        X = 0;
        Y = 1; // Leave Room For Menu Bar
        Width = Dim.Fill();
        Height = Dim.Fill();

        CpuWidget = new() {
            X = 1,
            Y = 2,
            Width = 40,
            Height = 4
        };

        MemoryWidget = new() {
            X = 1,
            Y = CpuWidget.Frame.Bottom + 2,
            Width = 40,
            Height = 4
        };
        Add(CpuWidget, MemoryWidget);
    }
}