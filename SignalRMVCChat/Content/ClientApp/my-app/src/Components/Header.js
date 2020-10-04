import React, {Component} from 'react'
import {MyCaller, CurrentUserInfo} from './../Help/Socket';
import {DataHolder} from './../Help/DataHolder';
import WhileWriting from "./WhileWriting";
import {Card} from "react-bootstrap";
import MarkAsResovled from "./MarkAsResovled";


export class ForwardChat extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.ForwardChat = this;
    }

    forwardChatSuccessCallback(res) {
        CurrentUserInfo.LayoutPage.showMsg('با موفقیت فوروارد شد');

    }

    setWaitingForSelectAdmin() {
        if (this.state.waitingForSelectAdmin) {
            CurrentUserInfo.LayoutPage.showError(null);
            this.setState({waitingForSelectAdmin: false});
            this.ForwardChats();
        }
    }

    ForwardChats() {

        if (!DataHolder.selectedCustomer) {
            CurrentUserInfo.LayoutPage.showError('هیچ چتی انتخاب نشده است');
            return;
        }
        if (!DataHolder.selectedAdmin) {
            if (CurrentUserInfo.AdminsPage) {
                this.setState({waitingForSelectAdmin: true});
                CurrentUserInfo.LayoutPage.setState({temp: Math.random(), focusForSelectingAdmin: true})
            }
            CurrentUserInfo.LayoutPage.showError('لطفا ادمین مورد نظر را جهت فوروارد انتخاب نمایید');
            return;
        }


        CurrentUserInfo.LayoutPage.showMsg('در حال فوروارد چت');

        MyCaller.Send('forwardChat', {
            myAccountId: DataHolder.selectedAdmin.Id,
            targetUserId: DataHolder.selectedCustomer.Id
        })
    }


    render() {
        return (
            <>
                {DataHolder.selectedCustomer && CurrentUserInfo.AdminsPage &&
                CurrentUserInfo.AdminsPage.state.arr &&
                CurrentUserInfo.AdminsPage.state.arr.length > 0
                && <button onClick={() => {
                    this.ForwardChats();
                }}>انتقال گفتگو</button>}

            </>);
    }
}

export default class MyHeader extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.MyHeader = this;
    }

    customerTypingCallback(res, IsTyping) {
        if (!res || !res.Content.targetCustomerId) {
            CurrentUserInfo.LayoutPage.showError('کاربر ی در حال تایپ است بدون کد کاربر ارسال شده است ')
            return
        }
        if (!DataHolder.selectedCustomer) {
            return;
        }

        DataHolder.selectedCustomer.IsTyping = IsTyping;

        this.setState({tmp: Math.random()});

    }

    customerStartTypingCallback(res) {
        this.customerTypingCallback(res, true)
    }

    customerStopTypingCallback(res) {
        this.customerTypingCallback(res, false)
    }

    search(searchTerm) {
        CurrentUserInfo.ChatPage.setState({scroll: false});

        if (!searchTerm || searchTerm == '') {
            CurrentUserInfo.CustomersPage.clearSearch();
            //CurrentUserInfo.AdminsPage.searchHandlerCallback(res.customerlist);
            CurrentUserInfo.ChatPage.clearSearch();

            return;
        }

        MyCaller.Send("SearchHandler",
            {
                customerId: DataHolder.selectedCustomer ? DataHolder.selectedCustomer.Id : null,
                searchTerm: searchTerm
            });
    }


    render() {
        return (
            <div>


                <ForwardChat></ForwardChat>

                {DataHolder.selectedCustomer && DataHolder.selectedCustomer.IsTyping &&
                <WhileWriting IsTyping={DataHolder.selectedCustomer.IsTyping}></WhileWriting>
                }

                <div className="form-group">

                    <input className="form-control" placeholder="جستجو"
                           value={this.state.searchTerm}
                           onChange={(e) => {
                               this.setState({searchTerm: e.target.value});
                               this.search(e.target.value);
                           }}/>
                </div>

            </div>
        )
    }
}


/*
* 
* function adminStartTypingCallback(){
    getDoc().querySelector('#gapwhileWriting').style.display=null;
}


function adminStopTypingCallback(){
    getDoc().querySelector('#gapwhileWriting').style.display='none';
}



function bindIsTyping(){
    var searchTimeout;
    getDoc().querySelector('#gapChatInput').onkeypress = function () {
        if (searchTimeout != undefined) clearTimeout(searchTimeout);


        searchTimeout = setTimeout(callServerScript, 250);
        MyCaller.Send('CustomerStartTyping');
    };
    
}

function callServerScript() {
    MyCaller.Send('CustomerStopTyping');

}
*/