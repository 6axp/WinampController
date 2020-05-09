
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
            return System.Text.Encoding.Unicode.GetString(title).Trim();
        }

        public PlayingStatus PlayingStatus
        {
            get
            {
                return (PlayingStatus)SendCommand(IpcCommands.IsPlaying);
            }
        }

        public void VolumeUp()
        {
            PushButton(Buttons.VolumeUp);
        }

        public void VolumeDown()
        {
            PushButton(Buttons.VolumeDown);
        }

        public void SetVolume(byte value)
        {
            SendCommand(IpcCommands.SetVolume, (int)value);
        }

        public byte GetVolume()
        {
            /* (requires Winamp 2.0+)
            ** SendMessage(hwnd_winamp,WM_WA_IPC,volume,IPC_SETVOLUME);
            ** IPC_SETVOLUME sets the volume of Winamp (between the range of 0 to 255).
            **
            ** If you pass 'volume' as -666 then the message will return the current volume.
            ** int curvol = SendMessage(hwnd_winamp,WM_WA_IPC,-666,IPC_SETVOLUME);
            */

            return (byte)SendCommand(IpcCommands.SetVolume, -666);
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
            return SendCommand(command, 0);
        }

        private IntPtr SendCommand(IpcCommands command, int value)
        {
            return NativeMethods.SendMessage(this.process.MainWindowHandle, WM_WA_IPC, value, (int)command);
        }
    }
}
