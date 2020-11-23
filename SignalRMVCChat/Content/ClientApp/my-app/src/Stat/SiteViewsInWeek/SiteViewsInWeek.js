import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInWeek extends StatBase {


    componentDidMount() {
        CurrentUserInfo.SiteViewsInWeek = this;
        this.setState({
            title: 'آمار بازدید در طی هفته ',
            type: null,
            arrName:'SiteViewsInWeek'
        });
    }

}

export default SiteViewsInWeek;