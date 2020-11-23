import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteViewsInHoursOfToday extends StatBase {


    componentDidMount() {
        CurrentUserInfo.SiteViewsInHoursOfToday = this;
        this.setState({
            title: 'آمار بازدید در طی ساعت های روز ',
            type: null,
            arrName:'SiteViewsInHoursOfToday'
        });
    }

  
}

export default SiteViewsInHoursOfToday;