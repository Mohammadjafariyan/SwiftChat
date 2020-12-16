import React, { Component } from "react";
import { Row, Col } from "react-bootstrap";
import SettingMenu from "./SettingMenu";
import { CurrentUserInfo, MyCaller } from "../../Help/Socket";
import SettingBody from "./SettingBody";
import SettingWelcome from "./SettingWelcome";
import { DataHolder } from "../../Help/DataHolder";
import SettingchangedAlert from './bodyComps/SettingChangedAlert';

export default class SettingLayout extends Component {
  state = {};

  constructor(props) {
    super(props);
    CurrentUserInfo.SettingLayout = this;

    DataHolder.Setting = {};
  }

  componentDidMount() {
    this.setState({ loading: true });

    MyCaller.Send("GetMyWebsiteSetting");
  }

  getMyWebsiteSettingCallback(res) {
    this.setState({ loading: false });

    if (!res || !res.Content) {
      console.error(" مقدار بازگشتی از سرور نال است ");
      return;
    }

    DataHolder.Setting = res.Content;
    this.setState({ rd: Math.random() });
  }

  render() {
    return (
      <div>
        <Row>
          <Col md={4}>
            <SettingMenu parent={this} />
          </Col>
          <Col>
            {!this.state.activeMenu && <SettingWelcome />}

            {this.state.activeMenu && (

                <SettingchangedAlert  />
                )}
            {this.state.activeMenu && (
              <SettingBody activeMenu={this.state.activeMenu} />
            )}
          </Col>
        </Row>
      </div>
    );
  }
}
