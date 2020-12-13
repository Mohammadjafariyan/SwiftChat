import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInWeek extends StatBase {

    constructor(props) {
        super(props);
        
        CurrentUserInfo.SiteViewsInWeek = this;
        this.state={
            title: 'آمار بازدید در طی هفته ',
            type: null,
            arrName:'SiteViewsInWeek'
        };
    }
    

    componentDidMount() {
    

        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

}

export default SiteViewsInWeek;