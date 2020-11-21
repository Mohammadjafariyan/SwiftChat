import React, {Component} from 'react';
import {CurrentUserInfo,MyCaller} from "../../Help/Socket";
import MyMultiSelect from "./MyMultiSelect";

class SelectSegments extends MyMultiSelect {
    state = {
        propName: 'segments'
    }


    componentDidMount() {

        CurrentUserInfo.SelectSegments = this;
        
        MyCaller.Send('GetTags')
    }


    getTagsCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError('دیتای خوانده شده برای برچسب ها نال است')
            return;
        }
        
        

        this.makeList(res.Content.EntityList);
    }

}

export default SelectSegments;