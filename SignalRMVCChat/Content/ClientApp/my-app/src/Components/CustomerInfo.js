import React, { Component } from "react";
import Badge from "react-bootstrap/Badge";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Form } from "react-bootstrap";
import { DataHolder } from "../Help/DataHolder";
import Button from "react-bootstrap/Button";
import Satistification from "./Satistification";
import MarkAsResovled from "./MarkAsResovled";
import { UserInformationService } from "./Service/UserInformationService";
import { _showError } from "../Pages/LayoutPage";
import BlockUser from "./BlockUser/BlockUser";
import CustomerTimeSpent from "./Profile/CustomerTimeSpent";
import {CurrentUserInfo} from "../CurrentUserInfo";

class CustomerInfo extends Component {
  state = {};

  constructor(arg) {
    super(arg);

    CurrentUserInfo.CustomerInfo = this;
  }

  saveUserInfoCallback(res) {
    if (!res || !res.Content) {
      _showError(" مقدار بازگشتی از سرور نال است ");
      return;
    }
    if (!DataHolder.selectedCustomer) {
      return;
    }

    DataHolder.selectedCustomer[res.Content.PropertyName] =
      res.Content.PropertyValue;

    this.setState({ tmp: Math.random() });
  }

  render() {
    if (!DataHolder.selectedCustomer) return <></>;

    console.log("CustomerInfo===>DataHolder.selectedCustomer====>");
    console.log(DataHolder.selectedCustomer);
    return (
      <div>
        <div className={"card  "}>
          <div className={"card-header "}>
            اطلاعات کاربر
            <span className="p-toast-message-icon pi pi-check"></span>
          </div>

          <div className="card-body" style={{ overflow: "auto" }}>
            <Badge pill variant="light">
              <img
                width={100}
                height={100}
                src={`${process.env.PUBLIC_URL}/Content/HelpImages/user-icon.png`}
              />

              {DataHolder.selectedCustomer.LastTrackInfo &&
                DataHolder.selectedCustomer.LastTrackInfo.country_code && (
                  <Badge pill variant="light">
                    {/*      <img
                                    src={`https://www.countryflags.io/${DataHolder.selectedCustomer.LastTrackInfo.country_code}/flat/64.png`}/>
                            */}

                    <img
                      style={{ width: "30px", height: "30px" }}
                      src={`/Content/flags/flag of ${DataHolder.selectedCustomer.LastTrackInfo.country_name}.gif`}
                    />
                  </Badge>
                )}

              {(!DataHolder.selectedCustomer.LastTrackInfo ||
                !DataHolder.selectedCustomer.LastTrackInfo.country_code) && (
                <Badge pill variant="light">
                  <img src={`https://www.countryflags.io/ir/flat/64.png`} />
                </Badge>
              )}
            </Badge>

            <Form.Group controlId="formBasicEmail">
              {!this.state.editingName && (
                <Form.Label
                  onClick={() => {
                    this.setState({ editingName: true });
                  }}
                >
                  <i className={"fa fa-pencil"}></i>

                  {DataHolder.selectedCustomer.Name}
                </Form.Label>
              )}
              {this.state.editingName && (
                <Form.Control
                  type="text"
                  placeholder="نام کاربر"
                  value={DataHolder.selectedCustomer.Name}
                  onChange={(e) => {
                    DataHolder.selectedCustomer.Name = e.target.value;
                    this.setState({ tmp: Math.random() });

                    if (CurrentUserInfo.CustomersPage) {
                      CurrentUserInfo.CustomersPage.setState({
                        tmp: Math.random(),
                      });
                    }
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      this.setState({ editingName: false });
                      UserInformationService.SaveUserInfo("Name");
                    }
                  }}
                />
              )}

              <br />

              {!this.state.editingEmail && (
                <Form.Label
                  onClick={() => {
                    this.setState({ editingEmail: true });
                  }}
                >
                  <i className={"fa fa-pencil"}></i>
                  <a href="#">
                    {DataHolder.selectedCustomer.Email
                      ? DataHolder.selectedCustomer.Email
                      : "ایمیل"}
                  </a>
                </Form.Label>
              )}
              {this.state.editingEmail && (
                <Form.Control
                  type="email"
                  placeholder="محل قرار گرفتن ایمیل کاربر"
                  value={DataHolder.selectedCustomer.Email}
                  onChange={(e) => {
                    DataHolder.selectedCustomer.Email = e.target.value;
                    this.setState({ tmp: Math.random() });
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      this.setState({ editingEmail: false });
                      UserInformationService.SaveUserInfo("Email");
                    }
                  }}
                />
              )}

              <br />

              {!this.state.editingPhone && (
                <Form.Label
                  onClick={() => {
                    this.setState({ editingPhone: true });
                  }}
                >
                  <i className={"fa fa-phone"}></i>
                  <b>
                    {DataHolder.selectedCustomer.Phone
                      ? DataHolder.selectedCustomer.Phone
                      : "شماره تلفن"}
                  </b>
                </Form.Label>
              )}
              {this.state.editingPhone && (
                <Form.Control
                  type="text"
                  placeholder="شماره تلفن"
                  value={DataHolder.selectedCustomer.Phone}
                  onChange={(e) => {
                    DataHolder.selectedCustomer.Phone = e.target.value;
                    this.setState({ tmp: Math.random() });
                  }}
                  onKeyDown={(e) => {
                    if (e.key === "Enter") {
                      this.setState({ editingPhone: false });
                      UserInformationService.SaveUserInfo("Phone");
                    }
                  }}
                />
              )}

              <Form.Text className="text-muted">
                {DataHolder.selectedCustomer.Address}
                {!DataHolder.selectedCustomer.Address && (
                  <p>آدرس کاربر مشخص نیست</p>
                )}
              </Form.Text>

              <Form.Text className="text-muted">
                {DataHolder.selectedCustomer.JobTitle}
                {!DataHolder.selectedCustomer.JobTitle && (
                  <p>شغل کاربر مشخص نیست</p>
                )}
              </Form.Text>

              <Form.Text className="text-muted">
                {DataHolder.selectedCustomer.CompanyName}
                {!DataHolder.selectedCustomer.CompanyName && (
                  <p>شرکت کاربر مشخص نیست</p>
                )}
              </Form.Text>

              <Button
                variant={"primary"}
                onClick={() => {
                  DataHolder.Back = null;
                  if (CurrentUserInfo.Menu)
                    CurrentUserInfo.Menu.setPage("CustomerProfilePage");

                  if (CurrentUserInfo.B4AdminMainMenu)
                    CurrentUserInfo.B4AdminMainMenu.setPage(
                      "CustomerProfilePage"
                    );
                }}
              >
                <Badge variant={"primary"}>
                  <i className={"fa fa-user"}></i>
                </Badge>
                نمایش جزییات رفتار
              </Button>
            </Form.Group>

            {(!DataHolder.selectedCustomer ||
              !DataHolder.selectedCustomer.UsersSeparationParams) && (
              <small>کاربر عضو نیست</small>
            )}

            <Satistification />

            <BlockUser Customer={DataHolder.selectedCustomer} />
          </div>
        </div>
      </div>
    );
  }
}

export default CustomerInfo;
