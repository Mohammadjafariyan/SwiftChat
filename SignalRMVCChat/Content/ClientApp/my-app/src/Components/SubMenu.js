import { ButtonGroup,Dropdown, Row, Col, Badge, Form } from "react-bootstrap";

import React, { Component } from "react";
import { MyCaller, CurrentUserInfo } from "./../Help/Socket";
import { DataHolder } from "./../Help/DataHolder";

import "../styles/myStyle.css";
import {ShowPlusCount} from "./Menu";

export default class SubMenu extends Component {
  constructor(arg) {
    super(arg);

    this.state = {onlyOfflineChecked:false};
    CurrentUserInfo.SubMenu = this;
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
      <div>
          
          <Row className='onMobile'>
              <ButtonGroup aria-label="Basic example">
                  <button
                      className={
                          "btn btn-default " +
                          (DataHolder.currentPage === "CustomerList" &&
                          DataHolder.filterType === "SepratePerPage"
                              ? " active "
                              : "")
                      }
                      type="button"
                      onClick={() => {
                          this.setPage('SepratePerPage');
                      }}
                  >
                      بر اساس صفحه ها
                  </button>
                  <button
                      className={
                          "btn btn-default " +
                          (DataHolder.currentPage === "CustomerList" &&
                          DataHolder.filterType === "NotChatted"
                              ? " active "
                              : "")
                      }
                      type="button"
                      onClick={() => {
                          this.setPage("NotChatted");
                      }}
                  >


                      <ShowPlusCount Count={this.state.NotChattedCount}></ShowPlusCount>


                      <span>              بدون گفتگو</span>
                  </button>


                  <button
                      className={
                          "btn btn-default " +
                          (DataHolder.currentPage === "CustomerList" &&
                          DataHolder.filterType === "Answered"
                              ? " active "
                              : "")
                      }
                      type="button"
                      onClick={() => {
                          this.setPage('Answered');
                      }}
                  >

                      <ShowPlusCount Count={this.state.TotalAnswered}></ShowPlusCount>

                      پاسخ داده شده
                  </button>

                  <button
                      className={
                          "btn btn-default " +
                          (DataHolder.currentPage === "CustomerList" &&
                          DataHolder.filterType === "WaitingForAnswer"
                              ? " active "
                              : "")
                      }
                      type="button"
                      onClick={() => {
                          this.setPage("WaitingForAnswer");
                      }}
                  >
                      <ShowPlusCount Count={this.state.TotalWaitingForAnswerCount}></ShowPlusCount>

                      <span>              در انتظار پاسخ
                </span>
                  </button>
              </ButtonGroup>    
          </Row>
          
        <Row className='onDesktop'> 
          <Col>
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
              {/*   <Dropdown.Item  onClick={() => {
                this.setPage("ChattedNever");
              }}>		بعد از دریافت پشتیبانی هرگز به سایت برگنشته اند</Dropdown.Item> */}
              </Dropdown.Menu>
            </Dropdown>
          </Col>
            
            
            

          <Col>
            <button
              className={
                "btn btn-default " +
                (DataHolder.currentPage === "CustomerList" &&
                DataHolder.filterType === "SepratePerPage"
                  ? " active "
                  : "")
              }
              type="button"
              onClick={() => {
                this.setPage('SepratePerPage');
              }}
            >
              بر اساس صفحه ها
            </button>
          </Col>

          <Col>
          <Form.Group controlId="formBasicCheckbox">
          <Form.Check checked={this.state.onlyOfflineChecked} type="checkbox" label="فقط آفلاین ها"  onChange={()=>{
              
              this.OnlyOfflines();
          }}/>
  </Form.Group>
          </Col>
          <Col>
            <button
              className={
                "btn btn-default " +
                (DataHolder.currentPage === "CustomerList" &&
                DataHolder.filterType === "NotChatted"
                  ? " active "
                  : "")
              }
              type="button"
              onClick={() => {
                this.setPage("NotChatted");
              }}
            >


                <ShowPlusCount Count={this.state.NotChattedCount}></ShowPlusCount>
             

<span>              بدون گفتگو</span>
            </button>
          </Col>

          <Col>
            <button
              className={
                "btn btn-default " +
                (DataHolder.currentPage === "CustomerList" &&
                DataHolder.filterType === "Answered"
                  ? " active "
                  : "")
              }
              type="button"
              onClick={() => {
                this.setPage('Answered');
              }}
            >

                <ShowPlusCount Count={this.state.TotalAnswered}></ShowPlusCount>

                پاسخ داده شده
            </button>
          </Col>
          <Col>
            <button
              className={
                "btn btn-default " +
                (DataHolder.currentPage === "CustomerList" &&
                DataHolder.filterType === "WaitingForAnswer"
                  ? " active "
                  : "")
              }
              type="button"
              onClick={() => {
                this.setPage("WaitingForAnswer");
              }}
            >
                <ShowPlusCount Count={this.state.TotalWaitingForAnswerCount}></ShowPlusCount>

                <span>              در انتظار پاسخ
                </span>
            </button>
          </Col>

        </Row>
        <hr />

      </div>
    );
  }
}
