import React, {Component} from 'react';
import {Card, Spinner} from "react-bootstrap";
import TagSingleCustomer from "../../Components/TagSingleCustomer";
import WhileWriting from "../../Components/WhileWriting";
import {Chart} from 'primereact/chart';
import {changeUserTypes} from "../../Help/ChangeUserTypes";
import {CurrentUserInfo} from "../../Help/Socket";
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";

class PageVisitStat extends Component {
    state = {
        loading: true,
        title: ' آمار بازدید سایت',
        type: 'pie'
    }

    constructor(props) {
        super(props);


        this.lightOptions = {
            legend: {
                labels: {
                    fontColor: '#495057'
                }
            }
        };


    }

    //استفاده شده در کلاس های فرزند
    getHorizontalChartData(dataArr) {
        let labels = [];
        let arr = dataArr;
        let datas = [];
        let datas2 = [];
        let bgColors = [];
        for (let i = 0; i < dataArr.length; i++) {
            let Key = dataArr[i].Key;
            let VisitCount = dataArr[i].VisitCount;
            let VisitorsCount = dataArr[i].VisitorsCount;
            let FaName = dataArr[i].FaName;


            labels.push(FaName ? FaName : Key);
            datas.push(VisitCount);
            datas2.push(VisitorsCount);


        }

        let chartData = {
            labels: this.getLabels(dataArr, labels),
            datasets: [
                {
                    label:this.state.label1 ? this.state.label1 : ' بازدید',
                    data: datas,
                    backgroundColor: _GetRandomColor(),
                    fill: false,
                    borderColor: _GetRandomColor()
                },
                {
                    label: this.state.label2 ? this.state.label2 : ' بازدید کننده',
                    data: datas2,
                    backgroundColor: _GetRandomColor(),
                    fill: false,
                    borderColor: _GetRandomColor()
                }
            ]
        };

        if (arr.length == 0) {
            chartData = {
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
        return chartData;
    }

    getLabels(dataArr, labels) {
        return labels;
    }

    componentDidMount() {
        CurrentUserInfo.PageVisitStat = this;


        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

    fill(trackinfosViewModellist) {
        let labels = [];
        let arr = [];
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

        if (arr.length == 0) {
            chartData = {
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

    getVisitedPagesForCurrentSiteCallback(res) {


        this.setState({loading: false});

        let arr = [];

        let trackinfosViewModellist = res.Content.mostVisitedPages;
        if (!trackinfosViewModellist) {
            CurrentUserInfo.LayoutPage.showError(
                ("دیتا نال است")
            );
            alert("دیتا نال است");
            return;
        }


        this.fill(trackinfosViewModellist, res.Content)


    }


    render() {
        return (
            <Card bg="Light">
                <Card.Header>

                    {this.state.title}
                </Card.Header>
                <Card.Body>


                    {this.state.loading &&
                    <Spinner animation="border" role="status" variant={'info'}>
                        <span className="sr-only">در حال خواندن اطلاعات...</span>
                    </Spinner>}


                    {this.state.chartData &&
                    <Chart type={this.state.type} data={this.state.chartData} options={this.lightOptions}/>
                    }


                    {this.state.arr && this.state.arr.length == 0 &&

                    <h4>هنوز اطلاعاتی برای چارت ثبت نشده است</h4>}
                </Card.Body>
            </Card>

        )
    }
}

export default PageVisitStat;


export function GetSummary(text) {

    if (!text)
        return text;
    if (text.length > 100) {
        text = text.toString().substr(0, 100) + '...';
    }
    return text;
}