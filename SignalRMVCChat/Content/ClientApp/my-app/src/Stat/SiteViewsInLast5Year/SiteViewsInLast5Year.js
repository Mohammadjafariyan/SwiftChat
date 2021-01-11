import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class SiteViewsInLast5Year extends StatBase {


    constructor(props) {
        super(props);
        
        CurrentUserInfo.SiteViewsInLast5Year = this;
        this.state={
            title: 'آمار بازدید در سال های گذشته(با مقایسه 5 سال)',
            type: 'line',
            arrName:'SiteViewsInLast5Year'
        };
    }
    
    componentDidMount() {
      

        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

  
}

export default SiteViewsInLast5Year;