import React, {Component} from 'react';
import {MyCaller} from "../../Help/Socket";
import {DataHolder} from "../../Help/DataHolder";
import {_showError} from "../../Pages/LayoutPage";
import ListGroup from "react-bootstrap/ListGroup";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class CustomerTimeSpent extends Component {
    state = {};

    componentDidMount() {

        CurrentUserInfo.CustomerTimeSpent = this;


        if (!this.props.Customer)
            return;


        MyCaller.Send('GetCustomerTrackingInfoDetail',
            {
                customerId: this.props.Customer.Id
            }
            /*withList:FALSE*/)
    }

    getCustomerTrackingInfoDetailCallback(res) {
        if (!res || !res.Content) {
            _showError('screenRecordCustomerCloseCallback res is null');
            return;
        }

        if (!this.props.Customer || this.props.Customer.Id != res.Content.customerId) {
            return;
        }
        /*
                    EnterTrack=enter,
                    LastTrack=lastTrack,
                    List=list*/


        this.props.Customer.EnterTrack = res.Content.EnterTrack;
        this.props.Customer.lastTrack = res.Content.LastTrack;

        this.setState({
            EnterTrack: res.Content.EnterTrack,
            LastTrack: res.Content.LastTrack
        })
    }

    render() {

        if (!this.props.Customer)
            return <></>;

        return (
            <div
                className={"card "}>
                <div className="card-header">
                  خلاصه لاگ
                </div>

                <div className="card-body" style={{display: 'flex', textAlign: 'center'}}>
            <ListGroup>
                {this.state.EnterTrack &&
                <ListGroup.Item>اولین بازدید :{this.state.EnterTrack.DateTimeText}</ListGroup.Item>}

                {this.state.LastTrack &&
                <ListGroup.Item>آخرین بازدید :{this.state.LastTrack.DateTimeText}</ListGroup.Item>}
                {this.state.EnterTrack &&
                <ListGroup.Item> اولین رفتار:{this.state.EnterTrack.CustomerTrackInfoTypeText}</ListGroup.Item>}

                {this.state.LastTrack && <ListGroup.Item>آخرین صفحه بازدید :<a href={this.state.LastTrack.Url}
                                                                               target='_blank'>{this.state.LastTrack.PageTitle}</a></ListGroup.Item>}
                {this.state.LastTrack &&
                <ListGroup.Item> آخرین مدت حضور :{this.state.LastTrack.TimeSpent}</ListGroup.Item>}
                {this.state.LastTrack &&
                <ListGroup.Item> آخرین رفتار:{this.state.LastTrack.CustomerTrackInfoTypeText}</ListGroup.Item>}

            </ListGroup>
                </div>
            </div>
            

        );
    }
}

export default CustomerTimeSpent;