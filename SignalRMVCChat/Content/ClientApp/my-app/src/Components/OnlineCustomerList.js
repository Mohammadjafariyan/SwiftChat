import React, { Component } from "react";
import { Row, Col, Card, Container } from "react-bootstrap";

import "../styles/myStyle.css";
import { MyCaller } from "../Help/Socket";
import { DataHolder } from "./../Help/DataHolder";
import { Badge } from "react-bootstrap";
import CustomerTags from "./CustomerTags";
import TagSingleCustomer from "./TagSingleCustomer";
import TagList from "./TagList";
import WhileWriting from "./WhileWriting";
import { CustomerProfileSideBar } from "./Profile/CustomerProfileSideBar";
import GlobalLoading from "./Utilities/GlobalLoading";
import {CurrentUserInfo} from "../CurrentUserInfo";
export default class OnlineCustomerList extends Component {
  state = {};

  selectCustomer(customer) {
    MyCaller.Send("selectCustomerForChat", {
      customerId: customer.Id,
    });
  }

  render() {
    return (
      <Container fluid>
        <Row>
          <Col md={2} className="onDesktop">
            <TagList />
          </Col>
          <Col md={10}>
            <GlobalLoading />
            {this.showCustomers()}
          </Col>
        </Row>

        {this.state.customerProfileSideBarVisible && (
          <CustomerProfileSideBar
            parent={this}
            Customer={DataHolder.selectedCustomer}
            visible={this.state.customerProfileSideBarVisible}
          />
        )}
      </Container>
    );
  }
  showCustomers() {

    return (
      <Row>
        {!this.props.list && <p>در حال خواندن اطلاعات</p>}
        {this.props.list && this.props.list.length == 0 && (
          <p>هیچ بازدیدکننده ای یافت نشد</p>
        )}

        {this.props.list &&
          this.props.list.map((item, i, arr) => {

    let lastMsg=item.LastMessage && item.LastMessage.Message? item.LastMessage.Message : '';

console.log('showCustomers==>item',item);
console.log('showCustomers==>item->lastMsg',lastMsg);
            return (
              <Col md={3} className="userCard">
                {" "}
                <Card bg="Light" key={i} text={item.Name}>
                  <Card.Header>
                    {item.Name}

                    <TagSingleCustomer customerId={item.Id} />
                    <WhileWriting IsTyping={item.IsTyping}></WhileWriting>
                  </Card.Header>
                  <Card.Body>
                    <Card.Text>
                      {this.showNewMessageCount(item)}

                      {lastMsg && (
                        <div>
                        <hr/>
                          {(!item.LastMessage.ChatType || item.LastMessage.ChatType == 1) && (
                            <span>
                            
                              <Badge variant="warning" className='text-summary' 
                              style={{height:'unset'}}>
                              آخرین پیغام : {lastMsg}
                              </Badge>
                            </span>
                          )}

                          {item.LastMessage.ChatType === 2 && (
                            <Badge variant="warning" className='text-summary'
                            style={{height:'unset'}}>
                              ربات :{lastMsg}{" "}
                            </Badge>
                          )}
                          {item.LastMessage.ChatType === 4 && (
                            <Badge variant="success" className='text-summary'
                            style={{height:'unset'}}>
                              فرم :{lastMsg}{" "}
                            </Badge>
                          )}
                          <hr/>
                        </div>
                      )}
                      <CustomerTags customer={item} />
                    </Card.Text>
                    <small>{item.Address}</small>
                    <br />
                    <Card.Link
                      href="#"
                      onClick={() => {
                        DataHolder.selectedCustomer = item;

                        DataHolder.currentPage = null;
                        CurrentUserInfo.LayoutPage.setState({
                          temp: Math.random(),
                        });
                        this.setState({ temp: Math.random() });

                        if (CurrentUserInfo.CustomersPage) {
                          this.selectCustomer(item);
                        }
                      }}
                    >
                      شروع گفتگو
                    </Card.Link>
                    <hr />

                    <Card.Link
                      onClick={() => {
                        DataHolder.selectedCustomer = item;

                        CurrentUserInfo.LayoutPage.setState({
                          temp: Math.random(),
                        });
                        this.setState({
                          temp: Math.random(),
                          customerProfileSideBarVisible: true,
                        });

                        if (CurrentUserInfo.CustomersPage) {
                          this.selectCustomer(item);
                        }
                      }}
                    >
                      نمایش پروفایل
                    </Card.Link>
                  </Card.Body>
                  <Card.Footer>
                    <small dir="rtl" className="text-muted">
                      {item.Time}
                    </small>
                  </Card.Footer>
                </Card>
              </Col>
            );
          })}
      </Row>
    );
  }

  showNewMessageCount(item) {
    if (item.NewMessageCount) {
      return <Badge variant="primary">{item.NewMessageCount}+</Badge>;
    } else {
      return (
        <>
          <i>بدون پیغام خوانده نشده</i>
        </>
      );
    }
  }
}
