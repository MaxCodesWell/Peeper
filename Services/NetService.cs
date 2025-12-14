using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Peeper.Models;

namespace Peeper.Services;

public class NetService {
    private class NetStats {
        public long RxBytes;
        public long TxBytes;
    }

    public enum NetType {
        Unknown,
        LAN,
        WIFI
    }

    private string? _iface;
    private NetStats? _lastStats;
    private DateTime _lastSampleTime;

    public NetInfo GetNetInfo() {

        _iface ??= DetectActiveInterface();

        if (_iface == null)
            return NetInfo.Empty("No active interface");

        var type = DetectNetType(_iface);

        var currentStats = ReadStats(_iface);
        var now = DateTime.UtcNow;

        double up = 0;
        double down = 0;

        if (_lastStats != null) {
            var seconds = (now - _lastSampleTime).TotalSeconds;
            if (seconds > 0) {
                down = (currentStats.RxBytes - _lastStats.RxBytes) / 1024.0 / seconds;
                up = (currentStats.TxBytes - _lastStats.TxBytes) / 1024.0 / seconds;
            }
        }

        _lastStats = currentStats;
        _lastSampleTime = now;

        return new NetInfo {
            Interface = (type == NetType.WIFI ? "Wireless" : "Wired") + " (" +  _iface + ")",
            NetType = type.ToString(),
            NetName = type == NetType.WIFI ? GetWifiSsid() : "Wired",
            SignalStrength = type == NetType.WIFI ? GetWifiSignal(_iface) : -1,
            UpKiB = up,
            DownKiB = down
        };
    }

    // ---------- detection ----------

    private string? DetectActiveInterface() {
        var ifaces = Directory.GetDirectories("/sys/class/net")
            .Select(Path.GetFileName)
            .Where(n => n != "lo")
            .ToList();

        // Prefer Wi-Fi interfaces (wl*, wlx*)
        var wifi = ifaces.FirstOrDefault(n => n.StartsWith("wl"));
        if (wifi != null)
            return wifi;

        // Otherwise fall back to first non-loopback (likely Ethernet)
        return ifaces.FirstOrDefault();
    }



    private NetType DetectNetType(string iface) {
        return iface.StartsWith("wl") ? NetType.WIFI : NetType.LAN;
    }

    // ---------- stats ----------

    private NetStats ReadStats(string iface) {
        return new NetStats {
            RxBytes = long.Parse(File.ReadAllText($"/sys/class/net/{iface}/statistics/rx_bytes")),
            TxBytes = long.Parse(File.ReadAllText($"/sys/class/net/{iface}/statistics/tx_bytes"))
        };
    }

    // ---------- Wi-Fi helpers ----------

    private string GetWifiSsid() {
        try {
            return RunCmd("iwgetid", "-r").Trim();
        }
        catch {
            return "Unknown";
        }
    }

    private int GetWifiSignal(string iface) {
        try {
            var output = RunCmd("iwconfig", iface);

            foreach (var line in output.Split('\n')) {
                if (line.Contains("Link Quality=")) {
                    var q = line.Split("Link Quality=")[1].Split(' ')[0];
                    var parts = q.Split('/');
                    return (int)(int.Parse(parts[0]) * 100.0 / int.Parse(parts[1]));
                }
            }
        }
        catch { }

        return -1;
    }

    private string RunCmd(string file, string args) {
        var p = new Process();
        p.StartInfo.FileName = file;
        p.StartInfo.Arguments = args;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.UseShellExecute = false;
        p.Start();
        var output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        return output;
    }
}
