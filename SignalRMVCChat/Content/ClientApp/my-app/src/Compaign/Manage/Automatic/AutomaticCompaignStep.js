import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import CompaignStep from "../Base/CompaignStep";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class AutomaticCompaignStep extends CompaignStep {
    componentDidMount() {

        this.items[0] = 
            {
                label: 'زمانبندی و شروط',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                    CurrentUserInfo.CompaignStep.setState(
                        {activeIndex: 0})

                    CurrentUserInfo.CurrentCompaign.setStep('CompaignConditions')

                }
            };
    }
}

export default AutomaticCompaignStep;