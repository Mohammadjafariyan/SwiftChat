import React, {Component} from 'react';
import EventTriggerIndex from "../Components/EventTriggers/EventTriggerIndex";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import EventTriggerSave from "../Components/EventTriggers/EventTriggerSave";

import { MyCaller,CurrentUserInfo } from './../Help/Socket';
import Spinner from "react-bootstrap/cjs/Spinner";
import {DataHolder} from "../Help/DataHolder";

class EventTriggersPage extends Component {
    state={};
    constructor(props) {
        super(props);
        CurrentUserInfo.EventTriggersPage=this;
    }
    
    
    componentWillUnmount() {
        DataHolder.selectedEventTrigger=null;
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
                            {!this.state.loading
                            &&
                            <EventTriggerSave/>
                            }
                            
                            {this.state.loading && 
                            
                           <>
                               <Spinner animation="border" role="status">
                                   <span className="sr-only">لطفا منتظر بمانید ....</span>
                               </Spinner>
                               <span className="sr-only">لطفا منتظر بمانید ....</span>

                           </>}
                        </Col>
                    </Row>
                </Container>
                
                
                <hr/>
               {/* <img src={`${process.env.PUBLIC_URL}/Content/HelpImages/event_triggers.png`} />*/}
                
              
            </div>
        );
    }
}

export default EventTriggersPage;