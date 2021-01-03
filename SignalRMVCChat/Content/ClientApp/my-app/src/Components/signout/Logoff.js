import React, { Component } from "react";
import { cookieManager } from "./../../Help/CookieManager";

export default class Logoff extends Component {
  render() {
    return (
      <>
        <li
          className="nav-item  no-arrow mx-1"
          aria-label="خروج از سیستم"
          data-microtip-position="left"
          role="tooltip"
        >
          <a
            className="nav-link "
            onClick={() => {
              cookieManager.removeItem("adminToken");

              window.location.reload();
            }}
          >
            <i class="fa fa-power-off" aria-hidden="true"></i>
          </a>
        </li>
      </>
    );
  
}

}
