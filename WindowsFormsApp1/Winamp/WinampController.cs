
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

        public static int UnknownPosition = -1;

        private WinampController(Process process)
        {
            this.process = process ?? throw new ArgumentNullException();
        }

        private static WinampController TryCreateInstance(Process process)
        {
            return (process != null && !process.HasExited) ? new WinampController(process) : null;
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

        public bool IsAlive
        {
            get
            {
                return !this.process.HasExited;
            }
        }

        public byte Volume
        {
            get
            {
                return this.GetVolume();
            }
            set
            {
                this.SetVolume(value);
            }
        }

        public int CurrentTrackPosition
        {
            get
            {
                return this.GetCurrentTrackPosition();
            }
            set
            {
                this.Play(value);
            }
        }

        public int NextTrackPosition
        {
            get
            {
                return this.GetNextTrackPosition();
            }
            /*
            set
            {
                this.SetNextTrackPosition(value);
            }
            */
        }

        public string CurrentTrack
        {
            get
            {
                return this.GetCurrentTrack();
            }
        }

        public string NextTrack
        {
            get
            {
                return this.GetTrackTitle(this.GetNextTrackPosition());
            }
        }

        /*
        // Not stable
        public void Restart()
        {
            SendCommand(IpcCommands.RestartWinamp);
        }
        */

        public string GetCurrentTrack()
        {
            var titleAddr = SendCommand(IpcCommands.GetPlayingTitleUnicode);
            var title = NativeMethods.ReadProcessMemory(process, titleAddr, 256);
            return System.Text.Encoding.Unicode.GetString(title).Trim();

            // or
            // return GetTrackTitle(GetCurrentTrack());
        }

        public string GetTrackTitle(int position)
        {
            var titleAddr = SendCommand(IpcCommands.GetPlaylistTitleUnicode, position);
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

        public int GetCurrentTrackPosition()
        {
            return (int)SendCommand(IpcCommands.GetListPosition);
        }

        public void SetCurrentTrackPosition(int position)
        {
            SendCommand(IpcCommands.SetListPosition, position);
        }

        public int GetNextTrackPosition()
        {
            return (int)SendCommand(IpcCommands.GetNextListPosition);
        }

        /*
        // not working
        public void SetNextTrackPosition(int position)
        {
             SendCommand(IpcCommands.GetNextListPosition, position);
        }
        */

        public void PlayNextTrack()
        {
            this.PushButton(Buttons.NextTrack);
        }

        public void PlayPreviousTrack()
        {
            this.PushButton(Buttons.PrevTrack);
        }

        public void Play()
        {
            this.PushButton(Buttons.Play);
        }

        public void Play(int postion)
        {
            this.SetCurrentTrackPosition(postion);
            this.Play();
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
