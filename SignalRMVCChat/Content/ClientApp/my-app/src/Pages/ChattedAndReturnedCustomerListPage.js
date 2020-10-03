import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class ChattedAndReturnedCustomerListPage extends Component {

    
    
    componentDidMount(){

    changeUserTypes('ChattedAndReturnedCustomerListPage');
}
    render() {
        return (
            <div>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
