import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class NotChatted extends Component {


    componentDidMount(){

        changeUserTypes('NotChatted');
    }

    render() {
        return (
            <div>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
