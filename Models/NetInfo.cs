namespace Peeper.Models;

public class NetInfo() {
    public string? NetName { get; set; } = "";
    public string NetType { get; set; } = "";
    public float SignalStrength { get; set; } = 0f;
    public string? Interface { get; set; } = null;    

    public double UpKiB { get; set; } = 0;
    public double DownKiB { get; set; } = 0;

    public static NetInfo Empty(string errorMessage) {
        return new NetInfo {
            Interface = null,
            NetType = errorMessage,
            NetName = null,
            SignalStrength = -1,
            UpKiB = 0,
            DownKiB = 0
        };
    }
}
