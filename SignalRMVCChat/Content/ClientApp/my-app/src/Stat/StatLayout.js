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

class StatLayout extends Component {
    componentDidMount() {
        changeUserTypes('GetVisitedPagesForCurrentSite');

    }

    render() {
        return (
            <div>

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


                <Row>
                    <Col>
                        <SiteVisit/>

                    </Col>

                </Row>
                
            </div>
        );
    }
}

export default StatLayout;