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
using System.IO.Ports;

namespace AntControl
{
   

    public partial class MainWindow : Window
    {
        SerialPort Serial;
  
        bool rel1;
        bool rel2;
        bool rel3;
        bool rel4;
        bool connect;
        int numPorts = 0;
        byte[] rx_data = new byte[256];


        public MainWindow()
        {
            InitializeComponent();
            Serial = new SerialPort();
            string[] ports = SerialPort.GetPortNames();
           // new ComboBox();

            foreach (string port in ports)
            {
                COMComboBox.ItemsSource = ports;
                numPorts++;
            }
        }


        private void Rel1_Click(object sender, RoutedEventArgs e)
        {
            if (rel1 == false)
            {
                //REL1.Background = Brushes.Red;
                rel1 = true;
                Serial.DiscardOutBuffer();
                Serial.Write("1Y");
            } else {
                //REL1.Background = Brushes.GreenYellow;
                Serial.Write("1N");
                rel1 = false;
            }
        }

        private void Rel2_Click(object sender, RoutedEventArgs e)
        {
            if (rel2 == false)
            {
                //REL2.Background = Brushes.Red;
                rel2 = true;
                Serial.Write("2Y");
            }
            else
            {
               // REL2.Background = Brushes.GreenYellow;
                Serial.Write("2N");
                rel2 = false;
            }
        }

        private void Rel3_Click(object sender, RoutedEventArgs e)
        {
            if (rel3 == false)
            {
                REL3.Background = Brushes.Red;
                rel3 = true;
                Serial.Write("ON" + 3);
            }
            else
            {
                REL3.Background = Brushes.GreenYellow;
                rel3 = false;
            }
        }
        private void Rel4_Click(object sender, RoutedEventArgs e)
        {
            if (rel4 == false)
            {
                REL4.Background = Brushes.Red;
                rel4 = true;
                Serial.Write("ON" + 4);
            }
            else
            {
                REL4.Background = Brushes.GreenYellow;
                rel4 = false;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (connect == false)
            {
                if (!(COMComboBox.Text == null))
                {
                    Serial.PortName = COMComboBox.Text;
                    Serial.Handshake = System.IO.Ports.Handshake.None;
                    Serial.BaudRate = 9600;
                    Serial.Parity = Parity.None;
                    Serial.DataBits = 8;
                    Serial.StopBits = StopBits.One;
                    Serial.ReadTimeout = 200;
                    Serial.Open();
                    Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
                    connect = true;
                    ButConnect.Content = "Разсоед";
                }
            }
            else
            {
                Serial.Close();
                connect = false;
                ButConnect.Content = "Соединен";
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            rx_data[0] = (byte)Serial.ReadByte();
            rx_data[1] = (byte)Serial.ReadByte();
            rx_data[2] = (byte)Serial.ReadByte();
            rx_data[3] = (byte)Serial.ReadByte();
            rx_data[4] = (byte)Serial.ReadByte();
            rx_data[5] = (byte)Serial.ReadByte();
            Serial.DiscardInBuffer();

            Dispatcher.Invoke(() =>
            {
                WriteDataAsync(rx_data);
            });
            //Dispatcher.Invoke(DispatcherPriority.Background, new Delegate(WriteData), recieved_data);
        }

        private async Task WriteDataAsync(byte[] rx)
        {
            if (rx[0] == 'Y')
            {
                REL1.Background = Brushes.Red;
            }
            else
            {

                REL1.Background = Brushes.GreenYellow;
            }

            if (rx[1] == 'Y')
            {
                REL2.Background = Brushes.Red;
            }
            else
            {
                REL2.Background = Brushes.GreenYellow;
            }
        }


    }
    }





