using System;
using SignalRMVCChat.DependencyInjection;
using SignalRMVCChat.WebSocket.Call.AdminScreenRecord;
using SignalRMVCChat.WebSocket.Call.ScreenRecord;
using SignalRMVCChat.WebSocket.FormCreator;
using SignalRMVCChat.WebSocket.Profile;
using SignalRMVCChat.WebSocket.SocialChannels;
using SignalRMVCChat.WebSocket.Typing;

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