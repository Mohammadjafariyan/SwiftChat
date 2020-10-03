import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class AllCustomerListPage extends Component {

    componentDidMount(){

    changeUserTypes('AllCustomerListPage');
}

    render() {
        return (
            <div>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
