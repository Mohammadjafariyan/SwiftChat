import React, {Component} from 'react';

import '../styles/myStyle.css'
import {ScrollPanel} from "primereact/scrollpanel";
import ListGroup from "react-bootstrap/cjs/ListGroup";
import {InputSwitch} from "primereact/inputswitch";
import {MyCaller} from "../Help/Socket";
import {Button} from "primereact/button";
import Col from "react-bootstrap/Col";
import {TypeNames} from "./Design/BotOrgChart";
import {CurrentUserInfo} from "../CurrentUserInfo";


class BotList extends Component {
    state = {

        list: [

        ]
    }

    constructor(props) {
        super(props);
        CurrentUserInfo.BotList=this;
    }


    componentDidMount() {

        MyCaller.Send('BotList')

    }

    botListCallback(res) {
        this.setState({loading:false})

        if (!res || !res.Content) {

            console.error(' مقدار بازگشتی از سرور نال است ');
            return;
        }

        this.setState({list: res.Content});
    }
    
    botSaveCallback(res){

        this.componentDidMount();
    }
    
    saveBot(bot){


        this.setState({loading:true})
        MyCaller.Send('BotSave', bot);

    }

    render() {
        return (
            <div className={'scrollpanel-demo'}>


                <Button disabled={this.state.loading} className={'p-button-raised  p-button-text'} label="جدید" icon="pi pi-plus"
                        onClick={() => {

                          this.saveBot({
                              Name: 'ربات جدید', IsPublish: false,
                              label: 'نود شروع',
                              type: TypeNames.Start,
                              className: 'p-person',
                              expanded: true,
                              data: {
                                  panelCollapsed:false
                              }
                          });
                        }}/>


                <ScrollPanel style={{width: '100%', minHeight: '100vh'}}>


                    <ListGroup as="ul">

                        {this.showList(true, 'ربات های فعال')}
                        {this.showList(false, 'ربات های غیر فعال')}

                    </ListGroup>
                </ScrollPanel>
            </div>
        );
    }

    showMenuItem(m) {
        return <ListGroup.Item as="li">


            <i className={'onHoverBlue fa fa-hand-pointer-o'} style={{float: 'left'}} onClick={() => {


                CurrentUserInfo.BotLayout.setState({selectedBot: m});


            }}></i>

            <span className={'onHoverBlue'} onClick={() => {


                CurrentUserInfo.BotLayout.setState({selectedBot: m});


            }}>
                {m.Name}
            </span>
            <br/>


            <InputSwitch checked={m.IsPublish} onChange={(e) => {
                m.IsPublish = e.value;
                this.setState({value: e.value, m: Math.random()})
                
                
                this.saveBot(m);
            }}/>

        </ListGroup.Item>;
    }

    showList(bool, msg) {
        return this.state.list && this.state.list.length > 0 && <>

            {this.state.list.filter(m => m.IsPublish == bool).length > 0 && <h6 style={{color: '#ddd'}}>{msg}</h6>}


            {this.state.list.filter(m => m.IsPublish == bool).map((m, arr, i) => {
                return this.showMenuItem(m)
            })}

        </>
    }
}

export default BotList;