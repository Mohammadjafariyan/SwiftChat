import React, {Component} from 'react';
import B4AdminMainMenu from "./B4AdminMainMenu";

import '../../styles/myStyle.css'
import {DataHolder} from "../../Help/DataHolder";
import { colors } from './../../Components/Utilities/GlobalLoading';
import {CurrentUserInfo} from "../../CurrentUserInfo";

class B4AdminMenu extends Component {

    state={
    };
    constructor(props) {
        super(props);
        CurrentUserInfo.B4AdminMenu=this;
    }

    render() {

    let color= localStorage.getItem("menubg") ? localStorage.getItem("menubg") :  colors[0];

        return (
            <>
                <ul
                    
                    style={{backgroundColor:color}}
                    className={"navbar-nav  sidebar sidebar-dark accordion"  + ` bg-gradient-${color} `} id="accordionSidebar">

                    {/*      {/* <!-- Sidebar - Brand-->*/}
                    <a className="sidebar-brand d-flex align-items-center justify-content-center" >
                        <div className="sidebar-brand-icon rotate-n-15">
                            <i className="fas fa-laugh-wink"></i>
                        </div>
                        <div className="sidebar-brand-text mx-3">مرکز پشتیبانی  <sup>پلاگین آنلاین</sup></div>
                    </a>

                    {/* <!-- Divider-->*/}
                    <hr className="sidebar-divider my-0" />

                    {/* <!-- Nav Item - Dashboard-->*/}
                    <li className={`nav-item ${ this.state.activeRoute=='old' ? 'active':''}`}>
                        <a className="nav-link" onClick={()=>{
                            CurrentUserInfo.B4AdminBody.route('old');
   
                        }} >
                            <i className="fas fa-fw fa-tachometer-alt"></i>
                            <span>نسخه قدیمی</span></a>
                    </li>

                    <li className={`nav-item ${ DataHolder.currentPage=='dashboard' ? 'active':''}`}>
                        <a className="nav-link" onClick={()=>{
                            CurrentUserInfo.B4AdminMainMenu.setPage('dashboard')

                        }} >
                            <i className="fa fa-desktop" aria-hidden="true"></i>
                            <span>پیشخوان</span></a>
                    </li>
                  

                    {/* <!-- Divider-->*/}
                    <hr className="sidebar-divider" />

                    {/* <!-- Heading-->*/}
                    <div className="sidebar-heading">
                        عملیات های اصلی
                    </div>

                   <B4AdminMainMenu/>


                   {/*
                     <!-- Nav Item - Pages Collapse Menu-->
                    <li className="nav-item">
                        <a className="nav-link collapsed" href="#" data-toggle="collapse"
                           data-target="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">
                            <i className="fas fa-fw fa-cog"></i>
                            <span>Components</span>
                        </a>
                        <div id="collapseTwo" className="collapse" aria-labelledby="headingTwo"
                             data-parent="#accordionSidebar">
                            <div className="bg-white py-2 collapse-inner rounded">
                                <h6 className="collapse-header">Custom Components:</h6>
                                <a className="collapse-item" href="buttons.html">Buttons</a>
                                <a className="collapse-item" href="cards.html">Cards</a>
                            </div>
                        </div>
                    </li>

                     <!-- Nav Item - Utilities Collapse Menu-->
                    <li className="nav-item">
                        <a className="nav-link collapsed" href="#" data-toggle="collapse"
                           data-target="#collapseUtilities" aria-expanded="true"
                           aria-controls="collapseUtilities">
                            <i className="fas fa-fw fa-wrench"></i>
                            <span>Utilities</span>
                        </a>
                        <div id="collapseUtilities" className="collapse" aria-labelledby="headingUtilities"
                             data-parent="#accordionSidebar">
                            <div className="bg-white py-2 collapse-inner rounded">
                                <h6 className="collapse-header">Custom Utilities:</h6>
                                <a className="collapse-item" href="utilities-color.html">Colors</a>
                                <a className="collapse-item" href="utilities-border.html">Borders</a>
                                <a className="collapse-item" href="utilities-animation.html">Animations</a>
                                <a className="collapse-item" href="utilities-other.html">Other</a>
                            </div>
                        </div>
                    </li>

                     <!-- Divider-->
                    <hr className="sidebar-divider" />

                     <!-- Heading-->
                    <div className="sidebar-heading">
                        Addons
                    </div>

                     <!-- Nav Item - Pages Collapse Menu-->
                    <li className="nav-item">
                        <a className="nav-link collapsed" href="#" data-toggle="collapse"
                           data-target="#collapsePages" aria-expanded="true"
                           aria-controls="collapsePages">
                            <i className="fas fa-fw fa-folder"></i>
                            <span>Pages</span>
                        </a>
                        <div id="collapsePages" className="collapse" aria-labelledby="headingPages"
                             data-parent="#accordionSidebar">
                            <div className="bg-white py-2 collapse-inner rounded">
                                <h6 className="collapse-header">Login Screens:</h6>
                                <a className="collapse-item" href="login.html">Login</a>
                                <a className="collapse-item" href="register.html">Register</a>
                                <a className="collapse-item" href="forgot-password.html">Forgot
                                    Password</a>
                                <div className="collapse-divider"></div>
                                <h6 className="collapse-header">Other Pages:</h6>
                                <a className="collapse-item" href="404.html">404 Page</a>
                                <a className="collapse-item" href="blank.html">Blank Page</a>
                            </div>
                        </div>
                    </li>

                     <!-- Nav Item - Charts-->
                    <li className="nav-item">
                        <a className="nav-link" href="charts.html">
                            <i className="fas fa-fw fa-chart-area"></i>
                            <span>Charts</span></a>
                    </li>

                     <!-- Nav Item - Tables-->
                    <li className="nav-item">
                        <a className="nav-link" href="tables.html">
                            <i className="fas fa-fw fa-table"></i>
                            <span>Tables</span></a>
                    </li>

                     <!-- Divider-->*/}
                    <hr className="sidebar-divider d-none d-md-block" />

                    {/* <!-- Sidebar Toggler (Sidebar)-->*/}
                    <div className="text-center d-none d-md-inline">
                        <button className="rounded-circle border-0" id="sidebarToggle">
                            
                            <i style={{color:'white'}} className={'fa fa-arrows-h'}></i>
                            
                        </button>
                    </div>

                </ul>
            </>
        );
    }
}

export default B4AdminMenu;