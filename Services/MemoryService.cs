using System;
using System.IO;
using Peeper.Models;
namespace Peeper.Services;

public class MemoryService {

    public MemoryInfo GetMemoryInfo() {
        long totalKiB = 0;
        long availableKiB = 0;

        foreach (var line in File.ReadLines("/proc/meminfo")) {
            if (line.StartsWith("MemTotal")) { totalKiB = ParseLine(line); }
            if (line.StartsWith("MemAvailable")) { availableKiB = ParseLine(line); }
        }

        long usedKiB = totalKiB - availableKiB;
        return new MemoryInfo(totalKiB, usedKiB, availableKiB);
    }

    private static long ParseLine(string line) {
        return long.Parse(line.Split(':')[1].Trim().Split(' ')[0]); // kB
    }
}

