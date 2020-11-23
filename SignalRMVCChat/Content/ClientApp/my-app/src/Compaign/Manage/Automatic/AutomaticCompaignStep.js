import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import CompaignStep from "../Base/CompaignStep";

class AutomaticCompaignStep extends CompaignStep {
    componentDidMount() {

        this.items[0] = 
            {
                label: 'زمانبندی و شروط',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                }
            };
    }
}

export default AutomaticCompaignStep;