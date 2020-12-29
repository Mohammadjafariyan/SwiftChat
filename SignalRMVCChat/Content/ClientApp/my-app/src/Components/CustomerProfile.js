import React, {Component} from 'react';
import Button from "react-bootstrap/Button";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../Help/Socket";

import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import CustomerTrackingList from "./Profile/CustomerTrackingList";
import {CustomerProfileSideBar} from "./Profile/CustomerProfileSideBar";

class CustomerProfile extends Component {
    state = {};


    render() {

        if (!this.props.Customer) {
            return <div>{this.showReturn()} </div>;
        }
        return (
            <div>
                {this.showReturn()}
                <hr/>


                <Row>


                    {/*  <Col md={4}>
                        <CustomerInfo  Customer={this.props.Customer}/>

                        <UsersSeparationData Customer={this.props.Customer}/>

                        <UserDeviceInfo Customer={this.props.Customer}/>



                        <CustomerTimeSpent Customer={this.props.Customer}/>

                        <CustomerData Customer={this.props.Customer}/>

                        <UserTags Customer={this.props.Customer}/>
                    </Col>*/}
                    <Col md={1}>
                        <Button variant={'light'} onClick={() => {

                            this.setState({customerProfileSideBarVisible: true,rn:Math.random()});

                        }}>
                            <i className={'fa fa-user'}/>

                            نمایش پروفایل

                        </Button>
                    </Col>
                    <Col md={11}>

                        <CustomerTrackingList Customer={this.props.Customer}/>

                    </Col>
                </Row>

                {this.state.customerProfileSideBarVisible && 
                <CustomerProfileSideBar parent={this}  Customer={this.props.Customer}
                                        visible={this.state.customerProfileSideBarVisible}/>
                }
            </div>
        );
    }

    showReturn() {
        return <Button variant={'light'} onClick={() => {


if(CurrentUserInfo.Menu)
            CurrentUserInfo.Menu.setPage(DataHolder.Back)

            if (CurrentUserInfo.B4AdminMainMenu)
                    CurrentUserInfo.B4AdminMainMenu.setPage(DataHolder.Back);
        }}>
            <i className={'fa fa-arrow-left'}/>

            بازگشت

        </Button>;
    }
}

export default CustomerProfile;


export const BackButton = () => {

    return <div>
        <Button variant={'light'} onClick={() => {


if(CurrentUserInfo.Menu)
            CurrentUserInfo.Menu.setPage(DataHolder.Back)

            if (CurrentUserInfo.B4AdminMainMenu)
                    CurrentUserInfo.B4AdminMainMenu.setPage(DataHolder.Back);
        }}>
            <i className={'fa fa-arrow-left'}/>

            بازگشت

        </Button>
    </div>

}