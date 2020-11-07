import React, {Component} from 'react';

import '../styles/myStyle.css'
import {ScrollPanel} from "primereact/scrollpanel";
import ListGroup from "react-bootstrap/cjs/ListGroup";
import {InputSwitch} from "primereact/inputswitch";
import {CurrentUserInfo} from "../Help/Socket";


class BotList extends Component {
    state = {

        list: [
            {Name: 'ربات یک', IsPublish: true, Id: 1},
            {Name: 'ربات 2', IsPublish: true, Id: 2},
            {Name: 'ربات 3', IsPublish: false, Id: 3},
            {Name: 'ربات 4', IsPublish: false, Id: 4},
        ]
    }

    render() {
        return (
            <div className={'scrollpanel-demo'}>
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
        return <ListGroup.Item as="li" >
            
            
            <i className={'onHoverBlue fa fa-hand-pointer-o'} style={{float:'left'}} onClick={() => {


                CurrentUserInfo.BotLayout.setState({selectedBot: m});


            }}></i>
            
            <span  className={'onHoverBlue'} onClick={() => {


                CurrentUserInfo.BotLayout.setState({selectedBot: m});


            }}>
                {m.Name}
            </span>
            <br/>


            <InputSwitch checked={m.IsPublish} onChange={(e) => {
                m.IsPublish = e.value;
                this.setState({value: e.value, m: Math.random()})
            }}/>

        </ListGroup.Item>;
    }

    showList(bool, msg) {
        return this.state.list && this.state.list.length > 0 && <>

            {this.state.list.filter(m => m.IsPublish == bool).length > 0 &&  <h6 style={{color: '#ddd'}}>{msg}</h6>}
           

            {this.state.list.filter(m => m.IsPublish == bool).map((m, arr, i) => {
                return this.showMenuItem(m)
            })}

        </>
    }
}

export default BotList;