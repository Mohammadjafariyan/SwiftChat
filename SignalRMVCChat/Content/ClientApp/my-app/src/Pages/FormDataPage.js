import React, {Component} from 'react';
import {CreatedForms} from "./FormCreatorPage";
import  {Card,Container,Form,Button,Alert,ListGroup,Row,Col} from "react-bootstrap";
import '../styles/myStyle.css'
import FormDataTable from "../Components/FormDataTable";

class FormDataPage extends Component {
    render() {
        return (
            <div>   
            
                <Container>
                    <Row>

                        <Col md={3} >
                            <CreatedForms/>

                        </Col>

                        <Col md={9}>
                            <FormDataTable>
                                
                            </FormDataTable>

                        </Col>


                    </Row>
                </Container>
           
            </div>
        );
    }
}

export default FormDataPage;