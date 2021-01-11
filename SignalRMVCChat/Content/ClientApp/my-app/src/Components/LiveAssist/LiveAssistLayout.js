import React, {Component} from 'react';
import LiveAssistInitialize from "./LiveAssistInitialize";
import {MyCaller} from "../../Help/Socket";
import {Spinner} from "react-bootstrap";
import Container from "react-bootstrap/Container";
import LiveAssistShower from "./LiveAssistShower";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class LiveAssistLayout extends Component {
    state={};
    
    constructor(arg) {
        super(arg);
        
        CurrentUserInfo.LiveAssistLayout=this;
    }
    render() {
        return (
            <div>

                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">لطفا منتظر بمانید ....</span>
                </Spinner>}

                {!this.state.step &&

                <LiveAssistInitialize></LiveAssistInitialize>
                }

                
                
                <LiveAssistShower>
                    
                </LiveAssistShower>
                
            </div>
        );
    }
}

export default LiveAssistLayout;