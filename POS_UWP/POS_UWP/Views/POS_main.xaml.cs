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
        public static bool start = false;

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

        /// <summary>
        /// UI Thread
        /// </summary>
        /// <param name="a"></param>
        private async void RunInUIThread(Action a)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () => a());
        }

        /// <summary>
        /// 음성인식 파일 설치 및 음성 기능 엑세스 객체 생성
        /// </summary>
        private async void InitializeSynthesizer()
        {
            //install the VCD file
            VCDInstall vch = new VCDInstall();
            await vch.InstallVCD();

            await InitializeRecognizer();
            speechSynthesizer = new SpeechSynthesizer();
        }

        /// <summary>
        /// 음성인식 AI 서비스 초기화
        /// </summary>
        /// <returns></returns>
        private async Task InitializeRecognizer()
        {
            await AIService.InitializeAsync();
            ListenStart.IsEnabled = true;
        }

        /// <summary>
        /// 음성인식 AI 서비스 종료 스레드
        /// </summary>
        private void AIService_OnListeningStopped()
        {
            RunInUIThread(() => tb_Mode.Text = "처리 중...");
        }

        /// <summary>
        /// 음성인식 AI 서비스 시작 스레드
        /// </summary>
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

        /// <summary>
        /// 음성 인식 처리 함수
        /// </summary>
        /// <param name="aiResponse"></param>
        private async void Output(AIResponse aiResponse)
        {
            string Result = aiResponse.Result.ResolvedQuery;
            
            //개점 음성 기능
            if (Result == "open" || Result.LastIndexOf("hey") == 0 || Result == "kidum" || Result == "kodum" || Result == "canal" || Result == "cancel"|| Result == "can come" || Result == "cadum" || Result == "kadam" || Result == "get down" || Result == "ok dumb"  || Result == "can't com" || Result == "can't tell me" || Result == "get some" || Result == "kadam" || Result == "can you tell me" || Result == "cape town" || Result == "kick time" || Result == "K" || Result == "K tell me" || Result == "K time" || Result=="kids um")
            {
                if (btn_Login.IsEnabled == true)
                {
                    btn_Login.IsEnabled = false;
                    
                    Frame.Navigate(typeof(POS_login));
                    tb_Mode.Text = "";
                }                   
                else
                {
                    tb_Mode.Text = Result;
                    MessageDialog mess = new MessageDialog("이미 개점이 되어있습니다.");
                    await mess.ShowAsync();
                    tb_Mode.Text = "";
                }
            }
            //마감 음성 기능
            else if(Result=="close" || Result == "malcolm" || Result == "hello" || Result== "malcom"|| Result == "baucom"|| Result == "agam"|| Result == "markham"|| Result == "mom com"|| Result == "bogam"|| Result == "barcham"|| Result == "my mom"|| Result == "mark"|| Result == "bagam"|| Result == "how come"|| Result == "bhaga"|| Result == "maken"|| Result == "mom home"|| Result == "how about"|| Result == "my god" || Result == "I'm"|| Result == "how long"|| Result == "my com"|| Result == "welcome"|| Result == "ha ha" || Result == "no problem")
            {
                if (btn_Finishing.IsEnabled == true && tb_Manager1.Text!="") //관리자가 존재하고 개점이 된 이후 상태
                {
                    tb_Mode.Text = "";
                    new DBConn_MemberTime().UpdateMemberTime(POS_main.managerName);
                    new DBConn_SaleSearch().Finish();
                    Web.sendDataToServer();
                    ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.Zero);
                }
                else
                {
                    tb_Mode.Text = "";
                    ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.Zero);
                }
            }
            //알바비 확인 음성인식
            else if (Result.Contains("poppy") || Result.Contains("puppy") || Result.Contains("bobby") || Result== "I about the" || Result== "I love you" || Result== "I'm not be" || Result == "I'm happy" || Result == "I don't be")
            {
                if(btn_Login.IsEnabled==false)
                    Frame.Navigate(typeof(POS_commute));
                else
                {
                    tb_Mode.Text = Result;
                    MessageDialog mess = new MessageDialog("개점을 먼저 하세요");
                    await mess.ShowAsync();
                    tb_Mode.Text = "";
                }

            }
            //매출
            else if (Result.Contains("true") || Result.Contains("sure") || Result == "mitscher" || Result == "later" || Result == "next year" || Result == "mature" || Result == "matroo" || Result == "better" || Result == "the trip" || Result == "picture" || Result == "neptune" || Result == "mitch" || Result == "bing trip" || Result == "bathroom" || Result == "lecture" || Result == "netrunner" || Result == "microsoft" || Result == "mid june" || Result == "vectren" || Result == "metrou")
            {
                if (btn_Login.IsEnabled == false)
                    Frame.Navigate(typeof(POS_salestatus));
                else
                {
                    tb_Mode.Text = Result;
                    MessageDialog mess = new MessageDialog("개점을 먼저 하세요");
                    await mess.ShowAsync();
                    tb_Mode.Text = "";
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

        /// <summary>
        /// 음성 인식 처리
        /// </summary>
        /// <param name="aiResponse"></param>
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
