import React, {Component} from 'react';
import {DataHolder} from "../../Help/DataHolder";
import Card from "react-bootstrap/Card";
import EventTriggerForm from "./EventTriggerForm";

class EventTriggerSave extends Component {
    render() {
        return (
            <div>
                {!DataHolder.selectedEventTrigger  && this.showMessage()}
                {DataHolder.selectedEventTrigger && <EventTriggerForm/>}
                
                
            </div>
        );
    }

    showMessage() {
        
        if (DataHolder.selectedEventTrigger)
return<></>
        
        
        return   <Card
            bg={'warning'}
        >
            <Card.Header>What is a Trigger and how can it be useful?</Card.Header>
            <Card.Body>
                {/*<Card.Title> Card Title </Card.Title>*/}
                <Card.Text>
                    <div className="cap-plugin-content-box-inner">
                        <p className="cap-font-sans-regular">Triggers can be used to <strong
                            className="cap-font-sans-semibold">schedule an automated action to all visitors that match
                            rules</strong> for a given event. Eg. you may create a Trigger to message people that are
                            viewing your Pricing page for 1 min.</p>

                        <p className="cap-font-sans-regular">You can use them to <strong
                            className="cap-font-sans-semibold">avoid churn on your website</strong> and message leaving
                            customers.</p>

                     
                    </div>
                </Card.Text>
            </Card.Body>
        </Card>;
    }
}

export default EventTriggerSave;

