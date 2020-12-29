import React, { Component } from "react";
import PageVisitStat from "./PageVisitStat/PageVisitStat";
import PageOnlineCount from "./PageOnlineCount/PageOnlineCount";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SiteVisit from "./SiteVisit/SiteVisit";
import StatDateFilter from "./StatDateFilter";
import { changeUserTypes } from "../Help/ChangeUserTypes";
import StatInMap from "./StatInMap/StatInMap";
import StatPerState from "./StatPerState/StatPerState";
import StatPerCity from "./StatPerCity/StatPerCity";
import StatForeignCountries from "./StatForeignCountries/StatForeignCountries";
import MostExitUrlInSite from "./MostExitUrlInSite/MostExitUrlInSite";
import SiteViewsMostOnlineTime from "./SiteViewsMostOnlineTime/SiteViewsMostOnlineTime";
import SiteViewsInLast5Year from "./SiteViewsInLast5Year/SiteViewsInLast5Year";
import SiteViewsInMonths from "./SiteViewsInMonths/SiteViewsInMonths";
import SiteViewsInHoursOfToday from "./SiteViewsInHoursOfToday/SiteViewsInHoursOfToday";
import SiteViewsInWeek from "./SiteViewsInWeek/SiteViewsInWeek";
import { MyCaller , CurrentUserInfo} from "../Help/Socket";
import { TabView } from "primereact/tabview";
import { TabPanel } from "primereact/tabview";
import SpecialStatsLayout from "./SpecialStats/SpecialStatsLayout";

class StatLayout extends Component {
    state={};
  componentDidMount() {

    CurrentUserInfo.StatLayout=this;
    MyCaller.Send("GetVisitedPagesForCurrentSite", {
      withStat: true,
    });
  }

  getVisitedPagesForCurrentSiteCallback(res) {


    this.setState({loading: false});

    this.setState({data: res})


}

  render() {
    return (
      <div>
        <StatDateFilter />


        <TabView activeIndex={this.state.activeIndex} onTabChange={(e) => this.setState({activeIndex: e.index})}>
    <TabPanel header="صفحه آمار یک">
    <Row>
          <Col>
            <PageVisitStat data={this.state.data} />
          </Col>
          <Col>
            <PageOnlineCount  data={this.state.data} ></PageOnlineCount>
          </Col>
        </Row>
        <Row>
          <Col>
            <StatInMap  data={this.state.data}  />
          </Col>
          <Col>
            <StatPerState   data={this.state.data} />
          </Col>
        </Row>
        <Row>
          <Col>
            <StatPerCity  data={this.state.data}  />
          </Col>
          <Col>
            <StatForeignCountries  data={this.state.data}  />
          </Col>
        </Row>

        {/*
                ------------------------------------ 1.	آمار صفحه ای که بیشترین خروج از سایت را داشته ----------------------------------
*/}
        <Col>
          <MostExitUrlInSite   data={this.state.data} />
        </Col>

        {/*
                ------------------------------------ 2.	آمار بازدید در طی هفته ----------------------------------
*/}
        <Col>
          <SiteViewsInWeek  data={this.state.data}  />
        </Col>

        {/*
                ------------------------------------ 2.	آمار بازدید در طی ساعات روز ----------------------------------
*/}
        <Col>
          <SiteViewsInHoursOfToday  data={this.state.data}  />
        </Col>

        {/*
                ------------------------------------ 4.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------
*/}
        <Col>
          <SiteViewsInMonths  data={this.state.data}  />
        </Col>

        {/*
                ------------------------------------ 5.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------
*/}
        <Col>
          <SiteViewsInLast5Year  data={this.state.data}  />
        </Col>

        {/*
                ------------------------------------ 6.	آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین ----------------------------------
*/}
        <Col>
          <SiteViewsMostOnlineTime  data={this.state.data}  />
        </Col>

       {/*  <Row>
          <Col>
            <SiteVisit  data={this.state.data}   />
          </Col>
        </Row> */}
    </TabPanel>
    

    <TabPanel header="صفحه آمار دوم">
    {this.state.data && <SpecialStatsLayout  data={this.state.data}  />}

</TabPanel>
</TabView>
      

        
      </div>
    );
  }
}

export default StatLayout;
