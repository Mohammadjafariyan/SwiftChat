import React, { Component } from "react";
import CompaignChildCompBase from "./CompaignChildCompBase";
import { Editor } from "primereact/editor";
import { _GetSelectedCompaign, _SaveSelectedCompaign } from "../CompaignSave";
import { CurrentUserInfo } from "../../../Help/Socket";
import { InputText } from "primereact/inputtext";
import { Checkbox } from "primereact/checkbox";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/cjs/Col";
import Button from "react-bootstrap/cjs/Button";
import { MyConfirm, ShowMessage } from "./CompaginHelps/CompaignUtility";
import { _showMsg } from "../../../Pages/LayoutPage";
import { Fieldset } from "primereact/fieldset";
import Card from "react-bootstrap/cjs/Card";
import { Alert } from "react-bootstrap";
import { OverlayPanel } from "primereact/overlaypanel";

import "./EditorStyle.css";
import CodeEditor from "./CompaginHelps/CodeEditor";
import { SelectButton } from "primereact/selectbutton";
import ShadowDomEditor from "./CompaginHelps/ShadowDomEditor";
import Maximize from "./CompaginHelps/Maximize";

class CompaignEditor extends CompaignChildCompBase {
  justifyOptions = [
    { name: "ویرایشگر", value: "editor" },
    { name: "کد html", value: "code" },
    { name: "بزرگنمایی", value: "max" },
  ];
  state = {};

  constructor(props) {
    super(props);

    this.op = React.createRef();
  }

  componentDidMount() {
    CurrentUserInfo.CompaignEditor = this;

    
    if (!_GetSelectedCompaign().Template) {
      _GetSelectedCompaign().Template = {};
    }
    this.setState({
      value: _GetSelectedCompaign().Template.Name,
      text1: _GetSelectedCompaign().Template.Html
        ? _GetSelectedCompaign().Template.Html
        : "",
      SendToEmail: _GetSelectedCompaign().SendToEmail,
      SendToChat: _GetSelectedCompaign().SendToChat,
      saveAsTemplateName: _GetSelectedCompaign().saveAsTemplateName,
      saveAsTemplate: _GetSelectedCompaign().saveAsTemplate,
      editorType: "editor",
    });
  }

  componentWillUnmount() {
    _GetSelectedCompaign().Template.Name = this.state.value;
    _GetSelectedCompaign().Template.Html = this.state.text1;
    _GetSelectedCompaign().SendToEmail = this.state.SendToEmail;
    _GetSelectedCompaign().SendToChat = this.state.SendToChat;
    _GetSelectedCompaign().saveAsTemplateName = this.state.saveAsTemplateName;
    _GetSelectedCompaign().saveAsTemplate = _GetSelectedCompaign().saveAsTemplate;
  }

  render() {
    return (
      <div>
        <Button
          variant={"warning"}
          id={"helpBtn"}
          type="button"
          label="راهنما"
          onClick={(e) => this.op.toggle(e)}
        >
          راهنما
        </Button>
        <OverlayPanel
          appendTo={this.op.current}
          ref={(el) => (this.op = el)}
          showCloseIcon
        >
          <Help />
        </OverlayPanel>

        <hr />

        <Row>
          <Col md={2}></Col>
          <Col>
            <label>عنوان</label>
            <br />
            <InputText
              style={{ width: "100%" }}
              value={this.state.value}
              onChange={(e) => {
                this.setState({ value: e.target.value });

                _GetSelectedCompaign().Template.Name = e.target.value;
              }}
            />
          </Col>
          <Col md={2}></Col>
        </Row>

        <br />

        <SelectButton
          value={this.state.editorType}
          options={this.justifyOptions}
          onChange={(e) => this.setState({ editorType: e.value })}
          optionLabel="name"
        />

        <hr />

        {this.state.editorType == "code" && (
          <>
            <CodeEditor
              code={this.state.text1}
              onChange={(val) => {
                this.setState({ text1: val });
                _GetSelectedCompaign().Template.Html = val;
              }}
            />
          </>
        )}
        {this.state.editorType == "editor" && (
          <ShadowDomEditor
            text={this.state.text1}
            onChange={(htmlVal) => {
              this.setState({ text1: htmlVal });
            }}
          ></ShadowDomEditor>
        )}

        {this.state.editorType == "max" && (
          <Maximize
            html={this.state.text1}
            onHide={() => {
              this.setState({ editorType: "editor" });
            }}
          />
        )}

        <br />
        <Row>
          <Col>
            <Checkbox
              value={this.state.SendToEmail}
              onChange={(e) => {
                this.setState({ SendToEmail: e.checked });
                _GetSelectedCompaign().SendToEmail = e.checked;
              }}
              checked={this.state.SendToEmail}
            />
            <label>ارسال به ایمیل</label>
          </Col>

          <Col>
            <Checkbox
              value={this.state.SendToChat}
              onChange={(e) => {
                this.setState({ SendToChat: e.checked });
                _GetSelectedCompaign().SendToChat = e.checked;
              }}
              checked={this.state.SendToChat}
            />
            <label>ارسال به چت</label>
          </Col>
        </Row>
        <hr />
        <Row>
          <Col>
            <Button
              variant={"info"}
              onClick={() => {
                if (_GetSelectedCompaign().saveAsTemplate) {
                  _showMsg("این نوشتار قبلا بعنوان قالب ذخیره شده است");
                }

                this.setState({ displayConfirm: true });
              }}
            >
              ذخیره بعنوان قالب آماده
            </Button>

            {/*     <Button className={'p-button-raised  p-button-text'} label="ذخیره" icon="pi pi-check"
                                onClick={() => {


                                    this.saveThenGoNext();

                                }}>
                            ارسال
                        </Button>*/}
          </Col>
        </Row>

        <MyConfirm
          parent={this}
          title={"لطفا یک نام برای این قالب اختصاص دهید"}
          display={this.state.displayConfirm}
          onConfirm={() => {
            _GetSelectedCompaign().saveAsTemplate = true;

            this.setState({ displayConfirm: false });
          }}
          body={
            <>
              <label>عنوان قالب </label>
              <br />
              <InputText
                placeholder={"یک نام برای قالب نیاز است"}
                value={this.state.saveAsTemplateName}
                onChange={(e) => {
                  this.setState({ saveAsTemplateName: e.target.value });

                  _GetSelectedCompaign().saveAsTemplateName = e.target.value;
                }}
              />
            </>
          }
        />
      </div>
    );
  }

  saveThenGoNext() {
    _SaveSelectedCompaign(() => {
      this.goNext();
    });
  }
}

export default CompaignEditor;

export const Help = (props) => {
  const msg2 = `
    {{ name.first}}`;

  const msg3 = `{{variable_name}}`;
  const msg4 = `{{variable_name | "مقدار متن جایگزین"}}`;
  const msg = `
     <span style="border-radius: 45%" class="btn btn-info">1</span>   نام و نام خانوادگی: {{ name.full }} or {{ name.full | "Fallback Full Name" }}
     <span style="border-radius: 45%" class="btn btn-info">2</span>   نام: {{ name.first }} or {{ name.first | "Fallback First Name" }}
     <span style="border-radius: 45%" class="btn btn-info">3</span>   نام خانوادگی: {{ name.last }} or {{ name.last | "Fallback Last Name" }}
     <span style="border-radius: 45%" class="btn btn-info">4</span>   ایمیل: {{ email }} or {{ email | "Fallback Email" }}
     <span style="border-radius: 45%" class="btn btn-info">5</span>   کشور: {{ country }} or {{ country | "Fallback Country" }}
     <span style="border-radius: 45%" class="btn btn-info">6</span>   شهر: {{ city }} or {{ city | "Fallback City" }}
     <span style="border-radius: 45%" class="btn btn-info">7</span>    وبسایت: {{ website }} or {{ website | "Fallback Website" }}
     <span style="border-radius: 45%" class="btn btn-info">8</span>    نام شرکت: {{ company.name }} or {{ company.name | "Fallback Company Name" }}
     <span style="border-radius: 45%" class="btn btn-info">9</span>   نام فیلد یک فرم: {{ form.field }} or {{ form.field | "Fallback Company Name" }}
     <span style="border-radius: 45%" class="btn btn-info">10</span>   مقدار داده سفارشی: {{ data.your_key }} or {{ data.your_key | "Fallback Value" }}`;
  return (
    <>
      <Card>
        <Alert variant={"warning"} className={"text-right"}>
          <h6>کدام متغیرها موجود هستند؟</h6>
          <p>می توانید از متغیرهای کاربر زیر در پیام های خود استفاده کنید</p>
          <h5>
            <b>نحوه استفاده به شکل زیر است</b>
          </h5>
          {msg4}
          یا
          {msg3}
          <pre
            className={"text-left"}
            dangerouslySetInnerHTML={{ __html: msg }}
          ></pre>
        </Alert>
      </Card>{" "}
      <br />
    </>
  );
};
