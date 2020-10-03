

import React, { Component } from 'react'
import { MyCaller, CurrentUserInfo } from './../Help/Socket';
import { DataHolder } from './../Help/DataHolder';

import '../styles/myStyle.css';
import SubMenu from './SubMenu';
import {Container, Row, Col, Badge} from 'react-bootstrap';
import {MyGlobal} from "../Help/MyGlobal";
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


            <div>
                <button className={'btn btn-default ' +(DataHolder.currentPage==='ProfilePage' ? ' active ' : '')} type="button"
                        onClick={() => {
                            this.setPage('ProfilePage');
                        }}>


                    پروفایل
                </button>

                
                <button className={'btn btn-default ' +(DataHolder.currentPage==='FormDataPage' ? ' active ' : '')} type="button"
                        onClick={() => {
                            this.setPage('FormDataPage');
                        }}>


                    اطلاعات کاربران  
                </button>
                <button id={'formCreatorButton'} className={'btn btn-default ' +(DataHolder.currentPage==='FormCreator' ? ' active ' : '')} type="button"
                        onClick={() => {
                            this.setPage('FormCreator');
                        }}>


                    فرم ساز
                </button>
                
                <button className={'btn btn-default ' +(DataHolder.currentPage==='SocialChannels' ? ' active ' : '')} type="button"
                        onClick={() => {
                            this.setPage('SocialChannels');
                        }}>


                    شبکه های اجتماعی
                </button>
                <button className={'btn btn-default ' + (DataHolder.currentPage==='AutomaticSend' ? ' active ' : '')} type="button"
                    onClick={() => {
                        this.setPage('AutomaticSend');
                    }}>
                    ارسال های اتوماتیک
                    </button>

                <button className={'btn btn-default ' + (!DataHolder.currentPage ? ' active ' : '')} type="button"
                        onClick={() => {
                            this.setPage(null);
                        }}>

                    <ShowPlusCount Count={this.state.TotalNewChatReceived}></ShowPlusCount>

                    اتاق چت
                </button>
                       
                       
                       
                {MyGlobal.isTestingEnvirement &&  
                        <button className={'btn btn-default ' + (!DataHolder.currentPage ? ' active ' : '')} type="button"
                    onClick={() => {
                        this.setPage('FakeServerMonitor');
                    }}>
                   FakeServerMonitor
                        </button>

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