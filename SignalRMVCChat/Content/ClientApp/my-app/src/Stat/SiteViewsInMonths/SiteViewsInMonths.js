import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class SiteViewsInMonths extends StatBase {

    constructor(props) {
        super(props);
        
        CurrentUserInfo.SiteViewsInMonths = this;
        this.state={
            title: 'آمار بازدید در ماه های امسال(با مقایسه)',
            type: null,
            arrName:'SiteViewsInMonths'
        };

    }
    

    componentDidMount() {
       

        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

   
}

export default SiteViewsInMonths;