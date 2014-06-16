using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//ORIGINALLY CREATED BY Pingo, Edited by Fleep 
namespace ProcessMemoryReaderLib
{
    class ProcessMemoryReaderApi
    {
        public const uint PROCESS_VM_READ = (0x0010);
        public const uint PROCESS_VM_WRITE = (0x0020);
        public const uint PROCESS_VM_OPERATION = (0x0008);
        public const uint PAGE_READWRITE = 0x0004;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_ACTIVATE = 0x6;
        public const int WM_HOTKEY = 0x0312;
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, UInt32 dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UInt32 dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern int GetKeyState(int vKey);

    }
    public class ProcessMemoryReader
    {
        public ProcessMemoryReader()
        {
        }
        public Process ReadProcess
        {
            get
            {
                return m_ReadProcess;
            }
            set
            {
                m_ReadProcess = value;
            }
        }
        private Process m_ReadProcess = null;
        private IntPtr m_hProcess = IntPtr.Zero;
        public void OpenProcess()
        {
            m_hProcess = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
        }
        public void CloseHandle()
        {
            int iRetValue;
            iRetValue = ProcessMemoryReaderApi.CloseHandle(m_hProcess);
            if (iRetValue == 0)
                throw new Exception("CloseHandle failed");
        }



        #region ReadMem
        public int ReadMem(int MemoryAddress, uint bytesToRead, out byte[] buffer)
        {
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            if (procHandle == IntPtr.Zero)
            {
                buffer = new byte[0];
                return 0;
            }

            buffer = new byte[bytesToRead];
            IntPtr ptrBytesReaded;
            ProcessMemoryReaderApi.ReadProcessMemory(procHandle, (IntPtr)MemoryAddress, buffer, bytesToRead, out ptrBytesReaded);
            ProcessMemoryReaderApi.CloseHandle(procHandle);
            return ptrBytesReaded.ToInt32();
        }

        //We use this to 
        public int ReadMultiLevelPointer(int MemoryAddress, uint bytesToRead, Int32[] offsetList)
        {
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            IntPtr pointer = (IntPtr)0x0;
            //IF THE PROCESS isnt available we return nothing
            if (procHandle == IntPtr.Zero)
            {
                return 0;
            }

            byte[] btBuffer = new byte[bytesToRead];
            IntPtr lpOutStorage = IntPtr.Zero;

            int pointerAddy = MemoryAddress;
            //int pointerTemp = 0;
            for (int i = 0; i < (offsetList.Length); i++)
            {
                if (i == 0)
                {
                    ProcessMemoryReaderApi.ReadProcessMemory(
                        procHandle,
                        (IntPtr)(pointerAddy),
                        btBuffer,
                        (uint)btBuffer.Length,
                        out lpOutStorage);
                }
                pointerAddy = (BitConverter.ToInt32(btBuffer, 0) + offsetList[i]);
                //string pointerAddyHEX = pointerAddy.ToString("X");

                ProcessMemoryReaderApi.ReadProcessMemory(
                    procHandle,
                    (IntPtr)(pointerAddy),
                    btBuffer,
                    (uint)btBuffer.Length,
                    out lpOutStorage);
            }
            return pointerAddy; 
        }

        public byte ReadByte(int MemoryAddress)
        {
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 1, out buffer);
            if (read == 0)
                return new byte();
            else
                return buffer[0];
        }
        public int ReadInt(int MemoryAddress)
        {
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 4, out buffer);
            if (read == 0)
                return 0;
            else
                return BitConverter.ToInt32(buffer, 0);
        }
        public uint ReadUInt(int MemoryAddress)
        {
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 4, out buffer);
            if (read == 0)
                return 0;
            else
                return BitConverter.ToUInt32(buffer, 0);
        }

		public string ReadString(int MemoryAddress)
		{
			byte[] buffer;
			int length = 0;

			for (int i = 0; ReadByte(MemoryAddress + i) != 0; i++) length = i + 1; // We want to find the null-terminator of the string to determine length
			
			int read = ReadMem(MemoryAddress, (uint) length, out buffer);

			if (read == 0) return "";
			else return System.Text.Encoding.Default.GetString(buffer);
		}

        public float ReadFloat(int MemoryAddress)
        {
            byte[] buffer;
            int read = ReadMem(MemoryAddress, 4, out buffer);
            if (read == 0)
                return 0;
            else
                return BitConverter.ToSingle(buffer, 0);
        }
        public byte[] ReadAMem(IntPtr MemoryAddress, uint bytesToRead, out int bytesReaded)
        {
            byte[] buffer = new byte[bytesToRead];

            IntPtr ptrBytesReaded;
            ProcessMemoryReaderApi.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, out ptrBytesReaded);
            bytesReaded = ptrBytesReaded.ToInt32();
            return buffer;
        }
        internal byte[] ReadAMem(int p, int p_2, out int bytesReadSize)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region WriteMem
        public int WriteMem(int MemoryAddress, byte[] buf)
        {
            IntPtr procHandle = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ | ProcessMemoryReaderApi.PROCESS_VM_WRITE | ProcessMemoryReaderApi.PROCESS_VM_OPERATION, 1, (uint)m_ReadProcess.Id);
            if (procHandle == IntPtr.Zero)
                return 0;

            uint oldProtect;
            ProcessMemoryReaderApi.VirtualProtectEx(procHandle, (IntPtr)MemoryAddress, (uint)buf.Length, ProcessMemoryReaderApi.PAGE_READWRITE, out oldProtect);
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(procHandle, (IntPtr)MemoryAddress, buf, (uint)buf.Length, out ptrBytesWritten);
            ProcessMemoryReaderApi.CloseHandle(procHandle);
            return ptrBytesWritten.ToInt32();
        }

        public void WriteByte(int MemoryAddress, byte b)
        {
            WriteMem(MemoryAddress, new byte[] { b });
        }
        public void WriteInt(int MemoryAddress, int w)
        {
            byte[] buf = BitConverter.GetBytes(w);
            WriteMem(MemoryAddress, buf);
        }
        public void WriteUInt(int MemoryAddress, uint u)
        {
            byte[] buf = BitConverter.GetBytes(u);
            WriteMem(MemoryAddress, buf);
        }
        public void WriteFloat(int MemoryAddress, float f)
        {
            byte[] buf = BitConverter.GetBytes(f);
            WriteMem(MemoryAddress, buf);
        }
        public void WriteAMem(IntPtr MemoryAddress, byte[] bytesToWrite, out int bytesWritten)
        {
            IntPtr ptrBytesWritten;
            ProcessMemoryReaderApi.WriteProcessMemory(m_hProcess, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);

            bytesWritten = ptrBytesWritten.ToInt32();
        }
        #endregion
        #region Keys
        [DllImport("user32.dll")]
        public static extern short GetKeyState(Keys nVirtKey);
        public enum VirtualKeyStates : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            //
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            //
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            //
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            //
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            //
            VK_KANA = 0x15,
            VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            //
            VK_ESCAPE = 0x1B,
            //
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            //
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            //
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            //
            VK_SLEEP = 0x5F,
            //
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            //
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            //
            VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
            //
            VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
            VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
            VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
            VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
            VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
            //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            //
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            //
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            //
            VK_OEM_1 = 0xBA,   // ';:' for US
            VK_OEM_PLUS = 0xBB,   // '+' any country
            VK_OEM_COMMA = 0xBC,   // ',' any country
            VK_OEM_MINUS = 0xBD,   // '-' any country
            VK_OEM_PERIOD = 0xBE,   // '.' any country
            VK_OEM_2 = 0xBF,   // '/?' for US
            VK_OEM_3 = 0xC0,   // '`~' for US
            //
            VK_OEM_4 = 0xDB,  //  '[{' for US
            VK_OEM_5 = 0xDC,  //  '\|' for US
            VK_OEM_6 = 0xDD,  //  ']}' for US
            VK_OEM_7 = 0xDE,  //  ''"' for US
            VK_OEM_8 = 0xDF,
            //
            VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
            VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
            VK_ICO_HELP = 0xE3,  //  Help key on ICO
            VK_ICO_00 = 0xE4,  //  00 key on ICO
            //
            VK_PROCESSKEY = 0xE5,
            //
            VK_ICO_CLEAR = 0xE6,
            //
            VK_PACKET = 0xE7,
            //
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            //
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE
        }
        public bool Keystate(Keys key)
        {
            int state = GetKeyState(key);
            if (state == -127 || state == -128)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
