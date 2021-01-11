import React, { Component } from "react";
import { MyCaller } from "../Help/Socket";
import { DataHolder } from "../Help/DataHolder";
import { OverlayPanel } from "primereact/overlaypanel";
import { SelectButton } from "primereact/selectbutton";
import { InputTextarea } from "primereact/inputtextarea";
import Badge from "react-bootstrap/cjs/Badge";
import { Button } from "react-bootstrap";
import { TabMenu } from "primereact/tabmenu";

import "../styles/myStyle.css";
import { Editor } from "primereact/editor";
import { Fieldset } from "primereact/fieldset";
import ChatFormTypeSwitch from "./ChatForm/ChatFormTypeSwitch";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class ChatForm extends Component {
  justifyOptions = [
    { name: "Option 1", icon: "pi pi-align-left", value: "left" },
    { name: "Option 1", icon: "pi pi-align-right", value: "Right" },
    { name: "Option 1", icon: "pi pi-align-center", value: "Center" },
    { name: "Option 1", icon: "pi pi-align-justify", value: "Justify" },
  ];

  constructor(arg) {
    super(arg);
    this.submit = this.submit.bind(this);

    this.state = {
        editorEnable: false,
        panelCollapsed: true,
        items: [ { label: "متن", icon: "pi pi-fw pi-comments" },
        { label: "یادداشت", icon: "pi pi-fw pi-comment" },
        { label: "یادآور", icon: "pi pi-fw pi-clock" },
        { label: "آماده", icon: "pi pi-fw pi-inbox" },
        { label: "ویراستار", icon: "pi pi-fw pi-pencil" },
        { label: "آیکون", icon: "pi pi-fw pi-image" }],
      };
    CurrentUserInfo.ChatForm = this;

    this.justifyTemplate = this.justifyTemplate.bind(this);

    this.op = React.createRef();
  }

  componentDidMount() {
      let items=[];
    if (
      DataHolder.selectedCustomer &&
      DataHolder.selectedCustomer.TelegramUserId
    ) {

        items=[
            { label: "متن", icon: "pi pi-fw pi-comments" },
            { label: "یادداشت", icon: "pi pi-fw pi-comment" },
            { label: "یادآور", icon: "pi pi-fw pi-clock" },
            { label: "آماده", icon: "pi pi-fw pi-inbox" },
          ];
    }else{
        items=[
            { label: "متن", icon: "pi pi-fw pi-comments" },
            { label: "یادداشت", icon: "pi pi-fw pi-comment" },
            { label: "یادآور", icon: "pi pi-fw pi-clock" },
            { label: "آماده", icon: "pi pi-fw pi-inbox" },
            { label: "ویراستار", icon: "pi pi-fw pi-pencil" },
            { label: "آیکون", icon: "pi pi-fw pi-image" },
          ];
    }

    this.setState({
      items: items,

    })
  }

  justifyTemplate(option) {
    return (
      <i
        onClick={(e) => this.op.toggle(e)}
        aria-haspopup
        aria-controls="overlay_panel"
        className={option.icon}
      ></i>
    );
  }

  submit(e) {
    e.preventDefault();

    this.props.onSubmit(e, this.state.activeItem);

    this.setState({ text: "" });

    this.bindIsTyping("");

    return false;
  }

  addText(text) {
    let prev = this.state.text ? this.state.text : "";
    this.setState({ text: prev + " " + text });
  }

  bindIsTyping(text) {
    if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.Id) {
      // CurrentUserInfo.LayoutPage.showError('کاربری انتخاب نشده است');
      return;
    }

    if (!(!text || text.trim().length === 0)) {
      this.isTypingSent = true;

      MyCaller.Send("AdminStartTyping", {
        customerId: DataHolder.selectedCustomer.Id,
      });
    } else {
      this.isTypingSent = false;

      MyCaller.Send("AdminStopTyping", {
        customerId: DataHolder.selectedCustomer.Id,
      });
    }
  }
  render() {
    if (!DataHolder.selectedCustomer) return <></>;

    return (
      <div>
        <Fieldset
          toggleable
          className={"bg-light text-black"}
          collapsed={this.state.panelCollapsed}
          onToggle={(e) => this.setState({ panelCollapsed: e.value })}
        >
          <ChatFormTypeSwitch type={this.state.activeItem} />
        </Fieldset>

        <form onSubmit={this.submit}>
          <TabMenu
            model={this.state.items}
            activeItem={this.state.activeItem}
            onTabChange={(e) => {
              this.setState({ activeItem: e.value });

              if (e.value.label === "ویراستار") {
                this.setState({ editorEnable: true });
              } else {
                this.setState({ editorEnable: false });
              }

              if (
                e.value.label === "یادداشت" ||
                e.value.label === "یادآور" ||
                e.value.label === "آماده" ||
                e.value.label === "آیکون"
              ) {
                this.setState({ panelCollapsed: false });
              } else {
                this.setState({ panelCollapsed: true });
              }
            }}
          />

          <div className="p-inputgroup">
            <Button
              style={{ height: "50px" }}
              aria-label="استفاده از ویراستار"
              data-microtip-position="top"
              role="tooltip"
              variant={this.state.editorEnable ? "success" : "light"}
              onClick={() => {
                this.setState({ editorEnable: !this.state.editorEnable });
              }}
            >
              <i className="pi pi-pencil"></i>
            </Button>
            <Button
              style={{ height: "50px" }}
              variant={"light"}
              onClick={() => {
                let label =
                  this.state.activeItem && this.state.activeItem.label
                    ? this.state.activeItem.label
                    : "";
                this.props.onSubmit(null, label);
                this.bindIsTyping("");

                this.setState({ text: "" });
              }}
              aria-label="ارسال پیغام"
              data-microtip-position="top"
              role="tooltip"
            >
              <Badge>
                <i className="fa fa-paper-plane" aria-hidden="true"></i>
              </Badge>
            </Button>

            {this.state.editorEnable && (
              <Editor
                className={this.getEditorsStyle()}
                style={{ height: "200px", width: "100%", overflow: "auto" }}
                value={this.state.text}
                onTextChange={(e) => {
                  this.props.onChange(e.htmlValue);
                  this.setState({ text: e.htmlValue });
                }}
              />
            )}

            {!this.state.editorEnable && (
              <InputTextarea
                rows={1}
                cols={30}
                autoResize
                style={{ direction: "rtl" }}
                onKeyPress={() => {}}
                value={this.state.text}
                placeholder="جهت ارسال فایل یا پیغام متنی اینجا تایپ نمایید"
                className={"form-control " + this.getEditorsStyle()}
                onChange={(e) => {
                  this.setState({ text: e.target.value });
                  this.props.onChange(e.target.value);

                  this.bindIsTyping(e.target.value);
                }}
                onPaste={(e) => {
                  this.props.onPaste(e);
                }}
                type={"textarea"}
              />
            )}
          </div>
        </form>

        {/* <SelectButton value={this.state.value3} options={this.justifyOptions}
                          onChange={(e) => this.setState({ value3: e.value })} itemTemplate={this.justifyTemplate} />
*/}

        <br />
        
        
        {this.props.upload && DataHolder.selectedCustomer && 
        !DataHolder.selectedCustomer.TelegramUserId && 
        <div style={{ display: "flex" }}>
          <Button
            variant={"light"}
            style={{ display: "block" }}
            className="btn btn-default"
            onClick={() => {
              document.getElementById("fileUpload").click();
            }}
          >
            <Badge>
              <i className="fa fa-upload" aria-hidden="true"></i>
            </Badge>
            ارسال فایل
          </Button>
          <input
            onChange={(e) => {
              this.props.upload(e);
            }}
            id="fileUpload"
            type="file"
            hidden="hidden"
          />
        </div>}
        
      </div>
    );
  }

  getEditorsStyle() {
    if (this.state.activeItem && this.state.activeItem.label === "یادداشت") {
      return "bg-warning";
    }
    return "";
  }
}

/*TYPING*/

var searchTimeout;
let searchTimeoutSelectedCustomerId;

function callServerScript() {
  if (!searchTimeoutSelectedCustomerId) {
    CurrentUserInfo.LayoutPage.showError("کاربری انتخاب نشده است.");
    return;
  }
  MyCaller.Send("AdminStopTyping", {
    customerId: searchTimeoutSelectedCustomerId,
  });
}

/*TYPING END*/
