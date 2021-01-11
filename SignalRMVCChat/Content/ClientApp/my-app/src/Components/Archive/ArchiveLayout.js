import React, { Component } from "react";
import { cookieManager } from "./../../Help/CookieManager";
import { Button } from 'react-bootstrap';

import './Archive.css'
import {CurrentUserInfo} from "../../CurrentUserInfo";
export default class ArchiveLayout extends Component {
  state = {};

  constructor(props) {
    super(props);
    CurrentUserInfo.ArchiveLayout = this;
  }

  refresh() {
    this.setUrl();

    this.setState({ mg: Math.random() });
  }
  componentDidMount() {
    this.setUrl();
  }

  setUrl() {
    let baseUrl = document.getElementById("baseUrl").value;
    let port = document.getElementById("port").value;

    let websiteToken = document.getElementById("websiteToken").value;
    let adminToken = cookieManager.getItem("adminToken");

    let returnUrl = `/Customer/Archive/Index?adminToken=${adminToken}`;
    let url = `http://${baseUrl}:${port}/OperatorsLogin/Index?returnUrl=${returnUrl}&token=${websiteToken}&adminToken=${adminToken}`;

    this.setState({ url: url });
  }
  render() {
    return (
      <div className={this.state.abs ? 'fullscreen' :''}>
        {!this.state.abs && (
          <Button
            variant="light"
            onClick={() => {
              this.setState({ abs: "absolute" });
            }}
          >
          بزرگ نمایی
            <i class="fa fa-window-maximize" aria-hidden="true"></i>
          </Button>
        )}

        {this.state.abs && (
          <Button
            variant="light"
            onClick={() => {
              this.setState({ abs: null });
            }}
          >
          کوچک نمایی
            <i class="fa fa-window-minimize" aria-hidden="true"></i>
          </Button>
        )}
        <br/>

        <iframe
        className="scrollbar" id="style-15"
          src={this.state.url}
          style={{
            width: "100%",
            height:this.state.abs ? '100%': "100vh",
            border: "none",
          }}
        />
      <div class="force-overflow"></div>
      </div>
    );
  }
}
