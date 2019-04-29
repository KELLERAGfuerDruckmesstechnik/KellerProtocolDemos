using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.CompilerServices;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using System.Threading;
using System.Threading.Tasks;

using System.IO.Ports; // From Nuget
using KellerProtocol.Communication; // Referenced project

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KellerProtocolUwpDemo
{
  /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var port = new System.IO.Ports.SerialPort(ComPortName, 9600, Parity.None, 8, StopBits.One)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 200,
                WriteTimeout = 200
            };
            _com = new SerialPortCommunication(port);
        }

        private readonly SerialPortCommunication _com;

        private const string ComPortName = "COM26";
        private const int Channel = 1;
        private const byte Address = 250;

        private void GetPortNamesButton_Click(object sender, RoutedEventArgs e)
        {
            // var ports = SerialPort.GetPortNames();  <--- Won't work (for now)
             var portNamesTask = GetPortNamesUwpAsync();
        }

        private void f48_btn_Click(object sender)
        {
            OutputTextBlock.Text += $"{DateTime.Now}: Try to execute F48 on Port {ComPortName}...{Environment.NewLine}";

            try
            {
                _com.Open(this);
                KellerProtocol.KellerProtocol.F48(_com, (byte)Address);
                _com.Close(this);
                OutputTextBlock.Text += $"{DateTime.Now}: Executed F48 on Port {ComPortName}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextBlock.Text += $"{DateTime.Now}: ERROR when executing F48 on Port {ComPortName}: {exception.Message}{Environment.NewLine}";
            }
        }

        private void f73_btn_Click(object sender)
        {
            OutputTextBlock.Text += $"{DateTime.Now}: Try to execute F73 on Port {ComPortName}...{Environment.NewLine}";

            try
            {
                _com.Open(this);
                double value = KellerProtocol.KellerProtocol.F73(_com, (byte)Address, Channel);
                _com.Close(this);
                OutputTextBlock.Text += $"{DateTime.Now}: Executed F73 on Port {ComPortName}.{Environment.NewLine}VALUE: {value} of channel {Channel}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextBlock.Text += $"{DateTime.Now}: ERROR when executing F73 on Port {ComPortName}: {exception.Message}{Environment.NewLine}";
            }
        }

        private void InitButton_Click(object sender, RoutedEventArgs e)
        {
            f48_btn_Click(sender);
        }

        private void GetChannelValueButton_Click(object sender, RoutedEventArgs e)
        {
            f73_btn_Click(sender);
        }


        /// <summary>
        /// SerialPort.GetPortNames() will NOT work in UWP. This method uses SerialDevice & DeviceInformation which seems to work. (Sometimes)
        /// https://stackoverflow.com/questions/48495093/how-to-get-available-serial-ports-in-uwp
        /// </summary>
        /// <returns></returns>
        public static async Task<List<string>> GetPortNamesUwpAsync()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            DeviceInformationCollection deviceCollection = await DeviceInformation.FindAllAsync(aqs);
            var portNamesList = new List<string>();
            foreach (DeviceInformation item in deviceCollection)
            {
                try
                {
                    SerialDevice serialDevice = await SerialDevice.FromIdAsync(item.Id);
                    string portName = serialDevice.PortName;
                    portNamesList.Add(portName);
                }
                catch
                {
                    continue;
                }
            }
            return portNamesList;
        }
    }
}
