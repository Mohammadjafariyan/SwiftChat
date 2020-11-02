import { ButtonGroup,Dropdown, Row, Col, Badge, Form } from "react-bootstrap";

import React, { Component } from "react";

import "../../styles/myStyle.css";
import Button from "react-bootstrap/Button";
import {ShowPlusCount} from "../../Components/Menu";
import {DataHolder} from "../../Help/DataHolder";
import {CurrentUserInfo} from "../../Help/Socket";

export default class B4AdminSubMenu extends Component {
    constructor(arg) {
        super(arg);
        

        this.state = {onlyOfflineChecked:false};
        CurrentUserInfo.B4AdminSubMenu = this;
    }

    OnlyOfflines(e){
        CurrentUserInfo.gapIsOnlyOnly=  !this.state.onlyOfflineChecked;
        this.setState({ onlyOfflineChecked:!this.state.onlyOfflineChecked });


        if (CurrentUserInfo.OnlineCustomerListHolder)
            CurrentUserInfo.OnlineCustomerListHolder.GetClientsListForAdmin();
    }

    setPage(page) {
        DataHolder.selectedCustomer=null;
        DataHolder.currentPage='CustomerList';
        DataHolder.filterType = page;
        CurrentUserInfo.LayoutPage.setState({ temp: Math.random() });
        this.setState({ temp: Math.random() });

        CurrentUserInfo.B4AdminBody.route('freeLayout');

    }


    totalUserCountsChangedCallback(res){
        /* if(!res || !res.Content.TotalWaitingForAnswerCount || !res.Content.NotChattedCount)
     {
       console.error(res);
       CurrentUserInfo.LayoutPage.showError('totalUserCountsChangedCallback error')
       return;
     }*/

        this.setState({TotalNewChatReceived:res.Content.TotalNewChatReceived,TotalWaitingForAnswerCount:res.Content.TotalWaitingForAnswerCount,NotChattedCount:res.Content.NotChattedCount})



    }
    render() {
        return (
            <>

                <ButtonGroup  variant='light'aria-label="Basic example">

              
                            <Dropdown>
                                <Dropdown.Toggle variant="default" id="dropdown-basic">
                                    ...
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.Item  onClick={() => {
                                        this.setPage("AllCustomerListPage");
                                    }}>	تمامی مراجعه کنندگان سایت</Dropdown.Item>
                                    <Dropdown.Item  onClick={() => {
                                        this.setPage("NotChattedLeftCustomerListPage");
                                    }}>	کاربرانی که بدون دریافت پشتیبانی سایت را ترک کرده اند </Dropdown.Item>
                                    <Dropdown.Item  onClick={() => {
                                        this.setPage("ChattedAndReturnedCustomerListPage");
                                    }}>	بعد از دریافت پشتیبانی مجددا به سایت بازگشته اند</Dropdown.Item>
                                       <Dropdown.Item  onClick={() => {
                this.setPage("ChattedNever");
              }}>		بعد از دریافت پشتیبانی هرگز به سایت برگنشته اند</Dropdown.Item> 
                                </Dropdown.Menu>
                            </Dropdown>




                            <Button variant='light'
                                    className={
                                        "btn btn-default " +
                                        (DataHolder.currentPage === "CustomerList" &&
                                        DataHolder.filterType === "SepratePerPage"
                                            ? " bg-primary text-white "
                                            : "")
                                    }

                                    onClick={() => {
                                        this.setPage('SepratePerPage');
                                    }}
                            >
                                بر اساس صفحه ها
                            </Button>
                          
                    
                            <Button variant='light'
                                    className={
                                        "btn btn-default " +
                                        (DataHolder.currentPage === "CustomerList" &&
                                        DataHolder.filterType === "NotChatted"
                                            ? " bg-primary text-white "
                                            : "")
                                    }

                                    onClick={() => {
                                        this.setPage("NotChatted");
                                    }}
                            >


                                <ShowPlusCount Count={this.state.NotChattedCount}></ShowPlusCount>


                                <span>              بدون گفتگو</span>
                            </Button>
                            <Button variant='light'
                                    className={
                                        "btn btn-default " +
                                        (DataHolder.currentPage === "CustomerList" &&
                                        DataHolder.filterType === "Answered"
                                            ? " bg-primary text-white "
                                            : "")
                                    }

                                    onClick={() => {
                                        this.setPage('Answered');
                                    }}
                            >

                                <ShowPlusCount Count={this.state.TotalAnswered}></ShowPlusCount>

                                پاسخ داده شده
                            </Button>
                            <Button variant='light'
                                    className={
                                        "btn btn-default " +
                                        (DataHolder.currentPage === "CustomerList" &&
                                        DataHolder.filterType === "WaitingForAnswer"
                                            ? " bg-primary text-white "
                                            : "")
                                    }

                                    onClick={() => {
                                        this.setPage("WaitingForAnswer");
                                    }}
                            >
                                <ShowPlusCount Count={this.state.TotalWaitingForAnswerCount}></ShowPlusCount>

                                <span>              در انتظار پاسخ
                </span>
                            </Button>
                    
                        <Form.Group controlId="formBasicCheckbox" className={'onlyoffline'}>
                            <Form.Check checked={this.state.onlyOfflineChecked} type="checkbox" label="فقط آفلاین ها"  onChange={()=>{

                                this.OnlyOfflines();
                            }}/>
                        </Form.Group>
                </ButtonGroup>

            </>
        );
    }
}
