using System;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.WebSocket.Call.AdminScreenRecord;
using SignalRMVCChat.WebSocket.Call.ScreenRecord;
using SignalRMVCChat.WebSocket.CustomerProfile;
using SignalRMVCChat.WebSocket.EventTrigger;
using SignalRMVCChat.WebSocket.FormCreator;
using SignalRMVCChat.WebSocket.HelpDesk;
using SignalRMVCChat.WebSocket.HelpDesk.Article;
using SignalRMVCChat.WebSocket.HelpDesk.Category;
using SignalRMVCChat.WebSocket.HelpDesk.Language;
using SignalRMVCChat.WebSocket.Profile;
using SignalRMVCChat.WebSocket.SocialChannels;
using SignalRMVCChat.WebSocket.Typing;
using SignalRMVCChat.WebSocket.LiveAssist;
using SignalRMVCChat.WebSocket.Rate;
using SignalRMVCChat.WebSocket.UsersSeparation;

namespace SignalRMVCChat.WebSocket
{
    public class WebSocketDispacher
    {
        public static ISocketHandler Dispatch(string incomming)
        {
            var request = MyWebSocketRequest.Deserialize(incomming);
            if (request == null)
            {
                return new ErrorSocketHandler();
            }

            if (request.IsAdminMode == true)
            {
                switch (request.Name)
                {
                    case "GetClientsListForAdmin":
                        return new AdminModeGetClientsListForAdminSocketHandler();

                    case "ReadChat":
                        return new AdminModeReadChatSocketHandler();

                    case "MultimediaPmSend":
                        return new AdminModeMultimediaPmSendSocketHandler();
                    case "AdminSendToCustomer":
                        return new AdminModeSendToCustomerSocketHandler();
                    case "GetCustomerActivityDetail":
                        throw new Exception("این امکان برای ادمین ها وجود ندارد");

                    #region تحویل پیام

                    case "MultimediaDeliverd":
                        return new AdminModeMultimediaDeliverdHandler();

                    #endregion

                    case "SearchHandler":
                        return new AdminModeSearchHandler();

                    case "CustomerReceivedMsg":
                        return new AdminModeAdminReceivedMsgHanlder();
                }
            }

            switch (request.Name)
            {

                #region Rate

                case "AdminSendRatingRequest":
                    return new AdminSendRatingRequestSocketHandler();
                    break;
                case "CustomerRate":
                    return new CustomerRateSocketHandler();
                    break;
                #endregion

                #region UsersSeparation
                    
                    
                case "CustomerSaveUsersSeparationValues":
                    return new CustomerSaveUsersSeparationValuesSocketHandler();
                    break;
                
                case "CustomerGetUsersSeparationConfig":
                    return new CustomerGetUsersSeparationConfigSocketHandler();
                    break;
                case "GetUsersSeparationForm":
                    return new GetUsersSeparationFormSocketHandler();
                    break;
                case "SaveUsersSeparationForm":
                    return new SaveUsersSeparationFormSocketHander();
                    break;
                #endregion

                #region LiveAssist 

                case "LiveAssistRequest":
                    return new LiveAssistRequestSocketHandler();
                    break;

                case "LiveAssistSendDoc":
                    return new LiveAssistSendDocSocketHandler();
                    break;

                case "LiveAssistFireEvent":
                    return new LiveAssistFireEventSocketHandler();
                    break;
                    
                case "LiveAssistFireEventByAdmin":
                    return new LiveAssistFireEventByAdminSocketHandler();
                    break;
                    
                    
                #endregion

                #region EventTrigger


                case "EventFired":
                    return new EventFiredSocketHandler();
                    break; 

                case "GetEventTriggers":
                    return new GetEventTriggersSocketHandler();
                    break; 
                
                case "EventTriggerSave":
                    return new EventTriggerSaveSocketHandler();
                    break; 
                case "EventTriggerDelete":
                    return new EventTriggerDeleteSocketHandler();
                    break; 
                    
                case "EventTriggerGetById":
                    return new EventTriggerGetByIdSocketHandler();
                    break; 
                    
                    
                #endregion
                #region CustomerProfile
                case "DeleteKey":
                    return new DeleteKeySocketHandler();
                    break; 
                    
                case "SaveKey":
                    return new SaveKeySocketHandler();
                    break; 
                
                case "GetCustomerDataList":
                    return new GetCustomerDataListSocketHandler();
                    break; 
                    
                case "GetRating":
                    return new GetRatingSocketHandler();
                    break;  
                case "GetLastVisitedPages":
                    return new GetLastVisitedPagesSocketHandler();
                    break;  
                    
                case "SaveUserInfo":
                    return new SaveUserInfoSocketHandler();
                    break;  
                #endregion
                
                #region HelpDesk
                case "HelpDeskGetById":
                    return new HelpDeskGetByIdSocketHandler();
                    break;  
                case "HelpDeskSaveDetail":
                    return new HelpDeskSaveDetailSocketHandler();
                    break;  
                    

                #region Article
                
                case  "EventTriggerGetAll":
                    return new EventTriggerGetAllSocketHandler();
                break;

                case "ArticleSave":
                    return new ArticleSaveSocketHandler();
                    break;  
                    
                case "ArticleGetById":
                    return new ArticleGetByIdSocketHandler();
                    break;  
                    
                case "ArticleDeleteById":
                    return new ArticleDeleteByIdSocketHandler();
                    break;  

                #endregion
                    
                case "SelectHelpDesk":
                    return new SelectHelpDeskSocketHandler();
                    break; 
                case "RemoveHelpDesk":
                    return new RemoveHelpDeskSocketHandler();
                    break; 
                case "CreateHelpDesk":
                    return new CreateHelpDeskSocketHandler();
                    break; 
                
                #region Language
                case "GetDefinedLanguages":
                    return new GetDefinedLanguagesSocketHandler();
                    break; 
                
                    
                case "Language_Get_List":
                    return new LanguageGetListSocketHandler();
                    break; 
                case "Language_GetCurrentHelpDesk_SelectedLanguage":
                    return new LanguageGetCurrentHelpDeskSelectedLanguageSocketHandler();
                    break; 

                    
                    
                #endregion
                
                case "Category_Get_List":
                    return new CategoryGetListSocketHandler();
                    break; 
                case "Category_Delete":
                    return new CategoryDeleteSocketHandler();
                    break;  
                case "Category_Save":
                    return new CategorySaveSocketHandler();
                    break;
                case "CategoryGetById":
                    return new CategoryGetByIdSocketHandler();
                    break;
                
                #endregion
                
                #region screenRecord

                case "ScreenRecordAdminShare":
                    return new ScreenRecordAdminShareSocketHandler();
                    break;
                case "ScreenRecordAdminShareRequest":
                    return new ScreenRecordAdminShareRequestSocketHandler();
                    break;
                case "ScreenRecordCustomerClose":
                    return new ScreenRecordCustomerCloseSocketHandler();
                    break;
                case "ScreenRecordAdminClose":
                    return new ScreenRecordAdminCloseSocketHandler();
                    break;

                case "SetScreenRecordAccessRequestIsAccepted":
                    return new SetScreenRecordAccessRequestIsAcceptedSocketHandler();
                    break;
                case "ScreenRecordAccessRequest":
                    return new ScreenRecordAccessRequestSocketHandler();
                    break;
                case "ScreenRecordSave":
                    return new ScreenRecordSaveSocketHandler();
                    break;

                #endregion

                #region Form Creator

                // نمایش فرم به کاربر در مواقع رفرش صفحه و ..
                case "CustomerGetFormSingle":
                    return new CustomerGetFormSingleSocketHandler();
                    break;


                // ارسال فرم به کاربر
                case "AdminSendFormToCustomer":
                    return new AdminSendFormToCustomerSocketHandler();
                    break;

                // حذف فرم
                case "DeleteForm":
                    return new DeleteFormSocketHandler();
                    break;

                // فرم های تعریف شده
                case "GetCreatedForms":
                    return new GetCreatedFormsSocketHandler();
                    break;

                // اطلاعات پر کرده توسط بازدیدکنندگان در یک فرم خاص
                case "GetFormData":
                    return new GetFormDataSocketHandler();
                    break;

                // یک فرم
                case "GetFormSingle":
                    return new GetFormSingleSocketHandler();
                    break;

                // ذخیره یک فرم جدید
                case "SaveForm":
                    return new SaveFormSocketHandler();
                    break;

                // ذخیره اطلاعات ورودی توسط بازدیدکننده 
                case "SaveFormData":
                    return new SaveFormDataSocketHandler();
                    break;

                #endregion

                #region Typing:

                case "GetSocialChannelsInfo":
                    return new GetSocialChannelsInfoSocketHandler();
                    break;
                case "SaveSocialChannelsInfo":
                    return new SaveSocialChannelsInfoSocketHandler();
                    break;


                case "CustomerStartTyping":
                    return new CustomerStartTypingSocketHandler();
                    break;
                case "CustomerStopTyping":
                    return new CustomerStopTypingSocketHandler();
                    break;
                case "AdminStartTyping":
                    return new AdminStartTypingSocketHandler();
                    break;
                case "AdminStopTyping":
                    return new AdminStopTypingSocketHandler();
                    break;

                #endregion

                case "GetSelectedAdmin":
                    return new GetSelectedAdminSocketHandler();
                    break;
                case "GetMyProfile":
                    return new GetMyProfileSocketHandler();
                    break;

                case "SaveMyProfile":
                    return new SaveMyProfileSocketHandler();
                    break;

                case "selectCustomerForChat":
                    return new SelectCustomerForChatSocketHandler();
                case "DeleteTagFormUserTagsById":
                    return new DeleteTagFormUserTagsByIdSocketHandler();
                case "DeleteTagById":
                    return new DeleteTagByIdSocketHandler();
                case "GetUserAddedToTags":
                    return new GetUserAddedToTagsSocketHandler();
                case "GetAllTagsForCurrentAdmin":
                    return new GetAllTagsForCurrentAdminSocketHandler();

                case "GetTags":
                    return new GetTagsSocketHandler();
                case "SetCurrentUserToTags":
                    return new SetCurrentUserToTagsSocketHandler();
                case "NewTagAdd":
                    return new NewTagAddSocketHandler();

                case "GetVisitedPagesForCurrentSite":
                    return new GetVisitedPagesForCurrentSiteSocketHandler();
                case "DeleteMessage":
                    return new DeleteMessageSocketHandler();
                case "EditMessage":
                    return new EditMessageSocketHandler();
                /*case "AdminSendToAdmin":
                    return new AdminSendToAdminSocketHandler();
                case "GetAdminsListForChat":
                    return new GetAdminsListForChatSocketHandler();*/
                case "forwardChat":
                    return new forwardChatSocketHandler();
                case "GetAdminsList":
                    return new GetAdminsListSocketHandler();
                case "GetAutomaticSendChatsSocketHandler":
                    return new GetAutomaticSendChatsSocketHandler();
                case "SaveAutomaticSendChatsSocketHandler":
                    return new SaveAutomaticSendChatsSocketHandler();
                case "Register":
                    return new CustomerRegisterSocketHandler();

                case "AdminReceivedMsg":
                    return new AdminReceivedMsgHanlder();

                case "CustomerReceivedMsg":
                    return new CustomerReceivedMsgHandler();

                case "ReadChat":
                    return new ReadChatSocketHandler();
                case "CustomerSendToAdmin":
                    return new CustomerSendToAdminSocketHandler();

                case "AdminLogin":
                    return new AdminLoginSocketHandler();

                case "GetClientsListForAdmin":
                    return new GetClientsListForAdminSocketHandler();

                case "AdminSendToCustomer":
                    return new AdminSendToCustomerSocketHandler();

                case "GetCustomerActivityDetail":
                    return new GetCustomerActivityDetailSocketHandler();
                case "MultimediaPmSend":
                    return new MultimediaPmSendSocketHandler();
                case "LoadReadyPm":
                    return new LoadReadyPmSocketHandler();

                case "MultimediaDeliverd":
                    return new MultimediaDeliverdHandler();

                case "SearchHandler":
                    return new SearchHandler();


                default:
                    return new ErrorSocketHandler();
            }
        }
    }
}