using System;
using System.Collections.Generic;
using Xamarin.Forms;

using System.Net;
using System.Net.Http;
using Newtonsoft;
using Newtonsoft.Json;

using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Linq;

namespace XFSpeechDemo
{
    public partial class MainPage : ContentPage
    {
        
        private ISpeechToText _speechRecongnitionInstance;
        public MainPage()
        {
            
            InitializeComponent();

            try
            {
                _speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }


            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

            MessagingCenter.Subscribe<ISpeechToText>(this, "Final", (sender) =>
            {
                start.IsEnabled = true;
            });

            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });
        }

        private void SpeechToTextFinalResultRecieved(string args)
        {
            recon.Text = args;
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            try
            {
                _speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                recon.Text = ex.Message;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                start.IsEnabled = false;
            }



        }


        string IPTEXT;
        int PORTTEXT;
        private async void Send_Clicked(object sender, EventArgs e)
      {
            IPTEXT = EntryIP.Text;


            if (IPTEXT == "")
            {
                //await DisplayAlert("Waring", "default IP will be used", "Ok");
                IPTEXT = "192.168.1.153";
            }

            if (EntryPort.Text == "")
            {
                //await DisplayAlert("Waring", "default Port will be used", "Ok");
                PORTTEXT = 59965;
            }
            else
            {
                PORTTEXT = Convert.ToInt32(EntryPort.Text);
            }


            TcpClient tcp = new TcpClient();
                string Request = "";
            try
            {
                string text = string.IsNullOrEmpty(recon.Text) ? "Привет" : recon.Text;
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                
                tcp.Connect(IPTEXT, PORTTEXT);
                tcp.ReceiveTimeout = 5;
                tcp.SendTimeout = 5000;
                var s = tcp.GetStream();

                var len = BitConverter.GetBytes(buffer.Length);

                await s.WriteAsync(len, 0, 4);
                await s.WriteAsync(buffer, 0, buffer.Length);

                var lenBuf2 = new Byte[4];
                await s.ReadAsync(lenBuf2, 0, 4);

                int length = BitConverter.ToInt32(lenBuf2, 0);

                byte[] Buffer = new byte[8192];
                int Count;
                while (length > 0)
                {
                    Count = await tcp.GetStream().ReadAsync(Buffer, 0, Buffer.Length);
                    length -= Count;
                    Request += Encoding.UTF8.GetString(Buffer, 0, Count);
                }

                DisplayAlert("Result", Request, "Ok");
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"{ex.Message} \n {ex.StackTrace} \n {Request}", "Ok");
            }
            finally
            {
                tcp.Close();
            }
       }

    
    }

}
