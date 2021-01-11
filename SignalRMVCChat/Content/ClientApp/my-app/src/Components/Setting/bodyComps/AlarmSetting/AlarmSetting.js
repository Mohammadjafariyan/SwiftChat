import React, { Component } from "react";
import { InputSwitch } from "primereact/inputswitch";
import { DataHolder } from "./../../../../Help/DataHolder";
import { MyCaller } from "./../../../../Help/Socket";
import { Dropdown } from "primereact/dropdown";
import { Button, Spinner } from "react-bootstrap";
import { GetBaseUrlWithWebsiteToken } from "../../../Alarm/Alarm";
import { GetBaseUrl } from "./../../../Alarm/Alarm";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {CurrentUserInfo} from "../../../../CurrentUserInfo";

export default class AlarmSetting extends Component {
  state = {};
  constructor(props) {
    super(props);
    CurrentUserInfo.AlarmSetting = this;

    this.AdminSoundRef = React.createRef();
    this.ViewerSoundRef = React.createRef();
  }

  componentDidMount() {
    console.log(
      "DataHolder.Setting.IsNotificationMuteForViewers-->",
      DataHolder.Setting.IsNotificationMuteForViewers
    );

    console.log(
      "DataHolder.Setting.IsNotificationMuteForViewers-->",
      DataHolder.Setting
    );

    this.setState({
      IsNotificationMuteForViewers:
        DataHolder.Setting.IsNotificationMuteForViewers,
    });

    if (!DataHolder.SoundList) {
      this.setState({ loading: true });
      MyCaller.Send("AlarmGetSounds");
    } else {
      this.setState({ list: DataHolder.SoundList });
    }
  }

  alarmGetSoundsCallback(res) {
    this.setState({ loading: false });

    if (!res || !res.Content) {
      console.error(" مقدار بازگشتی از سرور نال است ");
      return;
    }

    /*   let ViewerSound=res.Content.find(f=>f.label==DataHolder.Setting.ViewerSound);
    let AdminSound=res.Content.find(f=>f.label==DataHolder.Setting.AdminSound);
 */

    DataHolder.SoundList = res.Content;
    this.setState({ list: res.Content });
  }

  render() {
    return (
      <>
        <div>
          <Row>
          <Col>
              <InputSwitch
                checked={DataHolder.Setting.IsNotificationMuteForViewers}
                onChange={(e) => {
                  this.setState({ IsNotificationMuteForViewers: e.value });
                  DataHolder.Setting.IsNotificationMuteForViewers = e.value;
                }}
              />
            </Col>
            <Col>
              <label>هشدار برای بازدیدکنندگان غیرفعال باشد</label>
            </Col>
           
          </Row>
          {this.state.loading && (
            <Row>
              <Col md={4}></Col>
              <Col md={4}>
                <Spinner animation="border" role="status">
                  <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>
              </Col>
              <Col md={4}></Col>
            </Row>
          )}

          {this.state.list && (
            <>
              <Row>
                <Col>
                  <div className="form-group">
                    <Dropdown
                      value={DataHolder.Setting.AdminSound}
                      options={this.state.list}
                      onChange={(e) => {
                        this.setState({ AdminSound: e.value });

                        DataHolder.Setting.AdminSound = e.value;

                        this.AdminSoundRef.current.load();
                      }}
                      placeholder="انتخاب آلارم مخصوص شما"
                    />
                  </div>
                </Col>
                <Col>
                  <label>انتخاب آلارم مخصوص شما</label>
                </Col>
              </Row>
              {DataHolder.Setting.AdminSound && (
                <Row>
                  <Col>
                    <div className="form-group">
                      <br />
                      <audio controls ref={this.AdminSoundRef}>
                        <source
                          src={`${GetBaseUrl()}/Content/Alarm/${
                            DataHolder.Setting.AdminSound
                          }`}
                          type="audio/mp3"
                        />
                        مرورگر شما از پخش آهنگ پشتیبانی نمی کند
                      </audio>
                    </div>
                  </Col>
                  <Col>
                    <label>
                      <Button
                        variant={"info"}
                        onClick={() => {
                          this.AdminSoundRef.current.play();
                        }}
                      >
                        پخش
                      </Button>
                    </label>
                  </Col>
                </Row>
              )}

              <Row>
                <Col>
                  <div className="form-group">
                    <Dropdown
                      value={DataHolder.Setting.ViewerSound}
                      options={this.state.list}
                      onChange={(e) => {
                        this.setState({ ViewerSound: e.value });

                        DataHolder.Setting.ViewerSound = e.value;

                        this.ViewerSoundRef.current.load();
                      }}
                      placeholder="انتخاب آلارم مخصوص بازدیدکنندگان"
                    />
                  </div>
                </Col>
                <Col>
                  <label>انتخاب آلارم مخصوص بازدیدکنندگان</label>
                </Col>
              </Row>

              <Row>
                <Col>
                  {DataHolder.Setting.ViewerSound && (
                    <div className="form-group">
                      <audio controls ref={this.ViewerSoundRef}>
                        <source
                          src={`${GetBaseUrl()}/Content/Alarm/${
                            DataHolder.Setting.ViewerSound
                          }`}
                          type="audio/mp3"
                        />
                        مرورگر شما از پخش آهنگ پشتیبانی نمی کند
                      </audio>
                    </div>
                  )}
                </Col>
                <Col>
                  <label>
                    <Button
                      variant={"info"}
                      onClick={() => {
                        this.ViewerSoundRef.current.play();
                      }}
                    >
                      پخش
                    </Button>
                  </label>
                </Col>
              </Row>
            </>
          )}
        </div>
      </>
    );
  }
}
