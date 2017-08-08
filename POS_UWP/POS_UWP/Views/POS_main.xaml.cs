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
using System.Threading.Tasks;
using POS_UWP.DBConn;
using Windows.System;

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

        private async void InitializeSynthesizer()
        {
            //install the VCD file
            VCDInstall vch = new VCDInstall();
            await vch.InstallVCD();

            await InitializeRecognizer();
            speechSynthesizer = new SpeechSynthesizer();
        }

        private async Task InitializeRecognizer()
        {
            await AIService.InitializeAsync();
            ListenStart.IsEnabled = true;
        }

        private void AIService_OnListeningStopped()
        {
            RunInUIThread(() => tb_Mode.Text = "처리 중...");
        }

        private void AIService_OnListeningStarted()
        {
            RunInUIThread(() => tb_Mode.Text = "듣는 중...");
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
                Output(aiResponse);
            }

            AIService.OnListeningStarted += AIService_OnListeningStarted;
            AIService.OnListeningStopped += AIService_OnListeningStopped;
        }

        private async void Output(AIResponse aiResponse)
        {
            string Result = aiResponse.Result.ResolvedQuery;
            
            //개점 음성 기능
            if (Result == "open" || Result== "hey dom" || Result == "cancel" || Result == "cadum" || Result == "hey tell me" || Result == "hey dumb" || Result == "kadam" || Result == "get down" || Result == "okdumb" || Result == "hey tom" || Result == "can't com" || Result == "can't tell me" || Result == "get some" || Result== "hey don")
            {
                if (btn_Login.IsEnabled == true)
                {
                    Frame.Navigate(typeof(POS_login));
                    tb_Mode.Text = "";
                }                   
                else
                {
                    MessageDialog mess = new MessageDialog("이미 개점이 되어있습니다.");
                    await mess.ShowAsync();
                    tb_Mode.Text = "";
                }
            }
            //마감 음성 기능
            else if(Result=="close" || Result== "malcom"|| Result == "baucom"|| Result == "agam"|| Result == "markham"|| Result == "mom com"|| Result == "bogam"|| Result == "barcham"|| Result == "my mom"|| Result == "mark"|| Result == "bagam"|| Result == "how come"|| Result == "bhaga"|| Result == "maken"|| Result == "mom home"|| Result == "how about"|| Result == "my god" || Result == "I'm"|| Result == "how long"|| Result == "my com"|| Result == "welcome"|| Result == "ha ha")
            {
                if (btn_Finishing.IsEnabled == true && tb_Manager1.Text!="") //관리자가 존재하고 개점이 된 이후 상태
                {
                    tb_Mode.Text = "";
                    new DBConn_MemberTime().UpdateMemberTime(POS_main.managerName);
                    new DBConn_SaleSearch().Finish();
                    ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.Zero);
                }
                else
                {
                    tb_Mode.Text = "";
                    ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.Zero);
                }
            }
            else
            {
                tb_Mode.Text = Result;
                MessageDialog mess = new MessageDialog("잘못 들었습니다.");
                await mess.ShowAsync();
                tb_Mode.Text = "";
            }
        }

        private async void ProcessResult(AIResponse aiResponse)
        {
            RunInUIThread(() =>
            {
                tb_Mode.Text = "실행 중...";
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

        private async void ListenStart_Click(object sender, RoutedEventArgs e)
        {
            ListenStart.IsEnabled = false;            

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
                tb_Mode.Text = "음성인식 처리 실패";
            }
            catch (Exception ex)
            {
                recognitionActive = false;
                Debug.WriteLine(ex.ToString());
                tb_Mode.Text = $"Empty or error result: {Environment.NewLine}{ex}";
            }
            finally
            {
                tb_Mode.Text = "";
                ListenStart.IsEnabled = true;
            }
        }
    }
}
