import React, {Component} from 'react';
import {Card, Spinner} from "react-bootstrap";
import {Chart} from "primereact/chart";

class StatUtility extends Component {
    state={};

    constructor(props) {
        super(props);
        
    }


    render() {
        return (
            <>
                <Card bg="Light">
                    <Card.Header>

                        {this.props.title}
                    </Card.Header>
                    <Card.Body>

                        {this.props.loading &&
                        <Spinner animation="border" role="status" variant={'info'}>
                            <span className="sr-only">در حال خواندن اطلاعات...</span>
                        </Spinner>}
                        
                        {this.props.chartData &&
                        <Chart type={ this.props.type ? this.props.type :"horizontalBar"} data={this.props.chartData} options={this.props.basicOptions}/>
                        }


                        {this.props.arr && this.props.arr.length == 0 &&

                        <h4>هنوز اطلاعاتی برای چارت ثبت نشده است</h4>}

                    </Card.Body>
                </Card>
            </>
        );   
    }
}

export default StatUtility;