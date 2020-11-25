import React, {Component} from 'react';
import CompaignChildCompBase from "./CompaignChildCompBase";
import {SelectButton} from "primereact/selectbutton";
import {TabPanel, TabView} from "primereact/tabview";
import SelectCustomers from "./CompaginHelps/SelectCustomers";
import CompaignFilter from "../../Filter/CompaignFilter";
import SelectSegments from "../../../Routing/ChildComps/SelectSegments";
import {_GetSelectedCompaign, _SaveSelectedCompaign} from "../CompaignSave";
import {CurrentUserInfo} from "../../../Help/Socket";

class CompaignRecipients extends CompaignChildCompBase {

    state = {
        selected: {}
    }

    constructor(props) {
        super(props);
        this.state = {
            activeIndex: 0
        }
        this.justifyOptions = [
            {name: 'ارسال به همه کاربران', value: 'left'},
            {name: 'ارسال به کاربران انتخابی', value: 'Right'},
            {name: 'اعمال فیلتر پیشرفته', value: 'Center'},
            {name: 'ارسال به برچسب های خاص', value: 'Center'},
        ];

    }

    componentDidMount() {


        CurrentUserInfo.CompaignRecipients = this;
        this.setState({
            activeIndex: _GetSelectedCompaign().CompaignRecipientsTypeIndex
        })
    }

    componentWillUnmount() {
        _GetSelectedCompaign()['CompaignRecipientsTypeIndex'] = this.state.activeIndex;
        _GetSelectedCompaign()['segments'] = this.state.selected.segments;
        //_GetSelectedCompaign().selectedCustomers


    }


    render() {
        return (
            <div>


                <TabView activeIndex={this.state.activeIndex}
                         onTabChange={(e) => {

                             this.setState({activeIndex: e.index})
                             _GetSelectedCompaign().CompaignRecipientsTypeIndex = e.index;
                         }}>
                    <TabPanel header="ارسال به همه کاربران">
                    </TabPanel>
                    <TabPanel header="ارسال به کاربران انتخابی">

                        <SelectCustomers/>
                    </TabPanel>
                    <TabPanel header="اعمال فیلتر پیشرفته">
                        <CompaignFilter/>
                    </TabPanel>
                    <TabPanel header="ارسال به برچسب های خاص">

                        <SelectSegments/>
                    </TabPanel>
                </TabView>
            </div>
        );
    }
}

export default CompaignRecipients;