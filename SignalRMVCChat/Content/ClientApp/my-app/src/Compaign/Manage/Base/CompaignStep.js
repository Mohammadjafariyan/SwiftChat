import React, {Component} from 'react';
import {Steps} from "primereact/steps";
import {CurrentUserInfo} from "../../../Help/Socket";

class CompaignStep extends Component {
    constructor(props) {
        super(props);
        this.state = {
            activeIndex: 1
        };
        
        CurrentUserInfo.CompaignStep=this;

        this.items = [
            {
                label: 'دریافت کنندگان',
                command: (event) => {
                    //   this.toast.show({ severity: 'info', summary: 'First Step', detail: event.item.label });
                }
            },
            {
                label: 'قالب و متن',
                command: (event) => {
                    //  this.toast.show({ severity: 'info', summary: 'Seat Selection', detail: event.item.label });
                }
            },
            {
                label: 'ویرایشگر',
                command: (event) => {
                    //  this.toast.show({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
                }
            },
            {
                label: 'آنالیز',
                command: (event) => {
                    //    this.toast.show({ severity: 'info', summary: 'Last Step', detail: event.item.label });
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
                       readOnly={true}/>

            </>
        );
    }
}

export default CompaignStep;