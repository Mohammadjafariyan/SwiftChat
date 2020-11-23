import React, {Component} from 'react';
import {InputSwitch} from "primereact/inputswitch";
import {InputText} from "primereact/inputtext";

class RoutingInfo extends Component {
    state = {
        propName: 'Name',
        label:'عنوان',
        optionLabel:'Name'
    }
    componentDidMount() {

    }
    
    render() {
        return (
            <>
                <InputText style={{width:'100%',textAlign:'right'}} value={this.props.parent.state.selected[this.state.propName]}
                           onChange={(e) => {
                               this.setState({msg:Math.random()})
                               
                               this.props.parent.state.selected[this.state.propName]=e.target.value;
                           }} />

            </>
        );
    }
}

export default RoutingInfo;