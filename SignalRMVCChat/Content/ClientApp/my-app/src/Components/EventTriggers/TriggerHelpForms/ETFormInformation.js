import React, {Component, useState} from 'react';
import Card from "react-bootstrap/Card";

import '../../../styles/myStyle.css'
import {InputText} from "primereact/inputtext";
import {MyInput} from "../../Utilities/Utility";

import {MyCaller} from "../../../Help/Socket";
import {_showError} from "../../../Pages/LayoutPage";
import {SetEventTrigger} from "../EventTriggerForm";
import {DataHolder} from "../../../Help/DataHolder";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class EtFormInformation extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormInformation=this;
    }
    setName(Name){
        this.setState({Name})
    }
    getName(){
        return this.state.Name;
    }
    
    isValid(){
        if (!DataHolder.selectedEventTrigger.Name)
        {
            _showError('عنوان خالی است')
            return false
        }
        return true
    }
    
    componentDidMount() {
        SetEventTrigger(DataHolder.selectedEventTrigger);
    }

    render() {
        return (
            <div>
                <Card
                    bg={'light'}
                >
                    <Card.Header>
                        <span className={'float-right'}>
                            تنظیمات عمومی 
                        </span>
                        
                        <small className={'float-left '} style={{color:'#6c757d',direction:'rtl'}}>
                            نام راه انداز رویداد یا (Event Trigger) را وارد نمایید
                        </small>
                        
                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MyInput title={'Event Trigger نام '} name={'Name'} parent={this}/>
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }
}

export default EtFormInformation;

