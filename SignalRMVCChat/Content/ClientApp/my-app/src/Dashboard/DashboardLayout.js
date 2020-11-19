import React, {Component} from 'react';
import {cookieManager} from "../Help/CookieManager";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../Help/Socket";

class DashboardLayout extends Component {

    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.DashboardLayout = this;
    }

    refresh(){
        this.setUrl();

        this.setState({mg:Math.random()});
    }
    componentDidMount() {
        this.setUrl();
    }

    setUrl(){
        let baseUrl = document.getElementById('baseUrl').value;
        let port = document.getElementById('port').value;

        let websiteToken = document.getElementById('websiteToken').value;
        let adminToken = cookieManager.getItem('adminToken');

        let returnUrl = DataHolder.dashboardUrl ? DataHolder.dashboardUrl : '';
        let url = `http://${baseUrl}:${port}/OperatorsLogin/Index?returnUrl=${returnUrl}&token=${websiteToken}&adminToken=${adminToken}`;

        this.setState({url:url});
    }
    render() {
    

        return (
            <>

                <iframe src={this.state.url} style={{width: '100%', height: '100vh', border: 'none'}}/>


            </>
        );
    }
}

export default DashboardLayout;