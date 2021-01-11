import React, { Component } from "react";
import  { Card, Row, Col ,Container} from "react-bootstrap";
import { DataHolder } from "./../Help/DataHolder";
import {changeUserTypes} from "../Help/ChangeUserTypes";
import {CurrentUserInfo} from "../CurrentUserInfo";

export default class SepratePerPage extends Component {
  constructor(args) {
    super(args);

    this.state = { list: [] };

    CurrentUserInfo.SepratePerPage = this;
  }

  componentDidMount() {
    changeUserTypes('GetVisitedPagesForCurrentSite');

    /* let arr = [];
       let chat = {
         Message: "با سلام خوش آمدید",
       };
       for (let i = 0; i < 20; i++) {
         arr.push({ Name: "صفحه " + i, Url: "http://sso.com.em.co" });
       }
       this.setState({ list: arr });*/
  }

  getVisitedPagesForCurrentSiteCallback(res) {
    let arr = [];

    let trackinfosViewModellist = res.Content.mostVisitedPages;
    if (!trackinfosViewModellist) {
        CurrentUserInfo.LayoutPage.showError(
        ("دیتا نال است")
      );
      alert("دیتا نال است");
      return;
    }

   

    window["trackinfosViewModellist"] = trackinfosViewModellist;

    for (let i = 0; i < trackinfosViewModellist.length; i++) {
      let pageTitle = trackinfosViewModellist[i].PageTitle;
      let baseUrl = trackinfosViewModellist[i].BaseUrl;
      let VisitedCount = trackinfosViewModellist[i].VisitedCount;
      let Customers = trackinfosViewModellist[i].Customers;

      arr.push({
        Name: pageTitle,
        Url: baseUrl,
        VisitedCount,
        CustomersCount: Customers && Customers.length ? Customers.length : 0,
        Customers: Customers,
      });
    }

    this.setState({ list: arr });
  }

  select(link) {
    DataHolder.SelectedPage = link.Url;
    DataHolder.currentPage = "CustomerList";
    DataHolder.filterType = "SeparatePerPageCustomerListPage";
    CurrentUserInfo.LayoutPage.setState({ temp: Math.random() });
    this.setState({ temp: Math.random() });
  }

  render() {
    return (
      <div>
        {this.state.list &&
          this.state.list.map((link, i, arr) => {
            return (
                <Container key={i}>
              <Card>
                <Card.Body>
                  <Row>
                    <Col>
                      <span>تعداد بازدید : {link.VisitedCount}</span>
                    </Col>
                    <Col>
                      <span>تعداد آنلاین : {link.CustomersCount}</span>
                    </Col>

                    <Col>
                      <Card.Link
                        href="#"
                        onClick={() => {
                          this.select(link);
                        }}
                      >
                        مشاهده کاربران
                      </Card.Link>
                    </Col>
                    <Col>
                      <Card.Link
                        href="#"
                        onClick={() => {
                          this.select(link);
                        }}
                      >
                        {link.Name} - {link.Url}
                      </Card.Link>
                    </Col>
                  </Row>
                </Card.Body>
              </Card>
                </Container>
            );
          })}
      </div>
    );
  }
}
