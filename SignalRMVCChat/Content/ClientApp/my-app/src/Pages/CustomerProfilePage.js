import React, {Component} from 'react';
import CustomerProfile from "../Components/CustomerProfile";
import CustomerTimeSpent from "../Components/Profile/CustomerTimeSpent";
import {DataHolder} from "../Help/DataHolder";

class CustomerProfilePage extends Component {
    render() {
        return (
            <div>
                <CustomerProfile  Customer={DataHolder.selectedCustomer}/>
            </div>
        );
    }
}

export default CustomerProfilePage;