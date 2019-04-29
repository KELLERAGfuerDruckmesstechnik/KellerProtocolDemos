using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Collections;

namespace ccs30
{
    public class KellerProtocol
    {
        // Variablen
        private SerialPort cport;
        private bool echoOn;
        private List<Object> useComport;

        // Properties
        public int Baudrate
        {
            get { return cport.BaudRate; }

            set
            {
                if (cport.BaudRate != value)
                {
                    cport.BaudRate = value;
                }
            }
        }

        public bool EchoOn
        {
            get { return echoOn; }
        }

        public bool IsComportOpen
        {
            get { return cport.IsOpen; }
        }

        public string Name
        {
            get { return cport.PortName; }
        }


        // Konstruktor
        public KellerProtocol(string comport)
        {
            cport = new SerialPort(comport, 9600, Parity.None, 8, StopBits.One);
            cport.DtrEnable = true;
            cport.RtsEnable = true;
            cport.ReadTimeout = 200;
            cport.WriteTimeout = 200;

            useComport = new List<object>();
            checkEcho();
        }


        // Allgemeine Funktionen
        public override string ToString()
        {
            return base.ToString() + "_" + cport.PortName;
        }


        // Kommunikations-Funktionen
        private void checkEcho()
        {
            openComport(this);

            lock (this)
            {
                cport.Write("e");
                System.Threading.Thread.Sleep(10);
                if (cport.ReadExisting() == "e")
                    echoOn = true;
                else
                    echoOn = false;
            }

            closeComport(this);
        }


        public void openComport(object Sender)
        {
            useComport.Add(Sender);
            if (!cport.IsOpen)
            {
                lock (this)
                {
                    cport.Open();
                    System.Threading.Thread.Sleep(500);
                }
            }
        }


        public void closeComport(object Sender)
        {
            useComport.Remove(Sender);

            if ((useComport.Count == 0) && (cport.IsOpen))
            {
                lock (this)
                {
                    cport.Close();
                }
            }
        }


        // Bus-Management Funktionen
        public bool wakeUp()
        {
            try
            {
                this.F48(250);
                System.Threading.Thread.Sleep(5);
                return true;
            }
            catch (crcException) { System.Threading.Thread.Sleep(5); return true; }
            catch (TimeoutException) { return false; }
        }


        public bool hasTransmitter()
        {
            if (wakeUp())
                return true;
            else
            {
                try
                {
                    this.F48(250);
                    return true;
                }
                catch (crcException) { return true; }
                catch (TimeoutException) { return false; }
            }
        }


        // Busprotokoll-Funktionen
        public double F30(byte address, byte coeffNo)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 30;
            sndBuffer[2] = coeffNo;

            byte[] res = secureSendRecieve(sndBuffer, 4);
            Array.Reverse(res);
            return Math.Round(BitConverter.ToSingle(res, 0), 7);
        }


        public void F31(byte address, byte coeffNo, Single value)
        {
            byte[] b = BitConverter.GetBytes(value);

            byte[] sndBuffer = new byte[7];
            sndBuffer[0] = address;
            sndBuffer[1] = 31;
            sndBuffer[2] = coeffNo;
            sndBuffer[3] = b[3];
            sndBuffer[4] = b[2];
            sndBuffer[5] = b[1];
            sndBuffer[6] = b[0];

            secureSendRecieve(sndBuffer, 1);
            System.Threading.Thread.Sleep(5);
        }


        public byte F32(byte address, byte coeffNo)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 32;
            sndBuffer[2] = coeffNo;
            byte[] res = secureSendRecieve(sndBuffer, 1);

            return res[0];
        }


        public void F33(byte address, byte coeffNo, byte value)
        {
            byte[] sndBuffer = new byte[4];
            sndBuffer[0] = address;
            sndBuffer[1] = 33;
            sndBuffer[2] = coeffNo;
            sndBuffer[3] = value;

            secureSendRecieve(sndBuffer, 1);
            System.Threading.Thread.Sleep(5);
        }


        public byte[] F48(byte address)
        {
            byte[] sndBuffer = new byte[2];
            sndBuffer[0] = address;
            sndBuffer[1] = 48;

            return secureSendRecieve(sndBuffer, 6);
        }


        public byte F66(byte address, byte newAddress)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 66;
            sndBuffer[2] = newAddress;

            byte[] res = secureSendRecieve(sndBuffer, 1);
            return res[0];
        }


        public long F69(byte address)
        {
            byte[] sndBuffer = new byte[2];
            sndBuffer[0] = address;
            sndBuffer[1] = 69;

            byte[] res = secureSendRecieve(sndBuffer, 4);
            Array.Reverse(res);
            return BitConverter.ToInt32(res, 0);
        }


        public double F73(byte address, byte channel)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 73;
            sndBuffer[2] = channel;

            byte[] res = secureSendRecieve(sndBuffer, 5);
            Array.Reverse(res);

            // Status auswerten
            BitArray status = new BitArray(new byte[] { res[0] });

            if ((!status[channel]) && (!status[7]))
                return Math.Round(BitConverter.ToSingle(res, 1), 7);
            else
                return double.NaN;
        }


        public byte[] F95(byte address, byte command)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 95;
            sndBuffer[2] = command;

            byte[] result = secureSendRecieve(sndBuffer, 1);
            System.Threading.Thread.Sleep(5);
            return result;
        }


        public byte[] F95(byte address, byte command, double value)
        {
            byte[] b = BitConverter.GetBytes(value);

            byte[] sndBuffer = new byte[7];
            sndBuffer[0] = address;
            sndBuffer[1] = 95;
            sndBuffer[2] = command;
            sndBuffer[3] = b[3];
            sndBuffer[4] = b[2];
            sndBuffer[5] = b[1];
            sndBuffer[6] = b[0];

            byte[] result = secureSendRecieve(sndBuffer, 1);
            System.Threading.Thread.Sleep(5);
            return result;
        }


        public byte[] F100(byte address, byte bteIndex)
        {
            byte[] sndBuffer = new byte[3];
            sndBuffer[0] = address;
            sndBuffer[1] = 100;
            sndBuffer[2] = bteIndex;

            return secureSendRecieve(sndBuffer, 5);
        }


        public void F101(byte address, byte index, byte[] values)
        {
            byte[] sndBuffer = new byte[8];
            sndBuffer[0] = address;
            sndBuffer[1] = 101;
            sndBuffer[2] = index;
            sndBuffer[3] = values[0];
            sndBuffer[4] = values[1];
            sndBuffer[5] = values[2];
            sndBuffer[6] = values[3];
            sndBuffer[7] = values[4];

            secureSendRecieve(sndBuffer, 1);
            System.Threading.Thread.Sleep(5);
        }

        public byte[] secureSendRecieve(byte[] sndBuffer, int expectedRecieveBytes)
        {
            byte[] res;

            try
            {
                res = sendRecieve(sndBuffer, expectedRecieveBytes);
            }
            catch (DeviceNotInitializedException)
            {
                this.F48(sndBuffer[0]);
                res = sendRecieve(sndBuffer, expectedRecieveBytes);
            }

            return res;
        }


        public byte[] sendRecieve(byte[] sndBuffer, int expectedRecieveBytes)
        {
            int rcfBufsize = 0;
            byte[] rcfBuffer;
            int offset = 0;


            // crc hinzufügen
            byte[] toSend = new byte[sndBuffer.Length + 2];
            byte[] crc = crc16(sndBuffer, 0, sndBuffer.Length);

            for (int i = 0; i < sndBuffer.Length; i++)
                toSend[i] = sndBuffer[i];

            toSend[toSend.Length - 2] = crc[0];
            toSend[toSend.Length - 1] = crc[1];


            // Echo-Settings
            if (echoOn)
            {
                rcfBufsize = expectedRecieveBytes + toSend.Length + 4;
                offset = toSend.Length;
            }
            else
                rcfBufsize = expectedRecieveBytes + 4;


            rcfBuffer = new byte[rcfBufsize];

            // Empfangsbuffer leeren
            cport.ReadExisting();


            // Senden
            System.Threading.Thread.Sleep(1);
            cport.Write(toSend, 0, toSend.Length);


            // Empfangen
            int a = 0;
            while (a < rcfBufsize)
            {
                rcfBuffer[a] = (byte)cport.ReadByte();

                // Transmitter-Fehler überprüfen
                if (a == offset + 1)
                {
                    if (rcfBuffer[a] > 127)
                        rcfBufsize = offset + 5;
                }

                a++;
            }

            // crc überprüfen
            crc = crc16(rcfBuffer, offset, rcfBufsize - 2 - offset);
            if ((crc[0] != rcfBuffer[rcfBufsize - 2]) || (crc[1] != rcfBuffer[rcfBufsize - 1]))
                throw new crcException("F" + sndBuffer[1].ToString());

            // Transmitter-Exceptions
            if (rcfBuffer[offset + 1] > 127)
            {
                switch (rcfBuffer[offset + 2])
                {
                    case 1: throw new NotImplementedFunctionException();
                    case 2: throw new InvalidOperationException();
                    case 3: throw new MessageLenghException();
                    case 32: throw new DeviceNotInitializedException();
                }
            }

            // Sender überprüfen
            if (rcfBuffer[offset] != sndBuffer[0])
                throw new answerException();


            // Resultat aufbereiten
            byte[] result = new byte[expectedRecieveBytes];
            for (int i = offset + 2; i < rcfBufsize - 2; i++)
                result[i - offset - 2] = rcfBuffer[i];

            return result;
        }


        private byte[] crc16(byte[] buffer, int offset, int bteCount)
        {
            const ushort polynom = 0xA001;
            bool ex;
            ushort crc;

            crc = 0xFFFF;

            for (int i = 0; i < bteCount; i++)
            {
                crc = (ushort)(crc ^ buffer[offset + i]);

                for (int n = 0; n < 8; n++)
                {
                    if (crc % 2 == 1)
                        ex = true;
                    else
                        ex = false;
                    crc = (ushort)(crc / 2);
                    if (ex)
                        crc = (ushort)(crc ^ polynom);
                }
            }

            return new byte[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
        }
    }


    // Exceptions
    public class crcException : Exception
    {
        public crcException(string Message) : base(Message) { }
        public crcException() : base() { }
    }

    public class answerException : Exception
    {
        public answerException(string Message) : base(Message) { }
        public answerException() : base() { }
    }

    public class NotImplementedFunctionException : Exception
    {
        public NotImplementedFunctionException(string Message) : base(Message) { }
        public NotImplementedFunctionException() : base() { }
    }

    public class DeviceNotInitializedException : Exception
    {
        public DeviceNotInitializedException(string Message) : base(Message) { }
        public DeviceNotInitializedException() : base() { }
    }

    public class MessageLenghException : Exception
    {
        public MessageLenghException(string Message) : base(Message) { }
        public MessageLenghException() : base() { }
    }

}
