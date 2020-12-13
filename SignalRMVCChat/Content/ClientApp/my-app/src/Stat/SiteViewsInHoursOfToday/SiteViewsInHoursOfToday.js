import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInHoursOfToday extends StatBase {

    constructor(props) {
        super(props);
        
        CurrentUserInfo.SiteViewsInHoursOfToday = this;
        this.state={
            title: 'آمار بازدید در طی ساعت های روز ',
            type: null,
            arrName:'SiteViewsInHoursOfToday'
        };
    }
    

    componentDidMount() {
      


        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

  
}

export default SiteViewsInHoursOfToday;