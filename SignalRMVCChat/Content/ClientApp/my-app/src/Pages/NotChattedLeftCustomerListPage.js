import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class NotChattedLeftCustomerListPage extends Component {

    
    
    componentDidMount(){

        changeUserTypes('NotChattedLeftCustomerListPage');
    }
    render() {
        return (
            <div>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
