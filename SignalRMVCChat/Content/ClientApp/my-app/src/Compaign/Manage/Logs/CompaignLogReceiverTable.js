import React, {Component} from 'react';
import {CurrentUserInfo} from "../../../Help/Socket";
import {_SelectCustomerForChat, _TakeLazyTable, MyLazyTable} from "../ChildComps/CompaginHelps/CompaignUtility";
import {Spinner} from "react-bootstrap";
import {MyCard, MyFieldset} from "../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {ShowStatistification} from "../../../Components/Satistification";
import Button from "react-bootstrap/cjs/Button";
import {CustomerProfileSideBar} from "../../../Components/Profile/CustomerProfileSideBar";
import BlockUser from "../../../Components/BlockUser/BlockUser";
import ButtonGroup from "react-bootstrap/cjs/ButtonGroup";

class CompaignLogReceiverTable extends MyLazyTable {

    state = {
        getUrl: 'GetCompaignLogReceiverList',
        list: [],
        CustomData: {
            CompaignId: this.props.CompaignId,
            CompaignLogId: this.props.CompaignLogId,
        }
    }

    constructor(props) {
        super(props);

        this.ShowStatistificationBody = this.ShowStatistificationBody.bind(this);
        this.actionBody = this.actionBody.bind(this);
    }

    ShowStatistificationBody(row) {

        return <><ShowStatistification ratingCount={row.RatingCount}/></>
    }

    actionBody(row) {
        return <>
            <BlockUser Customer={row}/>


            <Button
                onClick={() => {
                    this.setState({
                        temp: Math.random(),
                        customerProfileSideBarVisible: true,
                        SelectedCustomer: row.Id
                    });
                }}
            >نمایش پروفایل</Button>
            
            <Button onClick={() => {

                _SelectCustomerForChat(row.Id);
            }}>

                شروع گفتگو
            </Button>
        </>
    }

    render() {
        return (
            <div>

                {this.state.customerProfileSideBarVisible &&
                <CustomerProfileSideBar parent={this} Customer={this.state.SelectedCustomer}
                                        visible={this.state.customerProfileSideBarVisible}/>
                }
                {super.render()}
            </div>
        );
    }


    componentDidMount() {
        CurrentUserInfo.CompaignLogReceiverTable = this;
        super.componentDidMount();
    }

    showColumns() {
        return <>
            <Column field="Name" header="نام کاربر"></Column>
            <Column field="Email" header="ایمیل"></Column>
            <Column field="Phone" header="شماره"></Column>
            <Column body={this.ShowStatistificationBody} header="رضایتمندی"></Column>

            <Column field="ProgressPercent" header="عملیات" body={this.actionBody}></Column>
        </>
    }
}

export default CompaignLogReceiverTable;




