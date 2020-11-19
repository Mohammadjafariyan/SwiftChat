import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SiteVisit from "./SiteVisit/SiteVisit";
import 'react-datepicker-persian/dist/react-datepicker-persian.min.css';
import moment from 'moment-jalaali';
import {Calendar} from "primereact/calendar";

class StatDateFilter extends Component {
    state={
      

    }
    render() {
        return (
            <>
                <h4>محدود کردن تاریخ</h4>
                <Row>
                    <Col>
                        <Calendar id="icon" value={this.state.range}
                                  onChange={(e) => this.setState({ range: e.value })} selectionMode="range" readOnlyInput
                                  showIcon  />

                        <label>محدوده تاریخ</label>

                    </Col>
                    
                    <Col>   
                        
                        <Row>
                            <Col>

                                <Calendar id="time12" value={this.state.from} onChange={(e) => this.setState({ from: e.value })}
                                          timeOnly />
                                <label>زمان تا</label>
                            </Col>
                            <Col><Calendar id="time12" value={this.state.to} onChange={(e) => this.setState({ to: e.value })}
                                           timeOnly/>
                                <label>زمان از</label>
</Col>
                        </Row>

                    </Col>
                   
                  

                </Row>
                <hr/>
            </>
        );
    }
}

export default StatDateFilter;