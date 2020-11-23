import React, {Component} from 'react';
import StatBase from "../StatBase";
import {CurrentUserInfo} from "../../Help/Socket";
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";

class MostExitUrlInSite extends StatBase {


    componentDidMount() {
        CurrentUserInfo.MostExitUrlInSite = this;
        this.setState({
            title: 'آمار صفحه ای که بیشترین خروج از سایت را داشته ',
            type: null,
            arrName:'MostExitUrlInSite'
        });
    }
}


export default MostExitUrlInSite;