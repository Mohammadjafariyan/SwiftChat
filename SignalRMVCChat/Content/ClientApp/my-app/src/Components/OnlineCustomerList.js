import React, { Component } from "react";
import { Row, Col, Card, Container } from "react-bootstrap";

import "../styles/myStyle.css";
import { CurrentUserInfo ,MyCaller} from "../Help/Socket";
import { DataHolder } from "./../Help/DataHolder";
import { Badge } from "react-bootstrap";
import CustomerTags from "./CustomerTags";
import TagSingleCustomer from "./TagSingleCustomer";
import TagList from "./TagList";
import WhileWriting from "./WhileWriting";
export default class OnlineCustomerList extends Component {


    selectCustomer(customer){
        
        MyCaller.Send('selectCustomerForChat',{

            customerId:customer.Id
            
        })
        
        
    }


  
  render() {
    return (
      <Container fluid>
          <Row>

              <Col md={2} className='onDesktop'>
                  <TagList  />
              </Col>
              <Col md={10}>
                  
                  {this.showCustomers()}
                  
              </Col>
          </Row>
        
          

          
      </Container>
    );
  }
    showCustomers(){
        return <Row>
            { !this.props.list && <p>در حال خواندن اطلاعات</p> }
            {this.props.list && this.props.list.length==0 && <p>هیچ بازدیدکننده ای یافت نشد</p> }


            {this.props.list &&
            this.props.list.map((item, i, arr) => {
                return (
                    <Col md={3} className="userCard">
                        {" "}
                        <Card bg="Light" key={i} text={item.Name}>
                            <Card.Header>{item.Name}

                                <TagSingleCustomer customerId={item.Id}/>
                                <WhileWriting IsTyping={item.IsTyping}></WhileWriting>
                            </Card.Header>
                            <Card.Body>
                                <Card.Text>
                                    {this.showNewMessageCount(item)}



                                    {item.Message && item.Message.Message && <div>

                                        {item.Message.ChatType === 1 && <span>

                                          <Badge variant="warning">

                                              {item.Message.Message}</Badge></span>}

                                        {item.Message.ChatType===2 && (
                                            <Badge variant="warning">
                                                ربات :{item.Message.Message}{" "}
                                            </Badge>
                                        )}
                                        {item.Message.ChatType === 4  && (
                                            <Badge variant="success">
                                                فرم :{item.Message.Message}{" "}
                                            </Badge>
                                        )}
                                    </div>}
                                    <CustomerTags customer={item} />
                                </Card.Text>
                                <small>{item.Address}</small>
                                <br />
                                <Card.Link
                                    href="#"
                                    onClick={() =>
                                    {


                                        DataHolder.selectedCustomer = item;

                                        DataHolder.currentPage = null;
                                        CurrentUserInfo.LayoutPage.setState({
                                            temp: Math.random(),
                                        });
                                        this.setState({ temp: Math.random() });

                                        if(CurrentUserInfo.CustomersPage){
                                            this.selectCustomer(item);
                                        }
                                    }}
                                >
                                    شروع چت
                                </Card.Link>
                            </Card.Body>
                            <Card.Footer>
                                <small dir="rtl" className="text-muted">{item.Time}</small>
                            </Card.Footer>
                        </Card>
                    </Col>
                );
            })}
        </Row>
    }

    showNewMessageCount(item) {
        if (item.NewMessageCount){
            return <Badge variant="primary">{item.NewMessageCount }+</Badge>

        }else{
            return <><i>بدون پیغام</i></>

        }
    }
}
