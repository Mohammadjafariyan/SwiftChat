import React, {Component} from 'react';
import Card from "react-bootstrap/Card";
import {AddLocalizedMessage, MyInput, MySwitcher} from "../../Utilities/Utility";
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import Table from "react-bootstrap/Table";
import Badge from "react-bootstrap/Badge";
import {_showError} from "../../../Pages/LayoutPage";
import {SetEventTrigger} from "../EventTriggerForm";
import {DataHolder} from "../../../Help/DataHolder";


class EtFormTriggerActions extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormTriggerActions=this;
    }

    setIsShowMessageEnabled(IsShowMessageEnabled){
        this.setState({IsShowMessageEnabled})
    }

    setIsOpenChatBox(IsOpenChatBox){
        this.setState({IsOpenChatBox})
    }

    setIsPlayASound(IsPlayASound){
        this.setState({IsPlayASound})
    }
    
    getIsShowMessageEnabled(){
        return this.state.IsShowMessageEnabled;
    }
    
    getIsOpenChatBox(){
        return this.state.IsOpenChatBox;
    }

    getIsPlayASound(){
        return this.state.IsPlayASound;
    }

    getlocalizedMessages(){
        return this.state.localizedMessages;
    }
    setlocalizedMessages(s){
        this.setState({localizedMessages:s});
    }

    isValid(){
        if (DataHolder.selectedEventTrigger.IsShowMessageEnabled && (!DataHolder.selectedEventTrigger.localizedMessages || DataHolder.selectedEventTrigger.localizedMessages.length==0))
        {
            _showError('در صورت نمایش پیغام ، یک پیغام وارد کنید')
            return false
        }
        return true;
    }
    componentDidMount() {
        SetEventTrigger(DataHolder.selectedEventTrigger);
    }
    render() {
        return (
            <div>
                <Card
                    bg={'light'}
                >
                    <Card.Header>
                        <span className={'float-right'}>
                          Event Trigger عملیات اصلی این  
                        </span>

                        <small className={'float-left '} style={{color:'#6c757d'}}>
                            میتوانید چند عملیات مختلف تعریف کنید
                        </small>

                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MySwitcher title={'نمایش یک پیغام'} name={'IsShowMessageEnabled'} parent={this}>
                                
                                
                                <AddLocalizedMessage onConfirm={(textArea,lang)=>{
                                    
                                    let localizedMessages= DataHolder.selectedEventTrigger.localizedMessages;
                                    if(!localizedMessages){
                                        localizedMessages=[];
                                    }

                                    localizedMessages.push({textArea,lang});
                                   
                                    DataHolder.selectedEventTrigger['localizedMessages']=localizedMessages;
                                    this.setState({localizedMessages});
                                }}>
                                    
                                    
                                    {DataHolder.selectedEventTrigger.localizedMessages && DataHolder.selectedEventTrigger.localizedMessages.length>0 && 
                                    this.showLocalizedMessages()}
                                    
                                    
                                </AddLocalizedMessage>
                                
                                
                                
                            </MySwitcher>

                            <hr/>
                            <MySwitcher title={'بازگردن گفتگو'} name={'IsOpenChatBox'} parent={this}>

                            </MySwitcher>
                            <hr/>

                            <MySwitcher title={'پخش کردن یک آهنگ'} name={'IsPlayASound'} parent={this}>

                            </MySwitcher>
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }

    showLocalizedMessages() {
         
        return <Table striped bordered hover>
            <thead>
            <tr>
                <th>پرچم</th>
                <th>متن</th>
                <th>زبان</th>
                <th>حذف</th>
            </tr>
            </thead>
            <tbody>
            {
                DataHolder.selectedEventTrigger.localizedMessages.map((m,i,arr)=>{


                    return  <tr>
                        <th><img width={50} height={30} alt={m.lang.name} src={m.lang.flag}  className={`flag flag-${m.lang.alpha2Code.toLowerCase()}`} /></th>
                        <th>{m.textArea}</th>
                        <th>{m.lang.name} {m.lang.Name}</th>
                        <th>
                            <Badge variant={'danger'} onClick={()=>{

                                let localizedMessages= DataHolder.selectedEventTrigger.localizedMessages;
                                if(!localizedMessages){
                                    localizedMessages=[];
                                }

                                localizedMessages=  localizedMessages.filter(lm=>lm!==m);

                                DataHolder.selectedEventTrigger.localizedMessages=localizedMessages
                                this.setState({localizedMessages});
                                
                            }}>
                                
                                <i className={'fa fa-trash'}></i>
                            </Badge>
                            
                        </th>
                    </tr>
                })
            }
            </tbody>
         
        </Table>
           
    }
}

export default EtFormTriggerActions;