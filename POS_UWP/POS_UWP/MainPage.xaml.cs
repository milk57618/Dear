using System;
using System.Collections.Generic;
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
using POS_UWP.Views;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using ApiAiSDK;
using System.Diagnostics;
using System.Threading.Tasks;
using ApiAiSDK.Model;
using Windows.UI.Popups;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace POS_UWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SpeechSynthesizer speechSynthesizer;
        private AIService AIService => (Application.Current as App)?.AIService;
        private volatile bool recognitionActive = false;

        public static bool startCheck = false;

        public MainPage()
        {
            
            this.InitializeComponent();

            InitializeSynthesizerAsync();

            mediaElement.MediaEnded += MediaElement_MediaEnded;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MediaElement_MediaEnded");
            start_Click(start, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var response = e.Parameter as AIResponse;
            if (response != null)
            {
                var aiResponse = response;
                Output(aiResponse);
            }

            AIService.OnListeningStarted += AIService_OnListeningStarted;
            AIService.OnListeningStopped += AIService_OnListeningStopped;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            AIService.OnListeningStarted -= AIService_OnListeningStarted;
            AIService.OnListeningStopped -= AIService_OnListeningStopped;

            speechSynthesizer?.Dispose();
            speechSynthesizer = null;
        }

        private void AIService_OnListeningStopped()
        {
            RunInUIThread(() => start.Content = "처리중...");
        }

        private void AIService_OnListeningStarted()
        {
            RunInUIThread(() => start.Content = "듣는중...");
        }

        private async Task InitializeRecognizer()
        {
            await AIService.InitializeAsync();
            start.IsEnabled = true;
        }

        private async void InitializeSynthesizerAsync()
        {
            //install the VCD file
            VCDInstall vch = new VCDInstall();
            await vch.InstallVCD();

            await InitializeRecognizer();
            speechSynthesizer = new SpeechSynthesizer();
        }

        private async void RunInUIThread(Action a)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a());
        }
        
        private async void ProcessResult(AIResponse aiResponse)
        {
            RunInUIThread(() =>
            {
                start.Content = "Listen";
                tbConnect.Text = "Connect Success!!";
                Output(aiResponse);
            });

            var speechText = aiResponse.Result?.Fulfillment?.Speech;
            if (!string.IsNullOrEmpty(speechText))
            {

                var speechStream = await speechSynthesizer.SynthesizeTextToStreamAsync(speechText);
                mediaElement.SetSource(speechStream, speechStream.ContentType);
                mediaElement.Play();
            }
        }

        private void Output(AIResponse aiResponse)
        {
            string Result = aiResponse.Result.ResolvedQuery;
            txtresolvedQuery.Text = Result;
            txtSpeach.Text = aiResponse.Result.Fulfillment.Speech;

            if (Result == "open")
            {
                Frame.Navigate(typeof(POS_main));
            }
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Stop();
            }
            try
            {
                if (!recognitionActive)
                {

                    recognitionActive = true;
                    var aiResponse = await AIService.StartRecognitionAsync();
                    recognitionActive = false;

                    if (aiResponse != null)
                    {
                        ProcessResult(aiResponse);
                    }
                }
                else
                {
                    AIService.Cancel();
                }
            }
            catch (OperationCanceledException)
            {
                recognitionActive = false;
                tbConnect.Text = "취소";
            }
            catch (Exception ex)
            {
                recognitionActive = false;
                Debug.WriteLine(ex.ToString());
                string text = $"Empty or error result: {Environment.NewLine}{ex}";
                MessageDialog mess = new MessageDialog(text);
                await mess.ShowAsync();
            }
            finally
            {
                start.Content = "듣는중";
            }
        }
    }
}
