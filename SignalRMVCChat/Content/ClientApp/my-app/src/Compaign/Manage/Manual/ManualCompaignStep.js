import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import CompaignStep from "../Base/CompaignStep";
import {CurrentUserInfo} from "../../../Help/Socket";

class ManualCompaignStep extends CompaignStep {
    componentDidMount() {

        this.items[0] =
            {
                label: 'دریافت کنندگان',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                    CurrentUserInfo.CompaignStep.setState(
                        {activeIndex: 0})
                    CurrentUserInfo.CurrentCompaign.setStep('CompaignRecipients')
                }
            };
    }
}

export default ManualCompaignStep;