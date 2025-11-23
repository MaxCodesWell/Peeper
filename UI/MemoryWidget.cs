using Terminal.Gui;
using Peeper.Services;
using static Peeper.Utils.Converters;

namespace Peeper.UI;

public class MemoryWidget : FrameView {
    private readonly MemoryService _mem;
    private readonly Label _memLabel;
    private readonly ProgressBar _bar;

    public MemoryWidget() : base("Memory") {
        _mem = new MemoryService();
        _memLabel = new Label("Memory: ...") { X = 1, Y = 0 };

        _bar = new ProgressBar() {
            X = 1,
            Y = 1,
            Width = Dim.Fill(1),
            Height = 1
        };
        Add(_memLabel, _bar);

        // Update every second
        Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (_) => {
            var info = _mem.GetMemoryInfo();
           
            var percentUsed = (float)info.UsedKiB / info.TotalKiB * 100;
            var UsedGiB = KiBToGiB(info.UsedKiB);
            var TotalGiB = KiBToGiB(info.TotalKiB);

            _memLabel.Text = $"Usage: {UsedGiB:0.000}/{TotalGiB:0.000} GiB : {percentUsed:0.0}%";
            _bar.Fraction = percentUsed / 100f;

            return true;
        });
    }
}

