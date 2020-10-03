import React, {Component} from 'react';
import '../styles/myStyle.css'
import { MyCaller, CurrentUserInfo } from "./../Help/Socket";

class TagSingleCustomer extends Component {


    render() {
       let isActive= CurrentUserInfo.NewTagModeEnabled===this.props.customerId ?
            ' userTagBlue ' :' df '
        return (
            <div>
                <i onClick={() => {
                    CurrentUserInfo.NewTagModeCustomerId =this.props.customerId;

                    if (CurrentUserInfo.NewTagModeEnabled){
                        CurrentUserInfo.NewTagModeEnabled=!CurrentUserInfo.NewTagModeEnabled;
                    }else{

                        CurrentUserInfo.NewTagModeEnabled = true;
                    }

    CurrentUserInfo.TagList.setState({tmp: Math.random()})

}} aria-label="برچسب زدن به کاربر" data-microtip-position="bottom" role="tooltip" className={"userTag gapBottomIcon fa fa-tag " +isActive}
                   aria-hidden="true"/>

            </div>
        );
    }
}

export default TagSingleCustomer;