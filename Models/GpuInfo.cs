namespace Peeper.Models;

public class GpuInfo() {
    public string Name { get; set; } = "";
    public long TotalMiB { get; set; } = 0;
    public long UsageMiB { get; set; } = 0;
}