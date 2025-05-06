using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    /// <summary>
    /// Represent a set of constants.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Calibration value position in SRAM buffer.
        /// </summary>
        public const int CALIBRATION_VALUE_SRAM_POS = 0xFFF;
        public const int TXCALIBRATION1_VALUE_SRAM_POS = 0xFFD;
        public const int TXCALIBRATION2_VALUE_SRAM_POS = 0xFFE;

        /// <summary>
        /// Calibration value position in OTP buffer.
        /// </summary>
        public const int CALIBRATION_VALUE_OTP_POS = 0x0006;

        /// <summary>
        /// Firmaware size constant.
        /// </summary>
        public const int FIRMWARE_SIZE = 0x1F000;
        /// <summary>
        /// Firmaware page size constant.
        /// </summary>
        public const int FIRMWARE_PAGE_SIZE = 256;
        /// <summary>
        /// Firmaware pages count constant.
        /// </summary>
        public const int PAGES_IN_FIRMWARE = (FIRMWARE_SIZE / FIRMWARE_PAGE_SIZE);
        /// <summary>
        /// Firmaware erase max time constant.
        /// </summary>
        public const int FIRMWARE_ERASE_TIME = 1000;
        /// <summary>
        /// Max sync count constant.
        /// </summary>
        public const byte MAX_SYNC = 32;


        /// <summary>
        /// Flash size.
        /// </summary>
        public static int FLASH_SIZE = 32768;
        /// <summary>
        /// Flash pages count.
        /// </summary>
        public static int PAGES_IN_FLASH = 256;     //start with ZW0102  settings
        /// <summary>
        /// Max page buffer length.
        /// </summary>
        public static int BYTES_IN_PAGE = (FLASH_SIZE / PAGES_IN_FLASH); //128 bytes = start with ZW0102  settings

        /// <summary>
        /// EEPROM start address constant.
        /// </summary>
        public const uint EEPROM_START = 0x00;
        /// <summary>
        /// EEPROM max classes count constant.
        /// </summary>
        public const uint EEP_MAX_CLASSES = 32 + EEPROM_START;
        /// <summary>
        /// EEPROM classes start address constant.
        /// </summary>
        public const uint EEP_CLASS_START = EEP_MAX_CLASSES;
        /// <summary>
        /// EEPROM bitmask length constant.
        /// </summary>
        public const byte EEP_BITMASK_LENGTH = 29;

        /// <summary>
        /// Blank value constant.
        /// </summary>
        public static byte BLANK_VALUE = 0xFF;
        /// <summary>
        /// SRAM blank value constant.
        /// </summary>
        public const byte SRAM_BLANK_VALUE = 0x00;
        /// <summary>
        /// SRAM page size constant.
        /// </summary>
        public static uint SRAM_PAGE_SIZE = 256;
        /// <summary>
        /// SRAM size constant.
        /// </summary>
        public const uint SRAM_SIZE = (16 * 1024);
        /// <summary>
        /// Size of the SRAM in Development mode of operation.
        /// </summary>
        public const uint SRAM_DEVMODE_SIZE = (12 * 1024);
        /// <summary>
        /// Address of part of SRAM for Development mode of operation.
        /// </summary>
        public const uint SRAM_DEVMODE_OFFSET = (4 * 1024);
        /// <summary>
        /// MTP blank value constant.
        /// </summary>
        public const byte MTP_BLANK_VALUE = 0x00;
        /// <summary>
        /// MTP page size constant.
        /// </summary>
        public const int MTP_PAGE_SIZE = 64;
        /// <summary>
        /// EEPROM blank value constant.
        /// </summary>
        public const byte EEPROM_BLANK_VALUE = 0x00;
        /// <summary>
        /// EEPROM max page count constant.
        /// </summary>
        public const byte EEPROM_PAGE_SIZE = 128;
        /// <summary>
        /// End Device API init data flag constant.
        /// </summary>
        public const byte GET_INIT_DATA_FLAG_END_DEVICE_API = 0x01;
        /// <summary>
        /// Timer support init data flag constant.
        /// </summary>
        public const byte GET_INIT_DATA_FLAG_TIMER_SUPPORT = 0x02;
        /// <summary>
        /// Secondary controller init data flag constant.
        /// </summary>
        public const byte GET_INIT_DATA_FLAG_SECONDARY_CTRL = 0x04;
        /// <summary>
        /// Is SUC init data flag constant.
        /// </summary>
        public const byte GET_INIT_DATA_FLAG_IS_SUC = 0x08;
        /// <summary>
        /// NVR start address constant
        /// </summary>
        public const byte NVR_START_ADDRESS = 0x09;
        /// <summary>
        /// NVR end address constant
        /// </summary>
        public const int NVR_END_ADDRESS = 256;

        public enum StateBits
        {
            /* Sate bits: */
            STATE_CRC_BUSY = (1 << 0),    /*CRC busy. This bit will go high when a ‘Run CRC Check’*/
            /*  command has been sent to the Single Chip. It will */
            /*  return to low when the CRC check procedure is done. */
            STATE_CRC_DONE = (1 << 1),    /*CRC done. This bit is cleared when a ‘Run CRC Check’ */
            /*  command is issued and it will be set if the CRC check */
            /*  procedure passes. */
            STATE_CRC_FAILED = (1 << 2),    /*CRC failed. This bit is cleared when a ‘Run CRC Check’ */
            /*  command is issued and it will be set if the CRC check */
            /*  procedure fails. */
            STATE_WRITE_BUSY = (1 << 3),    /*Write Operation busy. This bit is high if the OTP programming */
            /*  logic is busy programming the OTP*/
            STATE_WRITE_FAILED = (1 << 4),    /*Write Operation failed. This bit is cleared when a ‘Write OTP’*/
            /*  command is issued and it will be set if the OTP write */
            /*  operation fails.*/
            STATE_CONTINUE_FEFUSED = (1 << 5),    /*Cont operation refused. This bit will be set if either a */
            /*  ‘Continue Write Operation’ or a Continue Read Operation’ are */
            /*  refused. These operations will be refused if: */
            /*  A ‘Continue Write Operation’ is not succeeding a */
            /*  ‘Write SRAM’ or a ‘Continue Write Operation’ */
            /*  command. */
            /*  A ‘Continue Read Operation’ is not succeeding a */
            /*  ‘Read OTP’, a ‘Read SRAM’ or a ‘Continue Read */
            /*  Operation’ command */
            STATE_DEV_MODE_ENABLED = (1 << 6),    /*Development mode enabled. This bit is set if the */
            /*  ‘Development Mode’ has been enabled */
            STATE_EXEC_SRAM_MODE_ENABLED = (1 << 7),   /*Exec SRAM mode enabled. This bit is set if the*/
            /*  ‘Execute out of SRAM’ Mode has been enabled */
            /*Read the number of excessive writes, Stat1:Stat0. */
        }

    }
}
