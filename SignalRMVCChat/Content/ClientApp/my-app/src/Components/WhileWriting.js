import React, {Component} from 'react';
import '../styles/whileWriting.css'

import {MyCaller, CurrentUserInfo} from "./../Help/Socket";


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

                {this.props.IsTyping && <div className="typing-loader"></div> }
               

            </>
        );
    }
}

export default WhileWriting;