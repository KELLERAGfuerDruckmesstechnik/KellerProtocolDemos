using System.Collections.Generic;

namespace KellerProtocol.Communication
{
    /// <summary>
    /// Interface for a communication interface such as System.IO.SerialPort, Windows.Devices.SerialCommunication, TCP/IP or Bluetooth
    /// </summary>
    public interface ICommunication
    {
        /// <summary>Name of the interface (eg. SerialPort => "COM12")</summary>
        string Name { get; }

        /// <summary>= true, if the interface gives back an Echo</summary>
        bool EchoOn { get; set; }

        /// <summary>= true, Echo will be recognized automatically</summary>
        bool AutoEcho { get; set; }

        /// <summary>= true, when interface is open</summary>
        bool IsOpen { get; }

        /// <summary>speed of interface (With serial ports it is measured in Baudrate)</summary>
        int Speed { get; }

        /// <summary>The interface</summary>
        object Interface { get; set; }

        /// <summary>
        /// Send AND receive data from interface
        /// </summary>
        /// <param name="command">send data</param>
        /// <param name="rcfBuffer">received data</param>
        /// <param name="readByteCount">Count of to received bytes</param>
        void Send(byte[] command, out byte[] rcfBuffer, int readByteCount);

        /// <summary>
        /// Send AND receive data from interface
        /// </summary>
        /// <param name="command">send data</param>
        /// <param name="rcfBuffer">received data</param>
        /// <param name="endSign">receive until this character</param>
        void Send(byte[] command, out byte[] rcfBuffer, byte endSign);

        /// <summary>
        /// opens the interface
        /// </summary>
        /// <param name="sender">origin object that wants to open</param>
        void Open(object sender);

        /// <summary>
        /// closes the interface
        /// </summary>
        /// <param name="sender">origin object that wants to open</param>
        void Close(object sender);

        /// <summary>
        /// Configure multiple configure parameters at once
        /// </summary>
        /// <param name="newConfig">New parameters</param>
        void SetConfig(Dictionary<string, object> newConfig);

        /// <summary>
        /// Configures single configure parameter
        /// </summary>
        /// <param name="key">configuration key</param>
        /// <param name="value">configuration value</param>
        /// <returns>=true, if configuration was changed</returns>
        bool SetConfig(string key, object value);

        /// <summary>
        /// Read out one configuration parameter
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Value</returns>
        object GetConfig(string key);

        /// <summary>
        /// Read out all configuration parameters
        /// </summary>
        /// <returns>all configuration parameters</returns>
        Dictionary<string, object> GetConfigCopy();
    }
}