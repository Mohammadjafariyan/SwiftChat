import React, {Component} from "react";
import {CurrentUserInfo} from "../../Help/Socket";
import {DataHolder} from "../../Help/DataHolder";
import {Col, Container, Row} from "react-bootstrap";
import Button from "react-bootstrap/Button";
import {MyGlobal} from "../../Help/MyGlobal";
import SubMenu from "../../Components/SubMenu";
import {ShowPlusCount} from "../../Components/Menu";


import '../../styles/myStyle.css'

export default class B4AdminMainMenu extends Component {
    constructor(arg) {
        super(arg);

        this.state = {};
        CurrentUserInfo.B4AdminMainMenu = this;
    }

    setPage(page) {
        DataHolder.currentPage = page
        CurrentUserInfo.LayoutPage.setState({temp: Math.random()});
        this.setState({temp: Math.random()})
        CurrentUserInfo.B4AdminBody.route('freeLayout');
        
        if (CurrentUserInfo.OntTheFlyChatPanel){
            CurrentUserInfo.OntTheFlyChatPanel.showPanel();
        }
    }

    totalUserCountsChangedCallback(res) {

        if (!DataHolder.currentPage) {
            // یعنی در صفحه چت است
            return;
        }

        /*  if(!res || !res.Content.TotalWaitingForAnswerCount || !res.Content.NotChattedCount || !res.Content.TotalNewChatReceived)
          {
              console.error(res);
              CurrentUserInfo.LayoutPage.showError('totalUserCountsChangedCallback error')
              return;
          }*/

        this.setState({
            TotalNewChatReceived: res.Content.TotalNewChatReceived,
            TotalWaitingForAnswerCount: res.Content.TotalWaitingForAnswerCount,
            NotChattedCount: res.Content.NotChattedCount
        })


    }

    render() {
        return (

            <>


                <li className={'nav-item ' + (!DataHolder.currentPage ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage(null);

                    }}>
                        <i className={'fa fa-comments'} aria-hidden="true"></i>
                        <span>
                               <ShowPlusCount Count={this.state.TotalNewChatReceived}></ShowPlusCount>

                                اتاق چت
                            </span></a>
                </li>


                {/* <!-- Divider-->*/}
                <hr className="sidebar-divider"/>

                {/* <!-- Heading-->*/}
                <div className="sidebar-heading">
                    افزونه ها
                </div>


                <li className={'nav-item ' + (DataHolder.currentPage === 'Map' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('Map');
                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>نمایش در نقشه</span></a>
                </li>
                <li className={'nav-item ' + (DataHolder.currentPage === 'EventTrigger' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('EventTrigger');

                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>
                                ها     EventTrigger
                            </span></a>
                </li>

                <li className={'nav-item ' + (DataHolder.currentPage === 'HelpDeskPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('HelpDeskPage');
                        DataHolder.subPage = null;
                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>


                                Help Desk  مقالات
                            </span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'Bot' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('Bot');
                        DataHolder.subPage = null;
                    }}>
                        <i className={'fa fa-cogs'} aria-hidden="true"></i>
                        <span>


                                ربات (Bot)
                            </span></a>
                </li>

                <li className={'nav-item ' + (DataHolder.currentPage === 'BotData' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('BotData');
                        DataHolder.subPage = null;
                    }}>
                        <i className={'fa fa-cogs'} aria-hidden="true"></i>
                        <span>


                                           آمار ربات ها
                            </span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'usersSeparation' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('usersSeparation');
                    }}>
                        <i className="fas fa-fw fa-chart-area"></i>
                        <span>تفکیک کاربران عضو</span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'AutomaticSend' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('AutomaticSend');

                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>
                                ارسال های اتوماتیک
                            </span></a>
                </li>

                <li className={'nav-item ' + (DataHolder.currentPage === 'FormCreator' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('FormCreator');

                    }}>
                        <i className="fa fa-check-square" aria-hidden="true"></i>
                        <span>


                                فرم ساز
                            </span></a>
                </li>

                {/* <!-- Divider-->*/}
                <hr className="sidebar-divider"/>

                {/* <!-- Heading-->*/}
                <div className="sidebar-heading">
                    آمار و اطلاعات
                </div>
                <li className={'nav-item ' + (DataHolder.currentPage === 'FormDataPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('FormDataPage');
                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>اطلاعات کاربران</span></a>
                </li>


                {/* <!-- Divider-->*/}
                <hr className="sidebar-divider"/>

                {/* <!-- Heading-->*/}
                <div className="sidebar-heading">دیگر</div>

                <li className={'nav-item ' + (DataHolder.currentPage === 'ProfilePage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('ProfilePage');
                    }}>
                        <i className="fas fa-fw fa-chart-area"></i>
                        <span>پروفایل</span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'SocialChannels' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('SocialChannels');

                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>


                                شبکه های اجتماعی
                            </span></a>
                </li>


            </>
        )
    }
}
