import { MyCaller } from "./Socket";
import {DataHolder} from "./DataHolder";
import {CurrentUserInfo} from "../CurrentUserInfo";

export function changeUserTypes(type) {

    CurrentUserInfo.UserType = type;

    switch (type) {

        case 'AllCustomerListPage':
            CurrentUserInfo.UserType = null;

            if(CurrentUserInfo.OnlineCustomerListHolder)
            CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();
            return;
            break;
        case 'answered':
            break;

        case "SeparatePerPageCustomerListPage":
            if (!DataHolder.SelectedPage){
                CurrentUserInfo.LayoutPage.showError("هیچ صفحه ای انتخاب نشده است");
                return;
            }

            break;
        case 'chatted':
            break;
        case 'NotChatted':
            break;
        case 'NotChattedLeft':
            break;
        case 'WaitingForAnswer':
            break;

            
        case 'GetVisitedPagesForCurrentSite':

            MyCaller.Send('GetVisitedPagesForCurrentSite')
            return;
            break;
        case 'ChattedAndReturnedCustomerListPage':
            break;

        case 'NotChattedLeftCustomerListPage':
            break;
        case 'AssingedToMe':
            break;

                
        default:
            alert(' نوع شناخته نشد ' + type);
            return;
            break;
    }

    if(CurrentUserInfo.OnlineCustomerListHolder)
    CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();
    else{
        CurrentUserInfo.LayoutPage.showError('CurrentUserInfo.OnlineCustomerListHolder is null');

    }
}