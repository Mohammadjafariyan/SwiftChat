import React, {Component} from 'react';
import {InputSwitch} from "primereact/inputswitch";

class IsResolved extends Component {
    state = {
        propName: 'IsResolved',
        label:'وضعیت گفتگو',
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

export default IsResolved;