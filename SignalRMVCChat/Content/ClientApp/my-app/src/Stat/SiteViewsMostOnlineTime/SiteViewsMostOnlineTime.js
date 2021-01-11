import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class SiteViewsMostOnlineTime extends StatBase {

    constructor(props) {
        super(props);
        
        CurrentUserInfo.SiteViewsMostOnlineTime = this;
        this.state={
            title: 'آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین (به دقیقه)',
            type: null,
            arrName:'SiteViewsMostOnlineTime'
        };
    }
    

    componentDidMount() {
     


        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

}

export default SiteViewsMostOnlineTime;