import React, {Component} from 'react';
import CustomerTags from "./CustomerTags";
import {DataHolder} from "../Help/DataHolder";
import AddTag from "./AddTag";
import TagSingleCustomer from "./TagSingleCustomer";
import {Card} from "react-bootstrap";
import TagList from "./TagList";

class UserTags extends Component {
    
    
    render() {
        
        if (!this.props.Customer){
            return  <></>
        }
        
        return (
            <div>
                <div
                    className={"card "}>
                    <div className="card-header">
                        برچسب های کاربر
                    </div>

                    <div className="card-body">
                        <TagSingleCustomer customerId={this.props.Customer.Id}/>

                        <CustomerTags customer={this.props.Customer}/>
                        
                        
                        <TagList/>

                    </div>
                </div>

            </div>
        );
    }
}

export default UserTags;