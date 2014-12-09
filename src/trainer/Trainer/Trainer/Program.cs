using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;



namespace Trainer{


    /// <summary>
    /// This is our artificially intelligent, autonomous Pokemon trainer.
    /// Make sure an instance of VisualBoyAdvance is running with these settings:
    /// 1. Options -> Emulator -> Show Speed -> None
    /// 2. Key Controls:
    ///     A: Z
    ///     B: X
    ///     L: Q
    ///     R: S
    ///     START: ENTER
    ///     SELECT: BACKSPACE
    ///     . : RAM DUMP
    /// 3. Make sure AITrainer.RAM_FILENAME is set the cgb_wram.bin file in the VBA game directory.
    /// </summary>
    /// 

    class Program
    {
        static void Main(string[] args)
        {
            //Set VisualBoyAdvance as the active window
            IntPtr handle = NativeMethods.FindWindow(null, "VisualBoyAdvance");
            WindowsAPI.SwitchWindow(handle);

            AITrainer trainer = new AITrainer();
            trainer.DumpRAM();

            

        }

        internal static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }
    }
  


}
