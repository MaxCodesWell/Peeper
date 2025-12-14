using Terminal.Gui;
using Peeper.Services;

namespace Peeper.Widgets;

public class NetWidget : FrameView {
    private readonly NetService _net = new();

    private readonly Label _ifaceLabel = new("Interface: ?") { X = 1, Y = 0 };
    private readonly Label _nameLabel = new("Name: ?") { X = 1, Y = 1 };
    private readonly Label _speedLabel = new("↓ 0 KiB/s  ↑ 0 KiB/s") { X = 1, Y = 3};
    private readonly ProgressBar _signalBar = new() {
        X = 1,
        Y = 4,
        Width = Dim.Fill(2),
        Fraction = 0f,
        Height = 1 //7?
    };

    public NetWidget() : base(" Network ") {

        Add(_ifaceLabel, _nameLabel, _speedLabel, _signalBar);
        
        // Update every second
        Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (_) => { Refresh(); return true; });
    }

    public void Refresh() {
        var info = _net.GetNetInfo();

        _ifaceLabel.Text = $"Interface: {info.Interface}";
        _nameLabel.Text = $"Name: {info.NetName}";
        _speedLabel.Text =
            $"Down:  {info.DownKiB:0.0} KiB/s  Up:  {info.UpKiB:0.0} KiB/s";

        if (info.SignalStrength >= 0)
            _signalBar.Fraction = info.SignalStrength / 100f;
        else
            _signalBar.Fraction = 0;
    }
}
