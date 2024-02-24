import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SiteVisit from "./SiteVisit/SiteVisit";
//import 'react-datepicker-persian/dist/react-datepicker-persian.min.css';
import moment from 'moment-jalaali';
import {Calendar} from "primereact/calendar";
import Button from "react-bootstrap/cjs/Button";
import {MyCaller} from "../Help/Socket";
import {_showMsg} from "../Pages/LayoutPage";

class StatDateFilter extends Component {
    state={
      

    }

    applyFilter(){

        _showMsg('در حال اعمال فیلتر زمانی');
        MyCaller.Send('GetVisitedPagesForCurrentSite',{
            withStat:true,
            range:this.state.range,
            from:this.state.range,
            to:this.state.to,
        });
        
    }
    
    render() {
        return (
            <>
                <h4>محدود کردن تاریخ</h4>
                <Row>
                    <Col>
                        <Calendar id="icon" value={this.state.range}
                                  onChange={(e) => this.setState({ range: e.value })} selectionMode="range" readOnlyInput
                                  showIcon dateFormat="yy/mm/dd" />

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
                                           timeOnly />
                                <label>زمان از</label>
</Col>
                        </Row>

                    </Col>
                    <Col>
                        
                        <Button onClick={()=>{
                            
                            this.applyFilter();
                            
                        }}>
                            
                            اعمال
                        </Button>
                    </Col>
                   
                  

                </Row>
                <hr/>
            </>
        );
    }
}

export default StatDateFilter;