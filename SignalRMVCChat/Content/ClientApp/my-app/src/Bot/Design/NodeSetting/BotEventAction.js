import React, {Component, useState} from 'react';
import {Alert, Card} from "react-bootstrap";
import {ListBox} from "primereact/listbox";
import {Button} from "primereact/button";
import {Chips} from "primereact/chips";

import '../../../styles/myStyle.css'
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import {Dropdown} from "primereact/dropdown";
import {FormInModalWithTable, MyHook, MySwitcher} from "../../../Components/Utilities/Utility";
import {DataHolder} from "../../../Help/DataHolder";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";
import {SelectButton} from "primereact/selectbutton";
import {Calendar} from "primereact/calendar";
import {_showMsg} from "../../../Pages/LayoutPage";
import {Editor} from "primereact/editor";

const cities = [
    {name: 'ارسال پیغام به کاربر', code: 'SendMessage'},
    {name: 'ارسال یک فرم به کاربر', code: 'SendForm'},
    {name: 'بلاک کردن کاربر ', code: 'BlockUser'},

    {name: 'تغییر وضعیت مکالمه (حل شده یا حل نشده )', code: 'ChangeStatus'},
    {name: 'زدن برچسب به کاربر', code: 'SetTag'},
    {name: 'ارسال پیغام خصوصی به یک ادمین', code: 'SendPrivateNoteToAdmin'},

];

const yesNo = [
    {name: 'بله', value: true},
    {name: 'خیر', value: false},


];

class BotEventAction extends Component {
    state: {
        adminList: [],
    }

    constructor(props) {
        super(props);
        CurrentUserInfo.BotEventAction = this;

        MyHook.register('BotEventActionOk', this, 'BotEventAction');


        this.state = {
            adminList: [],
            SetTags: [], SendPrivateNoteToAdminMessageAdmins: []
        }
    }


    componentWillUnmount() {

        let arr = {
            selectedActionType: this.state.selectedActionType,
            SendMessage: this.state.SendMessage,
            selectedForm: this.state.selectedForm,
            BlockUser: this.state.BlockUser,
            ChangeStatus: this.state.ChangeStatus,
            SetTags: this.state.SetTags,
            SendPrivateNoteToAdminMessageAdmins: this.state.SendPrivateNoteToAdminMessageAdmins,
            SendPrivateNoteToAdminMessage: this.state.SendPrivateNoteToAdminMessage,
        }


        this.props.node.botAction = arr;
    }

    init() {

        this.setState({
                selectedActionType:  this.props.node.botAction.selectedActionType,
                SendMessage:  this.props.node.botAction.SendMessage,
                selectedForm:  this.props.node.botAction.selectedForm,
                BlockUser:  this.props.node.botAction.BlockUser,
                ChangeStatus:  this.props.node.botAction.ChangeStatus,
                SetTags:  this.props.node.botAction.SetTags ?this.props.node.botAction.SetTags :[],
                SendPrivateNoteToAdminMessageAdmins:  this.props.node.botAction.SendPrivateNoteToAdminMessageAdmins ? this.props.node.botAction.SendPrivateNoteToAdminMessageAdmins : [],
                SendPrivateNoteToAdminMessage:  this.props.node.botAction.SendPrivateNoteToAdminMessage,
            }
        );

    }

    GetAdminsListCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList || !res.Content.EntityList.length) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content.EntityList;


        this.setState({adminList: arr});
    }


    getCreatedFormsCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {

            CurrentUserInfo.LayoutPage.showError('لیست فرم ها نال است');
            return;
        }

        this.setState({formList: res.Content.EntityList})


    }


    componentDidMount() {

        DataHolder.selectedEventTrigger = {};
        MyCaller.Send("GetCreatedForms")
        MyCaller.Send('GetAdminsList')

        this.init();

    }

    BotEventActionOk() {

    }

    render() {
        return (
            <div style={{width: '80vw', overflow: 'hidden'}}>

                <Alert variant={'warning'}>
                    بلوک عملیات چیست؟
                    یک Action به شما امکان می دهد پیام پاسخ ارسال کنید یا کاری انجام دهید یک اقدام فقط در صورت لزوم می
                    تواند به یک بلوک رویداد یا بلوک شرط متصل شود.
                </Alert>


                <Row>


                    <Col>
                        <div className="cap-font-sans-semibold help-list" style={{textAlign: 'right'}}>
                            <div>


                                انتخاب نوع عملیات

                                <span className="p-badge p-badge-success">1</span>
                            </div>

                            <ListBox value={this.state.selectedActionType} filter listStyle={{maxHeight: '250px'}}
                                     options={cities}
                                     onChange={(e) => {
                                         this.setState({selectedActionType: e.value})

                                         this.setActionType(e.value)

                                     }}
                                     optionLabel="name"/>

                        </div>
                    </Col>

                    <Col>

                        <div>

                            تنظیمات مربوط به عملیات

                            <span className="p-badge p-badge-success">2</span>
                        </div>


                        <hr/>
                        {this.state.selectedActionType && <h5>{this.state.selectedActionType.name}</h5>}


                        {this.state.selectedActionType && this.state.selectedActionType.code == 'SendMessage' &&
                        <div className="card p-fluid">

                            <Editor value={this.state.SendMessage}
                                    onTextChange={(e) => this.setState({SendMessage: e.htmlValue})}/>

                        </div>}


                        {this.state.selectedActionType && this.state.selectedActionType.code == 'SendForm' &&
                        <div className="card p-fluid">


                            <Dropdown value={this.state.selectedForm} options={this.state.formList}

                                      onChange={(e) => {

                                          this.setState({selectedForm: e.value})
                                      }} optionLabel="Name" filter showClear filterBy="Name"
                                      placeholder="یک فرم انتخاب کنید"
                            />

                        </div>}


                        {this.state.selectedActionType && this.state.selectedActionType.code == 'BlockUser' &&
                        <div className="card p-fluid">


                            <SelectButton optionLabel="name" value={this.state.BlockUser} options={yesNo}
                                          onChange={(e) => this.setState({BlockUser: e.value})}/>


                        </div>}

                        {this.state.selectedActionType && this.state.selectedActionType.code == 'ChangeStatus' &&
                        <div className="card p-fluid">


                            <SelectButton optionLabel="name" value={this.state.ChangeStatus} options={yesNo}
                                          onChange={(e) => this.setState({ChangeStatus: e.value})}/>


                        </div>}


                        {this.state.selectedActionType && this.state.selectedActionType.code == 'SetTag' &&
                        <div className="card p-fluid">

                            <Chips separator="," value={this.state.SetTags}
                                   onChange={(e) => this.setState({SetTags: e.value})}></Chips>


                        </div>}

                        {this.state.selectedActionType && this.state.selectedActionType.code == 'SendPrivateNoteToAdmin' &&
                        <div className="card p-fluid">

                            <ListBox value={this.state.SendPrivateNoteToAdminMessageAdmins} filter
                                     listStyle={{maxHeight: '100px'}}
                                     options={this.state.adminList}
                                     onChange={(e) => {
                                         this.setState({SendPrivateNoteToAdminMessageAdmins: e.value})


                                     }}
                                     optionLabel="Name"

                                     multiple
                                     placeholder="یک یا چند ادمین انتخاب کنید"
                            />


                            <hr/>
                            <Editor value={this.state.SendPrivateNoteToAdminMessage}
                                    onTextChange={(e) => this.setState({SendPrivateNoteToAdminMessage: e.htmlValue})}/>


                        </div>}


                    </Col>
                </Row>
            </div>
        );
    }

    setActionType(value) {
        this.setState({selectedActionType: value})

    }
}

export default BotEventAction;