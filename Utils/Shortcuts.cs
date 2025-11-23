using System;
namespace Peeper.Utils;

public static class Shortcuts {

    public static long KiBToMiB(long kib) => kib / 1024;
    public static double MiBToGiB(long mib) => mib / 1024.0;
    public static double KiBToGiB(long kib) => kib / 1024.0 / 1024.0;


    public static bool ContainsAny(this string str, params string[] substrings) {
        foreach (var sub in substrings) {
            if (str.Contains(sub)) return true;
        }
        return false;
    }


}
