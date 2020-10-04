import React, {Component} from 'react';
import Badge from "react-bootstrap/Badge";
import {DataHolder} from "../Help/DataHolder";
import Row from "react-bootstrap/Row";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";

import '../styles/myStyle.css'
import {_showError} from "../Pages/LayoutPage";

class OtherTools extends Component {
    state={}
    render() {
        
        if (!DataHolder.selectedCustomer)
        {
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
                    <div className="card-body" style={{display:'flex',textAlign:'center'}}>

                       <Row>
                           <h1 aria-label="تماس صوتی" data-microtip-position="top" role="tooltip">
                               <Badge variant={'light'} >
                                   <a href='#'>   <i className={'fa fa-phone bigIcon'}></i></a>
                               </Badge>
                           </h1>

                           <h1 aria-label="تماس ویدئویی" data-microtip-position="top" role="tooltip">

                               <Badge variant={'light'}>
                                   <a href='#'>    <i className={'fa fa-video-camera bigIcon'}></i></a>
                               </Badge>
                           </h1>

                           <h1 aria-label="نمایش در نقشه" data-microtip-position="top" role="tooltip">
                               <Badge variant={'light'}>
                                   <a href='#'>    <i className={'fa fa-map-marker bigIcon'}></i></a>
                               </Badge>
                           </h1>

                           <h1 aria-label="نمایش برخط مانیتور کاربر" data-microtip-position="top" role="tooltip">
                               <Badge variant={'light'}>
                                   <a href='#'>    <i className={'fa fa-television bigIcon'}></i></a>
                               </Badge>
                           </h1>

                           <h1 aria-label="مرورگر مشترک" data-microtip-position="top" role="tooltip">
                               <Badge variant={'light'}>
                                   <a href='#'>    <i className={'fa fa-window-restore bigIcon'}></i></a>
                               </Badge>
                           </h1>


                          <div>
                              <h1 aria-label="Help Desk  ارسال از   " data-microtip-position="top" role="tooltip" onClick={()=>{
                                  
                                  this.openHelpDeskModal();
                              }}>
                                  <Badge variant={'light'}>
                                      <a href='#'>
                                          <i className="fa fa-file-text-o bigIcon" aria-hidden="true"></i>
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
        if (!CurrentUserInfo.SendFromHelpDeskModal){
            _showError('مدال help desk یافت نشد');
            return ;
        }

        CurrentUserInfo.SendFromHelpDeskModal.show()
    }
}

export default OtherTools;