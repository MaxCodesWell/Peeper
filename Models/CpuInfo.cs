namespace Peeper.Models;

public class CPUInfo(string name, float totalUsage) {
    public float TotalUsage { get; set; } = totalUsage;
    public string Name { get; set; } = name;
}