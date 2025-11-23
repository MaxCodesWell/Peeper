namespace Peeper.Models;

public class CPUInfo(float totalUsage) {
    public float TotalUsage { get; set; } = totalUsage;
}