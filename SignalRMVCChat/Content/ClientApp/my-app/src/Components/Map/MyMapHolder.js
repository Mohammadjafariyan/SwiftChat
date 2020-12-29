import React, { Component } from "react";
import MyMap from "./MyMap";
import { CurrentUserInfo } from "../../Help/Socket";
import { MyCaller } from "./../../Help/Socket";
import { DataHolder } from "./../../Help/DataHolder";

class MyMapHolder extends Component {
  state = {};

  constructor(props) {
    super(props);
    CurrentUserInfo.MyMapHolder = this;
  }

  getClientsListForAdminCallback(res) {
    CurrentUserInfo.MapPage.setState({ loading: false });

    this.setState({ loading: false });

    if (!res || !res.Content || !res.Content.EntityList) {
      CurrentUserInfo.LayoutPage.showError(
        "getClientsListForAdminCallback returns null"
      );
      return;
    }

    var arr = [];
    arr = res.Content.EntityList;
    
    
    this.setState({ users: arr, rn: Math.random() });

  }

  componentDidMount() {
    /*     CurrentUserInfo.selectedTagId = null;
    CurrentUserInfo.gapIsOnlyOnly = null;
    CurrentUserInfo.currentUsersIsAdmins = null; */
    // MyCaller.Send("GetClientsListForAdmin");
  }

  render() {
    return (
      <>
          <MyMap users={this.state.users} />

        {this.state.users && this.state.users.length == 0 && (
          <p>هیچ کاربری یافت نشد</p>
        )}
      </>
    );
  }
}

export default MyMapHolder;
