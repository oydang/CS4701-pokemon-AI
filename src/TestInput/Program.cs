using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TestInput
{
    public class RunForm
    {
        Form1 form;

        public RunForm()
        {
            this.form = new Form1(); 
        }

        /// <summary>
        /// Thread for backend. Starts gui thread
        /// </summary>
        public void runback()
        {
            System.Threading.Thread formthread = new System.Threading.Thread(this.rungui);
            formthread.Start();
            if (formthread.IsAlive)
            {
                generateInput(form);
            }
        }

        /// <summary>
        /// Thread for GUI
        /// </summary>
        public void rungui()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(this.form);
        }

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        
        /// <summary>
        /// Generate keyboard output to send to app
        /// </summary>
        static void generateInput(Form1 form)
        {
            IntPtr calcHandle = FindWindow("CalcFrame", "Calculator");
            if (calcHandle == IntPtr.Zero)
            {
                form.UpdateText("No calculator App open");
            }
            else
            {
                SetForegroundWindow(calcHandle);
                string text = "12";
                SendKeys.SendWait(text);
                //form.UpdateText(text);
            }
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RunForm runapp = new RunForm();
            runapp.runback();            
        }
    }
}
