import React, { Component } from "react";
import StatBase from "./../../StatBase";
import { CurrentUserInfo } from "../../../Help/Socket";

export default class CompaignSent extends StatBase {
  constructor(props) {
    super(props);

   
    CurrentUserInfo.CompaignSent = this;
    
    this.state = {
      title: "آمار ارسال های کمپین ها",
      type: "line",
      arrName: "CompaignSent",
    };
  }

  componentDidMount() {
    if (this.props.data) {
      this.getVisitedPagesForCurrentSiteCallback(this.props.data);
    }
  }
}
