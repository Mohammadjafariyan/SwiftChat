import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Alert, Card, Container} from "react-bootstrap";

import '../styles/myStyle.css'
import BotHelpMessage from "./BotHelpMessage";
import BotList from "./BotList";
import BotIndex from "./BotIndex";
import {CurrentUserInfo} from "../Help/Socket";
import BotDesign from "./Design/BotDesign";

class BotLayout extends Component {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.BotLayout = this;
    }

    render() {
        return (
            <>


                {this.state.selectedBot && <BotDesign/>}
                {!this.state.selectedBot && <BotIndex/>}


            </>
        );
    }
}

export default BotLayout;