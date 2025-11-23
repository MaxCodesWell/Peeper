using System;
using System.Threading;
using Peeper.Services;
using Peeper.UI;
using Terminal.Gui;

class Program {
    
    static void Main() {
        Application.Init();

        var win = new MainWindow();
        Application.Top.Add(win);
        
        Application.Run();
        Application.Shutdown();
    }
}
