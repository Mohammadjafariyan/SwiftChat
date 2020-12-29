import React, { Component } from "react";
import { ListBox } from "primereact/listbox";
import {Form} from 'react-bootstrap';
import {CurrentUserInfo, MyCaller} from '../../Help/Socket';
import {DataHolder} from '../../Help/DataHolder';

const citySelectItems = [
  { label: "اختصاص داده شده به من", value: "AssingedToMe" },
  { label: "در انتظار پاسخ", value: "WaitingForAnswer" },
  { label: " پاسخ داده شده", value: "answered" },
  { label: "بدون گفتگو", value: "NotChatted" },
  { label: "تمامی مراجعه کنندگان سایت", value: "AllCustomerListPage" },
  {
    label: "کاربرانی که بدون دریافت پشتیبانی سایت را ترک کرده اند ",
    value: "NotChattedLeftCustomerListPage",
  },
  {
    label: "بعد از دریافت پشتیبانی مجددا به سایت بازگشته اند",
    value: "ChattedAndReturnedCustomerListPage",
  }
];

export default class MyMapCustomerTypes extends Component {
  state = {
    onlyOfflineChecked: false,
  };

  constructor(props) {
      super(props);
      
      CurrentUserInfo.MyMapCustomerTypes=this;
  }

  componentDidMount(){
    this.setState({onlyOfflineChecked:CurrentUserInfo.gapIsOnlyOnly})
  }
  getClientsListForAdminCallback(res) {
    CurrentUserInfo.MapPage.setState({loading:false});

    console.log('MyMapCustomerTypes==>CgetClientsListForAdminCallback=>',res )


    
}
  
  OnlyOfflines(e) {
    CurrentUserInfo.gapIsOnlyOnly = this.state.onlyOfflineChecked!=null ? !this.state.onlyOfflineChecked : true;
    this.setState({ onlyOfflineChecked: CurrentUserInfo.gapIsOnlyOnly });

      this.GetClientsListForAdmin();
  }

  GetClientsListForAdmin() {

    console.log('GetClientsListForAdmin==>CurrentUserInfo.gapIsOnlyOnly=>',CurrentUserInfo.gapIsOnlyOnly )
    CurrentUserInfo.MapPage.setState({loading:true});

    MyCaller.Send("GetClientsListForAdmin", {
        userType: CurrentUserInfo.UserType,
        gapIsOnlyOnly : CurrentUserInfo.gapIsOnlyOnly
    });
}

  render() {
    return (
      <div>
        <Form.Group controlId="formBasicCheckbox">
          <Form.Check
            checked={this.state.onlyOfflineChecked}
            type="checkbox"
            label="شامل آفلاین ها"
            onChange={() => {
              this.OnlyOfflines();
            }}
          />
        </Form.Group>
        <ListBox
          value={this.state.selectedUserType}
          options={citySelectItems}
          onChange={(e) => {
            this.setState({ selectedUserType: e.value });
            CurrentUserInfo.UserType=e.value;
      this.GetClientsListForAdmin();
          }}
        />
      </div>
    );
  }
}
