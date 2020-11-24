import React, {Component, useEffect} from 'react';
import CompaignChildCompBase from "./CompaignChildCompBase";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {MyCard, MyFieldset} from "../../../Routing/Manage/RoutingSave";
import {GetMonthDays, RowHtml, ShowListOfCards} from "./CompaginHelps/CompaignUtility";
import Card from "react-bootstrap/cjs/Card";
import {TabPanel, TabView} from "primereact/tabview";
import CompaignFilter from "./CompaginHelps/CompaignFilter";
import {Dropdown} from "primereact/dropdown";
import {MyCaller, CurrentUserInfo} from "../../../Help/Socket";
import {_showError} from "../../../Pages/LayoutPage";
import {SelectButton} from "primereact/selectbutton";
import {weekdays} from "../../../Bot/Design/NodeSetting/BotEventCondition";
import {Accordion, AccordionTab} from "primereact/accordion";
import {Calendar} from "primereact/calendar";
import {RadioButton} from "primereact/radiobutton";
import {_GetSelectedCompaign} from "../CompaignSave";
import Button from "react-bootstrap/cjs/Button";

class CompaignConditions extends CompaignChildCompBase {
    state = {
        botList: [],
        eventTriggerList: [],
        //Conditions:[],
        everyDateTime: '',
        everyWeekTime: '',
        selectedDayOfEveryMonth: 1
    };

    days = GetMonthDays();

    constructor(props) {
        super(props);
        CurrentUserInfo.CompaignConditions = this;
    }

    componentDidMount() {

        MyCaller.Send('EventTriggerGetAll')
        MyCaller.Send('BotList')

        this.setState({
            activeIndex: _GetSelectedCompaign().CompaignConditionsTypeIndex
        })
    }
    
    componentWillUnmount() {
        //_GetSelectedCompaign()['Conditions']
        _GetSelectedCompaign()['everyDateTime'] =this.state.everyDateTime;
        _GetSelectedCompaign()['everyWeekTime'] =this.state.everyWeekTime;
        _GetSelectedCompaign()['selectedDayOfEveryMonth']=this.state.selectedDayOfEveryMonth;
        _GetSelectedCompaign()['selectedEventTrigger']=this.state.selectedEventTrigger;
        _GetSelectedCompaign()['selectedBot']=this.state.selectedBot;


          }

    eventTriggerGetAllCallback(res) {
        if (!res || !res.Content) {
            _showError("دیتای بازگشتی نال است سرور");
            return;
        }

        this.setState({eventTriggerList: res.Content})
    }

    botListCallback(res) {
        this.setState({loading: false})

        if (!res || !res.Content) {

            console.error(' مقدار بازگشتی از سرور نال است ');
            return;
        }

        this.setState({botList: res.Content});
    }

    render() {
        return (
            <>
                <TabView activeIndex={this.state.activeIndex}
                         onTabChange={(e) => {
                             this.setState({activeIndex: e.index});

                             _GetSelectedCompaign().CompaignConditionsTypeIndex = e.index;

                         }}>
                    <TabPanel header="ارسال مشروط">

                        {/*-----------------------------------------------------------*/}
                        {/* values is set to _GetSelectedCompaign()['Conditions']*/}

                        <CompaignFilter fieldName={'Conditions'} parent={this}/>

                        {/*END*/}
                        {/*-----------------------------------------------------------*/}
                    </TabPanel>
                    <TabPanel header="ارسال وقتی یک Event Trigger فعال شود ">
                        {this.SelectEventTrigger()}
                    </TabPanel>
                    <TabPanel header="ارسال وقتی یک ربات فعال شود">

                        {this.SelectBot()}

                    </TabPanel>
                    <TabPanel header="ارسال منظم زمانی">

                        {this.InTimeInterval()}

                    </TabPanel>
                   
                </TabView>

                <hr/>

                <Card>
                    <Button onClick={() => {
                        this.goNext();
                    }}>

                        مرحله بعدی
                    </Button>
                </Card>

            </>
        );
    }

    SelectBot = () => {
        return <MyFieldset title="انتخاب ربات تعریف شده">
            <Row>
                <Col>
                    <MyCard header={'انتخاب یک ربات'} title={'وقتی ربات اجرا شد ، کمپین اجرا شود'}>


                        <Dropdown value={this.state.selectedBot}
                                  options={this.state.botList}
                                  onChange={(e) => {

                                      this.setState({
                                          mg: Math.random(),
                                          selectedBot: e.value
                                      });
                                  }}
                                  optionLabel="name" filter showClear
                                  filterBy="name"
                                  placeholder='انتخاب ربات'
                        />
                        <label>انتخاب ربات</label>


                    </MyCard>
                </Col>
            </Row>
        </MyFieldset>
    }

    SelectEventTrigger = () => {


        return <MyFieldset title="های تعریف شده event Trigger ">
            <Row>
                <Col>
                    <MyCard header={'انتخاب یک رویداد'} title={'وقتی یک رویداد اتفاق افتاد ، کمپین اجرا شود'}>


                        <Dropdown value={this.state.selectedEventTrigger}
                                  options={this.state.eventTriggerList}
                                  onChange={(e) => {

                                      this.setState({
                                          mg: Math.random(),
                                          selectedEventTrigger: e.value
                                      });
                                  }}
                                  optionLabel="name" filter showClear
                                  filterBy="name"
                                  placeholder='انتخاب رویداد'
                        />
                        <label>انتخاب رویداد</label>


                    </MyCard>
                </Col>
            </Row>
        </MyFieldset>
    }


    InTimeInterval() {
        return <>
            <MyFieldset title="های تعریف شده event Trigger ">
                <Row>
                    <Col>
                        <MyCard header={'انتخاب یک رویداد'} title={'وقتی یک رویداد اتفاق افتاد ، کمپین اجرا شود'}>

                            <Accordion activeIndex={0}>
                                <AccordionTab header="هفتگی">
                                    <div className="card p-fluid">


                                        <label>روز های هفته را انتخاب کنید</label>
                                        <SelectButton value={this.state.weekdays} options={weekdays}
                                                      onChange={(e) => this.setState({weekdays: e.value})}
                                                      optionLabel="name" multiple/>

                                        <hr/>
                                        <label>ساعتی را انتخاب کنید</label>
                                        <Calendar timeOnly value={this.state.everyWeekTime}
                                                  onChange={(e) => this.setState({everyWeekTime: e.value})}
                                                  showTime/>
                                    </div>
                                </AccordionTab>
                                <AccordionTab header="هر روز و ساعت مشخص">


                                    <label>ساعتی را انتخاب کنید</label>
                                    <Calendar timeOnly value={this.state.everyDateTime}
                                              onChange={(e) => this.setState({everyDateTime: e.value})}
                                              showTime/>


                                </AccordionTab>
                                <AccordionTab header="ماهانه">

                                    <Dropdown value={this.state.selectedDayOfEveryMonth}
                                              options={this.days}
                                              onChange={(e) => {

                                                  this.setState({
                                                      mg: Math.random(),
                                                      selectedDayOfEveryMonth: e.value
                                                  });
                                              }}
                                                

                                              placeholder='انتخاب روز ماه'
                                    />
                                    <label>انتخاب روز ماه</label>
                                </AccordionTab>
                            </Accordion>


                        </MyCard>
                    </Col>
                </Row>
            </MyFieldset>
        </>
    }
}

export default CompaignConditions;