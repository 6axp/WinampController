
namespace baxp.Winamp
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal class NativeMethods
    {
        internal static byte[] ReadProcessMemory(Process process, IntPtr address, int size)
        {
            var result = new byte[0];
            
            var handle = OpenProcess(ProcessAccessFlags.VirtualMemoryRead, false, process.Id);

            if (handle != null)
            {
                var bytes = new byte[size];
                if (ReadProcessMemory(handle, address, bytes, size, out int outLen))
                {
                    result = bytes;
                }

                CloseHandle(handle);
            }

            return result;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [System.Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
    }
}
