import React, {Component} from 'react';
import SelectAdmin from "./SelectAdmin";
import RemindMe from "./RemindMe";
import ReadyPms from "./ReadyPms";

class ChatFormTypeSwitch extends Component {
    render() {
        return (
            <div>


                {this.props.type.label==='یادداشت' && <SelectAdmin/>}
                {this.props.type.label==='یادآور' && <RemindMe/>}
                {this.props.type.label==='آماده' && <ReadyPms/>}

                
            </div>
        );
    }
}

export default ChatFormTypeSwitch;