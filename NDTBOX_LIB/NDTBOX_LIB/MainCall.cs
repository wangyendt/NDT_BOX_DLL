using System;
using System.Threading;

namespace NDTBOX_LIB
{
    public class MainCall
    {
        public void Initialize()
        {
            // nothing to do    
        }

        public void Dispose()
        {
            // nothing to do    
        }

        public int add(int a, int b)
        {
            Thread.Sleep(1000);
            return a + b;
        }

        public bool IICInit(bool bOpen)
        {
            return IICoperation.IICOpen(bOpen);
        }

        public int[] GetRawdata(int channelNumber, double adc2vol)
        {
            return IICoperation.IICReadRawdata(channelNumber, adc2vol);
        }

        public int GetManuFactoryID()
        {
            return IICoperation.IICReadManuFactorID();
        }

        public int GetModuleID()
        {
            return IICoperation.IICReadModuelID();
        }

        public int GetFWVersion()
        {
            return IICoperation.IICReadFWVersion();
        }

        public string GetDeviceID()
        {
            return IICoperation.IICReadDeviceID();
        }

        public object[] GetAFEInfo()
        {
            object[] os = IICoperation.IICReadAFEInfo() as object[];
            int gain = (int)os[0];
            int channelNum = (int)os[1];
            int[] offset = os[2] as int[];
            return os;
        }

        public object[] GetBias(int channelNumber)
        {
            object[] os = IICoperation.IICReadBias(channelNumber) as object[];
            int[] biasPos = os[0] as int[];
            int[] biasNeg = os[1] as int[];
            return os;
        }

        public object[] GetResistance(int channelNumber)
        {
            object[] os = IICoperation.IICReadResistance(channelNumber) as object[];
            int[] resistance = os[0] as int[];
            return os;
        }

        public object[] GetNoise(int duration, int channelNumber, double adc2vol)
        {
            object[] os = IICoperation.IICReadNoise(duration, channelNumber, adc2vol) as object[];
            double[] noisePeak = os[0] as double[];
            double[] noiseStd = os[1] as double[];
            return os;
        }


        public bool IICClose(bool bOpen)
        {
            return IICoperation.IICClose(bOpen);
        }

        public bool IOInit()
        {
            return IOoperation.InitIO();
        }

        public bool IODeInit()
        {
            return IOoperation.DeInitIO();
        }

        public void PowerOn()
        {
            IOoperation.PowerOnModule();
        }

        public void PowerOff()
        {
            IOoperation.PowerOffModule();
        }

        public UInt32 GetIOHandle()
        {
            return IOoperation._hIOHandle;
        }
    }
}
