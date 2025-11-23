namespace Peeper.Models;

public class MemoryInfo(long total, long used, long free) {
    public long TotalKiB { get; set; } = total;
    public long UsageKiB { get; set; } = used;
    public long FreeKiB { get; set; } = free;
}