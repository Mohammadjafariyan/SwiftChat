import React, { Component } from "react";
import { WorkingHourSetting } from "./bodyComps/WorkingHourSetting/WorkingHourSetting";
import { Button } from "primereact/button";
import ActivePages from "./bodyComps/activeAndInactivePages/ActivePages";
import InActivePages from "./bodyComps/activeAndInactivePages/InActivePages";
import BoolSettings from "./bodyComps/BoolSetting/BoolSetting";
import { DataHolder } from "./../../Help/DataHolder";
import { CurrentUserInfo } from "../../Help/Socket";
import { _showMsg } from "../../Pages/LayoutPage";
import { MyCaller } from "./../../Help/Socket";
import { Spinner } from "react-bootstrap";
import AlarmSetting from './bodyComps/AlarmSetting/AlarmSetting';

export default class SettingBody extends Component {
  state = {};
  constructor(props) {
    super(props);

    CurrentUserInfo.SettingBody = this;
  }

  saveMyWebsiteSettingCallback(res) {
    this.setState({ loading: false });
    _showMsg("با موفقیت ذخیره شد");
  }

  render() {
    if (!this.props.activeMenu) {
      return <></>;
    }

    return (
      <div>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <Button
          className={"p-button-raised  p-button-text"}
          label="ذخیره"
          icon="pi pi-check"
          onClick={() => {
            //  console.log(JSON.stringify(DataHolder.Setting));

            this.setState({ loading: true });
            MyCaller.Send("SaveMyWebsiteSetting", DataHolder.Setting);

            if(CurrentUserInfo.Alarm){
              CurrentUserInfo.Alarm.audioUrl=null;
            }
          }}
        />
        <hr />
        <h4>{this.props.activeMenu.name}</h4>

        {this.props.activeMenu.id == "workingHourSetting" && (
          <WorkingHourSetting />
        )}

        {this.props.activeMenu.id == "ActivePages" && <ActivePages />}

        {this.props.activeMenu.id == "InActivePages" && <InActivePages />}

        {this.props.activeMenu.id == "BoolSettings" && <BoolSettings />}
       
       
        {this.props.activeMenu.id == "AlarmSetting" && <AlarmSetting />}


        
      </div>
    );
  }
}
