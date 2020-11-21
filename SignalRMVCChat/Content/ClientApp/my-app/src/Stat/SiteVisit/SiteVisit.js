import React, {Component} from 'react';
import {Chart} from "primereact/chart";
import {Card} from "react-bootstrap";
import {CurrentUserInfo} from "../../Help/Socket";

class SiteVisit extends Component {
    state={
        title:'بازدید سایت'
    }
    constructor(props) {
        super(props);

        this.basicData = {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            datasets: [
                {
                    label: 'My First dataset',
                    backgroundColor: '#42A5F5',
                    data: [65, 59, 80, 81, 56, 55, 40]
                },
                {
                    label: 'My Second dataset',
                    backgroundColor: '#FFA726',
                    data: [28, 48, 40, 19, 86, 27, 90]
                }
            ]
        };
    }
    
    componentDidMount() {
        CurrentUserInfo.SiteVisit=this;
        
        this.basicOptions = {
            legend: {
                labels: {
                    fontColor: '#495057'
                }
            },
            scales: {
                xAxes: [{
                    ticks: {
                        fontColor: '#495057'
                    }
                }],
                yAxes: [{
                    ticks: {
                        fontColor: '#495057'
                    }
                }]
            }
        };
    }

    render() {
        return (
            <>
                <Card bg="Light" >
                    <Card.Header>

                        {this.state.title}
                    </Card.Header>
                    <Card.Body>
                <Chart type="horizontalBar" data={this.basicData} options={this.basicOptions} />

                    </Card.Body>
                </Card>
            </>
        );
    }
}

export default SiteVisit;