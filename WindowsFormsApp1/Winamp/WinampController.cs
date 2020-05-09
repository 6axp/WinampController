
namespace baxp.Winamp
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class WinampController
    {
        private static readonly string ProcessName = "winamp";
        private static readonly int WM_USER = 1024;
        private static readonly int WM_WA_IPC = WM_USER;
        private static readonly int WM_COMMAND = 0x0111;

        public static Process GetWinampProcess()
        {
            return Process.GetProcessesByName(ProcessName).FirstOrDefault();
        }

        public static string GetCurrentTrack()
        {
            var process = GetWinampProcess();
            if (process != null)
            {
                var titleAddr = SendCommand(IpcCommands.GetPlayingTitleUnicode);
                var title = NativeMethods.ReadProcessMemory(process, titleAddr, 256);
                return System.Text.Encoding.Unicode.GetString(title);
            }

            return null;
        }

        public static bool IsAlive()
        {
            return GetWinampProcess() != null;
        }

        public static Process StartWinamp()
        {
            return Process.Start(ProcessName);
        }

        public static void NextTrack()
        {
            PushButton(Buttons.NextTrack);
        }

        public static void PreviousTrack()
        {
            PushButton(Buttons.PrevTrack);
        }

        public static void Play()
        {
            PushButton(Buttons.Play);
        }

        public static void Pause()
        {
            PushButton(Buttons.Pause);
        }

        public static void Stop()
        {
            PushButton(Buttons.Stop);
        }

        private static void PushButton(Buttons button)
        {
            var process = GetWinampProcess();
            if (process != null)
            {
                NativeMethods.SendMessage(process.MainWindowHandle, WM_COMMAND, (int)button, 0);
            }
        }

        private static IntPtr SendCommand(IpcCommands command)
        {
            var process = GetWinampProcess();
            if (process != null)
            {
                return NativeMethods.SendMessage(process.MainWindowHandle, WM_WA_IPC, 0, (int)command);
            }

            return IntPtr.Zero;
        }
    }
}
