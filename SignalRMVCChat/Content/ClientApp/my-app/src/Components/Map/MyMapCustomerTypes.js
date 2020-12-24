import React, { Component } from "react";
import { ListBox } from "primereact/listbox";
import {Form} from 'react-bootstrap';
import {CurrentUserInfo, MyCaller} from '../../Help/Socket';

const citySelectItems = [
  { label: "اختصاص داده شده به من", value: "AssingedToMe" },
  { label: "در انتظار پاسخ", value: "WaitingForAnswer" },
  { label: " پاسخ داده شده", value: "Answered" },
  { label: "بدون گفتگو", value: "NotChatted" },
  { label: "تمامی مراجعه کنندگان سایت", value: "AllCustomerListPage" },
  {
    label: "کاربرانی که بدون دریافت پشتیبانی سایت را ترک کرده اند ",
    value: "NotChattedLeftCustomerListPage",
  },
  {
    label: "بعد از دریافت پشتیبانی مجددا به سایت بازگشته اند",
    value: "ChattedAndReturnedCustomerListPage",
  },
  {
    label: "بعد از دریافت پشتیبانی هرگز به سایت برگنشته اند",
    value: "ChattedNever",
  },
  { label: "بر اساس صفحه ها", value: "SepratePerPage" },
];

export default class MyMapCustomerTypes extends Component {
  state = {
    onlyOfflineChecked: false,
  };

  constructor(props) {
      super(props);
      
      CurrentUserInfo.MyMapCustomerTypes=this;
  }
  getClientsListForAdminCallback(res) {
    CurrentUserInfo.MapPage.setState({loading:false});

}
  
  OnlyOfflines(e) {
    CurrentUserInfo.gapIsOnlyOnly = !this.state.onlyOfflineChecked;
    this.setState({ onlyOfflineChecked: !this.state.onlyOfflineChecked });

      this.GetClientsListForAdmin();
  }

  GetClientsListForAdmin() {

    CurrentUserInfo.MapPage.setState({loading:true});

    MyCaller.Send("GetClientsListForAdmin", {
        userType: CurrentUserInfo.UserType,
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

      this.GetClientsListForAdmin();
          }}
        />
      </div>
    );
  }
}
