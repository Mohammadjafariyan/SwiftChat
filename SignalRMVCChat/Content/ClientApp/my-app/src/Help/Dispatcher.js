import { cookieManager } from "./CookieManager";
import LoginPage from "./../Pages/LoginPage";
import { DataHolder } from "./DataHolder";
import { FormShowerInChatHolder } from "../Components/FormShowerInChat";
import React from "react";
import {CurrentUserInfo} from "../CurrentUserInfo";

class dispatcher {
  dispatch(res) {
    if (res.Type == -1) {
      //error
      console.error(res.Message);
    }
    console.log("dispatcher===>", res.Name);
    switch (res.Name) {
      /*-------------------- Voice Call--------------------*/

      case "vC_CustomerIsAcceptOrRejectCallback":
        if (CurrentUserInfo.VoiceCall) {
          CurrentUserInfo.VoiceCall.vC_CustomerIsAcceptOrRejectCallback(res);
        }
        break;
      case "vC_CustomerSpeakCallback":
        if (CurrentUserInfo.VoiceCall) {
          CurrentUserInfo.VoiceCall.vC_CustomerSpeakCallback(res);
        }
        break;

      case "vC_CustomerCallInitCallback":
        
        if (CurrentUserInfo.VoiceCall) {
          debugger;
          CurrentUserInfo.VoiceCall.componentDidMount();
          CurrentUserInfo.VoiceCall.vC_CustomerCallInitCallback(res);
        }
        break;

        
        
      /*-------------------- END--------------------*/

      /*-------------------- alarm--------------------*/
      case "alarmGetSoundsCallback":
        if (CurrentUserInfo.AlarmSetting) {
          CurrentUserInfo.AlarmSetting.alarmGetSoundsCallback(res);
        }
        break;

      case "alarmSetIsMuteSocketHandler":
        if (CurrentUserInfo.AlarmOnOff) {
          CurrentUserInfo.AlarmOnOff.alarmSetIsMuteSocketHandler(res);
        }
        break;
      /*-------------------- END--------------------*/

      /*-------------------- stats--------------------*/
      case "getTelegramBotSettingCallback":
        if (CurrentUserInfo.TelegramBotSave) {
          CurrentUserInfo.TelegramBotSave.getTelegramBotSettingCallback(res);
        }
        break;

      case "telegramBotSettingSaveCallback":
        if (CurrentUserInfo.TelegramBotSave) {
          CurrentUserInfo.TelegramBotSave.telegramBotSettingSaveCallback(res);
        }
        break;
      /*-------------------- END--------------------*/

      /*-------------------- stats--------------------*/
      case "saveEmailSettingCallback":
        if (CurrentUserInfo.EmailSetting) {
          CurrentUserInfo.EmailSetting.saveEmailSettingCallback(res);
        }
        break;
      case "getEmailSettingCallback":
        if (CurrentUserInfo.EmailSetting) {
          CurrentUserInfo.EmailSetting.getEmailSettingCallback(res);
        }
        break;
      case "saveEmailSettingErrorCallback":
        if (CurrentUserInfo.EmailSetting) {
          CurrentUserInfo.EmailSetting.saveEmailSettingErrorCallback(res);
        }
        break;

      /*-------------------- END--------------------*/

      /*-------------------- stats--------------------*/

      case "getVisitedPagesForCurrentSiteCallback":
        console.log("hi");
        if (CurrentUserInfo.StatLayout) {
          CurrentUserInfo.StatLayout.getVisitedPagesForCurrentSiteCallback(res);
        }

        if (CurrentUserInfo.SpecialStatsLayout) {
          CurrentUserInfo.SpecialStatsLayout.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.SepratePerPage) {
          CurrentUserInfo.SepratePerPage.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.PageVisitStat) {
          CurrentUserInfo.PageVisitStat.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.PageOnlineCount) {
          CurrentUserInfo.PageOnlineCount.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.StatPerState) {
          CurrentUserInfo.StatPerState.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.StatPerCity) {
          CurrentUserInfo.StatPerCity.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.StatInMap) {
          CurrentUserInfo.StatInMap.getVisitedPagesForCurrentSiteCallback(res);
        }

        if (CurrentUserInfo.StatForeignCountries) {
          CurrentUserInfo.StatForeignCountries.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }

        if (CurrentUserInfo.PageVisitStat) {
          CurrentUserInfo.PageVisitStat.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.PageOnlineCount) {
          CurrentUserInfo.PageOnlineCount.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.StatInMap) {
          CurrentUserInfo.StatInMap.getVisitedPagesForCurrentSiteCallback(res);
        }
        if (CurrentUserInfo.StatPerState) {
          CurrentUserInfo.StatPerState.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.StatPerCity) {
          CurrentUserInfo.StatPerCity.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.StatForeignCountries) {
          CurrentUserInfo.StatForeignCountries.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.MostExitUrlInSite) {
          CurrentUserInfo.MostExitUrlInSite.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.SiteViewsInWeek) {
          CurrentUserInfo.SiteViewsInWeek.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.SiteViewsInHoursOfToday) {
          CurrentUserInfo.SiteViewsInHoursOfToday.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.SiteViewsInMonths) {
          CurrentUserInfo.SiteViewsInMonths.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.SiteViewsInLast5Year) {
          CurrentUserInfo.SiteViewsInLast5Year.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (CurrentUserInfo.SiteViewsMostOnlineTime) {
          CurrentUserInfo.SiteViewsMostOnlineTime.getVisitedPagesForCurrentSiteCallback(
            res
          );
        }
        if (
          CurrentUserInfo.SiteVisit &&
          CurrentUserInfo.SiteVisit.getVisitedPagesForCurrentSiteCallback
        ) {
          CurrentUserInfo.SiteVisit.getVisitedPagesForCurrentSiteCallback(res);
        }
        break;
      /*-------------------- end--------------------*/

      /*-------------------- Compaign--------------------*/

      case "compaignManualExecuteCallback":
        if (CurrentUserInfo.CompaignTable) {
          CurrentUserInfo.CompaignTable.compaignManualExecuteCallback(res);
        }
        break;
      /*--------------------- compaignLog------------*/
      case "getCompaignLogListCallback":
        if (CurrentUserInfo.CompaignLogTable) {
          CurrentUserInfo.CompaignLogTable.getCompaignLogListCallback(res);
        }
        break;

      case "getCompaignLogReceiverListCallback":
        if (CurrentUserInfo.CompaignLogReceiverTable) {
          CurrentUserInfo.CompaignLogReceiverTable.getCallback(res);
        }
        break;

      /*--------------------- end------------*/

      case "getCompaignTemplatesCallback":
        if (CurrentUserInfo.CompaignTemplates) {
          CurrentUserInfo.CompaignTemplates.getCompaignTemplatesCallback(res);
        }
        break;
      case "searchCustomersCallback":
        if (CurrentUserInfo.SelectCustomers) {
          CurrentUserInfo.SelectCustomers.searchCustomersCallback(res);
        }
        break;
      case "getCompaignListCallback":
        if (CurrentUserInfo.CompaignLayout) {
          CurrentUserInfo.CompaignLayout.getCallback(res);
        }

        if (CurrentUserInfo.CompaignTable) {
          CurrentUserInfo.CompaignTable.getCompaignListCallback(res);
        }

        break;
      case "compaignSaveCallback":
        if (CurrentUserInfo.CompaignSave) {
          CurrentUserInfo.CompaignSave.compaignSaveCallback(res);
        }

        if (CurrentUserInfo.CompaignLayout) {
          CurrentUserInfo.CompaignLayout.saveCallback(res);
          CurrentUserInfo.CompaignLayout.saveDraftCallback(res);
        }

        break;
      case "deleteCompaignCallback":
        if (CurrentUserInfo.CompaignLayout) {
          CurrentUserInfo.CompaignLayout.deleteCallback(res);
        }
        break;
      case "setIsEnabledCompaignCallback":
        if (CurrentUserInfo.CompaignLayout) {
          CurrentUserInfo.CompaignLayout.setIsEnabledCallback(res);
        }
        break;
      case "saveCompaignCallback":
        if (CurrentUserInfo.CompaignLayout) {
          CurrentUserInfo.CompaignLayout.saveCallback(res);
        }
        break;
      /*-------------------- end--------------------*/

      /*-------------------- Routing--------------------*/
      case "getRoutingListCallback":
        if (CurrentUserInfo.RoutingLayout) {
          CurrentUserInfo.RoutingLayout.getCallback(res);
        }
        break;
      case "routingSaveCallback":
        if (CurrentUserInfo.RoutingSave) {
          CurrentUserInfo.RoutingSave.routingSaveCallback(res);
        }

        if (CurrentUserInfo.RoutingLayout) {
          CurrentUserInfo.RoutingLayout.saveCallback(res);
          CurrentUserInfo.RoutingLayout.saveDraftCallback(res);
        }

        break;
      case "deleteRoutingCallback":
        if (CurrentUserInfo.RoutingLayout) {
          CurrentUserInfo.RoutingLayout.deleteCallback(res);
        }
        break;
      case "setIsEnabledRoutingCallback":
        if (CurrentUserInfo.RoutingLayout) {
          CurrentUserInfo.RoutingLayout.setIsEnabledCallback(res);
        }
        break;
      case "saveRoutingCallback":
        if (CurrentUserInfo.RoutingLayout) {
          CurrentUserInfo.RoutingLayout.saveCallback(res);
        }
        break;

      /*-------------------- End--------------------*/

      /*-------------------- Tracking--------------------*/
      case "getContactsCallback":
        if (CurrentUserInfo.ContactsIndex) {
          CurrentUserInfo.ContactsIndex.getContactsCallback(res);
        }
      /*-------------------- End--------------------*/

      /*-------------------- Tracking--------------------*/
      case "getCustomerTrackingInfoDetailCallback":
        if (CurrentUserInfo.CustomerTimeSpent) {
          CurrentUserInfo.CustomerTimeSpent.getCustomerTrackingInfoDetailCallback(
            res
          );
        }

        if (CurrentUserInfo.CustomerProfile) {
          CurrentUserInfo.CustomerProfile.getCustomerTrackingInfoDetailCallback(
            res
          );
        }
        break;

      /*-------------------- End--------------------*/

      /*-------------------- BOT LOG--------------------*/
      case "getBotLogSingleCallback":
        if (CurrentUserInfo.BotLogLayout) {
          CurrentUserInfo.BotLogLayout.getBotLogSingleCallback(res);
        }
        break;

      case "getBotListCallback":
        if (CurrentUserInfo.BotLogForm) {
          CurrentUserInfo.BotLogForm.getBotListCallback(res);
        }

        if (CurrentUserInfo.CompaignConditions) {
          CurrentUserInfo.CompaignConditions.botListCallback(res);
        }

        break;
      case "getBotLogListCallback":
        if (CurrentUserInfo.BotLogTable) {
          CurrentUserInfo.BotLogTable.getBotLogListCallback(res);
        }
        break;

      /*-------------------- BOT--------------------*/
      case "botSaveCallback":
        if (CurrentUserInfo.BotDesign) {
          CurrentUserInfo.BotDesign.botSaveCallback(res);
        }
        if (CurrentUserInfo.BotList) {
          CurrentUserInfo.BotList.botSaveCallback(res);
        }
        break;
      case "botListCallback":
        if (CurrentUserInfo.BotList) {
          CurrentUserInfo.BotList.botListCallback(res);
        }
        break;
      case "botDeleteCallback":
        if (CurrentUserInfo.BotDesign) {
          CurrentUserInfo.BotDesign.botDeleteCallback(res);
        }
        break;

      /*--------------------END--------------------*/

      /*-------------------- REMIND ME--------------------*/
      case "remindMeFireCallback":
        if (CurrentUserInfo.B4AdminNavbar) {
          CurrentUserInfo.B4AdminNavbar.remindMeFireCallback(res);
        }

        break;

      /*--------------------END--------------------*/
      /*-------------------- PRIVATE NOTE--------------------*/
      case "adminPrivateNoteSendToAdminCallback":
        if (CurrentUserInfo.B4AdminNavbar) {
          CurrentUserInfo.B4AdminNavbar.adminPrivateNoteSendToAdminCallback(
            res
          );
        }

        if (CurrentUserInfo.ChatPage) {
          CurrentUserInfo.ChatPage.adminPrivateNoteSendToAdminCallback(res);
        }
        break;

      /*--------------------END--------------------*/

      /*-------------------- Remind Me--------------------*/
      case "deleteRemindMeCallback":
        if (CurrentUserInfo.RemindMe) {
          CurrentUserInfo.RemindMe.deleteRemindMeCallback(res);
        }
        break;

      case "getRemindMeListCallback":
        if (CurrentUserInfo.RemindMe) {
          CurrentUserInfo.RemindMe.getRemindMeListCallback(res);
        }
        break;
      case "saveRemindMeCallback":
        if (CurrentUserInfo.RemindMe) {
          CurrentUserInfo.RemindMe.saveRemindMeCallback(res);
        }
        break;

      /*--------------------END--------------------*/

      /*-------------------- Ready Pm--------------------*/
      case "getReadyPmsListCallback":
        if (CurrentUserInfo.ReadyPms) {
          CurrentUserInfo.ReadyPms.getReadyPmsListCallback(res);
        }
        break;

      case "removeReadyPmCallback":
        if (CurrentUserInfo.ReadyPms) {
          CurrentUserInfo.ReadyPms.removeReadyPmCallback(res);
        }
        break;
      case "saveReadyPmsCallback":
        if (CurrentUserInfo.ReadyPms) {
          CurrentUserInfo.ReadyPms.saveReadyPmsCallback(res);
        }
        break;

      /*--------------------END--------------------*/

      /* ===============Users BLOCK =============== */
      case "changeCustomerBlockStatusCallback":
        if (CurrentUserInfo.BlockUser) {
          CurrentUserInfo.BlockUser.changeCustomerBlockStatusCallback(res);
        }
        break;

      /* =============== END =============== */

      /* ===============Users Separation =============== */
      case "getUsersSeparationFormCallback":
        if (CurrentUserInfo.UsersSeparationForm) {
          CurrentUserInfo.UsersSeparationForm.getUsersSeparationFormCallback(
            res
          );
        }
        break;
      case "saveUsersSeparationFormCallback":
        if (CurrentUserInfo.UsersSeparationForm) {
          CurrentUserInfo.UsersSeparationForm.saveUsersSeparationFormCallback(
            res
          );
        }
        break;

      /* =============== END =============== */
      /* =============== live Assist =============== */
      case "liveAssistSendDocCallback":
        if (CurrentUserInfo.LiveAssistShower) {
          CurrentUserInfo.LiveAssistShower.liveAssistSendDocCallback(res);
        }
        break;

      case "liveAssistFireEventCallback":
        if (CurrentUserInfo.LiveAssistShower) {
          CurrentUserInfo.LiveAssistShower.liveAssistFireEventCallback(res);
        }
        break;

      /*========== eventTrigger==========*/
      case "eventTriggerGetAllCallback":
        if (CurrentUserInfo.EventTriggerIndex) {
          CurrentUserInfo.EventTriggerIndex.eventTriggerGetAllCallback(res);
        }

        if (CurrentUserInfo.CompaignConditions) {
          CurrentUserInfo.CompaignConditions.eventTriggerGetAllCallback(res);
        }
        break;

      case "eventTriggerSaveCallback":
        if (CurrentUserInfo.EventTriggerIndex) {
          CurrentUserInfo.EventTriggerIndex.eventTriggerSaveCallback(res);
        }
        break;

      case "eventTriggerDeleteCallback":
        if (CurrentUserInfo.EventTriggerIndex) {
          CurrentUserInfo.EventTriggerIndex.eventTriggerDeleteCallback(res);
        }
        break;

      /*===========eventTrigger end============*/

      /*========== rating==========*/

      case "getRatingCallback":
        if (CurrentUserInfo.Satistification) {
          CurrentUserInfo.Satistification.getRatingCallback(res);
        }
        break;
      /*===========rating end============*/

      /*========== rating==========*/

      case "getLastVisitedPagesCallback":
        if (CurrentUserInfo.LastPagesVisited) {
          CurrentUserInfo.LastPagesVisited.getLastVisitedPagesCallback(res);
        }
        break;
      /*========== END==========*/

      /*===========customer data===============*/
      case "saveKeyCallback":
        if (CurrentUserInfo.CustomerData) {
          CurrentUserInfo.CustomerData.saveKeyCallback(res);
        }
        break;

      case "deleteKeyCallback":
        if (CurrentUserInfo.CustomerData) {
          CurrentUserInfo.CustomerData.deleteKeyCallback(res);
        }
        break;

      case "getCustomerDataListCallback":
        if (CurrentUserInfo.CustomerData) {
          CurrentUserInfo.CustomerData.getCustomerDataListCallback(res);
        }
        break;

      case "saveUserInfoCallback":
        if (CurrentUserInfo.CustomerInfo) {
          CurrentUserInfo.CustomerInfo.saveUserInfoCallback(res);
        }
        break;

      /*============= customer data end =============*/

      /*=============language=============*/

      case "selectHelpDeskCallback":
        if (CurrentUserInfo.DefinedLanguages) {
          CurrentUserInfo.DefinedLanguages.selectHelpDeskCallback(res);
        }

        break;

      case "removeHelpDeskCallback":
        if (CurrentUserInfo.RemoveLanguage) {
          CurrentUserInfo.RemoveLanguage.removeHelpDeskCallback(res);
        }
        break;

      case "language_GetCurrentHelpDesk_SelectedLanguageCallback":
        if (CurrentUserInfo.LanguageHolder) {
          CurrentUserInfo.LanguageHolder.language_GetCurrentHelpDesk_SelectedLanguageCallback(
            res
          );
        }

        if (CurrentUserInfo.HelpDeskPage) {
          CurrentUserInfo.HelpDeskPage.language_GetCurrentHelpDesk_SelectedLanguageCallback(
            res
          );
        }

        if (CurrentUserInfo.GoToHelpCenter) {
          CurrentUserInfo.GoToHelpCenter.language_GetCurrentHelpDesk_SelectedLanguageCallback(
            res
          );
        }

        break;
      case "create_HelpDesk_Callback":
        if (CurrentUserInfo.LanguageHolder) {
          CurrentUserInfo.LanguageHolder.create_HelpDesk_Callback(res);
        }

        break;
      case "getDefinedLanguagesCallback":
        if (CurrentUserInfo.DefinedLanguages) {
          CurrentUserInfo.DefinedLanguages.getDefinedLanguagesCallback(res);
        }

        break;

      /*===============language END============*/

      /*======= helpDeskSetting==========*/

      case "helpDeskGetByIdCallback":
        if (CurrentUserInfo.HelpDeskSetting) {
          CurrentUserInfo.HelpDeskSetting.helpDeskGetByIdCallback(res);
        }

        break;
      case "helpDeskSaveDetailCallback":
        if (CurrentUserInfo.SettingLayout) {
          CurrentUserInfo.SettingLayout.helpDeskSaveDetailCallback(res);
        }

        if (CurrentUserInfo.HelpDeskSetting) {
          CurrentUserInfo.HelpDeskSetting.helpDeskSaveDetailCallback(res);
        }

        break;

      /*=============MyWebsiteSetting===========*/
      case "getMyWebsiteSettingCallback":
        if (CurrentUserInfo.SettingLayout) {
          CurrentUserInfo.SettingLayout.getMyWebsiteSettingCallback(res);
        }
        break;

      case "saveMyWebsiteSettingCallback":
        if (CurrentUserInfo.SettingBody) {
          CurrentUserInfo.SettingBody.saveMyWebsiteSettingCallback(res);
        }
        break;

      /*=============end===========*/

      /*======= helpDeskSettin
                  
                  
                  
                   */
      /*=============article===========*/

      case "helpdeskFeedbackForArticlesCallback":
        if (CurrentUserInfo.Comments) {
          CurrentUserInfo.Comments.helpdeskFeedbackForArticlesCallback(res);
        }
      case "articleSaveCallback":
        if (CurrentUserInfo.SaveArticle) {
          CurrentUserInfo.SaveArticle.articleSaveCallback(res);
        }

        break;
      case "articleGetByIdCallback":
        if (CurrentUserInfo.SaveArticle) {
          CurrentUserInfo.SaveArticle.articleGetByIdCallback(res);
        }

        break;
      case "articleDeleteByIdCallback":
        if (CurrentUserInfo.HelpDeskPage) {
          CurrentUserInfo.HelpDeskPage.articleDeleteByIdCallback(res);
        }

        break;

      /*============article END=============*/

      /*===========category===================*/

      case "category_GetById_Callback":
        if (CurrentUserInfo.CategorySave) {
          CurrentUserInfo.CategorySave.category_GetById_Callback(res);
        }
      case "category_Get_List_Callback":
        if (CurrentUserInfo.CategoryIndex) {
          CurrentUserInfo.CategoryIndex.category_Get_List_Callback(res);
        }

        if (CurrentUserInfo.SaveArticle) {
          CurrentUserInfo.SaveArticle.category_Get_List_Callback(res);
        }

        break;
      case "category_Save_Callback":
        if (CurrentUserInfo.CategorySave) {
          CurrentUserInfo.CategorySave.category_Save_Callback(res);
        }

        break;
      case "category_Delete_Callback":
        if (CurrentUserInfo.CategoryDelete) {
          CurrentUserInfo.CategoryDelete.category_Delete_Callback(res);
        }

        break;

      /*===========category END===================*/

      case "screenRecordAdminShareRequestCallback":
        if (CurrentUserInfo.ShowMyScreen) {
          CurrentUserInfo.ShowMyScreen.screenRecordAdminShareRequestCallback(
            res
          );
        }

        break;

      case "screenRecordCustomerCloseCallback":
        if (CurrentUserInfo.ScreenRecordShowerSendRequest) {
          CurrentUserInfo.ScreenRecordShowerSendRequest.screenRecordCustomerCloseCallback(
            res
          );
        }

        break;

      case "screenRecordAccessRequestSuccessCallback":
        if (CurrentUserInfo.ScreenRecordShowerSendRequest) {
          CurrentUserInfo.ScreenRecordShowerSendRequest.screenRecordAccessRequestSuccessCallback(
            res
          );
        }

        break;
      case "screenRecordAccessRequestFailCallback":
        if (CurrentUserInfo.ScreenRecordShowerSendRequest) {
          CurrentUserInfo.ScreenRecordShowerSendRequest.screenRecordAccessRequestFailCallback(
            res
          );
        }

        break;
      case "screenRecordSaveCallback":
        if (CurrentUserInfo.ScreenRecordShower) {
          CurrentUserInfo.ScreenRecordShower.screenRecordSaveCallback(res);
        }

        break;

      case "saveFormDataCallback":
        /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
        if (FormShowerInChatHolder) {
          /*form.AfterMessage,
                                          formId,
                                          chatId,
                                          FormValues*/

          let FormShowerInChat =
            FormShowerInChatHolder[
              res.Content.formId + "" + res.Content.UniqId
            ];

          if (FormShowerInChat) {
            FormShowerInChat.saveFormDataCallback(res);
          }
        }
        break;

      case "deleteFormCallback":
        if (CurrentUserInfo.FormCreatorPage) {
          CurrentUserInfo.FormCreatorPage.deleteFormCallback(res);
        }
        break;
      case "saveFormCallback":
        if (CurrentUserInfo.FormCreatorPage) {
          CurrentUserInfo.FormCreatorPage.saveFormCallback(res);
        }
        break;
      case "getFormDataCallback":
        if (CurrentUserInfo.FormDataTable) {
          CurrentUserInfo.FormDataTable.getFormDataCallback(res);
        }
        break;

      case "getFormSingleCallback":
        if (CurrentUserInfo.FormCreatorPage) {
          CurrentUserInfo.FormCreatorPage.getFormSingleCallback({
            Content: res.Content.form,
          });
        }

        /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
        if (FormShowerInChatHolder) {
          let FormShowerInChat =
            FormShowerInChatHolder[
              res.Content.form.Id + "" + res.Content.UniqId
            ];

          if (FormShowerInChat) {
            FormShowerInChat.getFormSingleCallback({
              Content: res.Content.form,
            });
          }
        }

        break;
      case "customerGetFormSingleCallback":
        //       debugger
        if (CurrentUserInfo.FormCreatorPage) {
          CurrentUserInfo.FormCreatorPage.getFormSingleCallback(res);
        }

        /*لیست کامپونت های فرم ها در این ابجکت نگه داری می شود|*/
        if (FormShowerInChatHolder) {
          let FormShowerInChat =
            FormShowerInChatHolder[
              res.Content.form.Id + "" + res.Content.UniqId
            ];

          if (FormShowerInChat) {
            FormShowerInChat.getFormSingleCallback({
              Content: res.Content.form,
            });
          }
        }

        break;

      case "customerStartTypingCallback":
        if (CurrentUserInfo.OnlineCustomerListHolder) {
          CurrentUserInfo.OnlineCustomerListHolder.customerStartTypingCallback(
            res
          );
        }

        if (CurrentUserInfo.MyHeader) {
          CurrentUserInfo.MyHeader.customerStartTypingCallback(res);
        }

        if (CurrentUserInfo.CustomersPage) {
          CurrentUserInfo.CustomersPage.customerStartTypingCallback(res);
        }

        if (CurrentUserInfo.ChatPage) {
          CurrentUserInfo.ChatPage.customerStartTypingCallback(res);
        }

        break;

      case "getCreatedFormsCallback":
        if (CurrentUserInfo.CreatedForms) {
          CurrentUserInfo.CreatedForms.getCreatedFormsCallback(res);
        }

        if (CurrentUserInfo.FormSelect) {
          CurrentUserInfo.FormSelect.getCreatedFormsCallback(res);
        }

        if (CurrentUserInfo.CustomerToolbar) {
          CurrentUserInfo.CustomerToolbar.getCreatedFormsCallback(res);
        }

        if (CurrentUserInfo.BotEventNodeSetting) {
          CurrentUserInfo.BotEventNodeSetting.getCreatedFormsCallback(res);
        }

        if (CurrentUserInfo.BotEventAction) {
          CurrentUserInfo.BotEventAction.getCreatedFormsCallback(res);
        }

        break;

      case "getSocialChannelsInfoCallback":
        if (CurrentUserInfo.SocialChannelsPage) {
          CurrentUserInfo.SocialChannelsPage.getSocialChannelsInfoCallback(res);
        }

        if (CurrentUserInfo.SendFromHelpDesk) {
          CurrentUserInfo.SendFromHelpDesk.getSocialChannelsInfoCallback(res);
        }
        break;

      case "saveSocialChannelsInfoCallback":
        if (CurrentUserInfo.SocialChannelsPage) {
          CurrentUserInfo.SocialChannelsPage.saveSocialChannelsInfoCallback(
            res
          );
        }

        break;
      case "customerStopTypingCallback":
        if (CurrentUserInfo.CustomersPage) {
          CurrentUserInfo.CustomersPage.customerStopTypingCallback(res);
        }

        if (CurrentUserInfo.OnlineCustomerListHolder) {
          CurrentUserInfo.OnlineCustomerListHolder.customerStopTypingCallback(
            res
          );
        }

        if (CurrentUserInfo.MyHeader) {
          CurrentUserInfo.MyHeader.customerStopTypingCallback(res);
        }

        if (CurrentUserInfo.ChatPage) {
          CurrentUserInfo.ChatPage.customerStopTypingCallback(res);
        }

        break;
      case "forwardChatSuccessCallback":
        CurrentUserInfo.ForwardChat.forwardChatSuccessCallback(res);
        break;

      case "GetAdminsListCallback":
        if (CurrentUserInfo.AdminsPage) {
          CurrentUserInfo.AdminsPage.GetAdminsListCallback(res);
        }

        if (CurrentUserInfo.SelectAdmin) {
          CurrentUserInfo.SelectAdmin.GetAdminsListCallback(res);
        }

        if (CurrentUserInfo.BotEventAction) {
          CurrentUserInfo.BotEventAction.GetAdminsListCallback(res);
        }

        if (CurrentUserInfo.SelectAdmins) {
          CurrentUserInfo.SelectAdmins.GetAdminsListCallback(res);
        }

        break;

      case "getAutomaticSendChatsSocketHandlerCallback":
        CurrentUserInfo.AutomaticSendPage.getAutomaticSendChatsSocketHandlerCallback(
          res
        );
        break;
      case "successCallback":
        CurrentUserInfo.LayoutPage.showMsg(res.Message);
        CurrentUserInfo.AutomaticSendPage.successCallback(res.Message);

        break;

      case "ClearCookie":
        //cookieManager.removeItem('customerToken')
        cookieManager.removeItem("adminToken");
        console.log(
          "ClearCookie==>adminToken===>",
          cookieManager.getItem("adminToken")
        );
        CurrentUserInfo.customerToken = null;

        CurrentUserInfo.LayoutPage.setState({
          tmp: Math.random(),
          isClearCookie: true,
        });

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

      case "getDefinedFormInputsCallback":
        if (CurrentUserInfo.BotEventCondition) {
          CurrentUserInfo.BotEventCondition.getDefinedFormInputsCallback(res);
        }
        if (CurrentUserInfo.BotEventNodeSetting) {
          CurrentUserInfo.BotEventNodeSetting.getDefinedFormInputsCallback(res);
        }
        break;

      case "getClientsListForAdminCallback":
        // CurrentUserInfo.CustomersPage.getClientsListForAdminCallback(res);
        if (!DataHolder.currentPage) {
          if (CurrentUserInfo.CustomersPage) {
            CurrentUserInfo.CustomersPage.getClientsListForAdminCallback(res);
          }
        } else {
          if (CurrentUserInfo.OnlineCustomerListHolder) {
            CurrentUserInfo.OnlineCustomerListHolder.getClientsListForAdminCallback(
                res
            );
          }

          if (DataHolder.currentPage === "Map") {
            /*  if (CurrentUserInfo.CustomersPage) {
              CurrentUserInfo.CustomersPage.getClientsListForAdminCallback(res);
            } */

            if (CurrentUserInfo.MyMapHolder) {
              CurrentUserInfo.MyMapHolder.getClientsListForAdminCallback(res);
            }
          }
        }
          
          
        if (CurrentUserInfo.CustomersPaging) {
          CurrentUserInfo.CustomersPaging.getClientsListForAdminCallback(res);
        }
        if (CurrentUserInfo.MyMapCustomerTypes) {
          CurrentUserInfo.MyMapCustomerTypes.getClientsListForAdminCallback(
            res
          );
        }

       

        /*else{
         
                         CurrentUserInfo.LayoutPage.showError('dispatcher CurrentUserInfo.OnlineCustomerListHolder is null');
         
                        }*/
        break;
      case "adminSendToCustomerCallback":
        //CurrentUserInfo.ChatPage.adminSendToCustomerCallback(res);
        break;
      case "adminSendToCustomerFailCallback":
        if (res.Message) CurrentUserInfo.LayoutPage.showError(res.Message);
        // CurrentUserInfo.plugin.adminSendToCustomerFailCallback(res);
        break;

      case "customerSendToAdminCallback":
        // if (CurrentUserInfo.ChatPage && !DataHolder.currentPage)

        if (CurrentUserInfo.ChatPage)
          CurrentUserInfo.ChatPage.customerSendToAdminCallback(res);

        if (CurrentUserInfo.CustomersPage)
          CurrentUserInfo.CustomersPage.customerSendToAdminCallback(res);

        if (CurrentUserInfo.OnlineCustomerListHolder)
          CurrentUserInfo.OnlineCustomerListHolder.customerSendToAdminCallback(
            res
          );

        if (CurrentUserInfo.Alarm) {
          CurrentUserInfo.Alarm.togglePlay();
        }
        break;
      case "msgDeliveredCallback":
        CurrentUserInfo.ChatPage.msgDeliveredCallback(res);
        break;
      case "multimediaPmSendCallback":
        CurrentUserInfo.ChatPage.multimediaPmSendCallback(res);

        if (CurrentUserInfo.OnlineCustomerListHolder)
          CurrentUserInfo.OnlineCustomerListHolder.multimediaPmSendCallback(
            res
          );

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
          CurrentUserInfo.OnlineCustomerListHolder.newCustomerOnlineCallback(
            res
          );
        }
        break;

      //end

      case "totalUserCountsChangedCallback":
        if (CurrentUserInfo.SubMenu)
          CurrentUserInfo.SubMenu.totalUserCountsChangedCallback(res);
        CurrentUserInfo.B4AdminSubMenu.totalUserCountsChangedCallback(res);

        if (CurrentUserInfo.Menu)
          CurrentUserInfo.Menu.totalUserCountsChangedCallback(res);
        CurrentUserInfo.B4AdminMainMenu.totalUserCountsChangedCallback(res);

        if (CurrentUserInfo.B4AdminNavbar)
          CurrentUserInfo.B4AdminNavbar.totalUserCountsChangedCallback(res);

        if (CurrentUserInfo.OnlineCustomerListHolder) {
          CurrentUserInfo.OnlineCustomerListHolder.totalUserCountsChangedCallback(
            res
          );
        }
        if (CurrentUserInfo.CustomersPage) {
          CurrentUserInfo.CustomersPage.totalUserCountsChangedCallback(res);
        }

        break;

      //customer online again
      case "customerOnlineAgainCallback":
        CurrentUserInfo.CustomersPage.customerOnlineAgainCallback(res);

        if (CurrentUserInfo.OnlineCustomerListHolder) {
          CurrentUserInfo.OnlineCustomerListHolder.customerOnlineAgainCallback(
            res
          );
        }

        //   CurrentUserInfo.plugin.customerOnlineAgainCallback(res);
        break;
      case "customerOfflineAgainCallback":
        CurrentUserInfo.CustomersPage.customerOfflineAgainCallback(res);
        //  CurrentUserInfo.plugin.customerOfflineAgainCallback(res);

        if (CurrentUserInfo.OnlineCustomerListHolder) {
          CurrentUserInfo.OnlineCustomerListHolder.customerOfflineAgainCallback(
            res
          );
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
        CurrentUserInfo.CustomersPage.searchHandlerCallback(
          res.Content.customerlist
        );
        //CurrentUserInfo.AdminsPage.searchHandlerCallback(res.customerlist);
        CurrentUserInfo.ChatPage.searchHandlerCallback(
          res.Content.sendMsgList,
          res.Content.receiveMsgList
        );
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

      case "getAllTagsForCurrentAdminCallback":
        if (CurrentUserInfo.TagList) {
          CurrentUserInfo.TagList.getAllTagsForCurrentAdminCallback(res);
        }

        break;

      case "deleteTagFormUserTagsByIdCallback":
        if (CurrentUserInfo.CustomerTags) {
          CurrentUserInfo.CustomerTags.deleteTagFormUserTagsByIdCallback(res);
        }

        break;
      case "deleteTagByIdCallback":
        if (CurrentUserInfo.TagList) {
          CurrentUserInfo.TagList.deleteTagByIdCallback(res);
        }

        break;

      case "getMyProfileCallback":
        if (CurrentUserInfo.ProfilePage) {
          CurrentUserInfo.ProfilePage.getMyProfileCallback(res);
        }

        if (CurrentUserInfo.B4AdminLayout) {
          CurrentUserInfo.B4AdminLayout.getMyProfileCallback(res);
        }

        if (CurrentUserInfo.B4AdminNavbar) {
          CurrentUserInfo.B4AdminNavbar.getMyProfileCallback(res);
        }

        if (CurrentUserInfo.CurrentPlanInMenu) {
          CurrentUserInfo.CurrentPlanInMenu.getMyProfileCallback(res);
        }

        if (CurrentUserInfo.AlarmOnOff) {
          CurrentUserInfo.AlarmOnOff.getMyProfileCallback(res);
        }

        break;
      case "saveMyProfileCallback":
        if (CurrentUserInfo.ProfilePage) {
          CurrentUserInfo.ProfilePage.saveMyProfileCallback(res);
        }
        break;
      case "userAddedToTagsCallback":
        if (CurrentUserInfo.ChatPage) {
          CurrentUserInfo.ChatPage.userAddedToTagsCallback(res);
        }

        // باعث loop می شود لذا if گذاشته ایم
        if (DataHolder.currentPage) {
          if (CurrentUserInfo.OnlineCustomerListHolder) {
            CurrentUserInfo.OnlineCustomerListHolder.userAddedToTagsCallback(
              res
            );
          }
        }

        // CurrentUserInfo.LayoutPage.showMsg('برچسب های انتخاب شده به کاربر زده شد')
        break;

      case "getTagsCallback":
        if (CurrentUserInfo.TagList) {
          CurrentUserInfo.TagList.getTagsCallback(res);
        }

        if (CurrentUserInfo.SelectSegments) {
          CurrentUserInfo.SelectSegments.getTagsCallback(res);
        }

        break;
      default:
        if (res && res.Message) {
          console.error(res.Message);

          if (CurrentUserInfo.LoginPage) {
            CurrentUserInfo.LoginPage.setState({ loading: false });
          }
          CurrentUserInfo.LayoutPage.showError(res.Message);

          if (res.Message.indexOf("کانکشکن متفاوت") >= 0) {
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
