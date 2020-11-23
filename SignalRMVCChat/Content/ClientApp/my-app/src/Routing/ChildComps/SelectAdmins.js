import React, {Component} from 'react';
import {MyCaller,CurrentUserInfo} from "../../Help/Socket";
import {MultiSelect} from "primereact/multiselect";
import MyMultiSelect from "./MyMultiSelect";

class SelectAdmins extends MyMultiSelect {
    state = {
        propName: 'admins',
        label:'انتخاب اوپراتور ها',
        optionLabel:'Name'
    }

    componentDidMount() {
        
        CurrentUserInfo.SelectAdmins=this;
        
        MyCaller.Send('GetAdminsList')
    }

    GetAdminsListCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList || !res.Content.EntityList.length) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content.EntityList;

       this.makeList(arr);
    }
    
    compare(e1, e2): boolean {
        return  e1.Name==e2.Name;
    }


}

export default SelectAdmins;