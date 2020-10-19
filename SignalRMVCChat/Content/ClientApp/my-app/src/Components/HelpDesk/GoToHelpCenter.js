import React, {Component} from 'react';
import {_getHelpDesk} from "../../Pages/HelpDeskPage";
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";

class GoToHelpCenter extends Component {
    state={};
    constructor(r) {
        super(r);
        CurrentUserInfo.GoToHelpCenter=this;
    }

    language_GetCurrentHelpDesk_SelectedLanguageCallback(res){
        
        
        
        
        this.setState({link:window.location.origin
                +"/HelpDesk/Index?websiteBaseUrl="+res.Content.baseUrl+"&lang="+_getHelpDesk().language.alpha2Code})
    }
    render() {
        if (!_getHelpDesk().helpDeskId)
            return <></>;

        return (
            <div>
                
                {this.state.link &&   
                
                
                <>
                    <a style={{float:'right'}} target={'_blank'} href={this.state.link}>
                        
                        <i className={'fa fa-paper-plane-o'}></i>
                        
                        Help Center وب سایت    </a>
                </>}
              
                
            </div>
        );
    }
}

export default GoToHelpCenter;