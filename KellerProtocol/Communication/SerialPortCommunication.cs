using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace KellerProtocol.Communication
{
    public class SerialPortCommunication : ICommunication
    {
        private SerialPort _serialPort;
       // private readonly List<object> _useComPort;
        private bool _saveMode;
        private string _comName;
        private readonly Dictionary<string, object> _config;


        private readonly object _lockThis;

        /// <summary>Standard-Baudrates</summary>
        public static int[] DefaultBaudrates =
        {
            110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400,
            460800, 921600
        };

        /// <inheritdoc />
        public SerialPortCommunication(SerialPort serialPort)
            : this(serialPort, true)
        {
        }

        /// <param name="serialPort">Comport, mit dem gearbeitet werden soll</param>
        /// <param name="autoEcho">If true -> echo will recognized automatically</param>
        private SerialPortCommunication(SerialPort serialPort, bool autoEcho)
        {
            _serialPort = serialPort;
           // _useComPort = new List<object>();
            EchoOn = true;
            _comName = serialPort.PortName;
            _lockThis = _comName; // Lock object should be a object that is unique to the serial port.

            _saveMode = (serialPort.BaudRate == 9600);

            _config = new Dictionary<string, object>
            {
                {"autoecho", autoEcho},
                {"baudrate", serialPort.BaudRate},
                {"parity", serialPort.Parity},
                {"stopbits", serialPort.StopBits},
                {"readtimeout", serialPort.ReadTimeout},
                {"writetimeout", serialPort.WriteTimeout},
                {"handshake", serialPort.Handshake},
                {"databits", serialPort.DataBits}
            };
        }

        /// <inheritdoc />
        public string Name => (_serialPort == null) ? _comName : _serialPort.PortName;

        /// <summary>
        /// = true -> The COM port gives back an Echo
        /// </summary>
        public bool EchoOn { get; set; }

        /// <summary>
        /// =true -> Das Echo wird beim öffnen vom Comport automatisch ermittelt
        /// </summary>
        public bool AutoEcho
        {
            get => (bool)GetConfig("autoecho");
            set => SetConfig("autoecho", value);
        }

        /// <inheritdoc />
        public bool IsOpen => (_serialPort != null && _serialPort.IsOpen);


        /// <summary>
        /// Baudrate from COM Port (most likely 115000 or 9600)
        /// </summary>
        public int Speed
        {
            get => _serialPort.BaudRate;
            set
            {
                if (_serialPort.BaudRate != value) _serialPort.BaudRate = value;
            }
        }



        /// <summary>
        /// Stop-Bit configuration
        /// </summary>
        public StopBits StopBits => _serialPort.StopBits;

        /// <summary>
        /// Parity-Bit configuration
        /// </summary>
        public Parity ParityBits => _serialPort.Parity;


        /// <inheritdoc />
        public object Interface
        {
            get => _serialPort;
            set
            {
                if (value is SerialPort port)
                {
                    lock (_lockThis)
                    {
                        _serialPort = port;
                        _comName = _serialPort.PortName;
                        if (/*(_useComPort.Count > 0) &&*/ !_serialPort.IsOpen)
                            OpenPort();
                    }
                }
                else if (value == null)
                {
                    _serialPort = null;
                }
            }
        }


        /// <summary>
        /// Konfiguriert mehrere Parameter der Schnittstelle.
        /// </summary>
        /// <param name="newConfig">neue Konfiguration</param>
        public void SetConfig(Dictionary<string, object> newConfig)
        {
            foreach (KeyValuePair<string, object> kv in newConfig)
            {
                SetConfig(kv.Key, kv.Value);
            }
        }


        /// <summary>
        /// Konfiguriert einzelne Parameter der Schnittstelle.
        /// </summary>
        /// <param name="key">Konfigurations-Variable</param>
        /// <param name="value">neuer Konfigurationswert</param>
        /// <returns>=true, wenn sich etwas geändert hat</returns>
        public bool SetConfig(string key, object value)
        {
            bool changed = false;
            if (_serialPort == null) return changed;

            switch (key)
            {
                case "handshake":
                    {
                        Handshake hs = (Handshake)value;
                        if (hs != _serialPort.Handshake)
                        {
                            _serialPort.Handshake = hs;
                            changed = true;
                            _config["handshake"] = hs;
                        }
                        break;
                    }
                case "baudrate":
                    {
                        int bd = (int)value;
                        if (bd != _serialPort.BaudRate)
                        {
                            _serialPort.BaudRate = bd;
                            changed = true;
                            _config["baudrate"] = bd;
                        }
                        if (bd == 9600)
                            _saveMode = true;
                        break;
                    }
                case "parity":
                    {
                        Parity parity = (Parity)value;
                        if (parity != _serialPort.Parity)
                        {
                            _serialPort.Parity = parity;
                            _config["parity"] = parity;
                            changed = true;
                        }
                        break;
                    }
                case "stopbits":
                    {
                        StopBits stop = (StopBits)value;
                        if (stop != _serialPort.StopBits)
                        {
                            _serialPort.StopBits = stop;
                            _config["stopbits"] = stop;
                            changed = true;
                        }
                        break;
                    }
                case "readtimeout":
                    {
                        int timeout = (int)value;
                        if (timeout != _serialPort.ReadTimeout)
                        {
                            _serialPort.ReadTimeout = timeout;
                            _config["readtimeout"] = timeout;
                            changed = true;
                        }
                        break;
                    }
                case "writetimeout":
                    {
                        int timeout = (int)value;
                        if (timeout != _serialPort.WriteTimeout)
                        {
                            _serialPort.WriteTimeout = timeout;
                            _config["writetimeout"] = timeout;
                            changed = true;
                        }
                        break;
                    }
                case "databits":
                    {
                        int databits = (int)value;
                        if (databits != _serialPort.DataBits)
                        {
                            _serialPort.DataBits = databits;
                            _config["databits"] = databits;
                            changed = true;
                        }
                        break;
                    }
                case "autoecho":
                    {
                        bool autoecho = (bool)value;
                        if (autoecho != (bool)_config["autoecho"])
                        {
                            _config["autoecho"] = autoecho;
                            changed = true;
                        }
                        break;
                    }
            }

            return changed;
        }

        /// <summary>
        /// Einzelne Konfiguration auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Wert</returns>
        public object GetConfig(string key)
        {
            return _config.ContainsKey(key) ? _config[key] : null;
        }

        /// <summary>
        /// Konfiguration auslesen 
        /// </summary>
        /// <returns>Konfiguration der Schnittstelle</returns>
        public Dictionary<string, object> GetConfigCopy()
        {
            var cfg = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in _config)
                cfg.Add(kv.Key, kv.Value);

            return cfg;
        }

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="readByteCount">erwartete anzahl Bytes</param>
        public void Send(byte[] command, out byte[] rcfBuffer, int readByteCount)
        {
            rcfBuffer = new byte[readByteCount];
            if (_serialPort == null) return;

            lock (_lockThis)
            {
                try
                {
                    //    if (!_serialPort.IsOpen) return;
                    // Empfangsbuffer leeren
                    _serialPort.ReadExisting();

                    // Command schreiben
                    if (_saveMode)
                        Thread.Sleep(1);
                    _serialPort.Write(command, 0, command.Length);


                    // Empfangen
                    int a = 0;
                    while (a < readByteCount)
                    {
                        rcfBuffer[a] = (byte)_serialPort.ReadByte();
                        a++;
                    }
                }
                catch (InvalidOperationException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="endSign">End-Zeichen</param>
        public void Send(byte[] command, out byte[] rcfBuffer, byte endSign)
        {
            rcfBuffer = new byte[0];

            if (_serialPort == null) return;

            lock (_lockThis)
            {
                try
                {
                    // Empfangsbuffer leeren
                    _serialPort.ReadExisting();

                    // Command schreiben
                    if (_saveMode)
                        Thread.Sleep(1);
                    _serialPort.Write(command, 0, command.Length);

                    // Empfangen
                    do
                    {
                        Array.Resize(ref rcfBuffer, rcfBuffer.Length + 1);
                        rcfBuffer[rcfBuffer.Length - 1] = (byte)_serialPort.ReadByte();
                    } while (rcfBuffer[rcfBuffer.Length - 1] != endSign);
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        /// <summary>
        /// Comport öffnen. Der Comport wird "virtuell" für jedes übergebene Objekt geöffnet
        /// und erst wieder geschlossen wenn das letzte Object den comport wieder geschlossen hat.
        /// </summary>
        /// <param name="sender">Object das den comport öffnen will.</param>
        public void Open(object sender)
        {
            if (_serialPort == null) return;

            lock (_lockThis)
            {
                /*_useComPort.Add(sender);*/
                if (!_serialPort.IsOpen)
                    OpenPort();
            }
        }

        private void OpenPort()
        {
            if (_serialPort == null) return;

            try
            {
                //Debug.WriteLine("?Try to open Port: (before) " + _serialPort.PortName);
                _serialPort.Open();
                //Debug.WriteLine("?Try to open Port: (wait) " + _serialPort.PortName);
                Thread.Sleep(100);
                //Debug.WriteLine("?Try to open Port: (after wait) " + _serialPort.PortName);
                _serialPort.DiscardInBuffer();
                if ((bool)GetConfig("autoecho"))
                    CheckEcho();
            }
            catch (UnauthorizedAccessException)
            {
                //log.log(LogMode.error, "Comport.closeComport():" + e.GetType().ToString() + " -> " + e.Message); 
                //Debug.WriteLine("Comport OpenPort():" + e.GetType().ToString() + " -> " + e.Message);
            }
            catch (IOException)
            {
                //http://stackoverflow.com/questions/14885288/io-exception-error-when-using-serialport-open
                //Debug.WriteLine(ioexp);
            }
        }


        /// <summary>
        /// Close the Comport
        /// </summary>
        /// <param name="sender">Object representing the COM port to be closed</param>
        public void Close(object sender)
        {
       //     _useComPort.Remove(sender);
            if ((_serialPort != null) /*&& (_useComPort.Count == 0)*/ && (_serialPort.IsOpen))
            {
                lock (_lockThis)
                {
                    try
                    {
                        Debug.WriteLine("?Try to close Port: (before) " + _serialPort.PortName);
                        _serialPort.Close();
                        Debug.WriteLine("?Try to close Port: (wait) " + _serialPort.PortName);
                        Thread.Sleep(500);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //log.log(LogMode.error, "Comport.closeComport():" + e.GetType().ToString() + " -> " + e.Message); 
                    }
                }
            }
        }


        private void CheckEcho()
        {
            if (_serialPort == null) return;

            lock (_lockThis)
            {
                _serialPort.Write("e");
                Thread.Sleep(250);
                EchoOn = (_serialPort.ReadExisting() == "e");

                //log.log(LogMode.debug, "Echo: " + echoOn.ToString());
            }
        }


        /// <summary>
        /// Serielle Schnittstelle als String ausgeben
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}