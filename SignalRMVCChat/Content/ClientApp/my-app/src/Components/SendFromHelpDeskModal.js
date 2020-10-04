import React, {Component} from 'react';
import {Button} from "primereact/button";
import {Dialog} from "primereact/dialog";
import SendFromHelpDesk from "./SendFromHelpDesk";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {DataTable} from "primereact/datatable";


class PrimengModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
            displayBasic: false,
            displayBasic2: false,
            displayModal: false,
            displayConfirmation: false,
            displayMaximizable: false,
            displayPosition: false,
            position: 'center'
        };

        this.onClick = this.onClick.bind(this);
        this.onHide = this.onHide.bind(this);
        
        CurrentUserInfo.SendFromHelpDeskModal=this;
    }

    renderFooter(name) {
        return (
            <div>
                <Button label="لغو" icon="pi pi-times" onClick={() => this.onHide(name)} className="p-button-text" />
                <Button label="تایید" icon="pi pi-check" onClick={() => this.onHide(name)} autoFocus />
            </div>
        );
    }
    onHide(name) {
        this.setState({
            [`${name}`]: false
        });
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
    
    show(){
        this.onClick('displayBasic');
    }
    
    render() {
        return (
            <div>
                {/*<Button label="Show" icon="pi pi-external-link" onClick={() => this.onClick('displayBasic')} />*/}

                <Dialog maximizable modal  header={this.props.title} visible={this.state.displayBasic} style={{ width: '50vw' }} footer={this.renderFooter('displayBasic')} onHide={() => this.onHide('displayBasic')}>
                   

                    
{this.props.children}                    
                </Dialog>

            </div>
        );
    }
}

export default PrimengModal;