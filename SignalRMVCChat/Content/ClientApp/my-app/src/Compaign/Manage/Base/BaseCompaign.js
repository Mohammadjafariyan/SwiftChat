﻿import React, {Component} from 'react';
import CompaignConditions from "../ChildComps/CompaignConditions";
import CompaignEditor from "../ChildComps/CompaignEditor";
import CompaignAnaylics from "../ChildComps/CompaignAnaylics";
import CompaignRecipients from "../ChildComps/CompaignRecipients";
import CompaignTemplates from "../ChildComps/CompaignTemplates";
import {CurrentUserInfo, Socket} from "../../../Help/Socket";
import {Card} from "primereact/card";

class BaseCompaign extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.CurrentCompaign=this;
    }

    render() {
        return (
            <div>
                <Card >
                    {this.getSteps()}

                </Card>
                <br/>
                <Card >
                    <ShowSwitch step={this.state.step}/>
                </Card>
            </div>
        );
    }
    
    setStep(step){
        this.setState({step:step})

    }

    getSteps(){
        return <></>
    }

   
}

export default BaseCompaign;


const  ShowSwitch=(props)=> {

    switch (props.step) {
        case 'CompaignConditions':
            return <CompaignConditions next={'CompaignTemplates'} currentStep={1}/>
            break;
        case 'CompaignEditor':
            return <CompaignEditor next={'CompaignAnaylics'} currentStep={3}/>
            break;
        case 'CompaignAnaylics':
            return <CompaignAnaylics currentStep={4}/>
            break;
        case 'CompaignRecipients':
            return <CompaignRecipients next={'CompaignTemplates'} currentStep={1}/>
            break;
        case 'CompaignTemplates':
            return <CompaignTemplates  next={'CompaignEditor'} currentStep={2}/>
            break;
            
        default:
            return <></>
            break;
    }

}