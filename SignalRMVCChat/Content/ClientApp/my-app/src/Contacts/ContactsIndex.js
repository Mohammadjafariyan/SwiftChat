﻿import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {TagsShow} from "../Components/CustomerTags";
import ButtonGroup from "react-bootstrap/cjs/ButtonGroup";
import Button from "react-bootstrap/cjs/Button";
import BlockUser from "../Components/BlockUser/BlockUser";
import {DataHolder} from "../Help/DataHolder";
import {CustomerProfileSideBar} from "../Components/Profile/CustomerProfileSideBar";
import {Card, Container} from "react-bootstrap";
import {Checkbox} from "primereact/checkbox";
import Badge from "react-bootstrap/Badge";

class ContactsIndex extends Component {
    state = {
        first: 0
    };

    constructor(props) {
        super(props);
        CurrentUserInfo.ContactsIndex = this;

        this.onPage = this.onPage.bind(this);


        this.Name = this.Name.bind(this);
        this.Email = this.Email.bind(this);
        this.Phone = this.Phone.bind(this);
        this.City = this.City.bind(this);
        this.State = this.State.bind(this);
        this.IsAuthen = this.IsAuthen.bind(this);
        this.Rate = this.Rate.bind(this);
        this.Tags = this.Tags.bind(this);
        this.Country = this.Country.bind(this);
        this.LastActive = this.LastActive.bind(this);
        this.Actions = this.Actions.bind(this);
        this.IsResovled = this.IsResovled.bind(this);

    }

    IsResovled(row) {
        if (row.Customer.IsResolved) {
            return <Badge variant={'success'}>حل شده</Badge>;

        } else {
            return <Badge variant={'danger'}>حل نشده</Badge>;
        }
    }

    Email(row) {
        return <Cell row={row} name={'Email'}/>;
    }

    Phone(row) {
        return <Cell row={row} name={'Phone'}/>;
    }

    City(row) {
        return <span>{row.LastTrackInfo.ShowCityName}</span>;
    }

    State(row) {
        return <span>{row.LastTrackInfo.ShowStateName}</span>;
    }

    IsAuthen(row) {
        if (row.Customer.UsersSeparationParams && row.Customer.UsersSeparationParams.length > 0) {
            return <span>عضو نیست</span>;

        } else {
            return <span>عضو است</span>;
        }
    }

    Rate(row) {
        let stars = [];
        for (let i = 1; i <= 6; i++) {
            stars.push(i);
        }
        return <>
            {stars.map((val, i, arr) => {

                if (i < row.Customer.RatingCount.length) {
                    return <i key={i} style={{color: '#f8d007'}} className="fa fa-star" aria-hidden="true"></i>

                } else {
                    return <i key={i} style={{color: 'black'}} className="fa fa-star" aria-hidden="true"></i>
                }
            })}
        </>
    }

    Tags(row) {
        return <TagsShow tags={row.Tags}/>;
    }

    Country(row) {
        return <span>
            
                <Badge pill variant="light">
                                <img
                                    src={`https://www.countryflags.io/${row.LastTrackInfo.country_code}/flat/64.png`}/>
                            </Badge>
            {row.LastTrackInfo.country_name}
        
        </span>;
    }

    LastActive(row) {
        return <span>{row.LastTrackInfo.DateTimeText}</span>;
    }

    Actions(row) {
        return <>

            <ButtonGroup>

                <BlockUser Customer={row.Customer}/>


                <Button
                    onClick={() => {
                        this.setState({
                            temp: Math.random(),
                            customerProfileSideBarVisible: true,
                            SelectedCustomer: row.Customer
                        });
                    }}
                >نمایش پروفایل</Button>

            </ButtonGroup>

            <Checkbox
                onChange={e => {
                    this.setState({mt: Math.random()});
                    row.checked = e.checked;
                }}
                checked={row.checked}></Checkbox>

        </>;
    }

    /*    Customer = customer,
                LastTrackInfo = customer.TrackInfos.OrderByDescending(o => o.Id).LastOrDefault(),
                Tags = customer.CustomerTags.Select(t => t.Tag.Name).ToArray()
           */
    getContactsCallback(res) {
        this.setState({loading: false})

        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }

        /*   let Customer = res.Content.data.customer;
           let LastTrackInfo = res.Content.data.customer;
           let Tags = res.Content.data.customer;*/
        let totalRecords = res.Content.totalRecords;


        this.setState({
            list: res.Content.data,
            totalRecords
        });

    }

    componentDidMount() {

        this.setState({loading: true})
        MyCaller.Send("GetContacts")

    }

    onPage(event) {
        this.setState({loading: true});

        const {first, rows} = event;

        MyCaller.Send("GetContacts", {page: first})
        this.setState({first: first});

        //imitate delay of a backend call

        /*         this.setState({
                     first,
                     customers: this.datasource.slice(first, first + rows),
                     loading: false
                 });*/
    }

    render() {
        return (
            <>


                <DataTable value={this.state.list}
                           paginator rows={10} totalRecords={this.state.totalRecords}
                           lazy first={this.state.first} onPage={this.onPage} loading={this.state.loading}
                           emptyMessage={'هنوز اطلاعاتی ثبت نشده است'}>

                    <Column header="نام" body={this.Name}></Column>
                    <Column header="ایمیل" body={this.Email}></Column>
                    <Column header="شماره" body={this.Phone}></Column>
                    <Column header="شهر" body={this.City}></Column>
                    <Column header="استان" body={this.State}></Column>
                    <Column header="عضویت" body={this.IsAuthen}></Column>
                    <Column header="رضایتمندی" body={this.Rate}></Column>
                    <Column header="برچسب ها" body={this.Tags}></Column>
                    <Column header="کشور" body={this.Country}></Column>
                    <Column header="آخرین فعالیت" body={this.LastActive}></Column>
                    <Column header="وضعیت چت" body={this.IsResovled}></Column>
                    <Column header="عملیات" body={this.Actions}></Column>
                    
                </DataTable>

                {this.state.customerProfileSideBarVisible &&
                <CustomerProfileSideBar parent={this} Customer={this.state.SelectedCustomer}
                                        visible={this.state.customerProfileSideBarVisible}/>
                }

            </>
        );
    }
}

export default ContactsIndex;


const Cell = (props) => {
    return <span>{props.row.Customer[props.name]}</span>
}