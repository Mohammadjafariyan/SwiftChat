import React, {Component} from 'react';
import {changeUserTypes} from "../../Help/ChangeUserTypes";
import OnlineCustomerListHolder from "../../Pages/OnlineCustomerListHolder";

class AssingedToMeCustomerLists extends Component {

    componentDidMount(){

        changeUserTypes('AssingedToMe');
    }

    render() {
        return (
            <div>

                <h5>کاربران اختصاص داده شده به شما (بصورت اتوماتیک)</h5>
                <OnlineCustomerListHolder  />

            </div>
        )
    }
}

export default AssingedToMeCustomerLists;