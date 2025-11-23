using System;
using System.Threading;
using Peeper.Services;
using Peeper.UI;
using Terminal.Gui;

class Program {
    static void Main() {
        Application.Init();
        var cpu = new CpuService();
        var mem = new MemoryService();

        var top = Application.Top;
        var win = new MainWindow();
        top.Add(win);

        Application.Run();
    }
}
