import React, {Component} from 'react';
import {CurrentUserInfo} from "../../Help/Socket";
import {_showMsg} from "../../Pages/LayoutPage";
import AutomaticCompaign from "./Automatic/AutomaticCompaign";
import ManualCompaign from "./Manual/ManualCompaign";

class CompaignSave extends Component {
    state = {};

    componentDidMount() {

        CurrentUserInfo.CompaignSave = this;

        this.props.parent.setState({Save: this});

        console.log('RoutingSave->this.props.selected', this.props.selected);
        this.setState({
            selected: this.props.selected
        });
    }

    compaignSaveCallback(res){
        _showMsg('ذخیره شد')
        this.props.parent.setState({
            selected: null
        });
    }
    
    render() {
        if (!this.state.selected){
            return  <></>;
        }

        
        return (
            <div>


                {!this.state.selected && <AutomaticCompaign ></AutomaticCompaign> }
                {this.state.selected && <ManualCompaign></ManualCompaign> }
                
                
            </div>
        );
    }
}

export default CompaignSave;



export const _GetSelectedCompaign=()=>{
    
    return CurrentUserInfo.CompaignSave.state.selected;
}