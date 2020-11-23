import React, {Component} from 'react';
import {Chart} from "primereact/chart";
import {Card} from "react-bootstrap";
import PageVisitStat, {GetSummary} from "../PageVisitStat/PageVisitStat";
import {CurrentUserInfo} from "../../Help/Socket";
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";
import StatUtility from "../StatUtility";

class StatForeignCountries extends PageVisitStat {
    state = {
        title: 'بازدید کنندگان خارجی',
        arrName:'StatForeignCountries'
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
        CurrentUserInfo.StatForeignCountries = this;
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
        if (!Content.foreignCountries) {
            return;
        }

        let chartData = this.getHorizontalChartData(Content.foreignCountries)


        this.setState({chartData: chartData, arr: Content.foreignCountries});
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

export default StatForeignCountries;