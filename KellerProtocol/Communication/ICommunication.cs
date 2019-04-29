using System.Collections.Generic;

namespace KellerProtocol.Communication
{
    /// <summary>
    /// Interface for a communication interface such as System.IO.SerialPort, Windows.Devices.SerialCommunication, TCP/IP or Bluetooth
    /// </summary>
    public interface ICommunication
    {
        /// <summary>Name der Schnittstelle</summary>
        string Name { get; }

        /// <summary>= true, wenn die Schnittstelle ein Echo zurückgibt</summary>
        bool EchoOn { get; set; }

        /// <summary>= true, Echo automatisch ermitteln</summary>
        bool AutoEcho { get; set; }

        /// <summary>= true, wenn die Schnittstelle offen ist</summary>
        bool IsOpen { get; }

        /// <summary>gibt die geschwindigkeit der Schnittstelle zurück</summary>
        int Speed { get; }

        /// <summary>Schnittstelle</summary>
        object Interface { get; set; }

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="readByteCount">erwartete anzahl Bytes</param>
        void Send(byte[] command, out byte[] rcfBuffer, int readByteCount);

        /// <summary>
        /// Daten über die Schnittstelle senden und empfangen
        /// </summary>
        /// <param name="command">gesendete Daten</param>
        /// <param name="rcfBuffer">empfangene Daten</param>
        /// <param name="endSign">empfangen bis zu diesem Zeichen</param>
        void Send(byte[] command, out byte[] rcfBuffer, byte endSign);

        /// <summary>
        /// Öffnet die Schnittstelle
        /// </summary>
        /// <param name="sender">ausführendes Objekt</param>
        void Open(object sender);

        /// <summary>
        /// Schliesst die Schnittstelle
        /// </summary>
        /// <param name="sender">ausführendes Objekt</param>
        void Close(object sender);

        /// <summary>
        /// Konfiguriert mehrere Parameter der Schnittstelle.
        /// </summary>
        /// <param name="newConfig">neue Konfiguration</param>
        void SetConfig(Dictionary<string, object> newConfig);

        /// <summary>
        /// Konfiguriert einzelne Parameter der Schnittstelle.
        /// </summary>
        /// <param name="key">Konfigurations-Variable</param>
        /// <param name="value">neuer Konfigurationswert</param>
        /// <returns>=true, wenn sich etwas geändert hat</returns>
        bool SetConfig(string key, object value);

        /// <summary>
        /// Einzelne Konfiguration auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Wert</returns>
        object GetConfig(string key);

        /// <summary>
        /// Konfiguration auslesen
        /// </summary>
        /// <returns>Konfiguration der Schnittstelle</returns>
        Dictionary<string, object> GetConfigCopy();
    }
}