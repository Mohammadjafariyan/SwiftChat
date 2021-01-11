import React, {Component} from 'react';
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class CompaignChildCompBase extends Component {


 
    goNext() {
        if (this.props.next)
            CurrentUserInfo.CurrentCompaign.setStep(this.props.next)


        if (this.props.currentStep)
            CurrentUserInfo.CompaignStep.setState({activeIndex:this.props.currentStep})


    }
}

export default CompaignChildCompBase;