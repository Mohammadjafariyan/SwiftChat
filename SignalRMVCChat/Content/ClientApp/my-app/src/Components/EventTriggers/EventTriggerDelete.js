import React, {Component} from 'react';
import {DataHolder} from "../../Help/DataHolder";
import Button from "react-bootstrap/Button";
import {MyCaller} from "../../Help/Socket";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class EventTriggerDelete extends Component {
    render() {
        if (!DataHolder.selectedEventTrigger)
            return  <></>
        return (
            <div>
                
                
                <Button
                    
                    id="deleteEventTrigger"
                    variant={'danger'}
                
                    onClick={()=>{
                        this.deleteEventTrigger();

                        CurrentUserInfo.EventTriggerIndex.deleteEventTrigger(DataHolder.selectedEventTrigger)
                        DataHolder.selectedEventTrigger=null;
                        CurrentUserInfo.EventTriggersPage.setState({tmp:Math.random()});
                    }}
                >
                    Event Trigger حذف 
                    <i className={'fa fa-trash'}></i>
                </Button>
                
                
                
                
                
                
            </div>
        );
    }

    deleteEventTrigger() {
        if (!DataHolder.selectedEventTrigger)
            return ;
        
        MyCaller.Send('EventTriggerDelete',{id:DataHolder.selectedEventTrigger.Id})
        
    }
}

export default EventTriggerDelete;