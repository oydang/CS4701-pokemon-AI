using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace Trainer
{

    public class AITrainer
    { 

        const string RAM_FILENAME = "C:\\Users\\Olivia\\Dropbox\\College\\Junior\\CS4700\\pokeBot\\Tracer-VisualboyAdvance1.7.1\\Tracer-VisualboyAdvance1.7.1\\tracer\\Pokemon Red\\cgb_wram.bin";
        private BinaryReader bReader;
        private Maps utilMaps;
        private Random generator;


        public AITrainer()
        {
            utilMaps = new Maps();
            generator = new Random();

        }

        /// <summary>
        /// Dumps the RAM in the current VisualBoyAdvance game into cgb_ram.bin
        /// </summary>
        public void DumpRAM()
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.wVk = 0;
            inputs[0].ki.dwFlags = 0x0008;
            inputs[0].ki.wScan = 0x34;

            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not press '.' to dump RAM");
            }

            Thread.Sleep(34);

            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP | WindowsAPI.KEYEVENTF_SCANCODE;
            intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
  
        }

        public string GetStat(Stats stat){
            string charName = null;
            this.bReader = new BinaryReader(File.Open(RAM_FILENAME, FileMode.Open));
            bReader.BaseStream.Position = 0x1158;
            for (int i = 0x1158; i <= 0x115E; i++)
            {
                charName += bReader.ReadByte().ToString("X2");
            }
            Debug.WriteLine(charName);
            bReader.Close();
            return charName;
        }


        /// <summary>
        /// Presses a key using SendInput
        /// </summary>
        /// <param name="ch"></param>
        ///
        public static void PressKey(char ch)
        {
            byte vk = WindowsAPI.VkKeyScan(ch);
            ushort scanCode = (ushort)WindowsAPI.MapVirtualKey(vk, 0);
            keyDown(scanCode);
            Thread.Sleep(340);
            keyUp(scanCode);
            Thread.Sleep(34);
        }

        private static void keyDown(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = 0;
            inputs[0].ki.wScan = (ushort)(scanCode & 0xff);

            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        private static void keyUp(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.wScan = scanCode;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP;
            uint intReturn = WindowsAPI.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

    }


}
