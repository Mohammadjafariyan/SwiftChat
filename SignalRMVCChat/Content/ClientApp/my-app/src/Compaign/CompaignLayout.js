import React, { Component } from "react";
import BaseCrudLayout from "../CRUD/BaseCrudLayout";
import { CurrentUserInfo, MyCaller } from "../Help/Socket";
import { Alert, Card } from "react-bootstrap";
import CompaignSave, { _GetSelectedCompaign } from "./Manage/CompaignSave";
import CompaignFilter from "./Filter/CompaignFilter";
import BaseIndex from "../CRUD/BaseIndex";
import CompaignIndex from "./Override/CompaignIndex";
import CompaignTable from "./Manage/ChildComps/CompaignTable";
import { MyModal } from "../Components/Modal";
import PrimengModal from "../Components/SendFromHelpDeskModal";
import SendFromHelpDesk from "../Components/SendFromHelpDesk";
import Button from "react-bootstrap/cjs/Button";
import ButtonGroup from "react-bootstrap/cjs/ButtonGroup";
import { ConfirmModal } from "../Components/HelpDesk/Language/ConfirmModal";
import { _showMsg } from "../Pages/LayoutPage";
import EmailSetting from './EmailSetting/EmailSetting';
import CompaignLogReceiverTable from './Manage/Logs/CompaignLogReceiverTable';

class CompaignLayout extends BaseCrudLayout {
  state = {
    get: "GetCompaignList",
    save: "CompaignSave",
    delete: "DeleteCompaign",
    setIsEnabled: "SetIsEnabledCompaign",
    saveDraft: "CompaignSave",
    menuCols: 3,
    bodyCols: 9,
  };

  constructor(prp) {
    super(prp);

    CurrentUserInfo.CompaignLayout = this;

    console.log("hi");
  }

 
  render() {
    return (
      <>
        {this.state.showModal && (
          <ConfirmModal
            body={
              <>
                <h4 className={"text-danger"}>تغییرات ذخیره نشده دارید </h4>
                <p className={"text-danger"}>
                  در صورت ادامه تغییرات ذخیره نخواهند شد
                </p>

                <p className={"text-info"}>آیا مایلید تغییرت ذخیره شود ؟</p>
              </>
            }
            display={this.state.showModal}
            onCancel={() => {
              this.setState({ showModal: false });
            }}
            onConfirm={() => {
              this.setState({ showModal: false });

              _showMsg("در حال ذخیره تغییرات");
              MyCaller.Send("CompaignSave", this.state.selectedForSave);
            }}
          />
        )}

        {this.state.CompaignChildPage == "CompaignTable" && (
          <>
            <CompaignFilter></CompaignFilter> <br />
            <CompaignTable
              title={this.state.title}
              dataType={this.state.dataType}
            ></CompaignTable>
          </>
        )}
        {this.state.CompaignChildPage == "CompaignAnaylics" && (
          <>
            <CompaignFilter></CompaignFilter> <br />
            <h5>این صفحه در دست ساخت می باشد</h5>
          </>
        )}

        {this.state.CompaignChildPage == "emailSetting" && (
          <>
          <CompaignFilter></CompaignFilter> <br />
            <EmailSetting></EmailSetting> <br />
          </>
        )}


        <MyModal show={this.state.showModal} handleClose={() => {
                    this.setState({showModal: false})
                }} title={"نمایش لیست کاربران دریافت کننده"} /*onSave={() => {
                this.setState({tmp: Math.random()})
            }}*/>


          <CompaignLogReceiverTable CompaignId={this.state.selectedCompaignId}
                                    CompaignLogId={this.state.selectedCompaignLogId}>
              
          </CompaignLogReceiverTable>
        </MyModal>

        
        {!this.state.CompaignChildPage && (
          <CompaignIndex
            onSaveClick={(form) => {
              this.onSaveClick(form);
            }}
            menuCols={this.state.menuCols}
            bodyCols={this.state.bodyCols}
            parent={this}
            {...this.props}
            {...this.state}
            get={this.state.get}
            save={this.state.save}
            delete={this.state.delete}
            setIsEnabled={this.state.setIsEnabled}
            RenderWelcome={this.RenderWelcome}
            saveDraft={this.state.saveDraft}
          >
            {this.RenderForm()}
          </CompaignIndex>
        )}
      </>
    );
  }

  onSaveClick(form) {
    debugger;
    if (CurrentUserInfo.CompaignConditions) {
      CurrentUserInfo.CompaignConditions.componentWillUnmount();
    }
    if (CurrentUserInfo.CompaignEditor) {
      CurrentUserInfo.CompaignEditor.componentWillUnmount();
    }
    if (CurrentUserInfo.CompaignRecipients) {
      CurrentUserInfo.CompaignRecipients.componentWillUnmount();
    }
    if (CurrentUserInfo.CompaignTemplates) {
      CurrentUserInfo.CompaignTemplates.componentWillUnmount();
    }

    if (CurrentUserInfo.CompaignFilter) {
      CurrentUserInfo.CompaignFilter.componentWillUnmount();
    }
    _GetSelectedCompaign().IsChanged = false;
    CurrentUserInfo.CompaignSave.setState({ smsg: Math.random() });

    this.setState({
      selectedNotChanged: JSON.stringify(_GetSelectedCompaign()),
    });

    _showMsg("در حال ذخیره کمپین");

    MyCaller.Send(this.state.save, _GetSelectedCompaign());
  }

  RenderWelcome() {
    return (
      <Card>
        <Card.Body>
          <Card.Text>
            <Alert variant={"warning"}>
              <div className="cap-plugin-content-box-inner">
                <p className="cap-font-sans-regular">
                  <span className="cap-font-sans-semibold">
                    کمپین های خودکار برای ارسال ایمیل به کاربران شما براساس
                    فیلترهای محرک رویداد از پیش تعریف شده استفاده می شوند. یک
                    ایمیل بنویسید ، برنامه ریزی کنید که در کدام شرایط فیلتر باید
                    برای یک کاربر خاص اجرا شود و آن را فعال کنید.
                    <b> کمپین های اتوماتیک ، </b>
                    <b> کمپین های دستی </b>
                  </span>
                </p>
                <p className="cap-font-sans-regular">
                  کمپین های خودکار چه کاری انجام می دهند؟ کمپین های خودکار ایمیل
                  ها یا پیام های گپ از پیش تعریف شده ای هستند که براساس
                  رویدادهای کاربر براساس هر کاربر ارسال می شوند. این ابزار مناسب
                  جذب مشتری است.{" "}
                </p>

                <ol
                  className="cap-font-sans-semibold help-list"
                  style={{ textAlign: "right" }}
                >
                  <li>تعریف کمپین جدید</li>
                  <li>انتخاب اتوماتیک یا دستی</li>
                  <li>تعریف قالب و متن ایمیل یا پیغام</li>
                  <li>ارسال زمانبندی شده یا دستی</li>
                </ol>

                <div className="help-screen-wrap">
                  <div className="help-screen"></div>
                </div>
              </div>
              <hr />
              در این بخش کمپین های (ارسال ایمیل یا پیغام) با سیاست خاصی به
              کاربران تعریف و ارسال میشود
            </Alert>
          </Card.Text>
        </Card.Body>
      </Card>
    );
  }

  RenderForm() {
    return (
      <>
        {this.state.selected && (
          <CompaignSave parent={this} selected={this.state.selected} />
        )}
      </>
    );
  }

  showSaveModal(selectedForSave, callback) {
    debugger;
    if (selectedForSave && this.state.selectedNotChanged) {
      let json1 = JSON.stringify(selectedForSave);
      let json2 = this.state.selectedNotChanged;

      if (json1 != json2) {
        this.setState({ showModal: true, selectedForSave });

        callback();
      }
    }
  }
}

export default CompaignLayout;
