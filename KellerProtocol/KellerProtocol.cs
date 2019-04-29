using System;
using System.Collections;
using KellerProtocol.Exceptions;
using KellerProtocol.Communication;

namespace KellerProtocol
{
    /// <summary>
    /// Please refer to document "Description of the Communication protocol" for Series 30 and Series40 pressure transmitters from KELLER
    /// Version 3.3
    /// http://www.keller-druck2.ch/swupdate/BusProtocols/BusProtocols.zip
    /// </summary>
    public static class KellerProtocol
    {
        /// <summary>
        /// Write Signal on COM port to wakeup device
        /// </summary>
        /// <returns></returns>
        public static bool WakeUp(ICommunication com)
        {         
            try
            {
                F48(com, 250);
                return true;
            }
            catch (CrcException)
            {
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Read configuration
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="coeffNo">Index to read</param>
        /// <returns>Double</returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static double F30(ICommunication com, byte address, byte coeffNo)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 30;
            sndBuffer[2] = coeffNo;

            byte[] res = SecureSendReceive(com, sndBuffer, 4);
            Array.Reverse(res);
            return Math.Round(BitConverter.ToSingle(res, 0), 7);
        }


        /// <summary>
        /// Write coefficient
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="coeffNo">Index to write</param>
        /// <param name="value">Value to write</param>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static void F31(ICommunication com, byte address, byte coeffNo, float value)
        {
            byte[] b = BitConverter.GetBytes(value);

            var sndBuffer = new byte[7];
            sndBuffer[0] = address;
            sndBuffer[1] = 31;
            sndBuffer[2] = coeffNo;
            sndBuffer[3] = b[3];
            sndBuffer[4] = b[2];
            sndBuffer[5] = b[1];
            sndBuffer[6] = b[0];

            SecureSendReceive(com, sndBuffer, 1);
        }

        /// <summary>
        /// Read Configuration
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="coeffNo">Index to read</param>
        /// <returns>Byte</returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte F32(ICommunication com, byte address, byte coeffNo)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 32;
            sndBuffer[2] = coeffNo;

            return SecureSendReceive(com, sndBuffer, 1)[0];
        }

        /// <summary>
        /// Write configuration
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="coeffNo">Index to write in</param>
        /// <param name="value">Value to write</param>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static void F33(ICommunication com, byte address, byte coeffNo, byte value)
        {
            var sndBuffer = new byte[4];
            sndBuffer[0] = address;
            sndBuffer[1] = 33;
            sndBuffer[2] = coeffNo;
            sndBuffer[3] = value;

            SecureSendReceive(com, sndBuffer, 1);
        }


        /// <summary>
        /// Initialize and unlock
        /// </summary>
        /// <returns>Byte-Array mit [class|group|year|week]</returns>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte[] F48(ICommunication com, byte address)
        {
            var sndBuffer = new byte[2];
            sndBuffer[0] = address;
            sndBuffer[1] = 48;

            return SecureSendReceive(com, sndBuffer, 6);
        }


        /// <summary>
        /// Write new device address
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="newAddress">New address</param>
        /// <returns></returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte F66(ICommunication com, byte address, byte newAddress)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 66;
            sndBuffer[2] = newAddress;

            byte[] res = SecureSendReceive(com, sndBuffer, 1);
            return res[0];
        }


        /// <summary>
        /// Read serial number
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <returns>Serial number</returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static long F69(ICommunication com, byte address)
        {
            var sndBuffer = new byte[2];
            sndBuffer[0] = address;
            sndBuffer[1] = 69;

            byte[] res = SecureSendReceive(com, sndBuffer, 4);
            Array.Reverse(res);
            return BitConverter.ToInt32(res, 0);
        }

        /// <summary>
        /// Read value of selected channel
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="channel">Channel number</param>
        /// <returns></returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static double F73(ICommunication com, byte address, byte channel)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 73;
            sndBuffer[2] = channel;

            byte[] res = SecureSendReceive(com, sndBuffer, 5);
            Array.Reverse(res);

            // read out state from first byte
            var status = new BitArray(new[] { res[0] });

            if ((channel > status.Length - 1) || (!status[channel] && !status[7]))
            {
                return Math.Round(BitConverter.ToSingle(res, 1), 7);
            }

            return double.NaN;
        }

        /// <summary>
        /// Calculations 
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="command">Which channel to set to 0</param>
        /// <returns></returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte[] F95(ICommunication com, byte address, byte command)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 95;
            sndBuffer[2] = command;

            byte[] result = SecureSendReceive(com, sndBuffer, 1);
            return result;
        }

        /// <summary>
        /// Calculations
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="command">Which channel to set to certain value</param>
        /// <param name="value">..the certain value</param>
        /// <returns></returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte[] F95(ICommunication com, byte address, byte command, double value)
        {
            byte[] b = BitConverter.GetBytes((float)value);

            var sndBuffer = new byte[7];
            sndBuffer[0] = address;
            sndBuffer[1] = 95;
            sndBuffer[2] = command;
            sndBuffer[3] = b[3];
            sndBuffer[4] = b[2];
            sndBuffer[5] = b[1];
            sndBuffer[6] = b[0];

            byte[] result = SecureSendReceive(com, sndBuffer, 1);
            return result;
        }

        /// <summary>
        /// Read Configuration
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="bteIndex">Index</param>
        /// <returns></returns>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static byte[] F100(ICommunication com, byte address, byte bteIndex)
        {
            var sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 100;
            sndBuffer[2] = bteIndex;

            return SecureSendReceive(com, sndBuffer, 5);
        }

        /// <summary>
        /// Write Configuration
        /// </summary>
        /// <param name="com">Communication interface</param>
        /// <param name="address">Device address</param>
        /// <param name="index">Index</param>
        /// <param name="values">new values</param>
        /// <exception cref="InvalidDeviceOperationException"></exception>
        /// <exception cref="CrcException"></exception>
        public static void F101(ICommunication com, byte address, byte index, byte[] values)
        {
            var sndBuffer = new byte[8];
            sndBuffer[0] = address;
            sndBuffer[1] = 101;
            sndBuffer[2] = index;
            sndBuffer[3] = values[0];
            sndBuffer[4] = values[1];
            sndBuffer[5] = values[2];
            sndBuffer[6] = values[3];
            sndBuffer[7] = values[4];

            SecureSendReceive(com, sndBuffer, 1);
        }

        private static byte[] SecureSendReceive(ICommunication com, byte[] sndBuffer, int expectedReceiveBytes)
        {
            try
            {
                return SendReceive(com, sndBuffer, expectedReceiveBytes);
            }
            catch (DeviceNotInitializedException)
            {
                if (TryWakeupToRetryCommand(com))
                {
                    return SendReceive(com, sndBuffer, expectedReceiveBytes);
                }

                throw;
            }
            catch (TimeoutException)
            {
                if (TryWakeupToRetryCommand(com))
                {
                    return SendReceive(com, sndBuffer, expectedReceiveBytes);
                }

                throw;
            }
        }

        private static bool TryWakeupToRetryCommand(ICommunication com)
        {

            var sndBuffer = new byte[2];
            sndBuffer[0] = 250;
            sndBuffer[1] = 48;
            try
            {
                SendReceive(com, sndBuffer, 6);
                return true;
            }
            catch (CrcException)
            {
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        private static byte[] SendReceive(ICommunication com, byte[] sndBuffer, int expectedReceiveBytes)
        {
            byte[] rcfBuffer = null;
            var offset = 0;
            int expectedBytes = expectedReceiveBytes + 4;

            // add CRC
            var toSend = new byte[sndBuffer.Length + 2];
            byte[] crc = Crc16(sndBuffer, 0, sndBuffer.Length);

            for (var i = 0; i < sndBuffer.Length; i++)
                toSend[i] = sndBuffer[i];

            toSend[toSend.Length - 2] = crc[0];
            toSend[toSend.Length - 1] = crc[1];

            if (com.EchoOn)
            {
                offset = toSend.Length;
                expectedBytes = expectedReceiveBytes + 4 + toSend.Length;
            }

            try
            {
                com.Send(toSend, out rcfBuffer, expectedBytes);
            }
            catch (TimeoutException e)
            {
                // check for device error
                if (rcfBuffer != null && rcfBuffer[offset + 1] < 127)
                {
                    throw new TimeoutException(e.Message);
                }

                expectedBytes = offset + 5;
            }

            if (rcfBuffer == null)
            {
                throw new Exception();
            }

            // check CRC
            crc = Crc16(rcfBuffer, offset, expectedBytes - 2 - offset);

            if ((crc[0] != rcfBuffer[expectedBytes - 2]) || (crc[1] != rcfBuffer[expectedBytes - 1]))
            {
                throw new CrcException("F" + sndBuffer[1] + " send:" + string.Join(", ", toSend) + "   receive:" +
                                       string.Join(",", rcfBuffer) + "  calc-crc:" + string.Join(",", crc));
            }

            // Device-Exceptions
            if (rcfBuffer[offset + 1] > 127)
            {
                string ret = string.Join(",", rcfBuffer);

                switch (rcfBuffer[offset + 2])
                {
                    case 1:
                        {
                            throw new NotImplementedFunctionException(com.Name + " receive:" + ret);
                        }
                    case 2:
                        {
                            throw new InvalidDeviceOperationException(com.Name + " receive:" + ret);
                        }
                    case 3:
                        {
                            throw new MessageLengthException(com.Name + " receive:" + ret);
                        }
                    case 32:
                        {
                            throw new DeviceNotInitializedException(com.Name + " receive:" + ret);
                        }
                }
            }

            // Check sender
            if (rcfBuffer[offset] != sndBuffer[0])
                throw new Exception();


            // Prepare result
            var result = new byte[expectedReceiveBytes];
            for (int i = offset + 2; i < rcfBuffer.Length - 2; i++)
                result[i - offset - 2] = rcfBuffer[i];

            return result;
        }


        private static byte[] Crc16(byte[] buffer, int offset, int bteCount)
        {
            const ushort polynom = 0xA001;

            ushort crc = 0xFFFF;

            for (var i = 0; i < bteCount; i++)
            {
                crc = (ushort)(crc ^ buffer[offset + i]);

                for (var n = 0; n < 8; n++)
                {
                    bool ex = crc % 2 == 1;
                    crc = (ushort)(crc / 2);
                    if (ex)
                        crc = (ushort)(crc ^ polynom);
                }
            }

            return new[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }
    }
}