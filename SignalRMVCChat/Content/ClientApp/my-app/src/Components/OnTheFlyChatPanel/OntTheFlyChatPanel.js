import React, {Component} from 'react';
import {Socket} from "../../Help/Socket";
import {DataHolder} from "../../Help/DataHolder";
import {Dialog} from "primereact/dialog";
import VoiceCallRunner from "../VoiceCallRunner";
import Chat, {gotoBottom} from "../Chat";
import Card from "react-bootstrap/Card";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class OntTheFlyChatPanel extends Component {
    state = {
        displayPosition:true,
        times:0
    };
    constructor(props) {
        super(props);

        CurrentUserInfo.OntTheFlyChatPanel=this;
    }

    showPanel(force){
      
        if (!force && this.state.times<1){
            
            
            this.setState({displayPosition:true})
            if (DataHolder.selectedCustomer){
                this.setState({times:this.state.times+1})
                setTimeout(()=>{
                    gotoBottom("chatPanel");
                },500)
            }
        }
        
        if (force) {
            this.setState({displayPosition: true});
            setTimeout(()=>{
                gotoBottom("chatPanel");
            },500)

        }

    }
    componentDidMount() {
        CurrentUserInfo.OntTheFlyChatPanel = this;

        if (!DataHolder.selectedCustomer)
            return <></>;

        this.setState({displayPosition:true})

        this.onClick('displayPosition', 'bottom-right')
    }

    show() {
        this.onClick('displayPosition', 'bottom-right')

    }

    render() {
    

        return (
            <div>

                
                <Dialog rtl={true} modal={false} header={DataHolder.selectedCustomer ? DataHolder.selectedCustomer.Name:'چت باکس'} visible={this.state.displayPosition}
                        position={'bottom-right'}
                        style={{width: '40vw'}}
                        maximizable={true}
                        onHide={() => this.setState({displayPosition:false})}>

                    {DataHolder.currentPage && DataHolder.selectedCustomer && <Chat chats={DataHolder.selectedCustomerChats?DataHolder.selectedCustomerChats:[]}/>}


                    {!DataHolder.currentPage || !DataHolder.selectedCustomer && this.showMessage('هیچ کاربری انتخاب نشده است')}
                    {DataHolder.currentPage || !DataHolder.selectedCustomer && this.showMessage('در صفحه چت امکان استفاده نیست')}
                </Dialog>


            </div>
        );
    }
    
    showMessage(msg){
        return  <Card
            bg={'warning'}
        >
            <Card.Body>
                {/*<Card.Title> Card Title </Card.Title>*/}
                <Card.Text>
                    <p>{msg}</p>
                </Card.Text>
            </Card.Body>
        </Card>;
    }

    showError() {
        return <div>
            <p style={{color: 'red'}}>{this.state.error}</p>
            <small style={{color: 'red'}}>{this.state.techError}</small>
        </div>
    }

    onClick(name, position) {
        let state = {
            [`${name}`]: true
        };

        if (position) {
            state = {
                ...state,
                position
            }
        }

        this.setState(state);
    }
}

export default OntTheFlyChatPanel;