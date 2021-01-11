import React, { Component } from "react";
import OnlineCustomerList from "./../Components/OnlineCustomerList";
import { MyCaller } from "./../Help/Socket";
import { DataHolder } from "../Help/DataHolder";
import { Badge } from "react-bootstrap";
import CustomersPaging from "./../Components/Map/CustomersPaging";
import { _SetLoading } from "../Components/Utilities/GlobalLoading";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class OnlineCustomerListHolder extends Component {
  constructor(args) {
    super(args);

    this.state = { list: [] };

    CurrentUserInfo.OnlineCustomerListHolder = this;
  }

  userAddedToTagsCallback(res) {
    this.GetClientsListForAdmin();
  }

  customerTypingHelper(res, isTyping) {
    if (!res || !res.Content.targetCustomerId) {
      CurrentUserInfo.LayoutPage.showError(
        "کاربر ی در حال تایپ است بدون کد کاربر ارسال شده است "
      );
      return;
    }
    if (!this.state.list) {
      return;
    }

    let j = this.state.list.findIndex(
      (f) => f.Id === res.Content.targetCustomerId
    );

    if (j === -1) {
      return;
    }

    this.state.list[j].IsTyping = isTyping;

    if (!this.state.list[j].LastMessage) {
      this.state.list[j].LastMessage = {};
    }
    this.state.list[j].LastMessage.Message = res.Content.text;

    this.setState({ tmp: Math.random() });
  }

  customerStartTypingCallback(res) {
    this.customerTypingHelper(res, true);
  }

  customerStopTypingCallback(res) {
    this.customerTypingHelper(res, false);
  }

  totalUserCountsChangedCallback(res) {
    /* CustomerId=w.Id,
             TotalNewChatSentByCustomer=w.Chats.Count(c =>
                 c.SenderType==ChatSenderType.CustomerToAccount && c.DeliverDateTime.HasValue==false),
             OnlineStatus*/

    if (!res.Content.CustomerList || !res.Content.CustomerList.length) {
      console.log("res.CustomerList is null or empty");
      return;
    }

    let arr = this.state.list;
    if (!arr) {
      arr = [];
      return;
    }

    for (let i = 0; i < res.Content.CustomerList.length; i++) {
      let CustomerId = res.Content.CustomerList[i].CustomerId;
      let TotalNewChatSentByCustomer =
        res.Content.CustomerList[i].TotalNewChatSentByCustomer;
      let OnlineStatus = res.Content.CustomerList[i].OnlineStatus;

      let j = arr.findIndex((f) => f.Id === CustomerId);

      if (j !== -1) {
        arr[j].OnlineStatus = OnlineStatus;
        arr[j].NewMessageCount = TotalNewChatSentByCustomer;
      }
    }

    this.setState({ list: arr, tmp: Math.random() });
  }

  customerOnlineAgainCallback(res) {
    this.newCustomerOnlineCallback(res);
  }

  customerOfflineAgainCallback(res) {
    this.newCustomerOnlineCallback(res);
  }

  newCustomerOnlineCallback(res) {
    if (res.Content.OnlineTp != CurrentUserInfo.UserType) {
      console.log(res);
      console.log(
        `res.Content.Type!=CurrentUserInfo.UserType res.Content.Type= ${res.Content.Type}!=${CurrentUserInfo.UserType}`
      );
      return;
    }

    let arr = this.state.list;
    if (!arr) {
      arr = [];
    }

    let i = arr.findIndex((f) => f.Id === res.Content.Id);
    if (i != -1) {
      /*   if (res.Content.OnlineStatus===0 || res.Content.OnlineStatus==='0'){
                   
               }*/
      arr[i] = res.Content;
    } else {
      arr.push(res.Content);
    }

    this.setState({ list: arr });
  }

  multimediaPmSendCallback(res) {
    this.customerSendToAdminCallback(res);
  }

  customerSendToAdminCallback(res) {
    //debugger;

    let CustomerId = res.Content.CustomerId;
    let Message = res.Content.Message;
    let TotalReceivedMesssages = res.Content.TotalReceivedMesssages;

    let chat = res.Content.Chat;

    if (!this.state.list) {
      return;
    }

    let i = this.state.list.findIndex((c) => c.Id == CustomerId);
    if (i != -1) {
      this.state.list[i].NewMessageCount = TotalReceivedMesssages;
      this.setState({ list: this.state.list });
    }
  }

  getClientsListForAdminCallback(res) {
    if (res && res.Content)
      if (res.Content.userType != CurrentUserInfo.UserType) {
        return;
      }

    _SetLoading(false);

    if (!res || !res.Content || !res.Content.EntityList) {
      CurrentUserInfo.LayoutPage.showError(
        "getClientsListForAdminCallback res is null"
      );
      return;
    }

    var arr = [];
    arr = res.Content.EntityList;

    this.setState({ list: arr });
  }

  GetClientsListForAdmin() {
    _SetLoading(true);

    MyCaller.Send("GetClientsListForAdmin", {
      userType: CurrentUserInfo.UserType,
      selectedPage: DataHolder.SelectedPage,
      gapIsOnlyOnly: CurrentUserInfo.gapIsOnlyOnly,
    });
  }

  render() {
    return (
      <div>
        <CustomersPaging />

        {showSelectedTag()}

        <OnlineCustomerList list={this.state.list} />
      </div>
    );
  }
}

function showSelectedTag() {
  if (!CurrentUserInfo.selectedTagId || !CurrentUserInfo.selectedTagName) {
    return <></>;
  }

  return (
    <>
      {" "}
      <Badge variant="info">
        <i
          style={{ fontSize: "14px" }}
          className="fa fa-tags"
          aria-hidden="true"
        ></i>

        <span>{CurrentUserInfo.selectedTagName}</span>
      </Badge>{" "}
      <span
        onClick={() => {
          CurrentUserInfo.TagList.getUsersByTagId(null, null);

          CurrentUserInfo.TagList.setState({ tmp: Math.random() });
          CurrentUserInfo.OnlineCustomerListHolder.setState({
            tmp: Math.random(),
          });
        }}
      >
        x
      </span>
    </>
  );
}
