

import React, { Component } from 'react'
import { MyCaller, CurrentUserInfo } from './../Help/Socket';
import { DataHolder } from './../Help/DataHolder';

import '../styles/myStyle.css';
import SubMenu from './SubMenu';
import {Container, Row, Col, Badge} from 'react-bootstrap';
import {MyGlobal} from "../Help/MyGlobal";
import Button from "react-bootstrap/Button";
import ButtonGroup from "react-bootstrap/ButtonGroup";
export default class Menu extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.Menu = this;
    }

    setPage(page) {
        DataHolder.currentPage = page
        CurrentUserInfo.LayoutPage.setState({ temp: Math.random() });
        this.setState({ temp: Math.random() })
    }

    totalUserCountsChangedCallback(res){
        
        if(!DataHolder.currentPage)
        {
            // یعنی در صفحه چت است
            return;
        }
        
      /*  if(!res || !res.Content.TotalWaitingForAnswerCount || !res.Content.NotChattedCount || !res.Content.TotalNewChatReceived)
        {
            console.error(res);
            CurrentUserInfo.LayoutPage.showError('totalUserCountsChangedCallback error')
            return;
        }*/

        this.setState({TotalNewChatReceived:res.Content.TotalNewChatReceived,TotalWaitingForAnswerCount:res.Content.TotalWaitingForAnswerCount,NotChattedCount:res.Content.NotChattedCount})



    }
    
    render() {
        return (

            <Container fluid>
  <Row>
  <Col>


            <div aria-label="Basic example">

                    
                <Button variant="light" className={'btn btn-default ' +(DataHolder.currentPage==='ProfilePage' ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage('ProfilePage');
                        }}>


                    پروفایل
                </Button>


                <Button variant="light" className={'btn btn-default ' +(DataHolder.currentPage==='FormDataPage' ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage('FormDataPage');
                        }}>


                    اطلاعات کاربران
                </Button>

                <Button variant="light" className={'btn btn-default ' +(DataHolder.currentPage==='HelpDeskPage' ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage('HelpDeskPage');
                            DataHolder.subPage=null;
                        }}>


                    Help Desk  مقالات
                </Button>
                <Button variant="light" id={'formCreatorButton'} className={'btn btn-default ' +(DataHolder.currentPage==='FormCreator' ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage('FormCreator');
                        }}>


                    فرم ساز
                </Button>
                
                <Button variant="light" className={'btn btn-default ' +(DataHolder.currentPage==='SocialChannels' ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage('SocialChannels');
                        }}>


                    شبکه های اجتماعی
                </Button>
                <Button variant="light" className={'btn btn-default ' + (DataHolder.currentPage==='AutomaticSend' ? ' bg-primary text-white ' : '')} type="button"
                    onClick={() => {
                        this.setPage('AutomaticSend');
                    }}>
                    ارسال های اتوماتیک
                    </Button>

                <Button variant="light" className={'btn btn-default ' + (!DataHolder.currentPage ? ' bg-primary text-white ' : '')} type="button"
                        onClick={() => {
                            this.setPage(null);
                        }}>

                    <ShowPlusCount Count={this.state.TotalNewChatReceived}></ShowPlusCount>

                    اتاق چت
                </Button>
                       
                       
                       
                {MyGlobal.isTestingEnvirement &&  
                        <Button variant="light" className={'btn btn-default ' + (!DataHolder.currentPage ? ' bg-primary text-white ' : '')} type="button"
                    onClick={() => {
                        this.setPage('FakeServerMonitor');
                    }}>
                   FakeServerMonitor
                        </Button>

                }
                <hr/>

                <SubMenu/>
            </div>
          </Col>
            </Row>
</Container>
        )
    }
}


export function ShowPlusCount(props){
    if (props.Count && props.Count>0){
        return (<Badge variant="info">{props.Count}+</Badge>)
    }else{
        return (<></>)
    }
}