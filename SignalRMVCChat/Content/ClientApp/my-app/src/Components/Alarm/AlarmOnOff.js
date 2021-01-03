import React, { Component } from "react";
import { CurrentUserInfo } from "../../Help/Socket";
import { MyCaller } from "./../../Help/Socket";

export default class AlarmOnOff extends Component {
  state = {};
  constructor(props) {
    super(props);
    CurrentUserInfo.AlarmOnOff = this;
  }

  componentDidMount() {
    let currentUser;
    if (
      CurrentUserInfo.B4AdminLayout &&
      CurrentUserInfo.B4AdminLayout.state &&
      CurrentUserInfo.B4AdminLayout.state.currentUser
    ) {
      currentUser = CurrentUserInfo.B4AdminLayout.state.currentUser;
    } else {
    }

    //currentUser.IsNotificationMute
    this.setState({ currentUser: currentUser });
  }

  getMyProfileCallback(res) {
    if (!res || !res.Content) {
      CurrentUserInfo.LayoutPage.showError("res is null for profile");
      return;
    }
    // CurrentUserInfo.LayoutPage.showMsg('اطلاعات پروفایل خوانده شد');

    this.setState({ currentUser: res.Content.MyAccount });
  }

  setIsMute(bool) {
    MyCaller.Send("AlarmSetIsMute", {
      IsNotificationMute: bool,
    });

    this.state.currentUser.IsNotificationMute = bool;
    this.setState({ md: Math.random() });
  }

  alarmSetIsMuteCallback(res) {}

  render() {
    return (
      <>
        {this.state.currentUser && this.state.currentUser.IsNotificationMute && (
          <a
            className="nav-link "
            role="button"
            onClick={() => {
              this.setIsMute(false);
            }}

            
            aria-label="صدای آلارم خاموش است"
                    data-microtip-position="right"
                    role="tooltip"
          >
            <i className="fa fa-bell-slash" aria-hidden="true"></i>
          </a>
        )}

        {this.state.currentUser && !this.state.currentUser.IsNotificationMute && (
         
          <a
            className="nav-link "
            role="button"
            onClick={() => {
              this.setIsMute(true);
            }}

            aria-label="صدای آلارم روشن است"
                    data-microtip-position="right"
                    role="tooltip"
          >
            <i className="fa fa-bell" aria-hidden="true"></i>
          </a>
          
        )}
      </>
    );
  }
}
/* 
const sampleAlarmJson = {
  Name,
  MyWebsiteId,
  AlarmType,
  IsMute,
};
 */