import React, { Component } from "react";
import { MyCaller} from "./../Help/Socket";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {
  CallValidation,
  FireValidation,
  Input,
} from "../Components/FormInputs";

import "../styles/myStyle.css";
import {
  Card,
  Container,
  Form,
  Button,
  Alert,
  ListGroup,
} from "react-bootstrap";
import { MyModal } from "../Components/Modal";
import { _showError } from "./LayoutPage";
import {CurrentUserInfo} from "../CurrentUserInfo";

class FormCreatorPage extends Component {
  constructor(prop) {
    super(prop);
    this.state = { showModal: false };

    CurrentUserInfo.FormCreatorPage = this;
  }

  DeleteElement(el, i) {
    if (!this.state.elements) {
      _showError("this.state.elements is null");
    }

    if (!this.state.elements[i]) {
      _showError("this.state.elements[i] is null");
    }

    let elements = this.state.elements;

    elements = elements.filter((e) => e != el);

    this.setState({ elements: elements });
  }

  getFormSingleCallback(res) {
    if (!res || !res.Content) {
      CurrentUserInfo.LayoutPage.showError("اطلاعات بازگشتی از سرور نال است");
      return;
    }

    console.log(res);

    this.setState({
      elements: res.Content.Elements,
      Name: res.Content.Name,
      AfterMessage: res.Content.AfterMessage,
      OnlyMe: res.Content.OnlyMe,
      Message: res.Content.Message,
      Id: res.Content.Id,
    });
  }

  addNewFormElement(el) {
    let elements = this.state.elements;
    if (!elements) {
      elements = [];
    }

    elements.push(el);
    this.setState({ elements: elements });

    this.setState({ elementsError: null });
  }

  deleteFormCallback(formId) {
    CurrentUserInfo.LayoutPage.showMsg("فرم حذف شد");
    this.clearForm();

    if (CurrentUserInfo.CreatedForms) {
      CurrentUserInfo.CreatedForms.componentDidMount();
    }
  }

  DeleteForm(formId) {
    CurrentUserInfo.LayoutPage.showMsg("در حال حذف فرم");

    MyCaller.Send("DeleteForm", { formId: formId });
  }

  render() {
    return (
      <Container>
        <Row>
          <Col>
            <Card>
              <Card.Body>
                <Card.Text>
                  <Alert variant={"info"}>
                    <b>
                      {" "}
                      دریافت اطلاعات از کاربر با تعریف انواع فرم و استفاده در
                      اتاق چت
                    </b>
                    <hr />
                    در این بخش فرم ها تعریف ، ویرایش و حذف می شوند
                  </Alert>
                </Card.Text>
              </Card.Body>
            </Card>
          </Col>
        </Row>
        <br />

        <Row>
          <Col md={3}>
            <CreatedForms />
          </Col>

          <Col md={9}>
            <Row>
              <Card>
                <Card.Body id={"form"}>
                  <Card.Title>
                    {this.state.Id && <span>ویرایش فرم</span>}
                    {!this.state.Id && <span>فرم جدید</span>}

                    {this.state.Id && (
                      <div style={{ float: "left" }}>
                        <Button
                          onClick={() => {
                            this.DeleteForm(this.state.Id);
                          }}
                          variant="default"
                        >
                          <a className={"pointer  floatLeft text-danger"}>
                            حذف فرم
                            <i className="fa fa-trash" aria-hidden="true"></i>
                          </a>
                        </Button>
                      </div>
                    )}
                  </Card.Title>
                  <Card.Text>
                    <Row>
                      <Col>
                        <Input
                          value={this.state.Message}
                          type={"textarea"}
                          formName={"form"}
                          required={true}
                          parent={this}
                          parentStateName={"Message"}
                          id={"beforeMsg"}
                          name={"متن فرم"}
                          placeholder={"مثال: درصورتی که آفلاین بودیم"}
                        />
                      </Col>

                      <Col>
                        <Input
                          value={this.state.Name}
                          type={"text"}
                          formName={"form"}
                          required={true}
                          parentStateName={"Name"}
                          parent={this}
                          id={"title"}
                          name={"عنوان فرم"}
                          placeholder={"عنوان فرم را وارد نمایید"}
                        />
                      </Col>
                    </Row>

                    <Row>
                      <Col>
                        <Input
                          value={this.state.AfterMessage}
                          type={"textarea"}
                          id={"afterMsg"}
                          formName={"form"}
                          required={true}
                          parent={this}
                          parentStateName={"AfterMessage"}
                          name={
                            "پیغام بعد از پر کردن فرم به کاربر نمایش داده می شود:"
                          }
                          placeholder={"مثال: با تشکر فرم ارسال گردید"}
                        />
                      </Col>
                      <Col>
                        <Form.Group controlId="onlyMe">
                          <Form.Check
                            type="checkbox"
                            label="انحصارا این فرم مخصوص شما باشد"
                            checked={this.state.OnlyMe}
                            onChange={(e) => {
                              let onlyMe = this.state.OnlyMe;
                              if (!onlyMe) {
                                onlyMe = true;
                              } else {
                                onlyMe = false;
                              }
                              this.setState({ OnlyMe: onlyMe });
                            }}
                          />
                        </Form.Group>
                      </Col>
                    </Row>
                    <hr />

                    <Row>
                      <Col md={3}>
                        <Button
                          id={"save"}
                          onClick={() => {
                            this.save();
                          }}
                        >
                          ذخیره تغییرات
                        </Button>{" "}
                      </Col>
                    </Row>
                    <hr />

                    <Row>
                      <Col md={5}>
                        <FormCreatorToolsMenu parent={this} />
                      </Col>
                      <Col md={7}>
                        <Card>
                          <Card.Body>
                            {this.state.elements && (
                              <h5>لیست فیلد های تعریف شده </h5>
                            )}
                            {(!this.state.elements ||
                              !this.state.elements.length) && (
                              <h5>از منوی ابزار ابتدا انتخاب نمایید</h5>
                            )}

                            {this.state.elements && (
                              <FormRenderer
                                elements={this.state.elements}
                                editable={true}
                              />
                            )}

                            {this.state.elementsError && (
                              <small style={{ color: "darkred" }}>
                                {this.state.elementsError}
                              </small>
                            )}
                          </Card.Body>
                        </Card>
                      </Col>
                    </Row>
                  </Card.Text>
                  {/* <Button variant="primary">Go somewhere</Button>*/}
                </Card.Body>
              </Card>
            </Row>
          </Col>
        </Row>

        <MyModal
          show={this.state.showModal}
          handleClose={() => {
            this.setState({ showModal: false });
          }}
          title={"ویرایش ورودی های فرم "}
          onSave={() => {
            this.setState({ tmp: Math.random() });
          }}
        >
          <EditElementForm />
        </MyModal>
      </Container>
    );
  }

  save() {
    if (!CallValidation("form")) {
      if (!this.state.elements || !this.state.elements.length) {
        this.setState({ elementsError: "هیچ ورودی برای فرم انتخاب نشده است" });
        return;
      }
    } else {
      if (!this.state.elements || !this.state.elements.length) {
        this.setState({ elementsError: "هیچ ورودی برای فرم انتخاب نشده است" });
        return;
      }
    }

    if (this.validateInputSubElements() === false) {
      this.setState({
        elementsError:
          "برای چک باکس ها و ورودی های انتخاب دار ، باید مقادیر تعریف نمایید",
      });
      return;
    }

    CurrentUserInfo.LayoutPage.showMsg("در حال ذخیره فرم");

    MyCaller.Send("SaveForm", {
      elements: this.state.elements,
      Name: this.state.Name,
      AfterMessage: this.state.AfterMessage,
      Message: this.state.Message,
      OnlyMe: this.state.OnlyMe ? true : false,
      Id: this.state.Id,
    });
  }

  clearForm() {
    this.setState({
      elements: [],
      Name: "",
      AfterMessage: "",
      Message: "",
      OnlyMe: false,
      Id: null,
    });
  }

  validateInputSubElements() {
    /*
     *
     * هیچ چک باکسی نباید بدون الیمنت زیر مجموع باشد
     * */

    for (let i = 0; i < this.state.elements.length; i++) {
      if (
        this.state.elements[i].type == "checkbox" ||
        this.state.elements[i].type == "radio"
      ) {
        if (
          !this.state.elements[i].SubElements ||
          this.state.elements[i].SubElements.length === 0
        ) {
          return false;
        }
      }
    }

    return true;
  }

  saveFormCallback(res) {
    CurrentUserInfo.LayoutPage.showMsg("تغییرات با موفقیت اعمال شد");

    if (CurrentUserInfo.CreatedForms) {
      CurrentUserInfo.CreatedForms.componentDidMount();
    }

    this.clearForm();
  }
}

export default FormCreatorPage;

export class FormCreatorToolsMenu extends Component {
  select(type) {
    this.props.parent.addNewFormElement({ type: type, Value: "" });
  }

  render() {
    return (
      <div className={"toolsMenuFormCreator"}>
        <h5>نوار ابزار</h5>

        <FormElement
          onClick={() => this.select("input")}
          id={"addElementInput"}
        >
          <input />
        </FormElement>

        <hr />
        <FormElement
          onClick={() => this.select("textarea")}
          id={"addElementTextArea"}
        >
          <textarea />
        </FormElement>
        <hr />
        <FormElement
          onClick={() => this.select("checkbox")}
          id={"addElementCheckBox"}
        >
          <input type="checkbox" />
        </FormElement>

        <hr />

        <FormElement
          onClick={() => this.select("radio")}
          id={"addElementRadio"}
        >
          <input type="radio" />
        </FormElement>

        <hr />
      </div>
    );
  }
}

const FormElement = (props) => {
  return (
    <div
      className={"fillerHolder"}
      id={props.id}
      onClick={() => {
        props.onClick();
      }}
    >
      <div className={"filler"}></div>
      {props.children}
    </div>
  );
};

const FormRendererSingleElement = (props) => {
  return (
    <div className={"FormRendererSingleElement"}>
      <Card className={"fullWidth"}>
        <Card.Body>
          <Row className={"fullWidth"}>
            <Col md={2}>
              <div style={{ float: "left" }}>
                {props.editable && (
                  <Button
                    onClick={() => {
                      CurrentUserInfo.FormCreatorPage.DeleteElement(
                        props.el,
                        props.j
                      );
                    }}
                    variant="default"
                  >
                    <a className={"pointer  floatLeft text-danger"}>
                      <i className="fa fa-trash" aria-hidden="true"></i>
                    </a>
                  </Button>
                )}
              </div>
              <br />
            </Col>

            <Col>
              <div>
                {props.el.Name && (
                  <>
                    <label>
                      <span>عنوان فیلد :</span>
                      {props.el.Name}
                    </label>
                    <br />
                  </>
                )}

                {props.children}

                {props.el.Help && (
                  <small>
                    <span>متن راهنما :</span>
                    {props.el.Help}
                  </small>
                )}
              </div>
            </Col>
            <Col md={1}>
              {props.editable && (
                <Button
                  className={"editElement"}
                  onClick={() => {
                    CurrentUserInfo.selectedElementIndex = props.j;
                    CurrentUserInfo.FormCreatorPage.setState({
                      showModal: true,
                    });
                  }}
                >
                  <i className="fa fa-pencil" aria-hidden="true"></i>
                </Button>
              )}
            </Col>
          </Row>
        </Card.Body>
      </Card>
    </div>
  );
};

export class FormRenderer extends Component {
  render() {
    return (
      <div>
        {this.props.elements &&
          this.props.elements.map((el, i, arr) => {
            if (el.type === "input") {
              return (
                <FormRendererSingleElement
                  el={el}
                  j={i}
                  editable={this.props.editable}
                >
                  <Input
                    readonly={!this.props.editable ? "readonly" : null}
                    value={el.Value}
                    onChange={(e) => {
                      el.Value = e.target.value;
                    }}
                  />
                </FormRendererSingleElement>
              );
            }

            if (el.type === "textarea") {
              return (
                <FormRendererSingleElement
                  el={el}
                  j={i}
                  editable={this.props.editable}
                >
                  {" "}
                  <textarea
                    readonly={!this.props.editable ? "readonly" : null}
                    className={"form-control"}
                    value={el.Value}
                    onChange={(e) => {
                      el.Value = e.target.value;
                    }}
                  ></textarea>
                </FormRendererSingleElement>
              );
            }

            if (el.type === "checkbox") {
              return (
                <FormRendererSingleElement
                  el={el}
                  j={i}
                  editable={this.props.editable}
                >
                  {showSubElements(el, this.props.editable)}

                  {/*   <Input type='checkbox'  value={el.Value}
                                                                             onChange={(e)=>{
                                                                                 el.Value=e.target.value;
                                                                             }}/>*/}
                </FormRendererSingleElement>
              );
            }

            if (el.type === "radio") {
              return (
                <FormRendererSingleElement
                  el={el}
                  j={i}
                  editable={this.props.editable}
                >
                  {showSubElements(el, this.props.editable)}

                  {/*     <Input type='radio'  value={el.Value} onChange={(e)=>{
                                el.Value=e.target.value;
                            }}/>*/}
                </FormRendererSingleElement>
              );
            }
          })}
      </div>
    );
  }
}

export class CreatedForms extends Component {
  constructor(props, context) {
    super(props, context);
    this.state = {};

    CurrentUserInfo.CreatedForms = this;
  }

  componentDidMount() {
    MyCaller.Send("GetCreatedForms");
  }

  getCreatedFormsCallback(res) {
    if (!res || !res.Content || !res.Content.EntityList) {
      CurrentUserInfo.LayoutPage.showError("لیست فرم ها نال است");
      return;
    }

    this.setState({ formList: res.Content.EntityList });
  }

  select(form) {
    CurrentUserInfo.LayoutPage.showMsg("در حال خواندن اطلاعات فرم");
    MyCaller.Send("GetFormSingle", { formId: form.Id });
  }

  render() {
    return (
      <div>
        {!this.state.formList && <p>در حال خواندن فرم های تعریف شده</p>}
        <ListGroup className={"listGroup"}>
          <ListGroup.Item className={" text-primary"}>
            <span> </span>

            <a className={"pointer  floatRight  text-secondary "}>ویرایش</a>

            <a
              className={"pointer  floatLeft  text-info "}
              title={"نمایش اطلاعات وارد شده"}
            >
              اطلاعات
            </a>
          </ListGroup.Item>

          {this.state.formList &&
            this.state.formList.map((el, i, arr) => {
              return (
                <>
                  <ListGroup.Item
                    key={i}
                    className={" text-primary"}
                    onClick={() => {
                      // this.select(el);
                    }}
                  >
                    <span>{el.Name}</span>

                    <a
                      className={"pointer  floatRight  text-secondary "}
                      onClick={() => {
                        debugger;

                        CurrentUserInfo.selectedForm = el;

                        CurrentUserInfo.LayoutPage.showMsg(
                          "در حال خواندن اطلاعات فرم"
                        );
                        MyCaller.Send("GetFormSingle", { formId: el.Id });
                        if (CurrentUserInfo.Menu)
                          CurrentUserInfo.Menu.setPage("FormCreator");

                        if (CurrentUserInfo.B4AdminMainMenu)
                          CurrentUserInfo.B4AdminMainMenu.setPage(
                            "FormCreator"
                          );
                      }}
                    >
                      <i className="fa fa-pencil" aria-hidden="true"></i>
                    </a>

                    <a
                      className={"pointer  floatLeft  text-info "}
                      title={"نمایش اطلاعات وارد شده"}
                      onClick={() => {
                        debugger;
                        CurrentUserInfo.selectedForm = el;

                        CurrentUserInfo.LayoutPage.showMsg(
                          "در حال خواندن اطلاعات "
                        );

                        MyCaller.Send("GetFormData", { formId: el.Id });
                        if (CurrentUserInfo.Menu)
                          CurrentUserInfo.Menu.setPage("FormDataPage");

                        if (CurrentUserInfo.B4AdminMainMenu)
                          CurrentUserInfo.B4AdminMainMenu.setPage(
                            "FormDataPage"
                          );
                      }}
                    >
                      <i className="fa fa-table" aria-hidden="true"></i>
                    </a>
                  </ListGroup.Item>
                </>
              );
            })}
        </ListGroup>
      </div>
    );
  }
}

function showSubElements(el, editable) {
  if (!el.SubElements) {
    if (el.type === "checkbox" || el.type === "radio") {
      return <p>مقادیر چک باک انتخاب نشده است</p>;
    } else {
      return <></>;
    }
  }

  return el.SubElements.map((subEl, j, arrr) => {
    return (
      <Row key={subEl.Tmp} dir={"rtl"}>
        <Col md={2} style={{ paddingTop: "42px" }}>
          <Input
            readonly={!editable ? "readonly" : null}
            checked={el.Value === subEl.Value ? "checked" : null}
            elname={el.FieldName}
            type={el.type}
          />
        </Col>
        <Col>
          <Input
            className={"subelement"}
            readonly={!editable ? "readonly" : null}
            value={subEl.Value}
            type={"text"}
            elname={el.FieldName}
            name={"متن"}
            onChange={(e) => {
              subEl.Value = e.target.value;
            }}
          />
        </Col>
        <Col>
          <Input
            className={"subelement"}
            readonly={!editable ? "readonly" : null}
            value={subEl.FieldName}
            elname={el.FieldName}
            type={"text"}
            name={"مقدار"}
            onChange={(e) => {
              subEl.FieldName = e.target.value;
            }}
          />
        </Col>
      </Row>
    );
  });
}

class EditElementForm extends Component {
  render() {
    if (
      CurrentUserInfo.selectedElementIndex == null ||
      CurrentUserInfo.selectedElementIndex == undefined
    ) {
      return <p>هیچ المنتی انتخاب نشده است</p>;
    }

    if (
      !CurrentUserInfo.FormCreatorPage ||
      !CurrentUserInfo.FormCreatorPage.state ||
      !CurrentUserInfo.FormCreatorPage.state.elements
    ) {
      return <p>مقادیر نال هستند</p>;
    }

    return (
      <div className={"editForm"}>
        <Input
          value={
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].Name
          }
          type={"text"}
          id={"fieldTitle"}
          onChange={(e) => {
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].Name = e.target.value;
          }}
          name={"عنوان فیلد"}
        />

        <Input
          value={
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].Help
          }
          id={"fieldHelp"}
          type={"text"}
          onChange={(e) => {
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].Help = e.target.value;
          }}
          name={"متن راهنما"}
        />

        <Input
          value={
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].FieldName
          }
          id={"fieldName"}
          type={"text"}
          onChange={(e) => {
            CurrentUserInfo.FormCreatorPage.state.elements[
              CurrentUserInfo.selectedElementIndex
            ].FieldName = e.target.value;
          }}
          name={"نام یکتای فیلد"}
        />

        {(CurrentUserInfo.FormCreatorPage.state.elements[
          CurrentUserInfo.selectedElementIndex
        ].type === "checkbox" ||
          CurrentUserInfo.FormCreatorPage.state.elements[
            CurrentUserInfo.selectedElementIndex
          ].type === "radio") && (
          <>
            <hr />
            <Button
              id={"addSubElements"}
              variant={"default"}
              onClick={() => {
                this.addSubElement(
                  CurrentUserInfo.FormCreatorPage.state.elements[
                    CurrentUserInfo.selectedElementIndex
                  ]
                );
              }}
            >
              {" "}
              <span>+</span> <span>مقادیر قابل انتخاب</span>{" "}
            </Button>
          </>
        )}

        {showSubElements(
          CurrentUserInfo.FormCreatorPage.state.elements[
            CurrentUserInfo.selectedElementIndex
          ],
          true
        )}
      </div>
    );
  }

  addSubElement(element) {
    if (!element.SubElements) {
      element.SubElements = [];
    }

    element.SubElements.push({ Tmp: Math.random() });

    this.setState({ tmp: Math.random() });
  }
}
