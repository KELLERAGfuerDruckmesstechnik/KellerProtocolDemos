using System;
using System.Windows.Forms;
using System.IO.Ports;

// Comport einbinden
using ccs30;

namespace S30csExample
{
    public partial class Example : Form
    {
        // Variablen
        private KellerProtocol selectedComport;


        // Konstruktor
        public Example()
        {
            InitializeComponent();

            // Vorhandene Comports auflisten
            foreach (string s in SerialPort.GetPortNames())
                comport_combo.Items.Add(new KellerProtocol(s));

            if (comport_combo.Items.Count > 0)
                comport_combo.SelectedIndex = 0;

            baud_combo.SelectedIndex = 0;
        }



        // Beispiel-Funktionen
        private void f48_btn_Click(object sender, EventArgs e)
        {
            selectedComport.openComport(this);

            try
            {
                byte[] result = selectedComport.F48((byte)adress_ud.Value);
                logByteResult("F48", result);
            }
            catch (Exception ex)
            {
                log(ex.GetType().ToString());
            }


            selectedComport.closeComport(this);
        }


        private void f69_btn_Click(object sender, EventArgs e)
        {
            selectedComport.openComport(this);

            try
            {
                long sn = selectedComport.F69((byte)adress_ud.Value);
                log("F69:\t"+ sn.ToString());
            }
            catch (Exception ex)
            {
                log(ex.GetType().ToString());
            }


            selectedComport.closeComport(this);
        }


        private void f73_btn_Click(object sender, EventArgs e)
        {
            selectedComport.openComport(this);

            try
            {
                double value = selectedComport.F73((byte)adress_ud.Value, (byte)channel_ud.Value);
                log("F73:\t" + value.ToString());
            }
            catch (Exception ex)
            {
                log(ex.GetType().ToString());
            }


            selectedComport.closeComport(this);
        }


        // Hilfs-Funktionen
        private void logByteResult(string function, byte[] values)
        {
            string line;

            line = function + ":\t";

            foreach (byte b in values)
                line += b.ToString() + "\t";

            log(line);
        }

        private void log(string message)
        {
            log_tb.Text += message + "\r\n";
        }

        private void log_tb_TextChanged(object sender, EventArgs e)
        {
            log_tb.Select(log_tb.Text.Length + 1, 2);
            log_tb.ScrollToCaret();
        }

        private void comport_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedComport = (KellerProtocol)comport_combo.SelectedItem;
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
