using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Windows;
using System.Runtime.Remoting.Channels.Tcp;
using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace RChartServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IChartObserver, INotifyPropertyChanged
    {
        private ChartMessage chartMessage;
        private string m_chartText;

        public MainWindow()
        {
            InitializeComponent();
            DataContext= this;
            InitializeRemoteServer();
            ChartTextList = new ObservableCollection<MessageBody>();
        }

        public ObservableCollection<MessageBody> ChartTextList
        {
            get;
            set;
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

        public void Notify(string text)
        {
            Dispatcher.Invoke(() =>
            {
                ChartTextList.Add(new MessageBody(text, DateTime.Now.ToLongTimeString(), HorizontalAlignment.Left));
            });
        }

        private void InitializeRemoteServer()
        {
            chartMessage = new ChartMessage();

            //************************************* TCP *************************************//
            // using TCP protocol
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full; 
            IDictionary dic = new Dictionary<string, string>();
            dic["name"] = "serverTcp";
            dic["port"] = "9090";
            TcpChannel channel = new TcpChannel(dic, clientProvider, serverProvider);
            //TcpChannel channel = new TcpChannel(9090);
            ChannelServices.RegisterChannel(channel, true);
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(ChartMessage), "HelloWorld", WellKnownObjectMode.Singleton);
            RemotingServices.Marshal(chartMessage, "HelloWorld");
            //************************************* TCP *************************************//
            ChartCach.Attach(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChartTextList.Add(new MessageBody(ChartText, DateTime.Now.ToLongTimeString(), HorizontalAlignment.Right));
                chartMessage.ToCient(ChartText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
