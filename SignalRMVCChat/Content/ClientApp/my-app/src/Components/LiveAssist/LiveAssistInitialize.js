import React, { Component } from "react";
import Table from "react-bootstrap/Table";
import Button from "react-bootstrap/Button";
import { MyCaller } from "../../Help/Socket";
import { DataHolder } from "../../Help/DataHolder";
import {_showError} from "../../Pages/LayoutPage";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class LiveAssistInitialize extends Component {
  state={};
  sendRequest() {
    if (!DataHolder.selectedCustomer) {
      _showError("هیچ کاربری انتخاب نشده است");
      return;
    }

    CurrentUserInfo.LiveAssistLayout.setState({ loading: true, step: "show" });
    MyCaller.Send("LiveAssistRequest", {
      customerId: DataHolder.selectedCustomer.Id,
    });

    
  }
  render() {
    return (
      <div>
        <i className="fa fa-television bigIcon" aria-hidden="true"></i>

        <Table striped bordered hover>
          <thead>
            <tr>
              <th>استارت صفحه مرورگر مشترک</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <p>برای شروع دکمه زیر را فشار دهید</p>

                <Button
                  variant={"success"}
                  onClick={() => {
                    this.sendRequest();
                  }}
                >
                  <i
                    className="fa fa-user-circle bigIcon"
                    aria-hidden="true"
                  ></i>
                  شروع
                </Button>
              </td>
            </tr>
          </tbody>
        </Table>
      </div>
    );
  }
}

export default LiveAssistInitialize;
