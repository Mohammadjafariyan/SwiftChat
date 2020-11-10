import React, {Component} from 'react'
import ChatPage from '../../Pages/ChatPage';
import LoginPage from '../../Pages/LoginPage';
import {cookieManager} from '../../Help/CookieManager';
import {MyGlobal} from '../../Help/MyGlobal';
import {MyCaller, CurrentUserInfo} from '../../Help/Socket';
import {DataHolder} from '../../Help/DataHolder';


import '../../styles/myStyle.css'
import WaitingForAnswer from '../../Pages/WaitingForAnswer';
import AllCustomerListPage from '../../Pages/AllCustomerListPage';
import Answered from '../../Pages/Answered';
import NotChatted from '../../Pages/NotChatted';
import NotChattedLeftCustomerListPage from '../../Pages/NotChattedLeftCustomerListPage';
import ChattedAndReturnedCustomerListPage from '../../Pages/ChattedAndReturnedCustomerListPage';
import SepratePerPage from '../../Pages/SepratePerPage';
import SeparatePerPageCustomerListPage from '../../Pages/SeparatePerPageCustomerListPage';
import FakeServerMonitor from '../../Pages/../fakeServer/FakeServerMonitor';
import ProfilePage from "../../Pages/ProfilePage";
import SocialChannelsPage from "../../Pages/SocialChannelsPage";
import FormCreatorPage from "../../Pages/FormCreatorPage";
import FormDataPage from "../../Pages/FormDataPage";
import {AutomaticSendPage} from "../../Components/Chat";

import {Toast} from 'primereact/toast';
import {Message} from "primereact/message";
import Spinner from "react-bootstrap/Spinner";
import {Button} from "primereact/button";
import HelpDeskPage, {_getHelpDesk} from "../../Pages/HelpDeskPage";
import SaveArticle from "../../Components/SaveArticle";
import LanguageHolder from "../../Components/HelpDesk/Language/LanguageHolder";
import Container from "react-bootstrap/Container";
import CustomerProfilePage from "../../Pages/CustomerProfilePage";
import EventTriggersPage from "../../Pages/Event_TriggersPage";
import LiveAssistPage from "../../Pages/LiveAssistPage";
import UsersSeparationLayout from "../../Components/UsersSeparation/UsersSeparationLayout";
import MapPage from "../../Pages/MapPage";
import BotPage from "../../Pages/BotPage";
import BotDataPage from "../../Pages/BotDataPage";


export default class B4AdminLayoutPage extends Component {
    toastRunning = 0;

    constructor(props) {
        super(props);
        this.state = {isLogin: false};
        CurrentUserInfo.LayoutPage = this;


    }

    showConstantMsg(msg, type, callback) {
        this.setState({constantMsg: msg, constantMsgType: type, constantMsgCallback: callback});

    }


    showError(msg) {

        if (msg) {
            if (this.toast) {
                this.toast.show({severity: 'error', summary: 'پیغام', detail: msg});
            } else {
                this.setState({err: msg});
                setTimeout(() => {
                    this.setState({err: null});

                }, 2000)
            }
        }


    }

    showMsg(msg, TYPE) {
        if (msg) {

            if (this.toast && this.toastRunning < 3 && this.toastRunning >= 0) {
                this.toastRunning++;
                this.toast.show({severity: TYPE ? TYPE : 'info', summary: 'پیغام', detail: msg});

            }
            this.setState({msg: msg});
            setTimeout(() => {
                this.setState({msg: null});

                this.toastRunning--;
                if (this.toastRunning <= 0) {
                    this.toastRunning = 0;
                }

            }, 3000)
        }
    }


    componentDidMount() {
        if (MyGlobal.isTestingEnvirement) {
            cookieManager.setItem("adminToken", "sdlflksdf")
            this.setState({isLogin: true});
        }
    }

    render() {


        let adminToken = cookieManager.getItem("adminToken");
        //console.log(adminToken)

        if (this.state.isClearCookie) {
            adminToken = null;

        }

        // console.log(adminToken)

        if (adminToken && adminToken != 'null' && adminToken != 'undefined') {

            if (!DataHolder.currentPage) {
                return (
                    <div>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>


                        <MessageAlerts/>

                        <ChatPage/>
                    </div>
                )
            } else if (DataHolder.currentPage === 'FakeServerMonitor') {
                return (
                    <div>
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <FakeServerMonitor></FakeServerMonitor>


                    </div>
                )
            } else if (DataHolder.currentPage === 'usersSeparation') {
                return (
                    <div>
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <UsersSeparationLayout></UsersSeparationLayout>


                    </div>
                )
            } else if (DataHolder.currentPage === 'Bot') {
                return (
                    <div>
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <BotPage></BotPage>


                    </div>
                )
            } else if (DataHolder.currentPage === 'BotData') {
                return (
                    <div>
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <BotDataPage></BotDataPage>

                    </div>
                )
            } else if (DataHolder.currentPage === "FormCreator") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <FormCreatorPage></FormCreatorPage>
                    </>)
            } else if (DataHolder.currentPage === "EventTrigger") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <EventTriggersPage></EventTriggersPage>
                    </>)
            } else if (DataHolder.currentPage === "AutomaticSend") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}
                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>


                        <MessageAlerts/>

                        <AutomaticSendPage></AutomaticSendPage>
                    </>)
            } else if (DataHolder.currentPage === "ProfilePage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <ProfilePage></ProfilePage>
                    </>)
            } else if (DataHolder.currentPage === "CustomerProfilePage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <CustomerProfilePage></CustomerProfilePage>
                    </>)
            } else if (DataHolder.currentPage === "FormDataPage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <FormDataPage></FormDataPage>
                    </>)
            } else if (DataHolder.currentPage === "SocialChannels") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        <SocialChannelsPage></SocialChannelsPage>
                    </>)
            } else if (DataHolder.currentPage === "HelpDeskPage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <Container>
                            <LanguageHolder onSelect={(countryCode) => {

                            }}/>
                        </Container>


                        <HelpDeskPage></HelpDeskPage>


                    </>)
            } else if (DataHolder.currentPage === "Map") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <MapPage></MapPage>


                    </>)
            } else if (DataHolder.currentPage === "LiveAssistPage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>


                        <LiveAssistPage></LiveAssistPage>


                    </>)
            } else if (DataHolder.currentPage === "CustomerList") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin && <div className="hideWhole"></div>}

                        <Toast position="bottom-left" ref={(el) => this.toast = el}/>

                        <MessageAlerts/>

                        {DataHolder.filterType === 'WaitingForAnswer' &&
                        <WaitingForAnswer/>}

                        {DataHolder.filterType === 'Answered' &&
                        <Answered/>}


                        {DataHolder.filterType === 'AllCustomerListPage' &&
                        <AllCustomerListPage/>}


                        {DataHolder.filterType === 'NotChatted' &&
                        <NotChatted/>}

                        {DataHolder.filterType === 'ChattedAndReturnedCustomerListPage' &&
                        <ChattedAndReturnedCustomerListPage/>}

                        {DataHolder.filterType === 'NotChattedLeftCustomerListPage' &&
                        <NotChattedLeftCustomerListPage/>}


                        {DataHolder.filterType === 'SepratePerPage' &&
                        <SepratePerPage/>}


                        {DataHolder.filterType === 'SeparatePerPageCustomerListPage' &&
                        <SeparatePerPageCustomerListPage/>}


                    </>)
            }


        } else {
            return (
                <div>
                    <Toast position="bottom-left" ref={(el) => this.toast = el}/>
                    <MessageAlerts/>

                    <LoginPage parent={this}/>
                </div>
            )

        }


    }
}


export function _showError(msgf) {
    CurrentUserInfo.LayoutPage.showError(msgf)

}


export function _showConstantMsg(msgf, type, callback) {
    CurrentUserInfo.LayoutPage.showConstantMsg(msgf, type, callback)

}

export function _showMsg(msg, TYPE) {
    CurrentUserInfo.LayoutPage.showMsg(msg, TYPE)

}


export function MessageAlerts(props) {

    return <>
        {CurrentUserInfo.LayoutPage.state.err &&
        <div className="alert alert-danger">{CurrentUserInfo.LayoutPage.state.err}</div>}
        {CurrentUserInfo.LayoutPage.state.msg &&
        <div className="alert alert-info">{CurrentUserInfo.LayoutPage.state.msg}</div>}
        {/*
        {CurrentUserInfo.LayoutPage.state.constantMsg && <Message severity={CurrentUserInfo.LayoutPage.state.constantMsgType ? CurrentUserInfo.LayoutPage.state.constantMsgType : 'success' }  text={CurrentUserInfo.LayoutPage.state.constantMsg} />}
*/}
        {CurrentUserInfo.LayoutPage.state.constantMsg &&
        <div
            className={"alert alert-" + (CurrentUserInfo.LayoutPage.state.constantMsgType ? CurrentUserInfo.LayoutPage.state.constantMsgType : 'success')}>


            <Button icon="pi pi-times" onClick={() => {
                if (CurrentUserInfo.LayoutPage.state.constantMsgCallback)
                    CurrentUserInfo.LayoutPage.state.constantMsgCallback()
            }} className="p-button-rounded p-button-text p-button-plain"/>


            {CurrentUserInfo.LayoutPage.state.constantMsg}

            <Spinner animation="grow" variant="info"/>


        </div>}

    </>
}