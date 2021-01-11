import React, {Component} from 'react';
import '../styles/myStyle.css'
import { MyCaller} from "./../Help/Socket";
import Badge from "react-bootstrap/Badge";
import {CurrentUserInfo} from "../CurrentUserInfo";

class TagSingleCustomer extends Component {


    render() {
       let isActive= CurrentUserInfo.NewTagModeEnabled===this.props.customerId ?
            ' userTagBlue ' :' df '
        return (
            <div>
                
                
                <Badge  aria-label="چسباندن برچسب به کاربر ، برای دسته بندی کاربران مفید است" data-microtip-position="top" role="tooltip" variant={'info'} onClick={() => {
                    CurrentUserInfo.NewTagModeCustomerId =this.props.customerId;

                    if (CurrentUserInfo.NewTagModeEnabled){
                        CurrentUserInfo.NewTagModeEnabled=!CurrentUserInfo.NewTagModeEnabled;
                    }else{

                        CurrentUserInfo.NewTagModeEnabled = true;
                    }

                    CurrentUserInfo.TagList.setState({tmp: Math.random()})

                }}>
                    <i  className={"fa fa-tag " +isActive}
                       aria-hidden="true"/>
                       
                       
                       <Badge>
                           <i className={'fa fa-plus'}></i>
                       </Badge>
                </Badge>

            </div>
        );
    }
}

export default TagSingleCustomer;