﻿import React, {Component} from 'react';
import BotOrgChart from "../Design/BotOrgChart";
import {CurrentUserInfo} from "../../Help/Socket";
import {Button} from "primereact/button";

class BotLogDetail extends Component {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.BotLogDetail = this;
    }

    render() {
        return (
            <div>

                <Button label="بستن" className="p-button-raised p-button-warning p-button-text" onClick={() => {

                    this.props.parent.setState({SelectedBotLog: null});

                }}/>

                <hr/>

                <BotOrgChart parent={this} bot={this.props.selectedBot}/>

            </div>
        );
    }
}

export default BotLogDetail;