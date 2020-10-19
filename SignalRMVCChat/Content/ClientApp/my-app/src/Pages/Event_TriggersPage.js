import React, {Component} from 'react';
import EventTriggerIndex from "../Components/EventTriggers/EventTriggerIndex";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import EventTriggerSave from "../Components/EventTriggers/EventTriggerSave";

import { MyCaller,CurrentUserInfo } from './../Help/Socket';

class EventTriggersPage extends Component {
    state={};
    constructor(props) {
        super(props);
        CurrentUserInfo.EventTriggersPage=this;
    }

    render() {
        return (
            <div>
                
                <Container>
                    <Row>
                        <Col md={2}>
                            <EventTriggerIndex/>
                        </Col>
                        <Col md={10}>
                            <EventTriggerSave/>
                        </Col>
                    </Row>
                </Container>
                
              
            </div>
        );
    }
}

export default EventTriggersPage;