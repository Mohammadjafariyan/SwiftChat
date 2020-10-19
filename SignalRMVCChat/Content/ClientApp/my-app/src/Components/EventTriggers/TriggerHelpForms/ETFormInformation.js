import React, {Component, useState} from 'react';
import Card from "react-bootstrap/Card";

import '../../../styles/myStyle.css'
import {InputText} from "primereact/inputtext";
import {MyInput} from "../../Utilities/Utility";

import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";

class EtFormInformation extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormInformation=this;
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
                        
                        <small className={'float-left text-light'}>
                            نام Event Trigger را وارد نمایید
                        </small>
                        
                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MyInput title={'نام Event Trigger'} name={'Name'} parent={this}/>
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }
}

export default EtFormInformation;

