using System;
using System.Runtime.InteropServices;

namespace NDTBOX_LIB
{
    public class USBIOComm
    {
        [DllImport("USBIOX.DLL")]
        public static extern UInt32 USBIO_OpenDevice(UInt32 iIndex);

        [DllImport("USBIOX.DLL")]
        public static extern UInt32 USBIO_CloseDevice(UInt32 iIndex);

        [DllImport("USBIOX.DLL")]
        public static extern bool USBIO_SetStream(UInt32 iIndex, UInt32 iMode);

        [DllImport("USBIOX.DLL")]
        public static extern bool USBIO_StreamI2C(UInt32 iIndex, UInt32 iWriteLength, UInt32 iWriteBuffer,
            UInt32 iReadLength, UInt32 oReadBuffer);

        [DllImport("usb2io.dll")]
        public static extern int USB2IO_SetIoCfg(UInt32 USB2IO_hdl, int IoNbr, int IoCfg);

        [DllImport("usb2io.dll")]
        public static extern int USB2IO_SetIoOut(UInt32 USB2IO_hdl, int IoNbr, int IoOut);

        [DllImport("usb2io.dll")]
        public static extern UInt32 USB2IO_Open(int Nbr);

        [DllImport("usb2io.dll")]
        public static extern int USB2IO_Close(UInt32 USB2IO_hdl);


        public static uint INVALID_HANDLE_VALUE = 0xFFFFFFFF;
        public enum IO_Cfg
        {
            IO_In = 0,
            IO_OD,
            IO_Push
        }
    }
}