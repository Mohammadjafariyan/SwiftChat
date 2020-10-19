import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import {_showError} from "../../Pages/LayoutPage";
import ListGroup from "react-bootstrap/ListGroup";
import {InputSwitch} from "primereact/inputswitch";
import Badge from "react-bootstrap/Badge";
import Button from "react-bootstrap/Button";
import {DataHolder} from "../../Help/DataHolder";

class EventTriggerIndex extends Component {
    
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EventTriggerIndex=this;
    }

    AddNewEventTrigger(){
        
        let list=this.state.list;
        
        
        let evTrig={Name:'Event Trigger جدید'};
        list.push(evTrig);
        
        this.setState({list:list});
        
        
        this.openEventTrigger(evTrig);
    }
    
    componentDidMount() {
        
        MyCaller.Send("EventTriggerGetAll")
    }

    eventTriggerGetAllCallback(res){
        if (!res || !res.Content){
            _showError("دیتای بازگشتی نال است سرور");
            return ;
        }
        
        this.setState({list:res.Content.EntityList})
    }

    render() {
        return (
            <div>
                
                

                {this.state.list &&
                <ListGroup>
                    {this.state.list.map((l,i,arr)=>{
                        
                        return <ListGroup.Item>
                            
                           <a href='#' className={'text-black'} onClick={()=>{
                               this.openEventTrigger(l);
                           }}>
                                {l.Name}
                           </a>

                         <div className={'float-right'}>
                             <InputSwitch checked={l.checked}
                                          onChange={(e) =>{
                                              if (!l.IsEnabled){
                                                  l.IsEnabled=true;
                                              }else{
                                                  l.IsEnabled=false;
                                              }
                                          }} />
                         </div>


                        </ListGroup.Item>;
                    })}

                </ListGroup>
                
                }

                <Button variant="info" onClick={()=>{
                    this.AddNewEventTrigger();
                }}>
                    
                    <Badge>
                        <i className={'fa fa-plus'}></i>
                    </Badge>
                    Event Trigger جدید</Button>
            </div>
        );
    }

    openEventTrigger(evTrig) {
      
        DataHolder.selectedEventTrigger=evTrig;

        if (CurrentUserInfo.EventTriggersPage)
            CurrentUserInfo.EventTriggersPage.setState({tmp:Math.random()});

    }
}

export default EventTriggerIndex;