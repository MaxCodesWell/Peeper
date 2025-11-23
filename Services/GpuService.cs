using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Peeper.Models;
using Peeper.Utils;
using Terminal.Gui.Graphs;

namespace Peeper.Services;


public class GpuService {

    public GpuInfo GetGpuInfo() {
        var vgaController = GetVGAControllerName()?.ToLower() ?? "";

        if (vgaController.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase)) {
            return GetNvidiaInfo();
        }
        else if (vgaController.ContainsAny(["AMD", "ATI", "Radeon", "Advanced Micro Devices"])) {
            return GetAmdInfo();
        }
        else {
            return new GpuInfo();
        }
    }

    //Check Users GPU Type
    private static string GetVGAControllerName() {

        var proc = new Process();
        proc.StartInfo.FileName = "lspci";
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();

        var lines = output.Split('\n')
                          .Where(l => l.Contains("VGA") || l.Contains("3D"))
                          .ToList();

        var name = "";
        if (lines.Count > 0) {
            var parts = lines[0].Split(":", StringSplitOptions.TrimEntries);
            if (parts.Length > 1) name = parts[2];
            else if(parts.Length > 0) name = parts[1];
            else name = lines[0];
        }
        return name;
    }

    private static GpuInfo GetNvidiaInfo() {
        string output;
        using (var proc = new Process()) {
            proc.StartInfo.FileName = "nvidia-smi";
            proc.StartInfo.Arguments = "--query-gpu=name,memory.total,memory.used,utilization.gpu --format=csv,noheader,nounits";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();

            output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
        }

        var parts = output.Split(',', StringSplitOptions.TrimEntries);
        GpuInfo gpuInfo = new();

        if (parts.Length > 0) gpuInfo.Name = parts[0];
        if (parts.Length > 1) gpuInfo.TotalMiB = long.Parse(parts[1]);
        if (parts.Length > 2) gpuInfo.UsageMiB = long.Parse(parts[2]);

        return gpuInfo;
    }

    private static GpuInfo GetAmdInfo() {
        string JSONoutput;
        using (var proc = new Process()) {
            proc.StartInfo.FileName = "rocm-smi";
            proc.StartInfo.Arguments = "--showuse --showmemuse --json";
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();

            JSONoutput = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
        }

        GpuInfo gpuInfo = new();

        //If No Info, Just Return Now
        if (string.IsNullOrEmpty(JSONoutput)) {
            gpuInfo.Name = "Unknown AMD GPU";
            return gpuInfo;
        }

        //Otherwise, Do some special case parsing because AMD SMI output is not uniform like Nvidia
        var root = JsonDocument.Parse(JSONoutput).RootElement;

        //Find the Card Name
        var firstGpu = root.EnumerateObject().First().Value;
        string? name = null;
        if (firstGpu.TryGetProperty("card_model", out var modelProp)) {
            name = modelProp.GetString();
        }
        else if (firstGpu.TryGetProperty("card_series", out var seriesProp)) {
            name = seriesProp.GetString();
        }
        else if (firstGpu.TryGetProperty("name", out var nameProp)) {
            name = nameProp.GetString();
        }

        //Get Memory Usage
        long used = 0;

        if (firstGpu.TryGetProperty("memory_used (MB)", out var usedProp) ||
            firstGpu.TryGetProperty("memory_used_mb", out usedProp) ||
            firstGpu.TryGetProperty("memory_used (bytes)", out usedProp) ||
            firstGpu.TryGetProperty("memory_used", out usedProp)) {

            // value could be string or number
            if (usedProp.ValueKind is JsonValueKind.String) {
                used = long.Parse(usedProp.GetString() ?? "0");
            }
            else if (usedProp.ValueKind is JsonValueKind.Number) {
                used = usedProp.GetInt64();
            }
        }

        //Get Total Memory
        long total = 0;
        if (firstGpu.TryGetProperty("memory_total (MB)", out var totalProp) ||
            firstGpu.TryGetProperty("memory_total_mb", out totalProp) ||
            firstGpu.TryGetProperty("memory_total (bytes)", out totalProp) ||
            firstGpu.TryGetProperty("memory_total", out totalProp)) {

            // again, value could be string or number
            if (totalProp.ValueKind == JsonValueKind.String) {
                total = long.Parse(totalProp.GetString() ?? "0");
            }
            else if (totalProp.ValueKind == JsonValueKind.Number) {
                total = totalProp.GetInt64();
            }
        }

        //Return Info
        gpuInfo.Name = name ?? "Unknown AMD GPU";
        if (total > 0) gpuInfo.TotalMiB = total;
        if (used > 0) gpuInfo.UsageMiB = used;

        return gpuInfo;
    }

}
