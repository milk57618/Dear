using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using ApiAiSDK;
using System.Diagnostics;
using Windows.UI.Popups;
using POS_UWP.DBConn;
using POS_UWP.Views;
using Windows.System;
using Windows.UI.Xaml.Controls;



namespace POS_UWP
{
    public sealed class CortanaService : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;
        private VoiceCommandServiceConnection voiceServiceConnection;
        private ApiAi apiAi;
        

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            serviceDeferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskCanceled;

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails != null)
            {

                var config = new AIConfiguration("6bcfd8fcf822482e935475852b97c02d",
                           SupportedLanguage.English);

                apiAi = new ApiAi(config);
                apiAi.DataService.PersistSessionId();

                try
                {
                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    voiceServiceConnection.VoiceCommandCompleted += VoiceCommandCompleted;
                    var voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                    var recognizedText = voiceCommand.SpeechRecognitionResult?.Text;

                    switch (voiceCommand.CommandName)
                    {
                        case "Open":  //개점
                            {
                                var aiResponse = await apiAi.TextRequestAsync(recognizedText);
                                //await apiAi.LaunchAppInForegroundAsync(voiceServiceConnection, aiResponse);

                                var processingMessage = new VoiceCommandUserMessage
                                {
                                    DisplayMessage = "Starting POS...",
                                    SpokenMessage = "Starting POS..."
                                };

                                var resp = VoiceCommandResponse.CreateResponse(processingMessage);
                                await voiceServiceConnection.ReportSuccessAsync(resp);

                                MainPage.startCheck = true;
                            }
                            break;
                        case "Close":  //마감
                            {
                                var aiResponse = await apiAi.TextRequestAsync(recognizedText);
                               
                                var processingMessage = new VoiceCommandUserMessage
                                {
                                    DisplayMessage = "Program The End...",
                                    SpokenMessage = "Program The End..."
                                };

                                var resp = VoiceCommandResponse.CreateResponse(processingMessage);
                                await voiceServiceConnection.ReportSuccessAsync(resp);

                                /*시스템 종료 함수*/
                                SetFinishing();

                                break;
                            }
                        case "SaleStatus":  //매출현황
                            {
                                
                            }
                            break;
                        case "SaleSearch":  //판매조회
                            {

                            }
                            break;
                        case "PayCheck": //아르바이트 시간 및 급여 확인
                            {

                            }
                            break;
                        default:
                            if (!string.IsNullOrEmpty(recognizedText))
                            {
                                var aiResponse = await apiAi.TextRequestAsync(recognizedText);
                                if (aiResponse != null)
                                {
                                    await apiAi.SendResponseToCortanaAsync(voiceServiceConnection, aiResponse);
                                }
                            }
                            else
                            {
                                
                                await SendResponse("Can't recognize");
                            }

                            break;
                    }

                }
                catch (Exception e)
                {
                    var message = e.ToString();
                    Debug.WriteLine(message);
                }
                finally
                {
                    serviceDeferral?.Complete();
                }
            }
        }

        private void SetFinishing()
        {
            /*마감 처리에 필요한 기능 중지*/
            new DBConn_MemberTime().UpdateMemberTime(POS_main.managerName);
            new DBConn_SaleSearch().Finish();
            
            /*시스템 종료*/
            ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(2));
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            serviceDeferral?.Complete();
        }

        private void VoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            serviceDeferral?.Complete();
        }

        private async Task SendResponse(string textResponse)
        {
            var userMessage = new VoiceCommandUserMessage
            {
                DisplayMessage = textResponse,
                SpokenMessage = textResponse
            };

            var response = VoiceCommandResponse.CreateResponse(userMessage);
            await voiceServiceConnection.ReportSuccessAsync(response);
        }

    }
}

