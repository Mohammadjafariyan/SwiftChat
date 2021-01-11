import React, { Component } from "react";
import BaseIndex from "../../CRUD/BaseIndex";
import ListGroup from "react-bootstrap/ListGroup";
import Card from "react-bootstrap/cjs/Card";
import { InputSwitch } from "primereact/inputswitch";
import CompaignFilter from "../Filter/CompaignFilter";
import { MyModal } from "../../Components/Modal";
import Badge from "react-bootstrap/Badge";
import { DataHolder } from "./../../Help/DataHolder";
import { MyCaller } from './../../Help/Socket';
import { _showMsg } from "../../Pages/LayoutPage";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class CompaignIndex extends BaseIndex {
  getCallback(res) {
    if (!res || !res.Content) {
      return;
    }

    let automatic = res.Content.filter((f) => f.IsAutomatic);
    let oneshot = res.Content.filter((f) => !f.IsAutomatic);
    this.setState({ automatic: automatic, oneshot: oneshot });

    this.openAfterLoadHelp(automatic);
    this.openAfterLoadHelp(oneshot);

    DataHolder.openAfterLoadId = null;
  }

  save(){

    if (this.props.saveDraft){
       
      MyCaller.Send(this.props.saveDraft,{
          SendToChat:true,
          SendToEmail:true
        });
   
    }else{
        MyCaller.Send(this.props.save,{});
    }
    _showMsg("در حال ایجاد رکورد جدید")

}

  openAfterLoadHelp(list) {
    let s = DataHolder.openAfterLoadId;
    console.log(s);
    if (DataHolder.openAfterLoadId && list) {
      let find = list.find((f) => f.Id == DataHolder.openAfterLoadId);

      if (find) {
        this.setState({
          selectedNotChanged: JSON.stringify(find),
          selected: find,
        });

        this.props.parent.setState({
          selected: find,
        });
        DataHolder.openAfterLoadId = null;
      }
    }
  }

  render() {
    return (
      <>
        <CompaignFilter></CompaignFilter> <br />
        {super.render()}{" "}
      </>
    );
  }

  componentDidMount() {
    CurrentUserInfo.CompaignIndex = this;

    super.componentDidMount();
  }

  openAfterLoad(compaignId) {
    DataHolder.openAfterLoadId = compaignId;
  }

  renderMenu() {
    return (
      <>
        {this.renderMenuHelp("کمپین های اتوماتیک", this.state.automatic)}
        {this.renderMenuHelp("کمپین های دستی", this.state.oneshot)}
      </>
    );
  }

  renderMenuHelp(title, list) {
    if (!list || !list.length) {
      return <></>;
    }
    return (
      <ListGroup>
        <ListGroup.Item className={"bg-info text-white"}>
          {title}
        </ListGroup.Item>
        {list.map((row, i, arr) => {
          return (
            <ListGroup.Item>
              <Card.Link
                onClick={() => {
                  this.setState({
                    selected: null,
                  });

                  this.props.parent.setState({
                    selected: null,
                  });

                  setTimeout(() => {
                    this.setState({
                      selectedNotChanged: JSON.stringify(row),
                      selected: row,
                    });

                    this.props.parent.setState({
                      selectedNotChanged: JSON.stringify(row),
                      selected: row,
                    });
                  }, 100);
                }}
              >
                {row.Name}

                {row.IsChanged && (
                  <Badge
                    variant={"warning"}
                    aria-label="تغییرات ذخیره نشده"
                    data-microtip-position="right"
                    role="tooltip"
                  >
                    +تغییرات
                  </Badge>
                )}
              </Card.Link>
              <br />
              <InputSwitch
                onChange={(e) => {
                  row.IsEnabled = e.value;

                  this.setIsEnabled(row.IsEnabled, row);

                  this.setState({ tm: Math.random() });
                }}
                checked={row.IsEnabled}
              ></InputSwitch>
            </ListGroup.Item>
          );
        })}
      </ListGroup>
    );
  }
}

export default CompaignIndex;
