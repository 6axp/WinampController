
namespace baxp.Winamp
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public sealed class WinampController
    {
        private static readonly string ProcessName = "winamp";
        private static readonly int WM_USER = 1024;
        private static readonly int WM_WA_IPC = WM_USER;
        private static readonly int WM_COMMAND = 0x0111;

        private readonly Process process;

        private WinampController(Process process)
        {
            this.process = process ?? throw new ArgumentNullException();
        }

        private static WinampController TryCreateInstance(Process process)
        {
            return (process != null) ? new WinampController(process) : null;
        }

        public static WinampController GetExisting()
        {
            var process = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            return TryCreateInstance(process);
        }

        public static WinampController StartNew()
        {
            var process = Process.Start(ProcessName);
            return TryCreateInstance(process);
        }

        public string GetCurrentTrack()
        {
            var titleAddr = SendCommand(IpcCommands.GetPlayingTitleUnicode);
            var title = NativeMethods.ReadProcessMemory(process, titleAddr, 256);
            return System.Text.Encoding.Unicode.GetString(title);
        }

        public void NextTrack()
        {
            this.PushButton(Buttons.NextTrack);
        }

        public void PreviousTrack()
        {
            this.PushButton(Buttons.PrevTrack);
        }

        public void Play()
        {
            this.PushButton(Buttons.Play);
        }

        public void Pause()
        {
            this.PushButton(Buttons.Pause);
        }

        public void Stop()
        {
            this.PushButton(Buttons.Stop);
        }

        private void PushButton(Buttons button)
        {
            NativeMethods.SendMessage(this.process.MainWindowHandle, WM_COMMAND, (int)button, 0);
        }

        private IntPtr SendCommand(IpcCommands command)
        {
            return NativeMethods.SendMessage(this.process.MainWindowHandle, WM_WA_IPC, 0, (int)command);
        }
    }
}
