import React, {Component} from 'react';
import CustomerToolbar from "../Components/CustomerToolbar";
import Chat from "../Components/Chat";
import Customers from "../Components/Customers";
import MyMap from "../Components/Map/MyMap";
import {Container} from "react-bootstrap";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {CurrentUserInfo} from "../Help/Socket";
import MyMapHolder from "../Components/Map/MyMapHolder";

class MapPage extends Component {
   

  
    render() {
        return (
            <div>
                <Container>





                    <Row >

                        <Col md={9}>
                            <MyMapHolder/>

                        </Col>
                        
                        <Col md={3}>
                            <Customers noReadChat={true} />
                        </Col>

                    </Row>
                </Container>
                
                
            </div>
        );
    }
}



export default MapPage;