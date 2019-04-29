using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO.Ports; // IMPORT VIA NUGET

using KellerProtocol.Communication; // Added project to references

namespace KellerProtocolWpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SerialPortCommunication _com;
        private string[] _comPorts = new string[0];

        private const string ComPortName = "COM29";
        private const int Channel = 1;
        private const byte Address = 250;

        public MainWindow()
        {
            InitializeComponent();

            var port = new System.IO.Ports.SerialPort(ComPortName, 9600, Parity.None, 8, StopBits.One)
            {
                DtrEnable = true, RtsEnable = true, ReadTimeout = 200, WriteTimeout = 200
            };
            _com = new SerialPortCommunication(port);
        }

        private void GetPortsButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to get a list of connected COM ports...{Environment.NewLine}";
            _comPorts = SerialPort.GetPortNames();
            OutputTextbox.Text += $"{DateTime.Now}: Found Ports: {string.Join(" - ",_comPorts)}{Environment.NewLine}";
            OutputTextbox.ScrollToEnd();
        }

        private void F48Button_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to execute F48 on Port {ComPortName}...{Environment.NewLine}";

            try
            {
                _com.Open(this);
                KellerProtocol.KellerProtocol.F48(_com, (byte)Address);
                _com.Close(this);
                OutputTextbox.Text += $"{DateTime.Now}: Executed F48 on Port {ComPortName}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextbox.Text += $"{DateTime.Now}: ERROR when executing F48 on Port {ComPortName}: {exception.Message}{Environment.NewLine}";
            }
            OutputTextbox.ScrollToEnd();
        }

        private void F73Button_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to execute F73 on Port {ComPortName}...{Environment.NewLine}";

            try
            {
                _com.Open(this);
                double value = KellerProtocol.KellerProtocol.F73(_com, (byte)Address, Channel);
                _com.Close(this);
                OutputTextbox.Text += $"{DateTime.Now}: Executed F73 on Port {ComPortName}.{Environment.NewLine}VALUE: {value} of channel {Channel}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextbox.Text += $"{DateTime.Now}: ERROR when executing F73 on Port {ComPortName}: {exception.Message}{Environment.NewLine}";
            }
            OutputTextbox.ScrollToEnd();
        }
    }
}
