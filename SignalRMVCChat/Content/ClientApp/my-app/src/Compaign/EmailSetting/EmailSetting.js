import React, { Component } from "react";
/*import FormBuilder from "@langleyfoxall/react-dynamic-form-builder";*/
import { _EmailSettingForm } from "./EmailSettingForm";
import { MyCaller} from "../../Help/Socket";
import { _showError, _showMsg } from "../../Pages/LayoutPage";
import { Spinner } from "react-bootstrap";
import { MyCard } from "../../Routing/Manage/RoutingSave";
import {CurrentUserInfo} from "../../CurrentUserInfo";
import {FormBuilder} from "../../Components/Form-Builder";

export default class EmailSetting extends Component {
  state = {};

  constructor(props) {
    super(props);

    this.state = {
      form: _EmailSettingForm,
    };

    this.onSubmit = this.onSubmit.bind(this);

    CurrentUserInfo.EmailSetting = this;
  }

  onSubmit(submission) {
    if (!submission.EmailName || !submission.CustomEmailName) {
      _showError("مقادیر فرم صحیح نیست ");
      return;
    }

    this.setState({ loading: true });

    MyCaller.Send("SaveEmailSetting", submission);
  }

  saveEmailSettingCallback(res) {
    this.setState({ loading: false });
    _showMsg("تنظیمات ایمیل ذخیره شد");
  }

  saveEmailSettingErrorCallback(res) {
    this.setState({ loading: false });

    _showError(
      res && res.Content ? res.Content : "تنظیمات ارسال شده صحیح نیست"
    );
  }

  componentDidMount() {
    this.setState({ loading: true });
    MyCaller.Send("GetEmailSetting");
  }

  getEmailSettingCallback(res) {

    let values = res && res.Content ? res.Content : {};


    this.setState({ loading: false ,
        values: values,
    });

  }

  render() {
    return (
      <div>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <MyCard
          header={"تنظیمات ایمیل"}
          title={"برای ارسال ایمیل های کمپین این تنظیمات لازم است"}
        >
          {this.state.values && (
            <FormBuilder
              defaultValues={this.state.values}
              form={this.state.form}
              onSubmit={this.onSubmit}
              submitButton={{
                text: "ثبت تنظیمات ایمیل",
              }}
              defaultInputClass={" form-control "}
              defaultSubmitClass={" btn btn-info mt-5 "}
            />
          )}
        </MyCard>
      </div>
    );
  }
}
