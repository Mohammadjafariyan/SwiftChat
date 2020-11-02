import React, {Component} from 'react'
import {MyCaller, CurrentUserInfo} from './../Help/Socket';
import {DataHolder} from './../Help/DataHolder';

import './../styles/myStyle.css';
import {Badge, Spinner} from "react-bootstrap";
import WhileWriting from "./WhileWriting";
import {TabPanel, TabView} from "primereact/tabview";

export default class Customers extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.CustomersPage = this;
    }


    customerTypingCallback(res, IsTyping) {
        if (!res || !res.Content.targetCustomerId) {
            CurrentUserInfo.LayoutPage.showError('کاربر ی در حال تایپ است بدون کد کاربر ارسال شده است ')
            return
        }
        if (!this.state.arr) {
            return;
        }

        let j = this.state.arr.findIndex(f => f.Id === res.Content.targetCustomerId);

        if (j === -1) {
            return;
        }

        this.state.arr[j].IsTyping = IsTyping;

        this.setState({tmp: Math.random()});

    }

    customerStartTypingCallback(res) {
        this.customerTypingCallback(res, true)
    }

    customerStopTypingCallback(res) {
        this.customerTypingCallback(res, false)
    }

    totalUserCountsChangedCallback(res) {
        if (!res.Content.CustomerList || !res.Content.CustomerList.length) {

            console.log('res.CustomerList is null or empty');
            return;
        }

        let arr = this.state.arr;
        if (!arr) {
            arr = [];
            return;
        }

        for (let i = 0; i < res.Content.CustomerList.length; i++) {
            let CustomerId = res.Content.CustomerList[i].CustomerId;
            let TotalNewChatSentByCustomer = res.Content.CustomerList[i].TotalNewChatSentByCustomer;
            let OnlineStatus = res.Content.CustomerList[i].OnlineStatus;


            let j = arr.findIndex(f => f.Id === CustomerId);

            if (j !== -1) {
                arr[j].OnlineStatus = OnlineStatus;
                arr[j].NewMessageCount = TotalNewChatSentByCustomer;
            }

        }

        this.setState({arr: arr, tmp: Math.random()})
    }

    customerSendToAdminCallback(res) {
        let CustomerId = res.Content.CustomerId;
        let Message = res.Content.Message;
        let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;

        let chat = res.Content.Chat;

        if (!TotalReceivedMesssages) {
            return;
        }


        // اگر کاربر کنونی ارسال کننده باشد
        if (DataHolder.selectedCustomer &&
            DataHolder.selectedCustomer.Id === CustomerId) {
            return;
        }


        if (!this.state.arr) {
            return;
        }

        let i = this.state.arr.findIndex(c => c.Id == CustomerId);
        if (i != -1) {

            this.state.arr[i].TotalUnRead = TotalReceivedMesssages;
            this.setState({arr: this.state.arr});
        }

    }

    clearSearch() {
        this.setState({arr: this.prevCustomersList});
    }

    searchHandlerCallback(searchCustomersList) {
        this.prevCustomersList = this.state.arr;

        if (!searchCustomersList) {
            searchCustomersList = [];
        }
        let customerList = [];
        for (var i = 0; i < searchCustomersList.length; i++) {
            let tmp = searchCustomersList[i].Customer;
            customerList.push(tmp);
        }

        this.setState({arr: customerList});

    }

    componentDidMount() {

        CurrentUserInfo.UserType = 'CustomersChattedWithMe';


        CurrentUserInfo.selectedTagId = null;
        CurrentUserInfo.gapIsOnlyOnly = null;
        CurrentUserInfo.currentUsersIsAdmins = null;


        MyCaller.Send("GetClientsListForAdmin", {
            userType: CurrentUserInfo.UserType
        });
    }
    
    getCustomersChattedWithMe(){
        this.setState({loading:true});

        CurrentUserInfo.UserType = 'CustomersChattedWithMe';

        MyCaller.Send("GetClientsListForAdmin", {
            userType: CurrentUserInfo.UserType
        });
        
    }

    getCustomerActivityDetailCallback(res) {
        //todo:
    }


    readChat() {

        if (!DataHolder.selectedCustomer) {
            CurrentUserInfo.LayoutPage.showError('کاربر انتخاب نشده است');
            return;
        }

        CurrentUserInfo.LayoutPage.showMsg('در حال خواندن اطلاعات چت');

        MyCaller.Send("ReadChat", {targetId: DataHolder.selectedCustomer.Id, pageNumber: 1});

    }

    getClientsListForAdminCallback(res) {

        this.setState({loading:false});

        if (!res || !res.Content || !res.Content.EntityList) {
            CurrentUserInfo.LayoutPage.showError('getClientsListForAdminCallback returns null');
            return;
        }

        var arr = [];
        arr = res.Content.EntityList;

        if (!this.props.noReadChat)
        {
            if (DataHolder.selectedCustomer) {


                if (!arr.find(f => f.Id == DataHolder.selectedCustomer.Id)) {
                    arr.push(DataHolder.selectedCustomer)
                    this.readChat();
                } else {


                    this.readChat();


                    if (DataHolder.selectedCustomer) {
                        this.GetUserAddedToTags(DataHolder.selectedCustomer.Id);
                    }
                }

                this.placeOnTop(arr);
            }
        }
    

        this.setState({arr: arr});

    }


    GetUserAddedToTags(target) {
        MyCaller.Send("GetUserAddedToTags", {target: target});
    }


    placeOnTop(arr) {
        arr.sort((x, y) => {
            return x.Id == DataHolder.selectedCustomer.Id ? -1 : 1;
        });

    }

    customerOnlineAgainCallback(res) {
        this.newCustomerOnlineCallback(res);
    }

    customerOfflineAgainCallback(res) {
        this.newCustomerOnlineCallback(res);
    }

    newCustomerOnlineCallback(res) {

        let arr = this.state.arr;
        if (!arr) {
            arr = [];
        }

        let i = arr.findIndex(f => f.Id === res.Content.Id);
        if (i != -1) {
            arr[i] = res.Content;
        } else {
            //  arr.push(res.Content);
        }

        this.setState({arr: arr});

    }


    render() {
        return (
            <div>
                <div className="card ">
                <TabView className="tabview-custom" activeIndex={this.state.activeIndex}
                         onTabChange={(e) => {


                             this.setState({activeIndex: e.index});
                             
                             if (e.index===2){
                                 
                                 this.getSharedChatBoxUsers();
                             }else{
                                 this.getCustomersChattedWithMe();
                             }
                         }}>
                    <TabPanel header="من" leftIcon="pi pi-user">

                        <h6>با من چت کرده اند</h6>

                        {this.state.loading &&
                        <Spinner animation="border" role="status">
                            <span className="sr-only">در حال خواندن اطلاعات...</span>
                        </Spinner>}
                        
                        
                        {this.state.arr && this.state.arr.length &&
                        <ShowOnlineUsers arr={this.state.arr} parent={this}/>}
                        
                    </TabPanel>
                    <TabPanel header="مشترک" rightIcon="pi pi-users ">
                    
                       <h6>چت باکس مشترک</h6>


                        {this.state.loading &&
                        <Spinner animation="border" role="status">
                            <span className="sr-only">در حال خواندن اطلاعات...</span>
                        </Spinner>}

                        {this.state.arr && this.state.arr.length &&
                        <ShowOnlineUsers arr={this.state.arr} parent={this}/>}
                    </TabPanel>
                 
                </TabView>
                </div>

                {/*    <div className="card ">
                    <div className="card-header">
                        کاربران آنلاین
                    </div>

                    <ul className="list-group list-group-flush">

                        {this.state.arr && this.state.arr.length &&
                        <ShowOnlineUsers arr={this.state.arr} parent={this}/>}

                    </ul>
                </div>*/}
            </div>
        )
    }

    getSharedChatBoxUsers() {
        this.setState({loading:true});
        CurrentUserInfo.UserType = 'SharedChatBox';


        MyCaller.Send("GetClientsListForAdmin", {
            userType: CurrentUserInfo.UserType
        });
    }
}
const showTotalUnRead = function (el) {
    if (el.NewMessageCount) {
        return <Badge variant="info">{el.NewMessageCount}+</Badge>

    } else {
        return <></>

    }
}

export function ShowOnlineUsers(props) {

    if (!props.arr || props.arr.length === 0) {
        return <></>
    }


    return props.arr.map((el, i, arr) => {

        let isSelected = false;

        if (props.isAdmins) {
            isSelected = DataHolder.selectedAdmin && el.Id == DataHolder.selectedAdmin.Id ? 'selectedUserInList' : '';

        } else {
            isSelected = DataHolder.selectedCustomer && el.Id == DataHolder.selectedCustomer.Id ? 'selectedUserInList' : '';

        }
        return <li   key={el.Id} onClick={(e) => {


            if (props.onClick) {

                props.onClick(el);
            } else {
                CurrentUserInfo.currentUsersIsAdmins = false;

                el.TotalUnRead = 0;
                DataHolder.selectedCustomer = el;
                props.parent.setState({temp: Math.random()});
                props.parent.readChat();


                if (CurrentUserInfo.CustomerToolbar) {
                    CurrentUserInfo.CustomerToolbar.setState({temp: Math.random()})
                }

                //  debugger
                if (DataHolder.selectedCustomer) {
                    props.parent.GetUserAddedToTags(DataHolder.selectedCustomer.Id);
                }


                if (CurrentUserInfo.ChatPage) {
                    CurrentUserInfo.ChatPage.setState({chats: []});
                }
                
                if (CurrentUserInfo.MyMap && el && el.LastTrackInfo ){
                    CurrentUserInfo.MyMap.AddMarker(el.LastTrackInfo.latitude,el.LastTrackInfo.longitude,el)
                }

                //CurrentUserInfo.CustomersPage.placeOnTop(arr);

                CurrentUserInfo.ChatPage.setState({scroll: false});
            }


        }}
                   className={`list-group-item  ${isSelected ? 'bg-info text-white' :''} ${el.IsResolved ? 'IsResolved' : 'UnIsResolved'} userInList ` + isSelected} key={el.Id}>
            {showTotalUnRead(el)}
            {el.Name}


            <WhileWriting IsTyping={el.IsTyping}></WhileWriting>


            <ShowFlag el={el}/>


            {/*
            {el.TotalUnRead && <i className="MsgCount">..</i>}
*/}


            {
                el.OnlineStatus === 0 &&
                <i className="gapStat float-right" style={{backgroundColor: 'green'}}></i>
            }
            {
                el.OnlineStatus === 1 &&
                <i className="gapStat float-right" style={{backgroundColor: 'grey'}}></i>
            }
            {
                el.OnlineStatus === 2 &&
                <i className="gapStat float-right" style={{backgroundColor: 'orange'}}></i>
            }


        </li>
    })

}


export const ShowFlag = (props) => {
    return <>  {props.el.LastTrackInfo && props.el.LastTrackInfo.country_code &&
    <Badge pill variant="light">
        <img style={{width: '30px', height: '30px'}}
             src={`https://www.countryflags.io/${props.el.LastTrackInfo.country_code}/flat/64.png`}/>
    </Badge>
    }

        {(!props.el.LastTrackInfo || !props.el.LastTrackInfo.country_code) &&


        <Badge pill variant="light">
            <img style={{width: '30px', height: '30px'}}
                 src={`https://www.countryflags.io/ir/flat/64.png`}/>
        </Badge>
        }
    </>
}