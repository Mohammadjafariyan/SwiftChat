import React, {Component, useState} from 'react';
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

const cities = [

    {name: 'کاربر از استان خاصی باشد', code: 'UserState'},
    {name: 'کاربر از شهر خاصی باشد', code: 'UserCity'},


    {name: 'پیغام کاربر شامل کلماتی باشد', code: 'Message'},
    {name: 'اگر فرم خاصی پر شود', code: 'Form'},
    {name: 'اگر فیلدی از کاربر دریافت شود', code: 'FormInput'},
    {name: 'اگر کاربر در یک صفحه خاص باشد', code: 'InPage'},

    {name: 'برچست خاصی به کاربر زده شود', code: 'Tagged'},
    {name: 'عنوان صفحه ای که کاربر در آن است شامل کلماتی باشد', code: 'PageTitle'},
    {name: 'مکالمه بصورت حل شده علامت زده شود', code: 'MarkAsResolved'},
    {name: 'رضایتمندی کاربر زده شود', code: 'Feedback'},


];

const yesNo = [
    {name: 'بله', value: true},
    {name: 'خیر', value: false},


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


class BotEventNodeSetting extends Component {
    state = {
        formList: [],
        formInputList: [],
        tags: [],
        patterns: [],
        pageTitlePatterns: [],
        MarkAsResolved: true,
        Feedback: true,
        UserCities: [],
        UserStates: [],
    };

    constructor(props) {
        super(props);
        CurrentUserInfo.BotEventNodeSetting = this;

        this.onCountryChange = this.onCountryChange.bind(this);

        MyHook.register('BotEventNodeSettingOk', this, 'BotEventNodeSetting');

    }

    BotEventNodeSettingOk() {


    }


    componentWillUnmount() {

        
        
        
        
        
        
        
        
        
        
        
        
        

        let arr = {
            selectedEventType: this.state.selectedEventType,
            patterns: this.state.patterns,
            selectedForm: this.state.selectedForm,
            selectedFormInput: this.state.selectedFormInput,
            links: this.state.links,
            tags: this.state.tags,
            pageTitlePatterns: this.state.pageTitlePatterns,
            timeFrom:this.state.timeFrom? GetDateTime(this.state.timeFrom):null,
            timeTo: this.state.timeTo?GetDateTime(this.state.timeTo):null,
            MarkAsResolved: this.state.MarkAsResolved,
            UserStates: this.state.UserStates,
            UserCities: this.state.UserCities,
        }


        this.props.node.botEvent = arr;
    }

    init() {

        let UserStates=this.props.node.botEvent.UserStates && this.props.node.botEvent.UserStates.length>0 ? IranStates().filter(f=>f.name==this.props.node.botEvent.UserStates[0].name):[];
        let UserCities=this.props.node.botEvent.UserCities && this.props.node.botEvent.UserCities.length>0 ? IranStates().filter(f=>f.name==this.props.node.botEvent.UserCities[0].name):[];

        this.setState({
                selectedEventType: this.props.node.botEvent.selectedEventType,
                patterns: this.props.node.botEvent.patterns ? this.props.node.botEvent.patterns:[],
                selectedForm: this.props.node.botEvent.selectedForm,
                selectedFormInput: this.props.node.botEvent.selectedFormInput,
                links: this.props.node.botEvent.links ? this.props.node.botEvent.links:[],
                tags: this.props.node.botEvent.tags ? this.props.node.botEvent.tags:[],
                pageTitlePatterns: this.props.node.botEvent.pageTitlePatterns ? this.props.node.botEvent.pageTitlePatterns:[],
                timeFrom: new Date(this.props.node.botEvent.timeFrom),
                timeTo: new Date(this.props.node.botEvent.timeTo),
                MarkAsResolved: this.props.node.botEvent.MarkAsResolved,
                UserStates: UserStates,
                UserCities:UserCities,
            }

        );

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
        if (!res || !res.Content ) {

            CurrentUserInfo.LayoutPage.showError('لیست ورودی فرم ها نال است');
            return;
        }

        this.setState({formInputList: res.Content})
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


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Message' &&
                    <div className="card p-fluid">

                        <Chips separator="," value={this.state.patterns}
                               onChange={(e) => this.setPatterns(e.value)}></Chips>

                        <small>علامت ! به معنای عدم تطابق</small>
                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Form' &&
                    <div className="card p-fluid">

                        <Dropdown value={this.state.selectedForm} options={this.state.formList}

                                  onChange={(e) => {

                                      this.setState({selectedForm: e.value})
                                  }} optionLabel="Name" filter showClear filterBy="Name"
                                  placeholder="یک فرم انتخاب کنید"
                        />


                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'FormInput' &&
                    <div className="card p-fluid">

                        <Dropdown value={this.state.selectedFormInput} options={this.state.formInputList}

                                  onChange={(e) => {

                                      this.setState({selectedFormInput: e.value})
                                  }} optionLabel="Name" filter showClear filterBy="Name"
                                  placeholder="یک فیلد انتخاب کنید"
                        />


                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'InPage' &&
                    <div className="card p-fluid">


                        <FormInModalWithTable name={'links'} parent={this} inputTitle={'Style CSS #id'}

                                              inputTitlePlaceHolder={'آدرس صفحات به regex'}
                                              linkTitle={"افزودن صفحات"}

                                              onConfirm={(links) => {

                                                  this.setState({links})
                                              }}
                        />

                    </div>}


            

                    
                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Tagged' &&
                    <div className="card p-fluid">

                        <Chips separator="," value={this.state.tags}
                               onChange={(e) => this.setState({tags: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'PageTitle' &&
                    <div className="card p-fluid">


                        <Chips separator="," value={this.state.pageTitlePatterns}
                               onChange={(e) => this.setState({pageTitlePatterns: e.value})}></Chips>


                    </div>}


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'WeekAndTime' &&
                    <div className="card p-fluid">


                        <label>روز های هفته را انتخاب کنید</label>
                        <SelectButton value={this.state.weekdays} options={weekdays}
                                      onChange={(e) => this.setState({weekdays: e.value})}
                                      optionLabel="name" multiple/>

                        <hr/>

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


                    {this.state.selectedEventType && this.state.selectedEventType.code == 'MarkAsResolved' &&
                    <div className="card p-fluid">


                        <SelectButton optionLabel="name" value={this.state.MarkAsResolved} options={yesNo}
                                      onChange={(e) => this.setState({MarkAsResolved: e.value})}/>


                    </div>}

                    {this.state.selectedEventType && this.state.selectedEventType.code == 'Feedback' &&
                    <div className="card p-fluid">


                        <SelectButton optionLabel="name" value={this.state.Feedback} options={yesNo}
                                      onChange={(e) => this.setState({Feedback: e.value})}/>


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
                                 multiple
                                 optionLabel="name"
                                 placeholder="یک یا چند شهر را انتخاب کنید"
                        />


                    </div>}


                </Col>
            </Row>


        </div>
    }
}

export default BotEventNodeSetting;



export function GetDateTime(m){

    if (!m)
        return  null;
    var dateString =
        m.getUTCFullYear() + "/" +
        ("0" + (m.getUTCMonth()+1)).slice(-2) + "/" +
        ("0" + m.getUTCDate()).slice(-2) + " " +
        ("0" + m.getUTCHours()).slice(-2) + ":" +
        ("0" + m.getUTCMinutes()).slice(-2) + ":" +
        ("0" + m.getUTCSeconds()).slice(-2);

    return dateString;
}

/*2020/11/12 14:36:28*/