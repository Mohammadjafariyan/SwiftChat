import React, {Component} from 'react';
import CustomerToolbar from "../Components/CustomerToolbar";
import Chat from "../Components/Chat";
import Customers from "../Components/Customers";
import MyMap from "../Components/Map/MyMap";
import {Container, Spinner} from "react-bootstrap";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {CurrentUserInfo} from "../Help/Socket";
import MyMapHolder from "../Components/Map/MyMapHolder";
import MyMapCustomerTypes from './../Components/Map/MyMapCustomerTypes';

class MapPage extends Component {
   
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.MapPage=this;
    }
    

  
    render() {
        return (
            <div>
                <Container>





                    <Row >

                        <Col md={9}>

                        {this.state.loading &&
                    <Spinner animation="border" role="status" variant='warning'>
                        <span className="sr-only">لطفا منتظر بمانید ....</span>
                    </Spinner>}
                    
                        {!this.state.loading &&        <MyMapHolder/>}

                        </Col>
                        
                        <Col md={3}>

 <MyMapCustomerTypes/>
                        
                            {/* <Customers noReadChat={true} /> */}
                        </Col>

                    </Row>
                </Container>
                
                
            </div>
        );
    }
}



export default MapPage;