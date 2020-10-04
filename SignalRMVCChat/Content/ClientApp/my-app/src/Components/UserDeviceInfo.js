import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";

class UserDeviceInfo extends Component {
    render() {

        if (!DataHolder.selectedCustomer)
            return <></>;

       if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.LastTrackInfo ||
           !DataHolder.selectedCustomer.LastTrackInfo.Browser ||
           !DataHolder.selectedCustomer.LastTrackInfo.OS)
            return <></>;

        return (
            <div>
                <div
                    className={"card "}>
                    <div className="card-header">
                       دستگاه کاربر
                    </div>

                    <div className="card-body">

                        <Badge>

                            <i className={'fa fa-cloud'}></i>
                            {DataHolder.selectedCustomer.LastTrackInfo.Browser}

                        </Badge>
                        <Badge>
                            <i className={'fa fa-browser'}></i>

                            {DataHolder.selectedCustomer.LastTrackInfo.OS}

                        </Badge>

                        <Badge>
                            <i className={'fa fa-cloud'}></i>

                            {DataHolder.selectedCustomer.LastTrackInfo.ip}

                        </Badge>

                        <Badge>
                            <i className={'fa fa-monitor'}></i>

                            {DataHolder.selectedCustomer.LastTrackInfo.PageTitle}

                        </Badge>
                        <Badge>
                            <i className={'fa fa-time'}></i>

                            {DataHolder.selectedCustomer.LastTrackInfo.Time}

                        </Badge>

                     
                    </div>
                </div>
            </div>
        );
    }
}

export default UserDeviceInfo;