import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class Answered extends Component {

    
    componentDidMount(){

    changeUserTypes('answered');
}

    render() {
        return (
            <div>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
