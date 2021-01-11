import React, { Component } from "react";
import { _SelectCustomerForChat } from "../../../Compaign/Manage/ChildComps/CompaginHelps/CompaignUtility";
import { MyCaller } from "../../../Help/Socket";
import { CustomerProfileSideBar } from "./../../../Components/Profile/CustomerProfileSideBar";
import { MyLazyTable } from "./../../../Compaign/Manage/ChildComps/CompaginHelps/CompaignUtility";
import BlockUser from './../../../Components/BlockUser/BlockUser';
import { Button } from 'react-bootstrap';
import { Column } from 'primereact/column';
import { Row } from 'react-bootstrap';
import { Col } from 'react-bootstrap';
import { Card } from 'react-bootstrap';
import { MyCard, MyFieldset } from './../../../Routing/Manage/RoutingSave';
import { DataTable } from 'primereact/datatable';
import { Spinner } from 'react-bootstrap';
import {CurrentUserInfo} from "../../../CurrentUserInfo";

export default class Comments extends MyLazyTable {
  state = {
    getUrl: "HelpdeskFeedbackForArticles",
    list: null,
    CustomData: {},
    first:0
  };

  constructor(props) {
    super(props);
    CurrentUserInfo.Comments = this;

    this.ShowStatistificationBody = this.ShowStatistificationBody.bind(this);
    this.actionBody = this.actionBody.bind(this);
    this.showColumns = this.showColumns.bind(this);

    
  }  

  ShowStatistificationBody(row) {
    return (
      <>
        {row.IsHelpful && (
          <i className="display-4 fa fa-smile-o text-success" aria-hidden="true"></i>
        )}
        {!row.IsHelpful && (
          <i className="display-4 fa fa-frown-o text-danger" aria-hidden="true"></i>
        )}
      </>
    );
  }

  actionBody(row) {
    return (
      <>
        <BlockUser Customer={row.Customer} />

        <Button
          onClick={() => {
            this.setState({
              temp: Math.random(),
              customerProfileSideBarVisible: true,
              SelectedCustomer: row.Customer,
            });
          }}
        >
          نمایش پروفایل
        </Button>

        <Button
          onClick={() => {
            _SelectCustomerForChat(row.Customer);
          }}
        >
          شروع گفتگو
        </Button>
      </>
    );
  }


  

  helpdeskFeedbackForArticlesCallback(res) {
    this.setState({ loading: false });

    if (!res || !res.Content) {
      console.error(" مقدار بازگشتی از سرور نال است ");
      return;
    }

    this.setState({
      list: res.Content.List,
      helpfulCount: res.Content.helpfulCount,
      nothelpfulCount: res.Content.nothelpfulCount,
      totalRecords:res.Content.totalRecords,
      
    });
  }

  render() {
    return (
      <div>
        <Card bg="light" border="primary">
          <Card.Header>آمار امتیازات دریافت شده</Card.Header>
          <Card.Body>
            <Row>
              <Col>
                <h3>بازخورد مقالات</h3>
              </Col>
              <Col>
                <h4>
                <i
                    className="fa fa-smile-o text-success"
                    aria-hidden="true"
                  ></i>{" "}
                     مفید بود :{" "}
                  {this.state.helpfulCount}
         
                </h4>
              </Col>
              <Col>
                <h4>
                <i
                    className="fa fa-frown-o text-danger"
                    aria-hidden="true"
                  ></i>{" "}
                   مفید نبود :{" "}
                  {this.state.nothelpfulCount}
                 
                </h4>
              </Col>
            </Row>

            <Row>
              {this.state.customerProfileSideBarVisible && (
                <CustomerProfileSideBar
                  parent={this}
                  Customer={this.state.SelectedCustomer}
                  visible={this.state.customerProfileSideBarVisible}
                />
              )}


              {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <MyFieldset title={this.props.title ?this.props.title : "انتخاب ربات تعریف شده"}>
          <Row>
            <Col>
              <MyCard
                header={this.props.header ?this.props.header :"انتخاب یک ربات"}
                title={this.props.title ?this.props.title :  "وقتی ربات اجرا شد ، کمپین اجرا شود"}
              >

              {this.state.list && 
                <DataTable
                  value={this.state.list}
                  paginator
                  emptyMessage={"هیچ اطلاعاتی یافت نشد"}
                  paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                  currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
                  rows={10}
                  totalRecords={this.state.totalRecords}
                  lazy
                  first={this.state.first}
                  onPage={this.onPage}
                  loading={this.state.loading}
                >
                   <Column field="Text" header="متن"></Column>
        <Column field="CreationDateTimeStr" header="تاریخ و ساعت"></Column>
        <Column field="Article.Title" header="مقاله"></Column>
        <Column
          body={this.ShowStatistificationBody}
          header="رضایتمندی"
        ></Column>

        <Column
          field="ProgressPercent"
          header="عملیات"
          body={this.actionBody}
        ></Column>
                </DataTable>
              }
              </MyCard>
            </Col>
          </Row>
        </MyFieldset>
            </Row>
          </Card.Body>
        </Card>
      </div>
    );
  }
}
