using ApiAiSDK;
using ApiAiSDK.Model;
using System;
using System.Diagnostics;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace POS_UWP.Views
{
    public sealed partial class POS_main : Page
    {
        public static string managerName = "";
        public static int managerID = 0;
        public static int PosId = -1;
        public static string strPosId = "";

        private SpeechSynthesizer speechSynthesizer;
        private AIService AIService => (Application.Current as App)?.AIService;
        private volatile bool recognitionActive = false;

        DispatcherTimer Timer = new DispatcherTimer();

        public POS_main()
        {
            this.InitializeComponent();
            DataContext = this;

            ListenStop.IsEnabled = false;

            InitializeSynthesizer();

            mediaElement.MediaEnded += MediaElement_MediaEnded;

            /*현재시간 나타내기*/
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0);
            Timer.Start();

            tb_Manager1.Text = managerName;

            /*개점 시 버튼 활성화*/
            if (POS_starting.openCheck == false)
            {
                btn_ChangeManager.IsEnabled = false;
                btn_Manage.IsEnabled = false;
                btn_Setting.IsEnabled = false;
                btn_Sale.IsEnabled = false;
                btn_ChangeManager.IsEnabled = false;
                btn_Finishing.IsEnabled = false;
                btn_SaleState.IsEnabled = false;
            }
            else
            {
                btn_Login.IsEnabled = false;
            }

            if (PosId != -1)
            {
                strPosId = setFiveLength(PosId);
                tb_PosNum.Text = strPosId;
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MediaElement_MediaEnded");
        }

        private string setFiveLength(int number)
        {
            string str = number.ToString();
            while(str.Length < 5)
            {
                str = "0" + str;
            }
            return str;
        }
        private void Timer_Tick(object sender, object e)
        {
            tb_Time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private async void RunInUIThread(Action a)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () => a());
        }

        private void InitializeSynthesizer()
        {
            speechSynthesizer = new SpeechSynthesizer();
        }

        private void AIService_OnListeningStopped()
        {
            RunInUIThread(() => tb_Mode.Text = "음성인식 종료");
        }

        private void AIService_OnListeningStarted()
        {
            RunInUIThread(() => tb_Mode.Text = "음성인식 시작");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            AIService.OnListeningStarted -= AIService_OnListeningStarted;
            AIService.OnListeningStopped -= AIService_OnListeningStopped;

            speechSynthesizer?.Dispose();
            speechSynthesizer = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            var response = e.Parameter as AIResponse;
            if (response != null)
            {
                var aiResponse = response;
                //OutputJson(aiResponse);
                //OutputParams(aiResponse);
            }

            AIService.OnListeningStarted += AIService_OnListeningStarted;
            AIService.OnListeningStopped += AIService_OnListeningStopped;
        }

        private async void ProcessResult(AIResponse aiResponse)
        {
            RunInUIThread(() =>
            {
                tb_Mode.Text = "실행 중...";
                //OutputJson(aiResponse);
               // OutputParams(aiResponse);
            });

            var speechText = aiResponse.Result?.Fulfillment?.Speech;
            if (!string.IsNullOrEmpty(speechText))
            {
                var speechStream = await speechSynthesizer.SynthesizeTextToStreamAsync(speechText);
                mediaElement.SetSource(speechStream, speechStream.ContentType);
                mediaElement.Play();
            }
        }

        private async void btn_Sale_Click(object sender, RoutedEventArgs e)
        {
            if (managerName != "")
            {
                Frame.Navigate(typeof(POS_sale));
            }
            else
            {
                MessageDialog md = new MessageDialog("관리자를 설정하세요");
                await md.ShowAsync();
            }
        }



        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_login));
        }

        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_setting));
        }

        private void btn_ChangeManager_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_adminsetting));
        }

        private void btn_Manage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_manager));
        }

        private void btn_Finishing_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_finishing));
        }

        private void btn_AS_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_AS));
        }

        private void btn_SaleState_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_salestatus));
        }

        private async void btn_test_Click(object sender, RoutedEventArgs e)
        {
            if (POS_starting.openCheck == false)
            {
                MessageDialog md = new MessageDialog("개점 하고 누르세요");
                await md.ShowAsync();
                return;
            }
            Web web = new Web();
            web.sendAllData();
        }

        private async System.Threading.Tasks.Task ListenStart_ClickAsync(object sender, RoutedEventArgs e)
        {
            ListenStart.IsEnabled = false;
            ListenStop.IsEnabled = true;

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
                tb_Mode.Text = "취소 중...";
            }
            catch (Exception ex)
            {
                recognitionActive = false;
                Debug.WriteLine(ex.ToString());
                tb_Mode.Text = $"Empty or error result: {Environment.NewLine}{ex}";
            }
            finally
            {
                tb_Mode.Text = "음성 인식 중...";
            }
        }

        private void ListenStop_Click(object sender, RoutedEventArgs e)
        {
            ListenStart.IsEnabled = true;
            ListenStop.IsEnabled = false;        
        }

        private void ListenStart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
