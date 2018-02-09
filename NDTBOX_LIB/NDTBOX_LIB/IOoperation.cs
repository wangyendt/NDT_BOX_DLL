using System;
using System.Threading;

namespace NDTBOX_LIB
{
    public class IOoperation
    {
        public static UInt32 _hIOHandle = USBIOComm.INVALID_HANDLE_VALUE;

        public static void PowerOnModule()
        {
            IOConfig(10, USBIOComm.IO_Cfg.IO_Push);

            IOSet(10, 0);

            Thread.Sleep(500);
        }

        public static void PowerOffModule()
        {
            IOConfig(10, USBIOComm.IO_Cfg.IO_Push);

            IOSet(10, 1);

            Thread.Sleep(500);
        }

        public static  bool InitIO()
        {
            _hIOHandle = USBIOComm.USB2IO_Open(1);

            if (_hIOHandle == USBIOComm.INVALID_HANDLE_VALUE)
            {
                return false;
            }
            return true;
        }

        public static bool DeInitIO()
        {
            USBIOComm.USB2IO_Close(_hIOHandle);
            return true;
        }

        private static bool IOConfig(int iPin, USBIOComm.IO_Cfg ioconfig)
        {
            bool bReturn = true;

            //if (!g_bIOOpened)
            //    return false;

            if ((iPin < 1) || (iPin > 10))
                return false;

            switch (ioconfig)
            {
                case USBIOComm.IO_Cfg.IO_In:
                    USBIOComm.USB2IO_SetIoCfg(_hIOHandle, iPin, 0);
                    break;
                case USBIOComm.IO_Cfg.IO_OD:
                    USBIOComm.USB2IO_SetIoCfg(_hIOHandle, iPin, 2);
                    break;
                case USBIOComm.IO_Cfg.IO_Push:
                    USBIOComm.USB2IO_SetIoCfg(_hIOHandle, iPin, 3);
                    break;
                default:
                    bReturn = false;
                    break;
            }

            return bReturn;
        }

        private static bool IOSet(int iPin, int IOValue)
        {
            bool bReturn = true;
            int iReturn = 0;

            //if (!g_bIOOpened)
            //    return false;

            iReturn = USBIOComm.USB2IO_SetIoOut(_hIOHandle, iPin, IOValue != 0 ? 1 : 0);

            if (iReturn != 0)
                bReturn = false;

            return bReturn;
        }
    }
}