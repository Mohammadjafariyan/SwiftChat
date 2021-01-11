import {MyCaller} from './../Help/Socket';
import React, {Component} from 'react';
import ScreenRecordShower from "./ScreenRecordShower";
import PrimengModal from "./SendFromHelpDeskModal";
import Button from "react-bootstrap/Button";
import {ProgressSpinner} from "primereact/progressspinner";
import {DataHolder} from "../Help/DataHolder";
import {_showError} from "../Pages/LayoutPage";
import Table from "react-bootstrap/Table";
import ShowMyScreen from "./ShowMyScreen";

import "../styles/myStyle.css"
import {CurrentUserInfo} from "../CurrentUserInfo";

class ScreenRecordShowerSendRequest extends Component {
    state = {};

    constructor(ar) {
        super(ar);

        CurrentUserInfo.ScreenRecordShowerSendRequest = this;
    }

    screenRecordCustomerCloseCallback(res){
        if (!res || !res.Content) {
            _showError('screenRecordCustomerCloseCallback res is null');
            return;
        }
        this.help(res);
        this.setState({errColor: 'darkred'})

        DataHolder.selectedCustomer.IsAcceptedToScreenRecord =null;
        this.setState({tmp: Math.random()})
    }

    screenRecordAccessRequestSuccessCallback(res) {
        this.setState({msg: res.Content})


        this.setState({errColor: 'green'})

        if (!res || !res.Content) {
            _showError('res is null');
            return;
        }

        if (!DataHolder.selectedCustomer) {
            _showError('هیج بازدیدکننده ای انتخاب نشده است لطفا ابتدا یک بازدیدکننده را انتخاب نمایید')
            return;
        }
        
       setTimeout(()=>{
            this.setState({msg:null})
        },1000)

        DataHolder.selectedCustomer.IsAcceptedToScreenRecord =true;
        this.setState({tmp: Math.random()})

    }

    help(res) {
        if (!res || !res.Content) {
            _showError('res is null');
            return;
        }
        this.setState({err: res.Content})
    }

    screenRecordAccessRequestFailCallback(res) {
        this.help(res);
        this.setState({errColor: 'darkred'})
    }
    
    componentWillUnmount() {
        MyCaller.Send('ScreenRecordAdminClose', {customerId: DataHolder.selectedCustomer.Id})

        this.setState({err:null,msg:null})
    }


    componentDidMount() {
        DataHolder.selectedCustomer.IsAcceptedToScreenRecord=null;
    }


    sendRequest() {

        this.setState({sent: true})
        MyCaller.Send('ScreenRecordAccessRequest', {customerId: DataHolder.selectedCustomer.Id})
    }

    render() {

        if (this.state.err) {
            return <> <p style={{color: this.state.errColor}}>{this.state.err}</p>
            
                {this.state.errColor=== 'green' &&

                <ProgressSpinner/>
                }
            
            </>

        }

        if (!DataHolder.selectedCustomer) {
            return <p>هیج بازدیدکننده ای انتخاب نشده است لطفا ابتدا یک بازدیدکننده را انتخاب نمایید</p>
        }

        if (DataHolder.selectedCustomer.IsAcceptedToScreenRecord===false) {
            return <>
                <i style={{fontSize: '18px'}} className="fa fa-frown-o" aria-hidden="true"></i>
                <p>کاربر اجازه نمایش مانیتور را نداد</p>

            </>
        }

        return (
            <div>


                {!this.state.sent &&
                <>
                    <i className="fa fa-television bigIcon" aria-hidden="true"></i>

                    <Table striped bordered hover>
                        <thead>
                        <tr>
                            <th>نمایش صفحه کاربر</th>
                            <th>ارسال صفحه شما</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr>
                            <td>
                                <p>ارسال درخواست اجازه کاربر برای نمایش صفحه مانیتور او</p>

                                <Button variant={'success'} onClick={() => {
                                    this.sendRequest()
                                }}>
                                    <i className="fa fa-user-circle bigIcon" aria-hidden="true"></i>

                                    ارسال درخواست مجوز</Button>
                            </td>
                            <td>

                                <p>اشتراک گذاری یکی از صفحات مرورگر یا کل صفحه مانیتور شما</p>


                                <Button variant={'success'} onClick={() => {
                                    this.showMyMonitor()
                                }}>
                                    <i className="fa fa-paper-plane-o bigIcon" aria-hidden="true"></i>

                                    شروع اشتراک گذاری  </Button>
                            </td>
                        </tr>
                        </tbody>
                    </Table>
                
                </>}

                {!DataHolder.selectedCustomer.IsAcceptedToScreenRecord && this.state.sent &&
                <>
                    <p>درخواست ارسال شد ، در انتظار تایید کاربر</p>
                    <ProgressSpinner/>

                </>
                }

                {this.state.showMyScreen &&
<ShowMyScreen/>}
                

                {!this.state.showMyScreen && DataHolder.selectedCustomer.IsAcceptedToScreenRecord &&
               
                <>
                    {this.state.msg && <p style={{color: this.state.errColor}}>{this.state.msg}</p>}
                    <ScreenRecordShower/>
                
                </>}


            </div>
        );
    }

    showMyMonitor() {

        DataHolder.selectedCustomer.IsAcceptedToScreenRecord=true;
        
        this.setState({showMyScreen:true,sent:true});
    }
}

export default ScreenRecordShowerSendRequest;