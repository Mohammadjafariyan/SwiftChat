import React, {Component} from 'react';
import '../styles/whileWriting.css'

import {MyCaller, CurrentUserInfo} from "./../Help/Socket";
import { Badge } from 'react-bootstrap';


class WhileWriting extends Component {


    constructor(props, context) {
        super(props, context);
        this.state={};
        CurrentUserInfo.WhileWriting=this;
    }

    
    render() {
        return (
            <>
             {/*   {this.props.IsTyping &&  <div id="gapwhileWriting">
                    <span className="lds-ellipsis">
                
             <div className="whileWriting"></div>
             <div className="whileWriting"></div>
             <div className="whileWriting"></div>
             <div className="whileWriting"></div>
            </span>

                </div> }*/}

                {this.props.IsTyping && <Badge variant="light"><div className="typing-loader"></div> <small>در حال تایپ</small></Badge> }
               

            </>
        );
    }
}

export default WhileWriting;