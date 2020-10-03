import React, { Component } from 'react'
import OnlineCustomerListHolder from './OnlineCustomerListHolder'
import { DataHolder } from './../Help/DataHolder';
import { changeUserTypes } from './../Help/ChangeUserTypes';

export default class SeparatePerPageCustomerListPage extends Component {

    componentDidMount(){

        changeUserTypes('SeparatePerPageCustomerListPage');
    }


    render() {
        return (
            <div>

            <h3>{DataHolder.SelectedPage && <span>{DataHolder.SelectedPage.Url} 
            - {DataHolder.SelectedPage.Name} </span>}</h3>
<OnlineCustomerListHolder  />
                
            </div>
        )
    }
}
