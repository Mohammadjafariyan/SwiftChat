import React, { Component } from "react";
import { Dropdown } from "primereact/dropdown";
import { MyCaller, CurrentUserInfo } from "../../../../Help/Socket";
import { Spinner } from "react-bootstrap";

export default class FormSelect extends Component {
  state = {};

  componentDidMount() {
    this.setState({ loading: true });
    MyCaller.Send("GetCreatedForms");

    CurrentUserInfo.FormSelect = this;
  }

  getCreatedFormsCallback(res) {
    if (!res || !res.Content || !res.Content.EntityList) {
      CurrentUserInfo.LayoutPage.showError("لیست فرم ها نال است");
      return;
    }

    this.setState({ loading: false });
    this.setState({ formList: res.Content.EntityList });

    if (this.props.preValue) {
      var selected = res.Content.EntityList.find(
        (f) => f.Id == this.props.preValue
      );

      this.setState({ selectedForm: selected });
    }
  }
  render() {
    return (
      <div>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <Dropdown
        id={this.props.id}
          value={this.state.selectedForm}
          options={this.state.formList}
          onChange={(e) => {
            this.setState({ selectedForm: e.value });

            this.props.onChange(e.value);
          }}
          optionLabel="Name"
          filter
          showClear
          filterBy="Name"
          placeholder="یک فرم انتخاب کنید"
        />
      </div>
    );
  }
}
