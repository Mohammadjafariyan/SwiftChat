import React, { Component } from "react";
import { Dialog } from "primereact/dialog";

export default class Maximize extends Component {
  state = {
    displayMaximizable:true
  };

  render() {
    return (
      <div>

        <Dialog
          header="پیش نمایش در حالت بزرگنمایی"
          maximized={true}
          visible={this.state.displayMaximizable}
          maximizable
          modal
          onHide={() => {
           // this.setState({ displayMaximizable: false });

            this.props.onHide();
          }}
        >
          <div dangerouslySetInnerHTML={{ __html: this.props.html }}></div>
        </Dialog>
      </div>
    );
  }
}
