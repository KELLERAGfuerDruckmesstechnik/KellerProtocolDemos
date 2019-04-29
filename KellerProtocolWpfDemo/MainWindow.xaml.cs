using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const int Channel = 1;
        private const byte Address = 250;

        private SerialPortCommunication _com;
        private string _chosenComPortName;
        private ObservableCollection<string> _foundComPorts = new ObservableCollection<string>();

        private ObservableCollection<string> FoundComPorts
        {
            get => _foundComPorts;
            set
            { 
                ComPortListComboBox.ItemsSource = value;
                ComPortListComboBox.SelectedIndex = value.Count-1; // take the last item 
                _foundComPorts = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _chosenComPortName = (string)ChosenComPortLabel.Content;
            SetComPort(_chosenComPortName);
        }

        private void SetComPort(string chosenComPortName)
        {
            var port = new System.IO.Ports.SerialPort(chosenComPortName, 9600, Parity.None, 8, StopBits.One)
            {
                DtrEnable = true, RtsEnable = true, ReadTimeout = 200, WriteTimeout = 200
            };
            _com = new SerialPortCommunication(port);
        }

        private void GetPortsButton_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to get a list of connected COM ports...{Environment.NewLine}";
            FoundComPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            OutputTextbox.Text += $"{DateTime.Now}: Found Ports: {string.Join(" - ",FoundComPorts)}{Environment.NewLine}";
            OutputTextbox.ScrollToEnd();
        }

        private void ComPortListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cmb)
            {
                _chosenComPortName = cmb.SelectedItem.ToString();
            }
            ChosenComPortLabel.Content = _chosenComPortName;
            SetComPort(_chosenComPortName);
        }

        private void F48Button_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to execute F48 on Port {_chosenComPortName}...{Environment.NewLine}";
            try
            {
                _com.Open(this);
                KellerProtocol.KellerProtocol.F48(_com, (byte)Address);
                _com.Close(this);
                OutputTextbox.Text += $"{DateTime.Now}: Executed F48 on Port {_chosenComPortName}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextbox.Text += $"{DateTime.Now}: ERROR when executing F48 on Port {_chosenComPortName}: {exception.Message}{Environment.NewLine}";
            }
            OutputTextbox.ScrollToEnd();
        }

        private void F73Button_Click(object sender, RoutedEventArgs e)
        {
            OutputTextbox.Text += $"{DateTime.Now}: Try to execute F73 on Port {_chosenComPortName}...{Environment.NewLine}";

            try
            {
                _com.Open(this);
                double value = KellerProtocol.KellerProtocol.F73(_com, (byte)Address, Channel);
                _com.Close(this);
                OutputTextbox.Text += $"{DateTime.Now}: Executed F73 on Port {_chosenComPortName}.{Environment.NewLine}VALUE: {value} of channel {Channel}{Environment.NewLine}";
            }
            catch (Exception exception)
            {
                OutputTextbox.Text += $"{DateTime.Now}: ERROR when executing F73 on Port {_chosenComPortName}: {exception.Message}{Environment.NewLine}";
            }
            OutputTextbox.ScrollToEnd();
        }
    }
}
