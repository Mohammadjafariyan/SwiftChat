import React, { Component } from 'react'
import ChatPage from './ChatPage';
import LoginPage from './LoginPage';
import { cookieManager } from './../Help/CookieManager';
import { MyGlobal } from './../Help/MyGlobal';
import { MyCaller, CurrentUserInfo } from './../Help/Socket';
import { DataHolder } from './../Help/DataHolder';
import Menu from "../Components/Menu";


import '../styles/myStyle.css'
import WaitingForAnswer from './WaitingForAnswer';
import AllCustomerListPage from './AllCustomerListPage';
import Answered from './Answered';
import NotChatted from './NotChatted';
import NotChattedLeftCustomerListPage from './NotChattedLeftCustomerListPage';
import ChattedAndReturnedCustomerListPage from './ChattedAndReturnedCustomerListPage';
import SepratePerPage from './SepratePerPage';
import SeparatePerPageCustomerListPage from './SeparatePerPageCustomerListPage';
import FakeServerMonitor from './../fakeServer/FakeServerMonitor';
import ProfilePage from "./ProfilePage";
import SocialChannelsPage from "./SocialChannelsPage";
import FormCreatorPage from "./FormCreatorPage";
import FormDataPage from "./FormDataPage";
import {AutomaticSendPage} from "../Components/Chat";

import { Toast } from 'primereact/toast';



export default class LayoutPage extends Component {
    constructor(props){
        super(props);
        this.state={isLogin:false};
        CurrentUserInfo.LayoutPage = this;


    }


    showError(msg) {
        
        if (msg){
            if (this.toast)
            {
                this.toast.show({severity: 'error', summary: 'پیغام', detail: msg});
            }
            else {
                this.setState({ err: msg });
                setTimeout(() => {
                    this.setState({ err: null });

                },2000)
            }
        }
    
      
    }
    showMsg(msg) {
        if (msg) {

            if (this.toast) {
                this.toast.show({severity: 'info', summary: 'پیغام', detail: msg});
            } else {
                this.setState({msg: msg});
                setTimeout(() => {
                    this.setState({msg: null});

                }, 2000)
            }
        }
    }

    componentWillMount(){
        if(MyGlobal.isTestingEnvirement){
            cookieManager.setItem("adminToken","sdlflksdf")
            this.setState({isLogin:true});
        }
    }
    render() {



        let adminToken= cookieManager.getItem("adminToken");
        //console.log(adminToken)

        if (this.state.isClearCookie){
            adminToken=null;
            
        }

       // console.log(adminToken)

        if (adminToken && adminToken!='null' && adminToken!='undefined') {

            if (!DataHolder.currentPage) {
                return (
                    <div>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}
                        <Toast ref={(el) => this.toast = el} />

                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
            {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

            <ChatPage />
                </div>
    )
            }else if(DataHolder.currentPage ==='FakeServerMonitor')
            
            {
              return( 
                  <div>
                      <Toast ref={(el) => this.toast = el} />
                      <Menu/>
                {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}
    
                
                
                <FakeServerMonitor></FakeServerMonitor>
                
                
                </div>
                )
            }

            
        else if (DataHolder.currentPage === "FormCreator") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}

                        <Toast ref={(el) => this.toast = el} />
                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        <FormCreatorPage></FormCreatorPage>
                    </>)
            }
            else if (DataHolder.currentPage === "AutomaticSend") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}
                        <Toast ref={(el) => this.toast = el} />

                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        <AutomaticSendPage></AutomaticSendPage>
                    </>)
            }

            
        else if (DataHolder.currentPage === "ProfilePage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}

                        <Toast ref={(el) => this.toast = el} />
                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        <ProfilePage></ProfilePage>
                    </>)
            }
            else if (DataHolder.currentPage === "FormDataPage") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}

                        <Toast ref={(el) => this.toast = el} />
                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        <FormDataPage></FormDataPage>
                    </>)
            }
            else if (DataHolder.currentPage === "SocialChannels") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}

                        <Toast ref={(el) => this.toast = el} />
                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        <SocialChannelsPage></SocialChannelsPage>
                    </>)
            }

            
            else if (DataHolder.currentPage === "CustomerList") {

                return (
                    <>
                        {this.state.focusForSelectingAdmin &&  <div className="hideWhole"></div>}

                        <Toast ref={(el) => this.toast = el} />
                        <Menu/>
                        {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}
                        {this.state.msg && <div className="alert alert-info">{this.state.msg}</div>}

                        {DataHolder.filterType==='WaitingForAnswer' && 
                        <WaitingForAnswer/>} 
                       
                        {DataHolder.filterType==='Answered' && 
                        <Answered/>}    


                        {DataHolder.filterType==='AllCustomerListPage' && 
                        <AllCustomerListPage/>}    


                        {DataHolder.filterType==='NotChatted' && 
                        <NotChatted/>}    

                        {DataHolder.filterType==='ChattedAndReturnedCustomerListPage' && 
                        <ChattedAndReturnedCustomerListPage/>}    

                        {DataHolder.filterType==='NotChattedLeftCustomerListPage' && 
                        <NotChattedLeftCustomerListPage/>}    


                        {DataHolder.filterType==='SepratePerPage' && 
                        <SepratePerPage/>}    



                        {DataHolder.filterType==='SeparatePerPageCustomerListPage' && 
                        <SeparatePerPageCustomerListPage/>}    

                        
                        
                        
                        </>)
            }

           
        }else{
            return (
                <div>
                    <Toast ref={(el) => this.toast = el} />
                    {this.state.err && <div className="alert alert-danger">{this.state.err}</div>}

                    <LoginPage parent={this}/>
                </div>
            )

        }

       
    }
}


export function _showError(msgf){
    CurrentUserInfo.LayoutPage.showError(msgf)

}

export function _showMsg(msg){
    CurrentUserInfo.LayoutPage.showMsg(msg)

}