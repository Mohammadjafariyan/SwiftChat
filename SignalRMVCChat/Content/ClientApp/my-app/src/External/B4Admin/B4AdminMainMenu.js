import React, {Component} from "react";
import {DataHolder} from "../../Help/DataHolder";
import {Col, Container, Row} from "react-bootstrap";
import Button from "react-bootstrap/Button";
import {MyGlobal} from "../../Help/MyGlobal";
import SubMenu from "../../Components/SubMenu";
import {ShowPlusCount} from "../../Components/Menu";


import '../../styles/myStyle.css'
import {CurrentUserInfo} from "../../CurrentUserInfo";

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
            NotChattedCount: res.Content.NotChattedCount,
            TotalAnswered: res.Content.TotalAnswered,
            AssignedToMeCount: res.Content.AssignedToMeCount,
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
                <li id="EventTrigger" className={'nav-item ' + (DataHolder.currentPage === 'EventTrigger' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('EventTrigger');

                    }}>
                        <i className={'fa fa-map-marker'} aria-hidden="true"></i>
                        <span>
                                ها     EventTrigger
                            </span></a>
                </li>


              


                <li id="HelpDeskPage" className={'nav-item ' + (DataHolder.currentPage === 'HelpDeskPage' ? ' active ' : '')}>
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


                <li id="usersSeparation" className={'nav-item ' + (DataHolder.currentPage === 'usersSeparation' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('usersSeparation');
                    }}>
                        <i className="fas fa-fw fa-chart-area"></i>
                        <span>تفکیک کاربران عضو</span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'RoutingPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('RoutingPage');
                    }}>
                        <i className="fas fa-fw fa-chart-area"></i>
                        <span>اختصاص اتوماتیک اوپراتور ها به صفحهات </span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'CompaignPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('CompaignPage');
                    }}>
                        <i className="fas fa fa-send-o"></i>
                        <span>کمپین(ارسال گروهی - اتوماتیک)</span></a>
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

                <li className={'nav-item ' +
                 (DataHolder.currentPage==='TelegramBotPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage("TelegramBotPage");

                    }}>
                       <i className="fa fa-telegram" aria-hidden="true"></i>
                        <span>

                                ربات تلگرام
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
                        <span>اطلاعات فرم ها</span></a>
                </li>


                <li className={'nav-item ' + (DataHolder.currentPage === 'ContactsPage' ? ' active ' : '')}>
                    <a className="nav-link" onClick={() => {
                        this.setPage('ContactsPage');
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



                <li className={'nav-item ' +
                 (DataHolder.currentPage==='SettingPage' ? ' active ' : '')}>
                    <a id="settingpage"  className="nav-link" onClick={() => {
                        this.setPage("SettingPage");

                    }}>
                       <i className="fa fa-cog" aria-hidden="true"></i>
 <span>

                                تنظیمات
                            </span></a>
                </li> 


                <li className={'nav-item ' +
                 (DataHolder.currentPage==='ArchivePage' ? ' active ' : '')}>
                    <a id="ArchivePage"  className="nav-link" onClick={() => {
                        this.setPage("ArchivePage");

                    }}>
                       <i className="fa fa-cog" aria-hidden="true"></i>
 <span>

                                آرشیو گفتگو ها
                            </span></a>
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
