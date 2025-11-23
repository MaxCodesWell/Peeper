using Terminal.Gui;
using Peeper.Services;
using static Peeper.Utils.Shortcuts;

namespace Peeper.UI;

public class MemoryWidget : FrameView {
    private readonly MemoryService _mem = new();
    private readonly Label _memLabel = new("Memory: ...") { X = 1, Y = 0 };
    private readonly ProgressBar _bar = new() {
        X = 1,
        Y = 1,
        Width = Dim.Fill(1),
        Height = 1
    };

    public MemoryWidget() : base("Memory") {

        Add(_memLabel, _bar);

        // Update every second
        Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (_) => {
            var info = _mem.GetMemoryInfo();

            var percentUsed = (float)info.UsageKiB / info.TotalKiB * 100;
            var UsedGiB = KiBToGiB(info.UsageKiB);
            var TotalGiB = KiBToGiB(info.TotalKiB);

            _memLabel.Text = $"Usage: {percentUsed:0.0}% : {UsedGiB:0.000}/{TotalGiB:0.000} GiB";
            _bar.Fraction = percentUsed / 100f;

            return true;
        });
    }
}

