using RChartServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
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
using static System.Net.Mime.MediaTypeNames;

namespace RChartClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ChartMessage chartMessage;
        private string m_chartText;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeRemoteClient();
            ChartTextList = new ObservableCollection<MessageBody>();
        }

        public ObservableCollection<MessageBody> ChartTextList
        {
            get;
            set;
        }

        private void InitializeRemoteClient()
        {
            //************************************* TCP *************************************//
            // using TCP protocol
            // running both client and server on same machines
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            IDictionary dic = new Dictionary<string, string>();
            dic["name"] = "clientTcp";
            dic["port"] = "0";
            TcpChannel chan = new TcpChannel(dic, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(chan, true);
            // Create an instance of the remote object
            chartMessage = (ChartMessage)Activator.GetObject(typeof(ChartMessage), "tcp://localhost:9090/HelloWorld");
            SwapObject swap = new SwapObject();
            chartMessage.ServerToClient += swap.ToClient;
            swap.SwapServerToClient += ChartMessage_ServerToClient;
            // if remote object is on another machine the name of the machine should be used instead of localhost.
            //************************************* TCP *************************************//
        }

        private void ChartMessage_ServerToClient(string text)
        {
            Dispatcher.Invoke(() =>
            {
                ChartTextList.Add(new MessageBody(text, DateTime.Now.ToLongTimeString(), HorizontalAlignment.Left));
            });
        }

        public string ChartText
        {
            get
            {
                return m_chartText;
            }
            set
            {
                m_chartText = value;
                OnPropertyChanged(nameof(ChartText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChartTextList.Add(new MessageBody(ChartText, DateTime.Now.ToLongTimeString(), HorizontalAlignment.Right));
                chartMessage.SetMessage(ChartText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
