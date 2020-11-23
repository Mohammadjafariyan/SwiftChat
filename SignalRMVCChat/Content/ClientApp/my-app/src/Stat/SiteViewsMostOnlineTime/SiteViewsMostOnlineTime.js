import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsMostOnlineTime extends StatBase {


    componentDidMount() {
        CurrentUserInfo.SiteViewsMostOnlineTime = this;
        this.setState({
            title: 'آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین (به دقیقه)',
            type: null,
            arrName:'SiteViewsMostOnlineTime'
        });
    }

}

export default SiteViewsMostOnlineTime;