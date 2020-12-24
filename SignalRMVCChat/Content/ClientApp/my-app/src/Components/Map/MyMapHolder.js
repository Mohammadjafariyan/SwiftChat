import React, {Component} from 'react';
import MyMap from "./MyMap";
import {CurrentUserInfo} from "../../Help/Socket";
import { MyCaller } from './../../Help/Socket';

class MyMapHolder extends Component {

    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.MyMapHolder=this;
    }

    getClientsListForAdminCallback(res) {
        CurrentUserInfo.MapPage.setState({loading:false});

        this.setState({loading: false});

        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError('getClientsListForAdminCallback returns null');
            return;
        }

        var arr = [];
        arr = res.Content.EntityList;

        this.setState({users:arr});
    }


    componentDidMount(){

    CurrentUserInfo.selectedTagId = null;
    CurrentUserInfo.gapIsOnlyOnly = null;
    CurrentUserInfo.currentUsersIsAdmins = null;

    MyCaller.Send("GetClientsListForAdmin");
    }
    render() {
        return (
            <>
                <MyMap users={this.state.users}/>
            </>
        );
    }
}

export default MyMapHolder;