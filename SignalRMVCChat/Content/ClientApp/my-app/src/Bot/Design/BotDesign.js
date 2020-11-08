import React, {Component} from 'react';
import BotOrgChart from "./BotOrgChart";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Button} from "primereact/button";
import {_showError, _showMsg} from "../../Pages/LayoutPage";

class BotDesign extends Component {

    save(){
        if (!this.BotOrgChart){
            _showError("هیچ دیزاینی یافت نشد لطفا مجددا امتحان کنید")
            return ;
        }
        
        _showMsg('در حال ذخیره')
    }
    render() {
        return (
            <div>
                
                <Row>

                    <Col>

                        <Button className={'p-button-raised  p-button-danger p-button-text'} label="لغو" icon="pi pi-trash" />


                    </Col>
                    <Col>
                        <Button className={'p-button-raised   p-button-warning p-button-text'} label="کنسل" icon="pi pi-times" />
                        <Button className={'p-button-raised  p-button-text'} label="ذخیره" icon="pi pi-check"  onClick={()=>{
                            
                            this.save();
                        }}/>

                    </Col>
                </Row>
                <br/>
                <BotOrgChart parent={this}/>
                
            </div>
        );
    }
}

export default BotDesign;