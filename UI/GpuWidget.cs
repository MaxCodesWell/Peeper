using Terminal.Gui;
using Peeper.Services;
using static Peeper.Utils.Shortcuts;

namespace Peeper.UI;

public class GpuWidget : FrameView {
    private readonly GpuService _gpu = new();
    private readonly Label _nameLabel = new("GPU: ...") { X = 1, Y = 0 };
    private readonly Label _usageLabel = new("GPU: ...") { X = 1, Y = 1 };

    private readonly ProgressBar _bar = new() {
        X = 1,
        Y = 3,
        Width = Dim.Fill(1),
        Height = 1
    };

    public GpuWidget() : base("GPU") {

        Add(_nameLabel, _usageLabel, _bar);

        // Update every second
        Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (_) => {
            var info = _gpu.GetGpuInfo();
            var percentUsed = (float)info.UsageMiB / info.TotalMiB * 100;

            _nameLabel.Text = $"{info.Name}";
            _usageLabel.Text = $"Usage: {percentUsed:0.0}% : {info.UsageMiB}/{info.TotalMiB} MiB";
            _bar.Fraction = percentUsed / 100f;

            return true;
        });
    }
}

