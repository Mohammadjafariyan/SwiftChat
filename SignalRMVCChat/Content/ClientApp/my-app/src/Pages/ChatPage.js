import React, { Component } from "react";
import MyHeader from "./../Components/Header";
import Chat from "./../Components/Chat";
import Customers from "./../Components/Customers";
import Admins from './../Components/Admins';

import './../styles/myStyle.css';
import CustomerToolbar from "../Components/CustomerToolbar";
import CustomerProfile from "../Components/CustomerProfile";
export default class ChatPage extends Component {
    constructor(props) {
        super(props);
        this.state={};
    }

    render() {
    return (
     <div className="container">
         
         
         
       
         
          <div className="row">

<div className="col-3 noMargin">
    {/*<Admins/>*/}
    
<CustomerToolbar/>
</div>
<div className="col-6 noMargin">
  <Chat />
</div>
<div className="col-3 noMargin">
  <Customers />
</div>

</div>
     </div>
    );
  }
}
