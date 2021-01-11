import React, { Component } from "react";
import StatBase from "./../../StatBase";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

export default class CompaignSent extends StatBase {
  constructor(props) {
    super(props);

   
    CurrentUserInfo.CompaignSent = this;
    
    this.state = {
      title: "آمار ارسال های کمپین ها",
      type: "line",
      arrName: "CompaignSent",
      label1:'ارسال های کمپین ها',
      label2:'   '
    };
  }

  componentDidMount() {
    if (this.props.data) {
      this.getVisitedPagesForCurrentSiteCallback(this.props.data);
    }
  }
}
