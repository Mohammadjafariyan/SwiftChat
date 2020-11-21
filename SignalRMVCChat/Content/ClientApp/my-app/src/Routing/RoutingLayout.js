import React, {Component} from 'react';
import BaseCrudLayout from "../CRUD/BaseCrudLayout";
import BaseIndex from "../CRUD/BaseIndex";
import RoutingSave from "./Manage/RoutingSave";
import {CurrentUserInfo} from "../Help/Socket";

class RoutingLayout extends BaseCrudLayout {
    state = {
        get: 'GetRoutingList',
        save: 'RoutingSave',
        delete: 'DeleteRouting',
        setIsEnabled: 'SetIsEnabledRouting',
        saveDraft: 'RoutingSave',
    };

    constructor(prp) {
        super(prp);

        CurrentUserInfo.RoutingLayout = this;
    }


    RenderWelcome() {
        return <p>سلام خوش آمدید</p>
    }

    RenderForm() {
        return <RoutingSave parent={this}/>
    }
}

export default RoutingLayout;