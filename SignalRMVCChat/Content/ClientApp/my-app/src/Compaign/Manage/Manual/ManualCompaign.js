import React, {Component} from 'react';
import BaseCompaign from "../Base/BaseCompaign";
import ManualCompaignStep from "./ManualCompaignStep";

class ManualCompaign extends BaseCompaign {

    
    componentDidMount() {
        this.setState({step:'CompaignRecipients'})
    }


    getSteps(){
        return <><ManualCompaignStep/></>
    }
}

export default ManualCompaign;