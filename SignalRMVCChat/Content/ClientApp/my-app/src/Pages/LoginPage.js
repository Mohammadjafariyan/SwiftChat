import React, { Component } from "react";
import { useState } from "react";
import { MyCaller} from "./../Help/Socket";
import { cookieManager } from "./../Help/CookieManager";
import { MyGlobal } from "../Help/MyGlobal";
import GlobalLoading from "../Components/Utilities/GlobalLoading";
import { Card } from "react-bootstrap";
import RandomBgGenerator from "./../Components/bg/RandomBgGenerator";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class LoginPage extends Component {
  constructor(arg) {
    super(arg);

    this.state = {};
    CurrentUserInfo.LoginPage = this;
  }

  adminLoginCallback(res) {
    debugger;
    this.setState({ loading: false });
    if (res.Type === 0) {
      var token = res.Token;

      cookieManager.setItem("adminToken", token);

      CurrentUserInfo.targetId = res.Content.Id;
      CurrentUserInfo.targetName = res.Content.Name;

      CurrentUserInfo.LayoutPage.setState({
        isLogin: true,
        isClearCookie: false,
      });

      window.location.reload();
    } else {
      this.setState({ err: "نام کاربری یا رمز عبور صحیح نیست" });
    }
  }

  loginForm() {
    return (
      <>
        {this.state.err && (
          <div className="alert alert-danger">{this.state.err}</div>
        )}

        <div className="form-group">
          <label for="exampleInputEmail1">نام کاربری</label>
          <input
            id={"username"}
            type="text"
            className="form-control"
            aria-describedby="emailHelp"
            placeholder="نام کاربری را وارد نمایید"
            onChange={(e) => this.setState({ username: e.target.value })}
          />

          <small id="emailHelp" className="form-text text-muted">
            نام کاربری و رمز عبور از پیشخوان / قسمت تعریف ادمین قابل دسترس است
          </small>
        </div>
        <div className="form-group">
          <label for="exampleInputPassword1">رمز عبور</label>
          <input
            id={"password"}
            onChange={(e) => this.setState({ password: e.target.value })}
            type="password"
            className="form-control"
            placeholder="رمز عبور"
          />
        </div>
        <button
          onClick={() => {
            this.login();
          }}
          type="submit"
          id={"login"}
          className="btn btn-primary"
        >
          ورود{" "}
        </button>
      </>
    );
  }

  render() {

    return (
      <>
        <div
          className=" h-100 row align-items-center"
          style={{
           
          }}
        >
          <div className="col-md-3 "></div>

          {this.state.loading && (
            <div className="col-md-6 ">
            <Card
                bg={"light"}
                style={{ boxShadow: "2px 7px 10px 2px" }}
              >
                <Card.Body>
                  <Card.Header>فرم ورود به سیستم</Card.Header>
                  <Card.Text>
<br/>
<br/>
              <GlobalLoading loading={this.state.loading} />

              <h4>در حال ورود به سیستم</h4>

              </Card.Text>
                </Card.Body>
              </Card>
            </div>
          )}

          {!this.state.loading && (
            <>
              <Card
                bg={"light"}
                style={{ boxShadow: "2px 7px 10px 2px" }}
                className="col-md-6 "
              >
                <Card.Body>
                  <Card.Header>فرم ورود به سیستم</Card.Header>
                  <Card.Text>
                    <br></br>

                    {this.loginForm()}
                  </Card.Text>
                </Card.Body>
              </Card>
            </>
          )}
          <div className="col-md-4 "></div>
        </div>
      </>
    );
  }

  login() {
    if (!this.state.username || !this.state.password) {
      this.setState({ err: "اطلاعات صحیح وارد نشده است" });
      return;
    }

    if (MyGlobal.isTestingEnvirement) {
      this.adminLoginCallback({
        Type: 0,
        Token: "sdlfjslkdfj",
        Content: {},
      });
    }

    this.setState({ loading: true });

    MyCaller.Send("AdminLogin", {
      username: this.state.username,
      password: this.state.password,
    });
  }
}
