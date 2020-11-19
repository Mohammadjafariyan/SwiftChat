import React, {Component} from 'react';
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";
import {CurrentUserInfo} from "../../Help/Socket";
import PageVisitStat, {GetSummary} from "../PageVisitStat/PageVisitStat";
import {changeUserTypes} from "../../Help/ChangeUserTypes";

class PageOnlineCount extends PageVisitStat {

    

    componentDidMount() {
        CurrentUserInfo.PageOnlineCount = this;
        
        this.setState({title:'آمار تعداد آنلاین های هر صفحه اکنون',
        type:'doughnut'})

       
    }


    fill(trackinfosViewModellist){
        let arr=[];
        let labels = [];
        let datas = [];
        let bgColors = [];
        for (let i = 0; i < trackinfosViewModellist.length; i++) {
            let pageTitle = trackinfosViewModellist[i].PageTitle;
            let baseUrl = trackinfosViewModellist[i].BaseUrl;
            let VisitedCount = trackinfosViewModellist[i].VisitedCount;
            let Customers = trackinfosViewModellist[i].Customers;



            labels.push(`${GetSummary(pageTitle)} - ${GetSummary(baseUrl)} `);
            datas.push(VisitedCount);

            let randomColor = _GetRandomColor();

            bgColors.push(randomColor);

            arr.push({
                Name: pageTitle,
                Url: baseUrl,
                VisitedCount,
                CustomersCount: Customers && Customers.length ? Customers.length : 0,
                Customers: Customers,
            });
        }

        let chartData = {
            labels: labels,
            datasets: [
                {
                    data: datas,
                    backgroundColor: bgColors,
                    hoverBackgroundColor: bgColors
                }
            ]
        };

        if (arr.length==0){
            chartData={
                labels: ['A', 'B', 'C'],
                datasets: [
                    {
                        data: [300, 50, 100],
                        backgroundColor: [
                            "#42A5F5",
                            "#66BB6A",
                            "#FFA726"
                        ],
                        hoverBackgroundColor: [
                            "#64B5F6",
                            "#81C784",
                            "#FFB74D"
                        ]
                    }
                ]
            };
        }

        this.setState({chartData: chartData, arr: arr});
    }
}

export default PageOnlineCount;