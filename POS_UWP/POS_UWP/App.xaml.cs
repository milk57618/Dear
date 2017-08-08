using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Animation;
using POS_UWP.Views;
using SQLite;
using POS_UWP.PosDB;
using Windows.UI.Popups;
using ApiAiSDK;
using ApiAiSDK.Model;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;

namespace POS_UWP
{

    /// <summary>
    /// 기본 응용 프로그램 클래스를 보완하는 응용 프로그램별 동작을 제공합니다.
    /// </summary>
    /// 
    sealed partial class App : Application
    {
        /// <summary>
        /// Singleton 응용 프로그램 개체를 초기화합니다. 이것은 실행되는 작성 코드의 첫 번째
        /// 줄이며 따라서 main() 또는 WinMain()과 논리적으로 동일합니다.
        /// </summary>
        private TransitionCollection transitions;
        public static string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "POS.sqlite")); //DataBase File
        public AIService AIService { get; private set; }  //use the AIService
        private ApiAi apiAi;
        private VoiceCommandServiceConnection voiceServiceConnection;
        private BackgroundTaskDeferral serviceDeferral;

        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
               Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
               Microsoft.ApplicationInsights.WindowsCollectors.Session);

            this.InitializeComponent();

            //Create Database File
            if (!CheckFileExists("POS.sqlite").Result)
            {
                using (var db = new SQLiteConnection(DB_PATH))
                {
                    db.CreateTable<Categorys>();
                    db.CreateTable<Product>();
                    db.CreateTable<Member>();
                    db.CreateTable<Sale>();
                    db.CreateTable<MemberTime>();
                    db.CreateTable<SaleSearch>();
                    db.CreateTable<SaleHistory>();

                }
            }           

            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;

            //Access Ai, Language is English
            var config = new AIConfiguration("6bcfd8fcf822482e935475852b97c02d",
                                 SupportedLanguage.English);

            AIService = AIService.CreateService(config);

            apiAi = new ApiAi(config);
            apiAi.DataService.PersistSessionId();
        }

        private void OnResuming(object sender, object e)
        {
            Debug.WriteLine("OnResuming");
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }           
        }

        /// <summary>
        /// 최종 사용자가 응용 프로그램을 정상적으로 시작할 때 호출됩니다. 다른 진입점은
        /// 특정 파일을 여는 등 응용 프로그램을 시작할 때
        /// </summary>
        /// <param name="e">시작 요청 및 프로세스에 대한 정보입니다.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
               // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
           
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                // 탐색 컨텍스트로 사용할 프레임을 만들고 첫 페이지로 이동합니다.
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 이전에 일시 중지된 응용 프로그램에서 상태를 로드합니다.
                }

                // 현재 창에 프레임 넣기
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(POS_main), e.Arguments);
                }
                // 현재 창이 활성 창인지 확인
                Window.Current.Activate();
            }
        }

        /*
         
             
           protected async override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            AIResponse aiResponse = null;
            try
            {
                aiResponse = await AIService.ProcessOnActivatedAsync(args);

                if (args.Kind == ActivationKind.VoiceCommand)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    if (rootFrame == null)
                    {
                        rootFrame = new Frame();
                        Window.Current.Content = rootFrame;
                        Window.Current.Activate();
                    }

                    VoiceCommandActivatedEventArgs vcArgs = args as VoiceCommandActivatedEventArgs;
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += VoiceCommandCompleted;
                    var voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                    var recognizedText = voiceCommand.SpeechRecognitionResult?.Text;

                    string voiceCommandName = vcArgs.Result.RulePath.FirstOrDefault();
                    switch (voiceCommandName)
                    {
                        case "Open":
                            rootFrame.Navigate(typeof(POS_main), vcArgs.Result);
                            break;
                        default:
                            break;
                    }

                    Window.Current.Activate();
                }

            }
            catch (Exception)
            {
            }

            
        }  
             
             
        */


        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }     

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            Debug.WriteLine("OnSuspending");

            var deferral = e.SuspendingOperation.GetDeferral();
            if (AIService != null)
            {
                AIService.Dispose();
                AIService = null;
            }
            deferral.Complete();
        }
    }
}
