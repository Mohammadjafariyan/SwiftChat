import {MyCaller} from '../../Help/Socket'
import {DataHolder} from "../../Help/DataHolder";
import {_showError, _showMsg} from "../../Pages/LayoutPage";

class _UserInformationService {


    SaveUserInfo(name, isAcceptNulls) {

        
        if (!DataHolder.selectedCustomer) {
            _showError("کاربری انتخاب نشده  است");
            return;
        }
        isAcceptNulls=isAcceptNulls? isAcceptNulls:false;

        let obj={};
        obj[name] = DataHolder.selectedCustomer[name];
        obj["Id"] = DataHolder.selectedCustomer["Id"];


        _showMsg("در حال ذخیره تغییرات ")

        if (isAcceptNulls) {
            MyCaller.Send('SaveUserInfo', {customer:DataHolder.selectedCustomer,isAcceptNulls,propertyName:name})
        } else {
            MyCaller.Send('SaveUserInfo', {customer:obj,isAcceptNulls,propertyName:name})
        }
    }

}

export const UserInformationService = new _UserInformationService();