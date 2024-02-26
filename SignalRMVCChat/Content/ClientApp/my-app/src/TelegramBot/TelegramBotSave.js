
import React, { Component } from "react";
import { MyCard, MyFieldset } from "./../Routing/Manage/RoutingSave";
import { Row, Col, Button, Alert } from "react-bootstrap";
import { MyCaller} from "./../Help/Socket";
/*import FormBuilder from "@langleyfoxall/react-dynamic-form-builder";*/
import { _TelegramBotForm } from "./TelegramBotForm";
import { _showError, _showMsg } from "../Pages/LayoutPage";
import  Spinner from 'react-bootstrap/Spinner';
import {CurrentUserInfo} from "../CurrentUserInfo";
import {FormBuilder} from "../Components/Form-Builder";

export default class TelegramBotSave extends Component {
  state = {
    form: _TelegramBotForm,
  };

  constructor(props) {
    super(props);

    CurrentUserInfo.TelegramBotSave = this;


    this.onSubmit=this.onSubmit.bind(this);
  }

  componentDidMount() {
    MyCaller.Send("GetTelegramBotSetting");
  }

  getTelegramBotSettingCallback(res) {
    if (!res || !res.Content) {
      console.error(" مقدار بازگشتی از سرور نال است ");
      return;
    }

    let values = res && res.Content ? res.Content : {};

    this.setState({ loading: false, values: values });
  }

  render() {
    return (
      <>
        <MyFieldset title={" تنظیمات ربات تلگرام "}>
          {this.state.loading && (
            <Spinner animation="border" role="status">
              <span className="sr-only">در حال خواندن اطلاعات...</span>
            </Spinner>
          )}

          <Row>
            <Col>{this.customersBot()}</Col>

            <Col>{this.operatorsBot()}</Col>
          </Row>
        </MyFieldset>
      </>
    );
  }

  operatorsBot() {
    let OperatorsBotName = "";
    if (this.state.values && this.state.values.OperatorsBotName) {
      OperatorsBotName = this.state.values.OperatorsBotName.replace("@", "");
    }
    return (
      <>
        <MyCard
          header={"تنظیمات ربات اوپراتور ها"}
          title={
            "شما میتوانید از طریق این ربات با بازدیدکنندگان سایت خود در ارتباط باشید"
          }
        >

        <h6>:   همچنین امکانات دیگری همچون </h6>
          <ol
            style={{ listStyle: "none", textAlign: "right", direction: "rtl" }}
          >
            <li>
              <Button
                variant="info"
                style={{
                  borderRadius: "50%",
                  padding: "0px",
                  paddingRight: "5px",
                  paddingLeft: "5px",
                }}
              >
                1
              </Button>{" "}
              گفتگو با بازدیدکنندگان سایت در محیط
              تلگرام
            </li>
            <li>
              <Button
                variant="info"
                style={{
                  borderRadius: "50%",
                  padding: "0px",
                  paddingRight: "5px",
                  paddingLeft: "5px",
                }}
              >
                2
              </Button>{" "}
              خلاصه بازدید سایت
            </li>

            <li>
              <Button
                variant="info"
                style={{
                  borderRadius: "50%",
                  padding: "0px",
                  paddingRight: "5px",
                  paddingLeft: "5px",
                }}
              >
                3
              </Button>{" "}
              انواع لیست کاربران
            </li>
            <li>
              <Button
                variant="info"
                style={{
                  borderRadius: "50%",
                  padding: "0px",
                  paddingRight: "5px",
                  paddingLeft: "5px",
                }}
              >
                4
              </Button>{" "}
              بلاک کاربر
            </li>

            <li>
              <Button
                variant="info"
                style={{
                  borderRadius: "50%",
                  padding: "0px",
                  paddingRight: "5px",
                  paddingLeft: "5px",
                }}
              >
                5
              </Button>{" "}
              تغییر وضعیت چت
            </li>

          </ol>
          {this.state.values && (
            <Alert variant="warning" style={{ direction: "rtl" }}>
              برای شروع کد
              <span className="mr-1 ml-1"></span>
              <b className="text-black">
                @{this.state.values.UniqOperatorCode}
              </b>
              <span className="mr-2 ml-2">را به ربات تلگرام</span>
              <span className="mr-1 ml-1"></span>
              <a href={"https://t.me/" + OperatorsBotName} target="_blank">
                <b>@{OperatorsBotName}</b>
              </a>
              <span className="mr-1 ml-1"></span>
              ارسال نمایید
            </Alert>
          )}
        </MyCard>
      </>
    );
  }

  customersBot() {
    return (
      <>
        <MyCard
          header={"تنظیمات ربات بازدیدکنندگان"}
          title={
            "بازدیدکنندگان سایت می توانند از طریق این ربات با شما ارتباط برقرار کنند"
          }
        >
          {this.state.values && (
            <FormBuilder
              defaultValues={this.state.values}
              form={this.state.form}
              onSubmit={this.onSubmit}
              submitButton={{
                text: "ثبت تنظیمات ربات",
              }}
              defaultInputClass={" form-control "}
              defaultSubmitClass={" btn btn-info mt-5 "}
            />
          )}
        </MyCard>
      </>
    );
  }

  telegramBotSettingSaveCallback(res) {
    this.setState({ loading: false });
    _showMsg("با موفقیت ذخیره شد");
  }

  onSubmit(submission) {
    if (!submission.CustomerBotWelcomeMessage || !submission.CustomerBotToken) {
      _showError("مقادیر فرم صحیح نیست ");
      return;
    }
    _showMsg("در حال ذخیره");

    this.setState({ loading: true });

    MyCaller.Send("TelegramBotSettingSave", submission);
  }
}
