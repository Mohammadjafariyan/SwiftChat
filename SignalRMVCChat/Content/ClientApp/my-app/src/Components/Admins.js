import React, {Component} from 'react'
import {ShowOnlineUsers} from './Customers'
import {CurrentUserInfo} from '../Help/Socket';
import {MyCaller} from './../Help/Socket';
import {DataHolder} from "../Help/DataHolder";

export default class Admins extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.AdminsPage = this;
    }

    newAccountOnlineCallback(res) {
        //todo:
    }

    adminOnlineAgainCallback(res) {
        //todo:
    }

    GetAdminsListCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList || !res.Content.EntityList.length) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content.EntityList;


        this.setState({arr: arr});
    }


    componentDidMount() {


        MyCaller.Send('GetAdminsList')

    }


    render() {
        return (
            <div>
                <div
                    className={"card adminsPanel" + (CurrentUserInfo.LayoutPage.state.focusForSelectingAdmin ? ' showSingle ' : '')}>
                    <div className="card-header">
                        ادمین های آنلاین
                    </div>
                    <ul className="list-group list-group-flush">
                        {this.state.arr && <ShowOnlineUsers isAdmins={true} onClick={(el) => {

                            DataHolder.selectedAdmin = el;
                            this.setState({temp: Math.random(), focusForSelectingAdmin: false});


                            if(CurrentUserInfo.ForwardChat &&
                                CurrentUserInfo.ForwardChat.state.waitingForSelectAdmin){
                                CurrentUserInfo.ForwardChat.setWaitingForSelectAdmin();
                            }else{
                                CurrentUserInfo.currentUsersIsAdmins=true;
                                CurrentUserInfo.LayoutPage.showMsg('در حال خواندن اطلاعات چت');

                                this.readChat();
                            }

                            CurrentUserInfo.LayoutPage.setState({temp: Math.random(), focusForSelectingAdmin: false})

                        }} arr={this.state.arr} parent={this}/>}

                    </ul>
                </div>
            </div>
        )
    }

    ///CALLED BY ADMINS
    readChat(){
        MyCaller.Send("ReadChat", { targetId: DataHolder.selectedAdmin.Id, pageNumber: 1 });

    }

}
