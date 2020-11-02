import React, {Component} from 'react';
import Badge from "react-bootstrap/Badge";
import {DataHolder} from "../Help/DataHolder";
import Row from "react-bootstrap/Row";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";

import '../styles/myStyle.css'
import {_showError} from "../Pages/LayoutPage";
import VoiceCall from "./VoiceCall";

class OtherTools extends Component {
    state = {}

    render() {

        if (!DataHolder.selectedCustomer) {
            return <></>;
        }

        return (
            <div>


                <div
                    className={"card "}>
                    <div className="card-header">
                        تماس و مکان یابی و ابزار
                    </div>

                    <small style={{color: 'green'}}> کاربر جهت تماس تصویری و صوتی در دسترس است</small>
                    <div className="card-body" style={{display: 'flex', textAlign: 'center'}}>

                        <Row>
                            <h1 aria-label="تماس صوتی" data-microtip-position="top" role="tooltip" onClick={()=>{

                                if (!DataHolder.selectedCustomer){
                                    _showError('لطفا ابتدا یک بازدیدکننده را انتخاب کنید');
                                    return ;
                                }
                                DataHolder.selectedCustomerForCall=DataHolder.selectedCustomer;
                                
                                if (CurrentUserInfo.VoiceCall){
                                    CurrentUserInfo.VoiceCall.show();
                                }else{
                                    _showError('CurrentUserInfo.VoiceCall is null');

                                }
                                this.setState({tmp:Math.random()});
                            }}>
                                <Badge variant={'light'}>
                                    <a href='#'> <i className={'fa fa-phone bigIcon'}></i></a>
                                </Badge>
                            </h1>
                            
                            <VoiceCall/>

                            <h1 aria-label="تماس ویدئویی" data-microtip-position="top" role="tooltip">

                                <Badge variant={'light'}>
                                    <a href='#'> <i className={'fa fa-video-camera bigIcon'}></i></a>
                                </Badge>
                            </h1>

                            <h1 aria-label="نمایش در نقشه" data-microtip-position="top" role="tooltip"  onClick={() => {


                                DataHolder.Back=null;
                                CurrentUserInfo.Menu.setPage('Map')

                                //CurrentUserInfo.Modals['LiveAssistModal'].show();

                            }}>
                                <Badge variant={'light'}>
                                    <a href='#'> <i className={'fa fa-map-marker bigIcon'}></i></a>
                                </Badge>
                            </h1>

                            <h1 aria-label="نمایش برخط مانیتور کاربر" data-microtip-position="top" role="tooltip"
                                onClick={() => {


                                    this.openCustomerScreenViewer();

                                }}>
                                <Badge variant={'light'}>
                                    <a href='#'> <i className={'fa fa-television bigIcon'}></i></a>
                                </Badge>
                            </h1>

                            <h1 aria-label="مرورگر مشترک یا کمک بر خط " data-microtip-position="top" role="tooltip"
                                onClick={() => {

                                    
                                    DataHolder.Back=null;
                                    CurrentUserInfo.Menu.setPage('LiveAssistPage')

                                    //CurrentUserInfo.Modals['LiveAssistModal'].show();

                                }}>
                                <Badge variant={'light'}>
                                    <a href='#'> <i className={'fa fa-window-restore bigIcon'}></i></a>
                                </Badge>
                            </h1>




                            <div>
                                <h1 aria-label="Help Desk  ارسال از   " data-microtip-position="top" role="tooltip"
                                    onClick={() => {

                                        this.openHelpDeskModal();
                                    }}>
                                    <Badge variant={'light'}>
                                        <a href='#'>
                                            <i className="fa fa-file-text bigIcon" aria-hidden="true"></i>
                                        </a>

                                    </Badge>

                                </h1>
                                {/*
                              <small>Help Desk ارسال از </small>
*/}

                            </div>
                        </Row>
                    </div>
                </div>
            </div>
        );
    }

    openHelpDeskModal() {
        if (!CurrentUserInfo.SendFromHelpDeskModal) {
            _showError('مدال help desk یافت نشد');
            return;
        }

        CurrentUserInfo.SendFromHelpDeskModal.show()
    }

    openCustomerScreenViewer() {
        if (!CurrentUserInfo.Modals
            || !CurrentUserInfo.Modals['ScreenRecordShowerSendRequest']) {

            _showError('CurrentUserInfo.Modals[\'ScreenRecordShowerSendRequest\'] is null')
        } else {
           
            CurrentUserInfo.Modals['ScreenRecordShowerSendRequest'].show();

        }
    }
}

export default OtherTools;