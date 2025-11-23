using System;
using System.IO;
using Peeper.Models;
namespace Peeper.Services;

public class CpuService {
    private long _prevIdle = 0;
    private long _prevTotal = 0;

    public CPUInfo GetCpuInfo() {
        string[] parts = File.ReadAllText("/proc/stat")
            .Split('\n')[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        long user = long.Parse(parts[1]);
        long nice = long.Parse(parts[2]);
        long system = long.Parse(parts[3]);
        long idle = long.Parse(parts[4]);
        long iowait = long.Parse(parts[5]);
        long irq = long.Parse(parts[6]);
        long softirq = long.Parse(parts[7]);

        long idleAll = idle + iowait;
        long nonIdle = user + nice + system + irq + softirq;
        long total = idleAll + nonIdle;

        long totalDelta = total - _prevTotal;
        long idleDelta = idleAll - _prevIdle;

        _prevTotal = total;
        _prevIdle = idleAll;

        if (totalDelta is 0) return new CPUInfo(0f);
        else return new CPUInfo((1.0f - ((float)idleDelta / totalDelta)) * 100f);
    }
}