import React, { Component } from "react";
import {CurrentUserInfo} from "../../CurrentUserInfo";

export default class Alarm extends Component {
  audioUrl = GetBaseUrlWithWebsiteToken("Alam/AdminAlarm");
  state = {
    play: false,
  };
  constructor(props) {
    super(props);
    CurrentUserInfo.Alarm = this;
  }

  audio = new Audio(this.audioUrl);

  componentDidMount() {
    this.audio.addEventListener("ended", () => this.setState({ play: false }));
  }

  componentWillUnmount() {
    this.audio.removeEventListener("ended", () =>
      this.setState({ play: false })
    );
  }

  togglePlay = () => {
    if (!this.audioUrl) {
      this.audioUrl = GetBaseUrlWithWebsiteToken("Alam/AdminAlarm");
      this.audio = new Audio(this.audioUrl);
    }

    this.setState({ play: true,rn:Math.random() }, () => {
      this.state.play ? this.audio.play() : this.audio.pause();
    });
  };

  render() {
    return <></>;
  }
}

export const GetBaseUrlWithWebsiteToken = (url) => {
  let baseUrl = document.getElementById("baseUrl").value;
  let port = document.getElementById("port").value;
  let websiteToken = document.getElementById("websiteToken").value;

  let userId;
  if (
    CurrentUserInfo.B4AdminLayout &&
    CurrentUserInfo.B4AdminLayout.state &&
    CurrentUserInfo.B4AdminLayout.state.currentUser
  ) {
    userId = CurrentUserInfo.B4AdminLayout.state.currentUser.Id;
  } else {
    return "";
  }

  return `http://${baseUrl}:${port}/${url}?userId=${userId}&websiteToken=${websiteToken}`;
};

export const GetBaseUrl = (url) => {
  let baseUrl = document.getElementById("baseUrl").value;
  let port = document.getElementById("port").value;

  return `http://${baseUrl}:${port}/`;
};
