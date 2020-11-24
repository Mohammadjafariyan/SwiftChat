import React, {Component} from 'react';
import BaseCompaign from "../Base/BaseCompaign";
import ManualCompaignStep from "../Manual/ManualCompaignStep";
import AutomaticCompaignStep from "./AutomaticCompaignStep";

class AutomaticCompaign extends BaseCompaign {

    componentDidMount() {
        this.setState({step:'CompaignConditions'})
    }


    getSteps(){
        return <><AutomaticCompaignStep/></>
    }
}

export default AutomaticCompaign;