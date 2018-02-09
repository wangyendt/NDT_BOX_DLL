namespace NDTBOX_LIB
{
    public class RegList
    {
        public enum Reg
        {
            REG_HOST_STATUS = 0x50,

            REG_DEBUG_MODE = 0x60,
            REG_DATA_READY = 0x61,
            REG_DEBUG_DATA1 = 0x62,
            REG_DEBUG_DATA2 = 0x63,
            REG_DEBUG_DATA3 = 0x64,
            REG_DEBUG_DATA4 = 0x65,
            REG_DEBUG_DATA5 = 0x66,
            REG_DEBUG_DATA6 = 0x67,
            REG_DEBUG_DATA7 = 0x68,
            REG_DEBUG_DATA8 = 0x69,
            REG_DEBUG_DATA9 = 0x6A,
            REG_DEBUG_DATA10 = 0x6B,
            REG_DEBUG_DATA11 = 0x6C,
            REG_DEBUG_DATA12 = 0x6D,
            REG_DEBUG_DATA13 = 0x6E,
            REG_DEBUG_DATA14 = 0x6F,

            REG_DEBUG_MODEB = 0x80,
            REG_DATA_READYB = 0x81,
            REG_DEBUG_DATAB = 0x82,

            REG_DEBUG_MODEC = 0x83,
            REG_DATA_READYC = 0x84,
            REG_DEBUG_DATAC = 0x85,

            REG_POINTER_NUMBER = 0x10,
            REG_POINTER1_DATA = 0x11,
            REG_FORCE_DATA_NUMBER = 0x20,
            REG_FORCE_DATA1 = 0x21,

            REG_DEVICE_ID = 0x02,
            REG_MANUFACTURER_ID = 0x03,
            REG_MODULE_ID = 0x04,
            REG_FW_VERSION = 0x05,

            REG_TASK_ENABLE = 0x0A,

            REG_TEST_REG = 0x1F,

            REG_UART_PRINT_ENABLE = 0xD4,
        };

        public enum Host
        {
            HOST_STATUS_NORMAL = 0x00,
        };

        public enum DataReady
        {
            DATA_READY_CLEAR = 0x00,
        };

        public enum DebugMode
        {
            DEBUG_MODE_OFF = 0x00,
            DEBUG_MODE_RAWDATA_OUT = 0x01,
            DEBUG_MODE_AFE_INFO = 0x04,
            DEBUG_MODE_RAED_PARAMETER = 0x06,
            DEBUG_MODE_DIRECTLY_PARAMETER = 0x08,
            DEBUG_MODE_WRITE_CAL_DATA_CHANNEL = 0x0A,
            DEBUG_MODE_READ_CAL_DATA_CHANNEL = 0x0B,
            DEBUG_MODE_RAWDATA_CRC16 = 0x10,
            DEBUG_MODE_NOISE_CALCULATE = 0x11,
            DEBUG_MODE_WRITE_GAIN_REG = 0x17,
            DEBUG_MODE_READ_GAIN_REG = 0x18,
            DEBUG_MODE_READ_VBIAS = 0x19,
            DEBUG_MODE_READ_TOTAL_RESISTANCE = 0x1A,

            DEBUG_MODE_ADCRAWDATA_CRC16 = 0x20,
            DEBUG_MODE_RAWDATA_COUNT_CRC16 = 0x21,
            DEBUG_MODE_ADCRAWDATA_COUNT_CRC16 = 0x22,
            DEBUG_MODE_RAWDATA_COUNT_FRAMES_CRC16 = 0x23,
            DEBUG_MODE_ADCRAWDATA_COUNT_FRAMES_CRC16 = 0x24,

            DEBUG_MODE_WRITE_CHANNEL_CAL_DATA = 0x30,
            DEBUG_MODE_READ_CHANNEL_CAL_DATA = 0x31,
            DEBUG_MODE_GPIO_SET = 0x34,
            DEBUG_MODE_GPIO_GET = 0x35,
            DEBUG_MODE_READ_VBIAS_TWOSTATUS = 0x36,
        };
    }
}