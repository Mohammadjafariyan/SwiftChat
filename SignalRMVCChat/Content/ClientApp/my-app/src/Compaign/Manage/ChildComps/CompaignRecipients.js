import React, {Component} from 'react';
import CompaignChildCompBase from "./CompaignChildCompBase";
import {SelectButton} from "primereact/selectbutton";
import {TabPanel, TabView} from "primereact/tabview";
import SelectCustomers from "./CompaginHelps/SelectCustomers";
import CompaignFilter from "../../Filter/CompaignFilter";
import SelectSegments from "../../../Routing/ChildComps/SelectSegments";

class CompaignRecipients extends CompaignChildCompBase {

    constructor(props) {
        super(props);
        this.state={
            activeIndex:0
        }
        this.justifyOptions = [
            {name: 'ارسال به همه کاربران', value: 'left'},
            {name: 'ارسال به کاربران انتخابی', value: 'Right'},
            {name: 'اعمال فیلتر پیشرفته', value: 'Center'},
            {name: 'ارسال به برچسب های خاص', value: 'Center'},
        ];

    }


    render() {
        return (
            <div>



                <TabView activeIndex={this.state.activeIndex} onTabChange={(e) => this.setState({ activeIndex: e.index })}>
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