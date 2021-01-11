import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class CompaignStep extends Component {
    constructor(props) {
        super(props);
        this.state = {
            activeIndex: 0
        };
        
        CurrentUserInfo.CompaignStep=this;

        this.items = [
            {
                label: 'دریافت کنندگان',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                    CurrentUserInfo.CompaignStep.setState(
                        {activeIndex: 0})

                    CurrentUserInfo.CurrentCompaign.setStep('CompaignRecipients')
                }
            },
            {
                label: 'قالب ایمیل یا پیغام',
                command: (event) => {
                    //  this.toast.show({ severity: 'info', summary: 'Seat Selection', detail: event.item.label });
                    CurrentUserInfo.CompaignStep.setState(
                        {activeIndex: 1})
                    CurrentUserInfo.CurrentCompaign.setStep('CompaignTemplates')
                }
            },
            {
                label: 'متن ایمیل یا پیغام',
                command: (event) => {
                    //  this.toast.show({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                    CurrentUserInfo.CompaignStep.setState(
                        {activeIndex: 2})

                    CurrentUserInfo.CurrentCompaign.setStep('CompaignEditor')
                }
            }
        ];
    }
  

    render() {
        return (
            <>
                <Steps model={this.items}
                       activeIndex={this.state.activeIndex}
                       onSelect={(e) => this.setState({activeIndex: e.index})}
                       readOnly={false}/>

            </>
        );
    }
}

export default CompaignStep;