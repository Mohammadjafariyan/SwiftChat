import React, {Component} from 'react'
import {_dispatcher} from './../Help/Dispatcher';
import {CurrentUserInfo} from '../Help/Socket';
import {MyGlobal} from "../Help/MyGlobal";

export default class FakeServerMonitor extends Component {


    constructor(props, context) {
        super(props, context);

        CurrentUserInfo.FakeServerMonitor = this;
    }

    customers = [];
    tags = [];

    ServerCall(name, data) {
        if (name === "DeleteTagById") {

            if (!data.tagId) {
                alert('data.tagId isnull');
            }
            if (!this.tags.filter(f => f.Id === data.tagId)) {
                alert('not found tag');
                return;
            }


            this.tags = this.tags.filter(f => f.Id != data.tagId);


            document.getElementById('getAllTagsForCurrentAdminCallback').click();
        }
    }

    render() {
        if (!MyGlobal.isTestingEnvirement)
        {
            return <></>;
        }
        
        return (
            <div>

                <a href='#' onClick={() => {

                    let customer = {
                        Name: 'کاربر آنلاین' + new Date().getTime(), OnlineStatus: 1, Id: Math.random(),
                        TotalUnRead: (Math.round(Math.random(1, 999999) * 20))

                    };

                    this.customers.push(customer);

                    _dispatcher.dispatch({Name: 'newCustomerOnlineCallback', Content: customer})

                }}>add new Customer</a>

                <br></br>

                <a href='#' onClick={() => {

                    let customer = {
                        Name: 'کاربر آنلاین' + new Date().getTime(), OnlineStatus: 1, Id: Math.random(),
                        TotalUnRead: (Math.round(Math.random(1, 999999) * 20))
                    };


                    this.customers.push(customer);


                    let Content = {};
                    Content.EntityList = this.customers;

                    _dispatcher.dispatch({Name: 'getClientsListForAdminCallback', Content: Content})

                }}>getClientsListForAdminCallback</a>


                <br/>

                <a href='#' onClick={() => {

                    let customer = {
                        Name: 'کاربر آنلاین' + new Date().getTime(), OnlineStatus: 1, Id: Math.random(),
                        TotalUnRead: (Math.round(Math.random(1, 999999) * 20))


                    };


                    this.customers.push(customer);


                    let Content = {};
                    Content.TotalWaitingForAnswerCount = this.customers.length;
                    Content.NotChattedCount = this.customers.length / 2;
                    Content.TotalNewChatReceived = this.customers.length;

                    _dispatcher.dispatch({Name: 'totalUserCountsChangedCallback', Content: Content})

                }}>totalUserCountsChangedCallback</a>


                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),
                        TotalUnRead: (Math.round(Math.random(1, 999999) * 20))


                    };


                    this.tags.push(tag);


                    let Content = {};
                    Content.EntityList = this.tags;

                    _dispatcher.dispatch({Name: 'getAllTagsForCurrentAdminCallback', Content: Content})

                }} id='getAllTagsForCurrentAdminCallback'>getAllTagsForCurrentAdminCallback</a>


                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),
                        TotalUnRead: (Math.round(Math.random(1, 999999) * 20))


                    };


                    this.tags.push(tag);


                    let Content = {};
                    Content.EntityList = this.tags;

                    _dispatcher.dispatch({Name: 'userAddedToTagsCallback', Content: Content})

                }} id='userAddedToTagsCallback'>userAddedToTagsCallback</a>
                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),
                        Image:"data:image/jpeg;base64,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"


                    };


                    let Content = {};
                    Content = tag;

                    _dispatcher.dispatch({Name: 'getMyProfileCallback', Content: Content})

                }} id='userAddedToTagsCallback'>getMyProfileCallback</a>

                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),

                    };
                    let tags=[];

                    for (let i = 0; i < 100; i++) {
                        tags.push(tag)
                    }


                    let Content = {};
                    Content = {};
                    Content.EntityList=tags

                    _dispatcher.dispatch({Name: 'getCreatedFormsCallback', Content: Content})

                }} id='getCreatedFormsCallback'>getCreatedFormsCallback</a>

                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),

                    };
                    let tags=[];

                    for (let i = 0; i < 100; i++) {
                        tags.push(tag)
                    }


                    let Content = {};
                    Content = {};
                    Content.EntityList=tags

                    _dispatcher.dispatch({Name: 'getFormSingleCallback', Content: {  elements:[{
                                Name:tag.Name,
                                Help:tag.Name,
                                Value:tag.Name,
                                type:'input'
                            }],
                            Name:tag.Name,
                            AfterMessage:tag.Name,
                            OnlyMe:true,
                            Id:51564}})

                }} id='getFormSingleCallback'>                getFormSingleCallback
                </a>

                <br/>

                <a href='#' onClick={() => {

                    let tag = {
                        Name: 'تگ' + new Date().getMinutes(), OnlineStatus: 1, Id: Math.random(),

                    };
                    let tags=[];

                    for (let i = 0; i < 100; i++) {
                        tags.push(tag)
                    }


                    let Content = {};
                    Content = {};
                    Content.EntityList=tags
                    
                    
                    let formData=[];

                    for (let i = 0; i < 20; i++) {
                        formData.push({Email:"Email-"+Math.random() + "@gmail.com"})
                    }
                 //   debugger

                    _dispatcher.dispatch({
                        Name: 'getFormDataCallback', Content: {
                            Form: {
                                elements: [{
                                    Name: 'نام فیلد',
                                    FieldName: 'Email',
                                    Help: tag.Name,
                                    Value: tag.Name,
                                    type: 'input'
                                }],
                                Name: tag.Name,
                                AfterMessage: tag.Name,
                                OnlyMe: true
                            },
                            FormData: formData,
                            Page:1,
                            Total:350

                        }
                    });
                            
                            /*  let formStructure = res.Content.Form;
        let formData = res.Content.FormData;
        let Page = res.Content.Page;
        let total = res.Content.Total;*/

                }} id='getFormDataCallback'>                getFormDataCallback
                </a>


                
                
            </div>
        )
    }
}
