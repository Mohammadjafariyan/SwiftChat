import React, {Component} from 'react';
import {InputSwitch} from "primereact/inputswitch";

class IsAuthenticated extends Component {
    state = {
        propName: 'IsAuthenticated',
        label:' کاربران عضو یا غیر عضو',
        optionLabel:'Name'
    }
    componentDidMount() {
        
    }

    render() {
        return (
            <>
                <InputSwitch checked={this.props.parent.state.selected[this.state.propName]}
                             onChange={(e) => {
                                 this.props.parent.state.selected[this.state.propName]=e.value;
                                 this.setState({mt:Math.random()})
                             }} />

                <label>{this.state.label}</label>
            </>
        );
    }
}

export default IsAuthenticated;