import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {ListGroup} from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Badge from "react-bootstrap/Badge";
import {DataHolder} from "../Help/DataHolder";
import {_showError} from "../Pages/LayoutPage";
import CustomerInfo from "./CustomerInfo";
import UserDeviceInfo from "./UserDeviceInfo";
import UserTags from "./UserTags";
import OtherTools from "./OtherTools";
import LastPagesVisited from "./LastPagesVisited";
import CustomerData from "./CustomerData";
import PrimengModal from "./SendFromHelpDeskModal";
import SendFromHelpDesk from "./SendFromHelpDesk";
import {Dialog} from "primereact/dialog";
import ScreenRecordShower from "./ScreenRecordShower";
import ScreenRecordShowerSendRequest from "./ScreenRecordShowerSendRequest";
import LiveAssistLayout from "./LiveAssist/LiveAssistLayout";
import UsersSeparationData from "./UsersSeparation/UsersSeparationData";
import CustomerTimeSpent from "./Profile/CustomerTimeSpent";

class CustomerToolbar extends Component {

    componentDidMount() {

        if (!DataHolder.selectedCustomer) {
            return;
        }
        MyCaller.Send('GetCreatedForms', {customerId: DataHolder.selectedCustomer.Id})
    }


    constructor(args) {
        super(args);

        this.state = {};
        CurrentUserInfo.CustomerToolbar = this;
    }


    getCreatedFormsCallback(res) {

        if (!res || !res.Content || !res.Content.EntityList) {

            CurrentUserInfo.LayoutPage.showError('لیست فرم ها نال است');
            return;
        }

        this.setState({formList: res.Content.EntityList})
    }

    render() {
        return (
            <div>

                
                <CustomerInfo Customer={DataHolder.selectedCustomer}/>

                <OtherTools Customer={DataHolder.selectedCustomer}/>
                <UsersSeparationData Customer={DataHolder.selectedCustomer}/>

                <CustomerTimeSpent Customer={DataHolder.selectedCustomer}/>

                <UserDeviceInfo Customer={DataHolder.selectedCustomer}/>
                <div
                    className={"card adminsPanel" + (CurrentUserInfo.LayoutPage.state.focusForSelectingAdmin ? ' showSingle ' : '')}>
                    <div className="card-header">
                        ارسال فرم
                    </div>

                    <div className="card-body">

                        {!DataHolder.selectedCustomer && <small>کاربری انتخاب نشده است</small>}
                        {DataHolder.selectedCustomer &&
                        (!this.state.formList || this.state.formList.length == 0)
                        && <small>هیچ فرمی تعریف نشده و یا تمامی فرم ها استفاده شده است</small>}


                        {this.state.formList &&
                        this.state.formList.map((el, i, arr) => {

                            return (<>

                                <Button onClick={() => {
                                    this.selectForm(el)
                                }} title={'جهت ارسال انتخاب نمایید'} variant="primary">{el.Name} <i
                                    style={{fontSize: '12px'}} className="fa fa-plus" aria-hidden="true"></i>
                                </Button>

                            </>)


                        })}
                    </div>
                </div>

                <CustomerData Customer={DataHolder.selectedCustomer}/>
                
                <UserTags Customer={DataHolder.selectedCustomer}/>
             
           
                {DataHolder.selectedCustomer && <LastPagesVisited Customer={DataHolder.selectedCustomer}/> }
           


                <PrimengModal title={'انتخاب از مقالات مرکز پشتیبانی'} ok={true}>
                    <SendFromHelpDesk></SendFromHelpDesk>


                </PrimengModal>

                <PrimengModal title={'نمایش برخط مانیتور بازدیدکننده'} currName={'ScreenRecordShowerSendRequest'} ok={false}>
                   
                    <ScreenRecordShowerSendRequest/>

                </PrimengModal>

              {/*  <PrimengModal title={'مرورگر مشترک'} currName={'LiveAssistModal'} ok={false}
                              maximized={true}>

                    <LiveAssistLayout/>

                </PrimengModal>*/}

                
            </div>
        );
    }

    selectForm(el) {

        if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.Id) {
            _showError('هیچ بازدید کننده ای انتخاب نشده است');
            return;
        }

        let count = CurrentUserInfo.ChatPage.state.chats &&
        CurrentUserInfo.ChatPage.state.chats.length ? CurrentUserInfo.ChatPage.state.chats.length : 0;
        count++
        MyCaller.Send('AdminSendFormToCustomer',
            {formId: el.Id, customerId: DataHolder.selectedCustomer.Id, UniqId: count})


        CurrentUserInfo.ChatPage.addChat({
            type: 'form', formId: el.Id, Message: '',
            UniqId: count
        }, true)


        let customerId=DataHolder.selectedCustomer && DataHolder.selectedCustomer.Id ? DataHolder.selectedCustomer.Id : null;
        MyCaller.Send('GetCreatedForms',{customerId})

    }
}

export default CustomerToolbar;