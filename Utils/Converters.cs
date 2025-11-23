using System;
namespace Peeper.Utils;

public static class Converters {
    
    public static long KiBToMiB(long kib) => kib / 1024;
    public static double MiBToGiB(long mib) => mib / 1024.0;
    public static double KiBToGiB(long kib) => kib / 1024.0 / 1024.0;
}
