import React, {Component} from 'react';
import {CurrentUserInfo} from "../Help/Socket";
import Badge from "react-bootstrap/Badge";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Form} from "react-bootstrap";
import {DataHolder} from "../Help/DataHolder";
import Button from "react-bootstrap/Button";
import Satistification from "./Satistification";
import MarkAsResovled from "./MarkAsResovled";

class CustomerInfo extends Component {
    state = {};
    
    constructor(arg) {
        super(arg);
        
        CurrentUserInfo.CustomerInfo=this;
    }

    render() {
        if (!DataHolder.selectedCustomer)
            return <></>;

        console.log('CustomerInfo===>DataHolder.selectedCustomer====>')
        console.log(DataHolder.selectedCustomer)
        return (
            <div>

                <div 
                    className={"card  "}>
                    <div className={"card-header "  }>
                        اطلاعات کاربر
                        <span className="p-toast-message-icon pi pi-check"></span>
                    </div>

                    <div className="card-body">
                        <Badge pill variant="light">
                            <img width={100} height={100}
                                 src={`${process.env.PUBLIC_URL}/Content/HelpImages/user-icon.png`}/>


                            {DataHolder.selectedCustomer.LastTrackInfo && DataHolder.selectedCustomer.LastTrackInfo.country_code &&
                            <Badge pill variant="light">
                                <img
                                    src={`https://www.countryflags.io/${DataHolder.selectedCustomer.LastTrackInfo.country_code}/flat/64.png`}/>
                            </Badge>
                            }

                            {(!DataHolder.selectedCustomer.LastTrackInfo || !DataHolder.selectedCustomer.LastTrackInfo.country_code) &&


                            <Badge pill variant="light">
                                <img
                                    src={`https://www.countryflags.io/ir/flat/64.png`}/>
                            </Badge>
                            }
                        </Badge>

                       

                        
                        <Form.Group controlId="formBasicEmail">
                            {!this.state.editingName &&
                            <Form.Label onClick={() => {

                                this.setState({editingName: true})
                            }}>

                                <i className={'fa fa-pencil'}></i>

                                {DataHolder.selectedCustomer.Name}</Form.Label>
                            }
                            {this.state.editingName &&
                            <Form.Control type="text" placeholder="نام کاربر" value={DataHolder.selectedCustomer.Name}
                                          onChange={(e) => {
                                              DataHolder.selectedCustomer.Name = e.target.value;
                                              this.setState({tmp: Math.random()})
                                              
                                              if (CurrentUserInfo.CustomersPage)
                                              {
                                                  CurrentUserInfo.CustomersPage.setState({tmp: Math.random()})
                                              }
                                              
                                          }} onKeyDown={(e) => {
                                if (e.key === 'Enter') {
                                    this.setState({editingName: false})

                                }
                            }}/>}


                            <br/>

                            {!this.state.editingEmail &&
                            <Form.Label onClick={() => {

                                this.setState({editingEmail: true})
                            }}>
                                <i className={'fa fa-pencil'}></i>
                                <a href='#'>{DataHolder.selectedCustomer.Email ? DataHolder.selectedCustomer.Email : 'ایمیل'}</a>

                            </Form.Label>
                            }
                            {this.state.editingEmail &&
                            <Form.Control type="email" placeholder="محل قرار گرفتن ایمیل کاربر" value={DataHolder.selectedCustomer.Email}
                                          onChange={(e) => {
                                              DataHolder.selectedCustomer.Email = e.target.value;
                                              this.setState({tmp: Math.random()})
                                          }} onKeyDown={(e) => {
                                if (e.key === 'Enter') {
                                    this.setState({editingEmail: false})

                                }
                            }}/>}


                            <br/>

                            {!this.state.editingPhone &&
                            <Form.Label onClick={() => {

                                this.setState({editingPhone: true})
                            }}>
                                <i className={'fa fa-phone'}></i>
                                <b >{DataHolder.selectedCustomer.Phone ? DataHolder.selectedCustomer.Phone : 'شماره تلفن'}</b>

                            </Form.Label>
                            }
                            {this.state.editingPhone &&
                            <Form.Control type="text" placeholder="شماره تلفن" value={DataHolder.selectedCustomer.Phone}
                                          onChange={(e) => {
                                              DataHolder.selectedCustomer.Phone = e.target.value;
                                              this.setState({tmp: Math.random()})
                                          }} onKeyDown={(e) => {
                                if (e.key === 'Enter') {
                                    this.setState({editingPhone: false})

                                }
                            }}/>}

                            
                            
                            
                            <Form.Text className="text-muted">
                                {DataHolder.selectedCustomer.Address}
                                {!DataHolder.selectedCustomer.Address && <p>آدرس کاربر مشخص نیست</p>}
                            </Form.Text>
                            
                            
                            
                            <Button variant={'primary'}>
                                <Badge variant={'primary'}>
                                    
                                    <i className={'fa fa-user'}></i>
                                </Badge>
                                نمایش کامل پروفایل کاربر
                                
                            </Button>
                        </Form.Group>
                        
                        
                        <Satistification/>

                    </div>
                </div>

            </div>
        );
    }
}

export default CustomerInfo;