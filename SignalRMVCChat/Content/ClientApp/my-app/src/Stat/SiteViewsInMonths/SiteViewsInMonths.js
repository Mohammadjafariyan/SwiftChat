import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInMonths extends StatBase {


    componentDidMount() {
        CurrentUserInfo.SiteViewsInMonths = this;
        this.setState({
            title: 'آمار بازدید در ماه های امسال(با مقایسه)',
            type: 'line'
        });
    }

   
}

export default SiteViewsInMonths;