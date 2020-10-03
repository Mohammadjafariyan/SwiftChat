import {CurrentUserInfo} from './Socket';
import {cookieManager} from './CookieManager';
import LoginPage from './../Pages/LoginPage';
import {DataHolder} from "./DataHolder";
import {FormShowerInChatHolder} from "../Components/FormShowerInChat";

class dispatcher {

    dispatch(res) {
        if (res.Type == -1)//error
        {
            console.error(res.Message);


        }
        console.log('dispatcher===>',res.Name)
        switch (res.Name) {
            
            
            case 'saveFormDataCallback':
                /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
                if (FormShowerInChatHolder) {


                    /*form.AfterMessage,
                                formId,
                                chatId,
                                FormValues*/

                    let FormShowerInChat=  FormShowerInChatHolder[res.Content.formId+''+res.Content.UniqId];

                    if (FormShowerInChat){
                        FormShowerInChat.saveFormDataCallback(res);
                    }
                }
                break;
            
            case 'deleteFormCallback':
                if (CurrentUserInfo.FormCreatorPage) {
                    CurrentUserInfo.FormCreatorPage.deleteFormCallback(res);
                }
                break;
            case 'saveFormCallback':
                if (CurrentUserInfo.FormCreatorPage) {
                    CurrentUserInfo.FormCreatorPage.saveFormCallback(res);
                }
                break;
            case 'getFormDataCallback':
                if (CurrentUserInfo.FormDataTable) {
                    CurrentUserInfo.FormDataTable.getFormDataCallback(res);
                }
                break;

            case 'getFormSingleCallback':
                if (CurrentUserInfo.FormCreatorPage) {
                    CurrentUserInfo.FormCreatorPage.getFormSingleCallback({Content:res.Content.form});
                }

                /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
                if (FormShowerInChatHolder) {

                    let FormShowerInChat=  FormShowerInChatHolder[res.Content.form.Id+''+res.Content.UniqId];

                    if (FormShowerInChat){
                        FormShowerInChat.getFormSingleCallback({Content:res.Content.form});
                    }
                }

                break;
            case 'customerGetFormSingleCallback':
                debugger
                if (CurrentUserInfo.FormCreatorPage) {
                    CurrentUserInfo.FormCreatorPage.getFormSingleCallback(res);
                }

                /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
                if (FormShowerInChatHolder) {
                 
                  let FormShowerInChat=  FormShowerInChatHolder[res.Content.form.Id+''+res.Content.UniqId];
                
                  if (FormShowerInChat){
                      FormShowerInChat.getFormSingleCallback({Content:res.Content.form});
                  }
                }

                break;

            case 'customerStartTypingCallback':

                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.customerStartTypingCallback(res)
                }

                if (CurrentUserInfo.MyHeader) {
                    CurrentUserInfo.MyHeader.customerStartTypingCallback(res)
                }


                if (CurrentUserInfo.CustomersPage) {
                    CurrentUserInfo.CustomersPage.customerStartTypingCallback(res)
                }


                break;

            case 'getCreatedFormsCallback':
                if (CurrentUserInfo.CreatedForms) {
                    CurrentUserInfo.CreatedForms.getCreatedFormsCallback(res)
                }

                if (CurrentUserInfo.CustomerToolbar) {
                    CurrentUserInfo.CustomerToolbar.getCreatedFormsCallback(res);
                }

                break;

            case "getSocialChannelsInfoCallback":

                if (CurrentUserInfo.SocialChannelsPage) {
                    CurrentUserInfo.SocialChannelsPage.getSocialChannelsInfoCallback(res)
                }
                break;


            case 'saveSocialChannelsInfoCallback':
                if (CurrentUserInfo.SocialChannelsPage) {
                    CurrentUserInfo.SocialChannelsPage.saveSocialChannelsInfoCallback(res)
                }

                break;
            case "customerStopTypingCallback":

                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.customerStopTypingCallback(res)
                }

                if (CurrentUserInfo.MyHeader) {
                    CurrentUserInfo.MyHeader.customerStopTypingCallback(res)
                }

                if (CurrentUserInfo.CustomersPage) {
                    CurrentUserInfo.CustomersPage.customerStopTypingCallback(res)
                }


                break;
            case "forwardChatSuccessCallback":
                CurrentUserInfo.ForwardChat.forwardChatSuccessCallback(res);
                break;

            case "GetAdminsListCallback":
                CurrentUserInfo.AdminsPage.GetAdminsListCallback(res);
                break;

            case "getAutomaticSendChatsSocketHandlerCallback":

                CurrentUserInfo.AutomaticSendPage.getAutomaticSendChatsSocketHandlerCallback(res);
                break;
            case "successCallback":
                CurrentUserInfo.LayoutPage.showMsg(res.Message);
                CurrentUserInfo.AutomaticSendPage.successCallback(res.Message);

                break;


            case "ClearCookie":


                //cookieManager.removeItem('customerToken')
                cookieManager.removeItem('adminToken')
                console.log('ClearCookie==>adminToken===>', cookieManager.getItem('adminToken'))
                CurrentUserInfo.customerToken = null;

                CurrentUserInfo.LayoutPage.setState({tmp: Math.random(), isClearCookie: true})

                //window.location.reload();
                break;
            case "newSendPMByMeInAnotherPlaceCallback":
                CurrentUserInfo.ChatPage.newSendPMByMeInAnotherPlaceCallback(res);
                break;
              
            case "readChatCallback":
                CurrentUserInfo.ChatPage.readChatCallback(res);
                CurrentUserInfo.ChatPage.LoadForms(res);
                
                break;
            case "adminLoginCallback":
                CurrentUserInfo.LoginPage.adminLoginCallback(res);
                break;

            case "getClientsListForAdminCallback":
                // CurrentUserInfo.CustomersPage.getClientsListForAdminCallback(res);


                if (!DataHolder.currentPage) {
                    if (CurrentUserInfo.CustomersPage) {
                        CurrentUserInfo.CustomersPage.getClientsListForAdminCallback(res);
                    }

                } else {

                    if (CurrentUserInfo.OnlineCustomerListHolder) {
                        CurrentUserInfo.OnlineCustomerListHolder.getClientsListForAdminCallback(res);
                    }
                }


                /*else{
 
                 CurrentUserInfo.LayoutPage.showError('dispatcher CurrentUserInfo.OnlineCustomerListHolder is null');
 
                }*/
                break;
            case "adminSendToCustomerCallback":
                //CurrentUserInfo.ChatPage.adminSendToCustomerCallback(res);
                break;
            case "adminSendToCustomerFailCallback":
                if (res.Message)
                    CurrentUserInfo.LayoutPage.showError(res.Message);
                // CurrentUserInfo.plugin.adminSendToCustomerFailCallback(res);
                break;

            case "customerSendToAdminCallback":
                if (CurrentUserInfo.ChatPage && !DataHolder.currentPage)
                    CurrentUserInfo.ChatPage.customerSendToAdminCallback(res);

                if (CurrentUserInfo.CustomersPage)
                    CurrentUserInfo.CustomersPage.customerSendToAdminCallback(res);

                if (CurrentUserInfo.OnlineCustomerListHolder)
                    CurrentUserInfo.OnlineCustomerListHolder.customerSendToAdminCallback(res);


                break;
            case "msgDeliveredCallback":
                CurrentUserInfo.ChatPage.msgDeliveredCallback(res);
                break;
            case "multimediaPmSendCallback":
                CurrentUserInfo.ChatPage.multimediaPmSendCallback(res);


                if (CurrentUserInfo.OnlineCustomerListHolder)
                    CurrentUserInfo.OnlineCustomerListHolder.multimediaPmSendCallback(res);

                break;
            case "multimediaDeliveredCallback":
                CurrentUserInfo.ChatPage.multimediaDeliveredCallback(res);
                break;



            //new Accont or Customer
            case "newAccountOnlineCallback":
                CurrentUserInfo.AdminsPage.newAccountOnlineCallback(res);
                break;

            case "newCustomerOnlineCallback":
                //todo:server add coiunt of new  users
                if (CurrentUserInfo.CustomersPage) {
                    CurrentUserInfo.CustomersPage.newCustomerOnlineCallback(res);

                }


                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.newCustomerOnlineCallback(res);

                }
                break;

            //end


            case 'totalUserCountsChangedCallback':

                CurrentUserInfo.SubMenu.totalUserCountsChangedCallback(res);
                CurrentUserInfo.Menu.totalUserCountsChangedCallback(res);

                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.totalUserCountsChangedCallback(res);

                }
                if (CurrentUserInfo.CustomersPage) {
                    CurrentUserInfo.CustomersPage.totalUserCountsChangedCallback(res)
                }


                break;

            //customer online again
            case "customerOnlineAgainCallback":
                CurrentUserInfo.CustomersPage.customerOnlineAgainCallback(res);

                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.customerOnlineAgainCallback(res);

                }

                //   CurrentUserInfo.plugin.customerOnlineAgainCallback(res);
                break;
            case "customerOfflineAgainCallback":
                CurrentUserInfo.CustomersPage.customerOfflineAgainCallback(res);
                //  CurrentUserInfo.plugin.customerOfflineAgainCallback(res);

                if (CurrentUserInfo.OnlineCustomerListHolder) {
                    CurrentUserInfo.OnlineCustomerListHolder.customerOfflineAgainCallback(res);

                }
                break;
            //end

            //admin online again
            case "adminOnlineAgainCallback":
                CurrentUserInfo.AdminsPage.adminOnlineAgainCallback(res);
                break;
            case "adminOfflineAgainCallback":
                CurrentUserInfo.AdminsPage.adminOfflineAgainCallback(res);
                break;
            //end


            case "getCustomerActivityDetailCallback":
                CurrentUserInfo.CustomersPage.getCustomerActivityDetailCallback(res);
                break;


            case "searchHandlerCallback":
                /* customerlist,
                                    sendMsgList,
                                    receiveMsgList*/
                ;
                CurrentUserInfo.CustomersPage.searchHandlerCallback(res.Content.customerlist);
                //CurrentUserInfo.AdminsPage.searchHandlerCallback(res.customerlist);
                CurrentUserInfo.ChatPage.searchHandlerCallback(res.Content.sendMsgList, res.Content.receiveMsgList);
                break;

            case "loadReadyPmCallback":
                CurrentUserInfo.plugin.loadReadyPmCallback(res);
                break;
            case "DeleteMessageCallback":
                CurrentUserInfo.ChatPage.DeleteMessageCallback(res);
                break;
            case "EditMessageCallback":
                CurrentUserInfo.ChatPage.EditMessageCallback(res);
                break;
            case 'getVisitedPagesForCurrentSiteCallback':
                CurrentUserInfo.SepratePerPage.getVisitedPagesForCurrentSiteCallback(res);
                break;

            case"getAllTagsForCurrentAdminCallback":

                if (CurrentUserInfo.TagList) {
                    CurrentUserInfo.TagList.getAllTagsForCurrentAdminCallback(res);
                }

                break;

            case 'deleteTagFormUserTagsByIdCallback':

                if (CurrentUserInfo.CustomerTags) {
                    CurrentUserInfo.CustomerTags.deleteTagFormUserTagsByIdCallback(res);
                }

                break;
            case 'deleteTagByIdCallback':

                if (CurrentUserInfo.TagList) {
                    CurrentUserInfo.TagList.deleteTagByIdCallback(res);
                }

                break;

            case 'getMyProfileCallback':
                if (CurrentUserInfo.ProfilePage) {
                    CurrentUserInfo.ProfilePage.getMyProfileCallback(res);
                }
                break;
            case 'saveMyProfileCallback':
                if (CurrentUserInfo.ProfilePage) {
                    CurrentUserInfo.ProfilePage.saveMyProfileCallback(res);
                }
                break;
            case 'userAddedToTagsCallback':
                if (CurrentUserInfo.ChatPage) {
                    CurrentUserInfo.ChatPage.userAddedToTagsCallback(res);

                }


                // باعث loop می شود لذا if گذاشته ایم
                if (DataHolder.currentPage) {
                    if (CurrentUserInfo.OnlineCustomerListHolder) {
                        CurrentUserInfo.OnlineCustomerListHolder.userAddedToTagsCallback(res);
                    }
                }

                // CurrentUserInfo.LayoutPage.showMsg('برچسب های انتخاب شده به کاربر زده شد')
                break;

            case "getTagsCallback":
                if (CurrentUserInfo.TagList) {
                    CurrentUserInfo.TagList.getTagsCallback(res);
                }
                break;
            default:
                if (res && res.Message) {

                    console.error(res.Message);

                    CurrentUserInfo.LayoutPage.showError(res.Message);


                    if (res.Message.indexOf('کانکشکن متفاوت') >= 0) {
                        /*cookieManager.removeItem('customerToken')
                        cookieManager.removeItem('adminToken')
                        CurrentUserInfo.customerToken=null;
                        _currentAdminInfo.adminToken=null;*/

                        /*   if (debugMode){
                               alert('اتصال مجدد')
                           }
                           cookieManager.removeItem('customerToken')
                           cookieManager.removeItem('adminToken')
                           CurrentUserInfo.customerToken=null;
                           _currentAdminInfo.adminToken=null;
                           startUp();*/
                    }
                }
                break;

        }

    }

}

export const _dispatcher = new dispatcher();


