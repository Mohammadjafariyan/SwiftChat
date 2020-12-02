import React, { Component } from "react";
import CompaignChildCompBase from "./CompaignChildCompBase";
import { SelectButton } from "primereact/selectbutton";
import { TabPanel, TabView } from "primereact/tabview";
import SelectCustomers from "./CompaginHelps/SelectCustomers";
import SelectSegments from "../../../Routing/ChildComps/SelectSegments";
import { _GetSelectedCompaign, _SaveSelectedCompaign } from "../CompaignSave";
import { CurrentUserInfo } from "../../../Help/Socket";
import CompaignFilter from "./CompaginHelps/CompaignFilter";
import { Card } from 'primereact/card';
import  Button  from 'react-bootstrap/cjs/Button';

class CompaignRecipients extends CompaignChildCompBase {
  state = {
      activeIndex: 0,
    selected: {},
  };

  constructor(props) {
    super(props);

    this.justifyOptions = [
      { name: "ارسال به همه کاربران", value: "left" },
      { name: "ارسال به کاربران انتخابی", value: "Right" },
      { name: "اعمال فیلتر پیشرفته", value: "Center" },
      { name: "ارسال به برچسب های خاص", value: "Center" },
    ];
  }

  componentDidMount() {
    CurrentUserInfo.CompaignRecipients = this;
    this.setState({
      activeIndex: _GetSelectedCompaign().CompaignRecipientsTypeIndex,
    });
  }

  componentWillUnmount() {
    _GetSelectedCompaign()[
      "CompaignRecipientsTypeIndex"
    ] = this.state.activeIndex;
    _GetSelectedCompaign()["segments"] = this.state.selected
      ? this.state.selected.segments
      : null;
    //_GetSelectedCompaign().selectedCustomers
  }

  render() {
    return (
      <div>
        <TabView
          activeIndex={this.state.activeIndex}
          onTabChange={(e) => {
            this.setState({ activeIndex: e.index });
            _GetSelectedCompaign().CompaignRecipientsTypeIndex = e.index;
          }}
        >
          <TabPanel header="ارسال به همه کاربران"></TabPanel>
          <TabPanel header="ارسال به کاربران انتخابی">
            <SelectCustomers />
          </TabPanel>
          <TabPanel header="اعمال فیلتر پیشرفته">
            <CompaignFilter fieldName={"Conditions"} parent={this} />
          </TabPanel>
          <TabPanel header="ارسال به برچسب های خاص">
          
           <SelectSegments 
            parent={this}/>

          </TabPanel>
        </TabView>


        <Card>
                    <Button onClick={() => {
                        this.goNext();
                    }}>

                        مرحله بعدی
                    </Button>
                </Card>
      </div>
    );
  }
}

export default CompaignRecipients;
