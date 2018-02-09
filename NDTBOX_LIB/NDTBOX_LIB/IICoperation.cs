using System;
using System.Threading;

namespace NDTBOX_LIB
{
    public class IICoperation
    {

        public static bool IICWriteRead(UInt32 iIndex, UInt32 iWriteLength, UInt32 iWriteBuffer, UInt32 iReadLength,
            UInt32 iReadBuffer)
        {
            bool bResult = USBIOComm.USBIO_StreamI2C(iIndex, iWriteLength, iWriteBuffer, iReadLength, iReadBuffer);

            return bResult;
        }

        public static void SetIICSpeed(bool bFast)
        {
            if (bFast)
            {
                USBIOComm.USBIO_SetStream(0, 0x81); // 0x81, 高速100KHz, 0X80, 低速20KHz
            }
            else
            {
                USBIOComm.USBIO_SetStream(0, 0x80); // 0x81, 高速100KHz, 0X80, 低速20KHz
            }
        }

        public static void DebugModeBClean()
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODEB;
            IIcWriteBuffer[2] = (byte)RegList.DebugMode.DEBUG_MODE_OFF;

            uint writeLen = 3;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;
            IIcWriteBuffer[2] = (byte)RegList.DataReady.DATA_READY_CLEAR;

            writeLen = 3;
            readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));
        }

        /// <summary>
        /// 打开IIC
        /// </summary>
        /// <returns></returns>
        public static bool IICOpen(bool bOpen)
        {
            // 如果已经打开IIC, 则返回false
            if (bOpen)
            {
                return false;
            }

            // 尝试打开IIC设备
            uint val_Handle;
            try
            {
                val_Handle = USBIOComm.USBIO_OpenDevice(0);
            }
            catch
            {
                // Please check if the file \"USBIOX.DLL\" and \"usbio.dll\" are in the root directory
                return false;
            }
            if (val_Handle == USBIOComm.INVALID_HANDLE_VALUE || val_Handle == 0)
            {
                // IIC detected failed!
                return false;
            }

            SetIICSpeed(true);          // 打开高速模式, true为高速, false为低速

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bOpen"></param>
        /// <returns></returns>
        public static bool IICClose(bool bOpen)
        {
            // 如果打开则关闭
            if (bOpen)
            {
                USBIOComm.USBIO_CloseDevice(0);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <returns></returns>
        public static int[] IICReadRawdata(int channelNumber, double adc2vol)
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            ushort crc16 = 0;

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODE;
            IIcWriteBuffer[2] = 0x01;

            uint writeLen = 3;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READY;

            writeLen = 2;
            readLen = 1;

            int iCount = 0;
            do
            {
                Thread.Sleep(10);

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                iCount++;
            } while (IIcReadBuffer[0] == 0 && iCount < 100);

            // 初始化异常值
            int[] rawdata = new int[0];

            if (IIcReadBuffer[0] == channelNumber)
            {
                rawdata = new int[channelNumber];

                for (int i = 0; i < rawdata.Length; i++)
                {
                    IIcWriteBuffer[1] = (byte)(RegList.Reg.REG_DEBUG_DATA1 + i);

                    writeLen = 2;
                    readLen = 2;

                    IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen,
                        MyConvertor.byteArrToUint(IIcReadBuffer));

                    rawdata[i] = (short)(MyConvertor.byteArrToInt(IIcReadBuffer[1] << 8 | IIcReadBuffer[0]) * adc2vol);
                }
            }
            return rawdata;
        }

        /// <summary>
        /// 从IIC读取厂家ID
        /// </summary>
        /// <returns></returns>
        public static int IICReadManuFactorID()
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_MANUFACTURER_ID;

            uint writeLen = 2;
            uint readLen = 2;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            int value = MyConvertor.byteArrToInt(IIcReadBuffer[1] << 8 | IIcReadBuffer[0]);

            return value;
        }

        /// <summary>
        /// 从IIC读取模组ID
        /// </summary>
        /// <returns></returns>
        public static int IICReadModuelID()
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_MODULE_ID;

            uint writeLen = 2;
            uint readLen = 2;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            int value = MyConvertor.byteArrToInt(IIcReadBuffer[1] << 8 | IIcReadBuffer[0]);

            return value;
        }

        /// <summary>
        /// 从IIC读取固件版本
        /// </summary>
        /// <returns></returns>
        public static int IICReadFWVersion()
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_FW_VERSION;

            uint writeLen = 2;
            uint readLen = 2;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            int value = MyConvertor.byteArrToInt(IIcReadBuffer[1] << 8 | IIcReadBuffer[0]);

            return value;
        }

        /// <summary>
        /// 从IIC读取DeviceID, 即UID
        /// </summary>
        /// <returns></returns>
        public static string IICReadDeviceID()
        {
            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEVICE_ID;

            uint writeLen = 2;
            uint readLen = 10;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            string strDeviceID = string.Format("{0:X2}{1:X2}-{2:X2}{3:X2}{4:X2}{5:X2}-{6:X2}{7:X2}{8:X2}{9:X2}",
                IIcReadBuffer[9], IIcReadBuffer[8], IIcReadBuffer[7], IIcReadBuffer[6], IIcReadBuffer[3],
                IIcReadBuffer[4], IIcReadBuffer[3], IIcReadBuffer[2], IIcReadBuffer[1], IIcReadBuffer[0]);

            return strDeviceID;
        }

        /// <summary>
        /// 从IIC中读取AFE信息, 包括Gain, Channel Number和Offset
        /// </summary>
        /// <returns></returns>
        public static object IICReadAFEInfo()
        {
            DebugModeBClean();

            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODEB;
            IIcWriteBuffer[2] = (byte)RegList.DebugMode.DEBUG_MODE_AFE_INFO;

            uint writeLen = 3;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;

            writeLen = 2;
            readLen = 1;

            int iCount = 0;
            do
            {
                Thread.Sleep(10);

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                iCount++;
            } while (IIcReadBuffer[0] == 0 && iCount < 100);


            // 初始化一个异常值
            int gain = -1;
            int chnlNum = -1;
            int[] offset = new int[0];

            if (IIcReadBuffer[0] != 0)
            {
                uint iAFEInfoBytes = IIcReadBuffer[0];

                IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_DATAB;

                writeLen = 2;
                readLen = iAFEInfoBytes;

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen,
                    MyConvertor.byteArrToUint(IIcReadBuffer));


                if (iAFEInfoBytes == 2 + IIcReadBuffer[1] * 4)
                {
                    gain = IIcReadBuffer[0];
                    chnlNum = IIcReadBuffer[1];
                    int iGetChnlNum = chnlNum;

                    offset = new int[iGetChnlNum];
                    for (int chnl = 0; chnl < iGetChnlNum; chnl++)
                    {
                        offset[chnl] = (IIcReadBuffer[2 + chnl * 4 + 1]);
                        offset[chnl] <<= 8;
                        offset[chnl] += (IIcReadBuffer[2 + chnl * 4]);
                        offset[chnl] = MyConvertor.byteArrToInt(offset[chnl]);
                    }
                }
                else
                {
                    // AFE, Offset, ChannelNum获取失败
                }
            }
            else
            {
                // DataReady始终为0, AFE等信息获取失败
            }

            DebugModeBClean();

            return new object[] { gain, chnlNum, offset };
        }

        /// <summary>
        /// 从IIC读取VBias+和VBias-
        /// </summary>
        /// <returns></returns>
        public static object IICReadBias(int channelNumber)
        {
            DebugModeBClean();

            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODEB;
            IIcWriteBuffer[2] = (byte)RegList.DebugMode.DEBUG_MODE_READ_VBIAS;

            uint writeLen = 3;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;

            writeLen = 2;
            readLen = 1;

            int iCount = 0;
            do
            {
                Thread.Sleep(10);

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                iCount++;
            } while (IIcReadBuffer[0] == 0 && iCount < 100);


            // 初始化一个异常值
            int[] bias_pos = new int[0];
            int[] bias_neg = new int[0];

            if (IIcReadBuffer[0] == channelNumber * 4)
            {
                IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_DATAB;

                writeLen = 2;
                readLen = IIcReadBuffer[0];

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen,
                    MyConvertor.byteArrToUint(IIcReadBuffer));

                bias_pos = new int[channelNumber];
                bias_neg = new int[channelNumber];

                for (int iChn = 0; iChn < channelNumber; iChn++)
                {
                    bias_pos[iChn] = (short)(IIcReadBuffer[4 * iChn] | (IIcReadBuffer[4 * iChn + 1] << 8));
                    bias_neg[iChn] = (short)(IIcReadBuffer[4 * iChn + 2] | (IIcReadBuffer[4 * iChn + 3] << 8));
                }
            }
            else
            {
                // DataReady值不符合期望, Bias获取失败
            }

            DebugModeBClean();

            return new object[] { bias_pos, bias_neg };
        }

        /// <summary>
        /// 从IIC读取电阻信息
        /// </summary>
        /// <returns></returns>
        public static object IICReadResistance(int channelNumber)
        {
            DebugModeBClean();

            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODEB;
            IIcWriteBuffer[2] = (byte)RegList.DebugMode.DEBUG_MODE_READ_TOTAL_RESISTANCE;

            uint writeLen = 3;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;

            writeLen = 2;
            readLen = 1;

            int iCount = 0;
            do
            {
                Thread.Sleep(10);

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                iCount++;
            } while (IIcReadBuffer[0] == 0 && iCount < 100);


            // 初始化一个异常值
            int[] resistance = new int[0];

            if (IIcReadBuffer[0] == channelNumber * 4)
            {
                IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_DATAB;

                writeLen = 2;
                readLen = IIcReadBuffer[0];

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen,
                    MyConvertor.byteArrToUint(IIcReadBuffer));

                resistance = new int[channelNumber];

                for (int iChn = 0; iChn < channelNumber; iChn++)
                {
                    resistance[iChn] = (IIcReadBuffer[4 * iChn] | (IIcReadBuffer[4 * iChn + 1] << 8) | (IIcReadBuffer[4 * iChn + 2] << 16) | (IIcReadBuffer[4 * iChn + 3] << 24));
                }
            }
            else
            {
                // DataReady值不符合期望, Resistance获取失败
            }

            DebugModeBClean();

            return new object[] { resistance };
        }

        /// <summary>
        /// 从IIC读取噪声信息
        /// </summary>
        /// <param name="noiseDuration"></param>
        /// 单位: 秒
        /// <returns></returns>
        public static object IICReadNoise(int noiseDuration, int channelNumber, double adc2vol)
        {
            DebugModeBClean();

            int noiseTestFrames = Math.Max(1, noiseDuration * 100);      // convert seconds to frames

            byte[] IIcWriteBuffer = new byte[256];
            byte[] IIcReadBuffer = new byte[256];

            IIcWriteBuffer[0] = 0xA0;
            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_DATAB;
            IIcWriteBuffer[2] = (byte)noiseTestFrames;
            IIcWriteBuffer[3] = (byte)(noiseTestFrames >> 8);

            uint writeLen = 4;
            uint readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;
            IIcWriteBuffer[2] = 0x02;

            writeLen = 3;
            readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_MODEB;
            IIcWriteBuffer[2] = (byte)RegList.DebugMode.DEBUG_MODE_NOISE_CALCULATE;

            writeLen = 3;
            readLen = 0;

            IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

            writeLen = 2;
            readLen = 1;

            int iCount = 0;
            do
            {
                Thread.Sleep(10);

                IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DATA_READYB;

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                iCount++;
            } while ((IIcReadBuffer[0] != channelNumber * 6) && (iCount < 5 * noiseTestFrames));


            // 初始化一个异常值
            double[] noise_peak = new double[0];
            double[] noise_std = new double[0];

            if (IIcReadBuffer[0] == channelNumber * 6)
            {
                IIcWriteBuffer[1] = (byte)RegList.Reg.REG_DEBUG_DATAB;

                writeLen = 2;
                readLen = IIcReadBuffer[0];

                IICWriteRead(0, writeLen, MyConvertor.byteArrToUint(IIcWriteBuffer), readLen, MyConvertor.byteArrToUint(IIcReadBuffer));

                noise_peak = new double[channelNumber];
                noise_std = new double[channelNumber];

                for (int iChn = 0; iChn < channelNumber; iChn++)
                {
                    uint iPeak = (uint)(IIcReadBuffer[6 * iChn] | (IIcReadBuffer[6 * iChn + 1] << 8));
                    uint iSum = (uint)(IIcReadBuffer[6 * iChn + 2] | (IIcReadBuffer[6 * iChn + 3] << 8) |
                                       (IIcReadBuffer[6 * iChn + 4] << 16) | (IIcReadBuffer[6 * iChn + 5] << 24));

                    double dAvg = (double)iSum / noiseTestFrames;
                    dAvg = Math.Max(1, dAvg);
                    double dAvgStd = Math.Sqrt(dAvg);

                    noise_peak[iChn] = iPeak * adc2vol;      // 1 ADC = 0.36 mV
                    noise_std[iChn] = dAvgStd * adc2vol;     // 1 ADC = 0.36mV
                }
            }

            DebugModeBClean();

            return new object[] { noise_peak, noise_std };
        }
    }
}