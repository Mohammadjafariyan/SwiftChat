import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import {_showError, _showMsg} from "../../Pages/LayoutPage";
import ListGroup from "react-bootstrap/ListGroup";
import {InputSwitch} from "primereact/inputswitch";
import Badge from "react-bootstrap/Badge";
import Button from "react-bootstrap/Button";
import {DataHolder} from "../../Help/DataHolder";
import {SetEventTrigger} from "./EventTriggerForm";

class EventTriggerIndex extends Component {
    
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EventTriggerIndex=this;
    }

    deleteEventTrigger(eventHandler){
        
        let list=this.state.list.filter(l=>l!==eventHandler)
        this.setState({list:list});
    }

    eventTriggerSaveCallback(res){
        CurrentUserInfo.EventTriggersPage.setState({loading: false})
        
        DataHolder.selectedEventTrigger.Id=res.Id;

        _showMsg("با موفقیت ذخیره شد")
        
        this.componentDidMount();

    }


    eventTriggerDeleteCallback(res){
        CurrentUserInfo.EventTriggersPage.setState({loading: false})

        _showMsg("با موفقیت حذف شد")

    }

    AddNewEventTrigger(){
        
        let list=this.state.list;
        
        if (!list){
            list=[];
        }
        
        
        let evTrig={Name:'Event Trigger جدید',RunInDesktopDevices:true,RunInMobileDevices:true};
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
        
        
        
        this.setState({list:res.Content})
    }

    render() {
        return (
            <div>

                <Button variant="info" onClick={()=>{
                    this.AddNewEventTrigger();
                }}>

                    <Badge>
                        <i className={'fa fa-plus'}></i>
                    </Badge>
                    Event Trigger جدید</Button>

                {this.state.list &&
                <ListGroup>
                    {this.state.list.map((l,i,arr)=>{
                        
                        return <ListGroup.Item key={i}>
                            
                           <a href='#' className={'text-black'} onClick={()=>{
                               this.openEventTrigger(l);
                           }}>
                                {l.Name}
                           </a>

                         <div className={'float-right'}>
                             <InputSwitch checked={l.IsEnabled}
                                          onChange={(e) =>{
                                              
                                              
                                              if (!l.Id){
                                                 
                                                  _showError(" را زخیره کنید   (Event Trigger) لطفا ابتدا این رویداد ")
                                                  return ;
                                              }
                                              
                                              if (!l.IsEnabled){
                                                  l.IsEnabled=true;
                                              }else{
                                                  l.IsEnabled=false;
                                              }

                                              MyCaller.Send('EventTriggerSave', l);


                                              this.setState({tmp:Math.random()})
                                          }} />
                         </div>


                        </ListGroup.Item>;
                    })}

                </ListGroup>
                
                }

           
            </div>
        );
    }

    openEventTrigger(evTrig) {



        DataHolder.selectedEventTrigger=evTrig;

       /* setTimeout(()=>{
            debugger;
            SetEventTrigger(DataHolder.selectedEventTrigger)


        },100)*/

        this.setState({tmp:Math.random()})

        if (CurrentUserInfo.EventTriggersPage)
            CurrentUserInfo.EventTriggersPage.setState({tmp:Math.random()});

    }
}

export default EventTriggerIndex;