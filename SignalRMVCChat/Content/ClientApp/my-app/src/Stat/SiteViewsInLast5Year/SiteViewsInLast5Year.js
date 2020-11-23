import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInLast5Year extends StatBase {


    componentDidMount() {
        CurrentUserInfo.SiteViewsInLast5Year = this;
        this.setState({
            title: 'آمار بازدید در سال های گذشته(با مقایسه 5 سال)',
            type: 'line',
            arrName:'SiteViewsInLast5Year'
        });
    }

  
}

export default SiteViewsInLast5Year;