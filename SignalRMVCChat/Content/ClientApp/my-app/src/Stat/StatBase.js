import React, {Component} from 'react';
import PageVisitStat from "./PageVisitStat/PageVisitStat";
import {CurrentUserInfo} from "../Help/Socket";
import StatUtility from "./StatUtility";

class StatBase extends PageVisitStat {
    state = {
        title: 'به تفکیک استان'
    }

    constructor(props) {
        super(props);
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

    componentDidMount() {
        CurrentUserInfo.StatPerState = this;
        
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
        if (!Content.states) {
            return;
        }

        let res = this.getChartData(Content);


        this.setState({chartData: res.chartData, arr: res.list});
    }

    //chartData ,list
    getChartData(Content) {

        let arr = Content[this.state.arrName];
        
        if (this.state.arrName=='SiteViewsInMonths'){
            debugger;
        }

        let chartData = this.getHorizontalChartData(arr)

        return {chartData:chartData,list:arr};
    }


    render() {
        return (
            <>
                <StatUtility title={this.state.title} chartData={this.state.chartData} arr={this.state.arr}
                       type={this.state.type}      loading={this.state.loading}/>
            </>
        );
    }
}
export default StatBase;