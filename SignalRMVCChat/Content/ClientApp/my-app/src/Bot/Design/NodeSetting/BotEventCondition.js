﻿import React, {Component, useState} from 'react';
import {Alert, Card} from "react-bootstrap";
import {ListBox} from "primereact/listbox";
import {Button} from "primereact/button";
import {Chips} from "primereact/chips";

import '../../../styles/myStyle.css'
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import {Dropdown} from "primereact/dropdown";
import {FormInModalWithTable, IranCities, IranStates, MyHook, MySwitcher} from "../../../Components/Utilities/Utility";
import {DataHolder} from "../../../Help/DataHolder";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";
import {SelectButton} from "primereact/selectbutton";
import {Calendar} from "primereact/calendar";
import {_showMsg} from "../../../Pages/LayoutPage";
import {Editor} from "primereact/editor";

const cities = [
    {name: 'در روز هفته خاصی به سایت مراجعه کند', code: 'Week'},
    {name: 'در ساعت خاصی به سایت مراجعه کند', code: 'Time'},


    {name: 'کاربر از استان خاصی باشد', code: 'UserState'},
    {name: 'کاربر از شهر خاصی باشد', code: 'UserCity'},


    {name: 'چک کردن وضعیت مکالمه (حل شده یا نشده )', code: 'IsResovled'},
    {name: 'اگر برچسب خاصی داشته باشد', code: 'HasTag'},
    {name: 'اگر آدرس صفحه تطابق داشته باشد', code: 'PageUrl'},
    {name: 'آگر عنوان صفحه دارای کلماتی خاص باشد', code: 'PageTitleCondition'},
    {name: 'اگر نام کاربر دارای کلمات خاص باشد', code: 'UserName'},


];

const yesNo = [
    {name: 'حل شده', value: true},
    {name: 'حل نشده', value: false},


];

const weekdays = [
    {name: 'شنبه', code: 0},
    {name: 'یکشنبه', code: 1},
    {name: 'دوشنبه', code: 2},
    {name: 'سه شنبه', code: 3},
    {name: 'چهارشنبه', code: 4},
    {name: 'پنج شنبه', code: 5},
    {name: 'جمعه', code: 6},


];


class BotEventCondition extends Component {
    state = {
        formList: [],
        formInputList: [],
        HasTag: [],
        patterns: [],
        pageTitlePatterns: [],
        PageUrlPatterns: [],
        MarkAsResolved: true,
        IsResovled: true,
        PageTitleConditions: [],
        UserNames: [],
        UserCities: [],
        UserStates: [],

    };

    constructor(props) {
        super(props);
        CurrentUserInfo.BotEventNodeSetting = this;

        this.onCountryChange = this.onCountryChange.bind(this);

        MyHook.register('BotEventNodeSettingOk', this, 'BotEventNodeSetting');

    }


    componentWillUnmount() {

        let arr = {
            selectedEventType: this.state.selectedEventType,
            weekdays: this.state.weekdays,
            timeFrom: this.state.timeFrom,
            timeTo: this.state.timeTo,
            IsResovled: this.state.IsResovled,
            HasTag: this.state.HasTag,
            PageUrlPatterns: this.state.PageUrlPatterns,
            PageTitleConditions: this.state.PageTitleConditions,
            UserNames: this.state.UserNames,
            UserStates: this.state.UserStates,
            UserCities: this.state.UserCities,
        }


         this.props.node.botCondition = arr;
    }

    init() {

        this.setState({
                selectedEventType:  this.props.node.botCondition.selectedEventType,
                weekdays:  this.props.node.botCondition.weekdays ? this.props.node.botCondition.weekdays :[],
                timeFrom:  this.props.node.botCondition.timeFrom,
                timeTo:  this.props.node.botCondition.timeTo,
                IsResovled:  this.props.node.botCondition.IsResovled,
                HasTag:  this.props.node.botCondition.HasTag ? this.props.node.botCondition.HasTag : [],
                PageUrlPatterns:  this.props.node.botCondition.PageUrlPatterns ? this.props.node.botCondition.PageUrlPatterns:[],
                PageTitleConditions:  this.props.node.botCondition.PageTitleConditions ? this.props.node.botCondition.PageTitleConditions:[],
                UserNames:  this.props.node.botCondition.UserNames ? this.props.node.botCondition.UserNames:[],
                UserStates:  this.props.node.botCondition.UserStates ? this.props.node.botCondition.UserStates:[],
                UserCities:  this.props.node.botCondition.UserCities ? this.props.node.botCondition.UserCities:[],
            }

        );

    }

    BotEventNodeSettingOk() {


    }


    setSelectedEventType(val) {
        this.setState({selectedEventType: val});
    }

    setPatterns(val) {
        this.setState({patterns: val});
    }

    getCreatedFormsCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {

            CurrentUserInfo.LayoutPage.showError('لیست فرم ها نال است');
            return;
        }

        this.setState({formList: res.Content.EntityList})


    }


    getDefinedFormInputsCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList) {

            CurrentUserInfo.LayoutPage.showError('لیست فرم ها نال است');
            return;
        }

        this.setState({formInputList: res.Content.EntityList})
    }

    componentDidMount() {

        this.init();
        DataHolder.selectedEventTrigger = {};
        MyCaller.Send("GetCreatedForms")
        MyCaller.Send("GetDefinedFormInputs")
    }

    onCountryChange(e) {
        this.setState({selectedCountry: e.value});
    }

    render() {
        return <div style={{width: '80vw', overflow: 'hidden'}}>

            <Alert variant={'warning'}>
                بلوک رویداد چیست؟
                یک رویداد منتظر می ماند تا اتفاقی بیفتد.
                بلوک رویداد را می توان به یک اکشن بلوک متصل کرد به عنوان مثال. وقتی کاربر مثلاً می گوید ، یک پیام پاسخ
                ارسال
                کنید. "امروز پروازهای من را به پاریس نشان دهید.
            </Alert>


            <Row>


                <Col>
                    <div className="cap-font-sans-semibold help-list" style={{textAlign: 'right'}}>
                        <div>


                            انتخاب نوع رویداد

                            <span className="p-badge p-badge-success">1</span>
                        </div>

                        <ListBox value={this.state.selectedEventType} filter listStyle={{maxHeight: '250px'}}
                                 options={cities}
                                 onChange={(e) => {
                                     this.setSelectedEventType(e.value)


                                 }}
                                 optionLabel="name"/>

                    </div>
                </Col>

                <Col>
                    <div>

                        تنظیمات مربوط به رویداد

                        <span className="p-badge p-badge-success">2</span>
                    </div>


                    <hr/>
                    {this.state.selectedEventType && <h5>{this.state.selectedEventType.name}</h5>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Week' &&
                    <div className="card p-fluid">


                        <label>روز های هفته را انتخاب کنید</label>
                        <SelectButton value={this.state.weekdays} options={weekdays}
                                      onChange={(e) => this.setState({weekdays: e.value})}
                                      optionLabel="name" multiple/>


                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Time' &&
                    <div className="card p-fluid">

                        <Row>
                            <Col>
                                <label>زمان از </label>
                                <Calendar id="time12" value={this.state.timeFrom}
                                          onChange={(e) => this.setState({timeFrom: e.value})}
                                          timeOnly hourFormat="12"/>

                            </Col>
                            <Col>
                                <label>زمان تا </label>
                                <Calendar id="time13" value={this.state.timeTo}
                                          onChange={(e) => this.setState({timeTo: e.value})}
                                          timeOnly hourFormat="12"/>
                            </Col>
                        </Row>

                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'IsResovled' &&
                    <div className="card p-fluid">


                        <SelectButton optionLabel="name" value={this.state.IsResovled} options={yesNo}
                                      onChange={(e) => this.setState({IsResovled: e.value})}/>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'HasTag' &&
                    <div className="card p-fluid">

                        <Chips separator="," value={this.state.HasTag}
                               onChange={(e) => this.setState({HasTag: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'PageUrl' &&
                    <div className="card p-fluid">


                        <Chips separator="," value={this.state.PageUrlPatterns}
                               onChange={(e) => this.setState({PageUrlPatterns: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'PageTitleCondition' &&
                    <div className="card p-fluid">


                        <Chips separator="," value={this.state.PageTitleConditions}
                               onChange={(e) => this.setState({PageTitleConditions: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'UserName' &&
                    <div className="card p-fluid">


                        <Chips separator="," value={this.state.UserNames}
                               onChange={(e) => this.setState({UserNames: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'UserState' &&
                    <div className="card p-fluid">

                        <ListBox value={this.state.UserStates} filter listStyle={{maxHeight: '300px'}}
                                 options={IranStates()}
                                 onChange={(e) => {
                                     this.setState({UserStates: e.value})


                                 }}
                                 optionLabel="name"
                                 multiple
                                 placeholder="یک یا چند استان را انتخاب کنید"
                        />


                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'UserCity' &&
                    <div className="card p-fluid">

                        <ListBox value={this.state.UserCities} filter listStyle={{maxHeight: '300px'}}
                                 options={IranCities()}
                                 onChange={(e) => {
                                     this.setState({UserCities: e.value})


                                 }}
                                 optionLabel="name"
                                 multiple
                                 placeholder="یک یا چند شهر را انتخاب کنید"
                        />


                    </div>}


                </Col>
            </Row>


        </div>
    }
}

export default BotEventCondition;