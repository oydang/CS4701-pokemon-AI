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

    /// <summary>
    /// The trainer controls all keypresses
    /// </summary>
    public class AITrainer
    { 

        public AITrainer()
        {
        }

        /// <summary>
        /// Dumps the RAM in the current VisualBoyAdvance game into cgb_ram.bin by pressing the '.' key
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
            Thread.Sleep(1000); //Wait for cgb_wram.bin file to be updated

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
