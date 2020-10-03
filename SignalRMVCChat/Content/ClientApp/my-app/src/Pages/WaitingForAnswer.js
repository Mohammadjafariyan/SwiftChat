import React, { Component } from 'react'
import OnlineCustomerList from './../Components/OnlineCustomerList';
import OnlineCustomerListHolder from './OnlineCustomerListHolder';
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class WaitingForAnswer extends Component {



    componentDidMount(){

        changeUserTypes('WaitingForAnswer');
    }

    render() {
        return (
            <div>
                


<OnlineCustomerListHolder  />

            </div>
        )
    }
}




