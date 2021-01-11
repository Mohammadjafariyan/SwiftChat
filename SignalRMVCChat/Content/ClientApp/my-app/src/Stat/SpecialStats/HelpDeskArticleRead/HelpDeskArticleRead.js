import React, { Component } from 'react'
import StatBase from './../../StatBase';
import {CurrentUserInfo} from "../../../CurrentUserInfo";

export default class HelpDeskArticleRead  extends StatBase {

    constructor(props) {
        super(props);
     
        CurrentUserInfo.HelpDeskArticleRead = this;
        this.state={
            title: 'آمار بازدید مقالات مرکز پشتیبانی',
            type: 'bar',
            arrName:'HelpDeskArticleRead'
        };
    }
    

    componentDidMount() {
       

        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

  
}
