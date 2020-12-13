import React, {Component} from 'react';
import PageVisitStat, {GetSummary} from "../PageVisitStat/PageVisitStat";
import {_GetRandomColor} from "../../Components/Utilities/ColorsList";
import MyMap from "../../Components/Map/MyMap";
import {Card, Spinner} from "react-bootstrap";
import {Chart} from "primereact/chart";
import {CurrentUserInfo} from "../../Help/Socket";
import {changeUserTypes} from "../../Help/ChangeUserTypes";

class StatInMap extends PageVisitStat {


    componentDidMount() {
        CurrentUserInfo.StatInMap = this;


        this.setState({title: 'نمایش در نقشه'})

        
        if(this.props.data){
            this.getVisitedPagesForCurrentSiteCallback(this.props.data)
        }
    }

    fill(trackinfosViewModellist) {
        this.setState({loading: false});
        let arr = [];

        for (let i = 0; i < trackinfosViewModellist.length; i++) {
            let pageTitle = trackinfosViewModellist[i].PageTitle;
            let baseUrl = trackinfosViewModellist[i].BaseUrl;
            let VisitedCount = trackinfosViewModellist[i].VisitedCount;
            let Customers = trackinfosViewModellist[i].Customers;


            for (let j = 0; j < Customers.length; j++) {
                arr.push(Customers[j]);
            }
        }


        this.setState({users: arr});
    }


    render() {
        return (
            <div>

                <Card bg="Light">
                    <Card.Header>

                        {this.state.title}
                    </Card.Header>
                    <Card.Body>


                        {this.state.loading &&
                        <Spinner animation="border" role="status" variant={'info'}>
                            <span className="sr-only">در حال خواندن اطلاعات...</span>
                        </Spinner>}

                        {this.state.users && this.state.users.length >= 0 &&
                        <MyMap users={this.state.users}/>}
                    </Card.Body>
                </Card>


            </div>
        );
    }
}

export default StatInMap;