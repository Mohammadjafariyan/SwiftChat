import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import CompaignStep from "../Base/CompaignStep";

class ManualCompaignStep extends CompaignStep {
    componentDidMount() {

        this.items[0] =
            {
                label: 'دریافت کنندگان',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                }
            };
    }
}

export default ManualCompaignStep;