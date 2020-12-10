import React, {Component} from 'react';
import PageVisitStat from "./PageVisitStat/PageVisitStat";
import PageOnlineCount from "./PageOnlineCount/PageOnlineCount";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SiteVisit from "./SiteVisit/SiteVisit";
import StatDateFilter from "./StatDateFilter";
import {changeUserTypes} from "../Help/ChangeUserTypes";
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
import {MyCaller} from "../Help/Socket";
import { TabView } from 'primereact/tabview';
import { TabPanel } from 'primereact/tabview';
import SpecialStatsLayout from './SpecialStats/SpecialStatsLayout';

class StatLayout extends Component {
    componentDidMount() {
        MyCaller.Send('GetVisitedPagesForCurrentSite',{
            withStat:true
        })

    }

    render() {
        return (
            <div>

<TabView>
    <TabPanel header="آمار های عمومی">
    <StatDateFilter/>
                <Row>
                    <Col>
                        <PageVisitStat/>

                    </Col>
                    <Col>
                        <PageOnlineCount></PageOnlineCount>

                    </Col>
                </Row>
                <Row>
                    <Col>
                        <StatInMap/>

                    </Col>
                    <Col>
                        <StatPerState/>

                    </Col>
                </Row>
                <Row>
                    <Col>
                        <StatPerCity/>

                    </Col>
                    <Col>
                        <StatForeignCountries/>

                    </Col>

                </Row>


{/*
                ------------------------------------ 1.	آمار صفحه ای که بیشترین خروج از سایت را داشته ----------------------------------
*/}
                <Col>
                    <MostExitUrlInSite/>
                </Col>


{/*
                ------------------------------------ 2.	آمار بازدید در طی هفته ----------------------------------
*/}
                <Col>
                    <SiteViewsInWeek/>
                </Col>


{/*
                ------------------------------------ 2.	آمار بازدید در طی ساعات روز ----------------------------------
*/}
                <Col>
                    <SiteViewsInHoursOfToday/>
                </Col>


{/*
                ------------------------------------ 4.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------
*/}
                <Col>
                    <SiteViewsInMonths/>
                </Col>



{/*
                ------------------------------------ 5.	آمار بازدید در ماه های امسال(با مقایسه) ----------------------------------
*/}
                <Col>
                    <SiteViewsInLast5Year/>
                </Col>



{/*
                ------------------------------------ 6.	آمار ترتیب صفحه ها بر اساس بیشترین زمان آنلاین ----------------------------------
*/}
                <Col>
                    <SiteViewsMostOnlineTime/>
                </Col>





                <Row>
                    <Col>
                        <SiteVisit/>

                    </Col>

                </Row>
    </TabPanel>
    <TabPanel header="آمار های خاص">
       

       <SpecialStatsLayout/>
    </TabPanel>
</TabView>

                
                
            </div>
        );
    }
}

export default StatLayout;