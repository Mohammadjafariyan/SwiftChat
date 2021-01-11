import React, {Component} from 'react';
import {Chart} from "primereact/chart";
import {Card} from "react-bootstrap";
import PageVisitStat, {GetSummary} from "../PageVisitStat/PageVisitStat";
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";
import StatUtility from "../StatUtility";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class StatPerCity extends PageVisitStat {
    state = {
        title: 'به تفکیک شهر',
        arrName:'cities'
        
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
        CurrentUserInfo.StatPerCity = this;
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

        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }


    fill(trackinfosViewModellist, Content) {
        /*
        *     mostVisitedPages=mostVisitedPages,
                        states=states,
                        cities=cities,
                        foreignCountries=foreignCountries
                        * 
                        *       Key = q.Key,
                        VisitCount = q.Count(),
                        VisitorsCount*/

        this.setState({loading: false});
        if (!Content.cities) {
            return;
        }

        let chartData = this.getHorizontalChartData(Content.cities)


        this.setState({chartData: chartData, arr: Content.cities});
    }


    render() {
        return (
            <>
                <StatUtility title={this.state.title} chartData={this.state.chartData} arr={this.state.arr}
                             loading={this.state.loading}/>
            </>
        );
    }
}

export default StatPerCity;