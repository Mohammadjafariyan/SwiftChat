import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Customers from "../Components/Customers";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/Col";
import Chat from "../Components/Chat";
import LiveAssistLayout from "../Components/LiveAssist/LiveAssistLayout";
import Container from "react-bootstrap/Container";
import '../styles/myStyle.css'
class LiveAssistPage extends Component {
    render() {
        return (
            <div>


                {!DataHolder.selectedCustomer && <>

                    <Row>

                        <Col>
                            <Customers />

                        </Col>
                        <Col>
                            <p> ابتدا یک کاربر را انتخاب کنید</p>

                        </Col>
                    </Row>


                </>}

                {DataHolder.selectedCustomer && <>

                    
                    <Container>
                        <Row>


                            <Col md={9} className={'scallSmall'}>
                                <LiveAssistLayout/>

                            </Col>

                            <Col md={3} >
                                <Chat />

                            </Col>
                        </Row>
                    </Container>

                </>}


            </div>
        );
    }
}

export default LiveAssistPage;