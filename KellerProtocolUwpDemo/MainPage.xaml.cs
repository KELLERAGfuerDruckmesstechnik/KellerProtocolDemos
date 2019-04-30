using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string _selectedComPort = "";
        private byte _selectedChannel = 1;
        private byte _selectedAddress = 250;

        private ObservableCollection<string> _foundComPorts = new ObservableCollection<string>();

        private ObservableCollection<string> FoundComPorts
        {
            get => _foundComPorts;
            set
            {
                ComPortComboBox.ItemsSource = value;
                ComPortComboBox.SelectedIndex = value.Count - 1; // take the last item 
                _foundComPorts = value;
            }
        }

        private SerialPortCommunication _com = null;

        public MainPage()
        {
            this.InitializeComponent();
            //  _com = InitializeSerialPortCommunication("COM26");
        }

        private static SerialPortCommunication InitializeSerialPortCommunication(string comPortName)
        {
            var port = new System.IO.Ports.SerialPort(comPortName, 9600, Parity.None, 8, StopBits.One)
            {
                DtrEnable = true,
                RtsEnable = true,
                ReadTimeout = 200,
                WriteTimeout = 200,
            };
            return new SerialPortCommunication(port);
        }

        private async void GetPortNamesButton_Click(object sender, RoutedEventArgs e)
        {
            // var ports = SerialPort.GetPortNames();  <--- Won't work (for now)
            OutputTextBlock.Text += $"{DateTime.Now}: Start searching for COM ports. Please wait a while....{Environment.NewLine}";
            ObservableCollection<string> portNames = await GetPortNamesUwpAsync();
            FoundComPorts = portNames;
            OutputTextBlock.Text += $"{DateTime.Now}: ... found port names: {string.Join(", ", portNames)}{Environment.NewLine}";
            OutputTextBlock.Text += $"{DateTime.Now}: Please select a suitable COM port from the ComboBox and press 'F48' and 'F73'!{Environment.NewLine}";
        }

        private void ComPortListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                _selectedComPort = cmb.SelectedItem?.ToString();
            }

            _com = InitializeSerialPortCommunication(_selectedComPort);
        }

        private void F48Button_Click(object sender, RoutedEventArgs e)
        {
            if (_com == null)
            {
                OutputTextBlock.Text +=
                $"{DateTime.Now}: No COM port chosen. Please press the button 'Get Port Names' and select a COM port.{Environment.NewLine}";
                return;
            }

            OutputTextBlock.Text +=
            $"{DateTime.Now}: Try to execute F48 on Port {_selectedComPort}...{Environment.NewLine}";
            try
            {
                _com.Open(this);
                KellerProtocol.KellerProtocol.F48(_com, (byte)_selectedAddress);
                _com.Close(this);
                OutputTextBlock.Text += $"{DateTime.Now}: Executed F48 on Port {_selectedComPort}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextBlock.Text +=
                $"{DateTime.Now}: ERROR when executing F48 on Port {_selectedComPort}: {exception.Message}{Environment.NewLine}";
            }
        }

        private void F73Button_Click(object sender, RoutedEventArgs e)
        {
            if (_com == null)
            {
                OutputTextBlock.Text +=
                $"{DateTime.Now}: No COM port chosen. Please press the button 'Get Port Names' and select a COM port.{Environment.NewLine}";
                return;
            }

            OutputTextBlock.Text += $"{DateTime.Now}: Try to execute F73 on Port {_selectedComPort}...{Environment.NewLine}";
            try
            {
                _com.Open(this);
                double value = KellerProtocol.KellerProtocol.F73(_com, (byte)_selectedAddress, (byte)_selectedChannel);
                _com.Close(this);
                OutputTextBlock.Text += $"{DateTime.Now}: Executed F73 on Port {_selectedComPort}.{Environment.NewLine}VALUE: {value} of channel {_selectedChannel}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextBlock.Text +=
                $"{DateTime.Now}: ERROR when executing F73 on Port {_selectedComPort}: {exception.Message}{Environment.NewLine}";
            }
        }
        /// <summary>
        /// SerialPort.GetPortNames() will NOT work in UWP. This method uses SerialDevice & DeviceInformation which seems to work. (Sometimes)
        /// https://stackoverflow.com/questions/48495093/how-to-get-available-serial-ports-in-uwp
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<string>> GetPortNamesUwpAsync()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            DeviceInformationCollection deviceCollection = await DeviceInformation.FindAllAsync(aqs);
            var portNamesList = new ObservableCollection<string>();
            foreach (DeviceInformation item in deviceCollection)
            {
                ////speed up with excluding irrelevant devices
                ////can be removed to show all devices
                //if (!item.Name.StartsWith("K1") && !item.Name.StartsWith("COM"))
                //{
                //    continue;
                //}
                try
                {
                    SerialDevice serialDevice = await SerialDevice.FromIdAsync(item.Id);
                    if (serialDevice == null)
                    {
                        continue;
                    }
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

        private void ChannelNumber_Changed(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            if (int.TryParse(textBox.Text, out int result))
            {
                _selectedChannel = (byte)result;
            }
            else
            {
                OutputTextBlock.Text += $"{DateTime.Now}: Channel needs to be a number e.g. '1'";
            }
        }
        private void Address_Changed(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            if (int.TryParse(textBox.Text, out int result))
            {
                _selectedAddress = (byte)result;
            }
            else
            {
                OutputTextBlock.Text += $"{DateTime.Now}: Address needs to be a number e.g. '250'";
            }
        }
    }
}