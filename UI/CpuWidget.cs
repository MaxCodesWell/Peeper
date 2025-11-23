using Terminal.Gui;
using Peeper.Services;

namespace Peeper.UI;

public class CpuWidget : FrameView {
    private readonly CpuService _cpu;
    private readonly Label _cpuLabel;
    private readonly ProgressBar _bar;

    public CpuWidget() : base("CPU") {
        _cpu = new CpuService();
        _cpuLabel = new Label("CPU: ...") { X = 1, Y = 0 };

        _bar = new ProgressBar() {
            X = 1,
            Y = 1,
            Width = Dim.Fill(1),
            Height = 1
        };
        Add(_cpuLabel, _bar);

        // Update every second
        Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (_) => {
            float usage = _cpu.GetCpuInfo().TotalUsage;
            _cpuLabel.Text = $"Usage: {usage:0.0}%";
            _bar.Fraction = usage / 100f;

            return true;
        });
    }
}
