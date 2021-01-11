import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import BotList from "./BotList";
import BotHelpMessage from "./BotHelpMessage";
import {CurrentUserInfo} from "../CurrentUserInfo";

class BotIndex extends Component {
    constructor(props) {
        super(props);
        CurrentUserInfo.BotIndex = this;
    }

    render() {
        return (
            <>

                <Row>
                    <Col md={2}>
                        <BotList/>

                    </Col>
                    <Col md={10}>
                        <BotHelpMessage/>

                    </Col>
                </Row>
            </>
        );
    }
}

export default BotIndex;