import React, { Component } from "react";
import { CurrentUserInfo, MyCaller } from "../../../Help/Socket";
import { MyCard, MyFieldset } from "../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import Badge from "react-bootstrap/cjs/Badge";
import Button from "react-bootstrap/cjs/Button";
import { Spinner } from "react-bootstrap";
import { MyModal } from "../../../Components/Modal";
import CompaignLogReceiverTable from "../Logs/CompaignLogReceiverTable";
import { _showMsg } from "../../../Pages/LayoutPage";

class CompaignTable extends Component {
  state = {
    list: [],
  };

  constructor(props) {
    super(props);

    CurrentUserInfo.CompaignTable = this;

    this.statusBody = this.statusBody.bind(this);
    this.deliverStatusBody = this.deliverStatusBody.bind(this);
    this.actionBody = this.actionBody.bind(this);
  }

  statusBody(row) {
    switch (row.Status) {
      case 1:
        return (
          <>
            <span className={"text-success"}>آماده فعال سازی</span>
          </>
        );
        break;
      case 2:
        return (
          <>
            <span className={"text-success"}>آماده ارسال</span>
          </>
        );

      case 3:
        return (
          <>
            <span className={"text-black"}>تنظیم نشده</span>
          </>
        );
        break;
      case 4:
        return (
          <>
            <span className={"text-warning"}>در حال ارسال</span>
          </>
        );
        break;

      case 5:
        return (
          <>
            <span className={"text-danger"}>متوقف شده</span>
            <br />
            <small className={"text-danger"}>{row.StoppedLog}</small>
          </>
        );
      case 6:
        return (
          <>
            <span className={"text-info"}>ارسال شده است</span>
          </>
        );
        break;
    }

    return <></>;
  }

  deliverStatusBody(row) {
    return (
      <>
        <Badge variant={"success"}>(دریافت کننده) {row.ReceiverCount}</Badge>
        <Badge variant={"success"}>(تحویل داده شده) {row.DeliverCount}</Badge>
      </>
    );
  }

  actionBody(row) {
    return (
      <>
        <Button
          onClick={() => {
            this.setState({
              showModal: true,
              selectedCompaignLogId: row.Id,
            });
          }}
        >
          <i className="fa fa-list"></i>
          دریافت کنندگان
        </Button>
        <Button
          variant={"info"}
          onClick={() => {
            CurrentUserInfo.CompaignLayout.setState({
              CompaignChildPage: null,
              dataType: null,
              title: null,
            }); //<CompaignTable currentStep={4}/>

            CurrentUserInfo.CompaignIndex.openAfterLoad(
              row.CompaignId ? row.CompaignId : row.Id
            );
          }}
        >
          <i className="fa fa-cog"></i>
        </Button>

        {!row.IsAutomatic && (
          <Button
            variant={"success"}
            onClick={() => {
              _showMsg("ارسال درخواست اجرای کمپین");
              this.compaignManualExecute(
                row.CompaignId ? row.CompaignId : row.Id
              );
            }}
          >
            اجرا
            <i className="fa fa-send"></i>
          </Button>
        )}
      </>
    );
  }

  compaignManualExecuteCallback() {
    _showMsg("با موفقیت اجرا شد");
  }

  compaignManualExecute(compaignId) {
    MyCaller.Send("CompaignManualExecute", { compaignId: compaignId });
  }
  componentDidMount() {
    this.setState({ loading: true });
    MyCaller.Send("GetCompaignList", { dataType: this.props.dataType });
  }

  componentWillReceiveProps(nextProps: Readonly<P>, nextContext: any) {
    this.componentDidMount();
  }
  getCompaignListCallback(res) {
    this.setState({ loading: false });
    if (!res || !res.Content) {
      return;
    }

    this.setState({
      list: res.Content,
    });
  }

  render() {
    return (
      <div>
        <h4>{this.props.title}</h4>

        <MyModal
          show={this.state.showModal}
          handleClose={() => {
            this.setState({ showModal: false });
          }}
          title={
            "نمایش لیست کاربران دریافت کننده"
          } /*onSave={() => {
                this.setState({tmp: Math.random()})
            }}*/
        >
          <CompaignLogReceiverTable
            title={"لیست دریافت کنندگان"}
            header={"کاربرانی که ایمیل یا پیغام برای آن ها ارسال شده است"}
            CompaignId={this.state.selectedCompaignId}
            CompaignLogId={this.state.selectedCompaignLogId}
          ></CompaignLogReceiverTable>
        </MyModal>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <MyFieldset title={" لیست کمپین  " + this.props.title}>
          <Row>
            <Col>
              <MyCard header={"جدول کمپین های تعریف شده"}>
                <DataTable
                  value={this.state.list}
                  paginator
                  emptyMessage={"هیچ اطلاعاتی یافت نشد"}
                  paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                  currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
                  rows={10}
                  rowsPerPageOptions={[10, 20, 50]}
                >
                  <Column field="Name" header="عنوان"></Column>
                  <Column
                    field="Status"
                    header="وضعیت"
                    body={this.statusBody}
                  ></Column>
                  <Column field="ExecutionCount" header="تعداد اجرا"></Column>
                  <Column
                    field="LastChangeDateTimeStr"
                    header="تاریخ آخرین تغییر"
                  ></Column>
                  <Column field="ProgressPercent" header="درصد پیشرفت"></Column>
                  <Column field="StoppedLog" header="لاگ خطای توقف"></Column>
                  <Column
                    field="ProgressPercent"
                    header="عملیات"
                    body={this.actionBody}
                  ></Column>
                </DataTable>
              </MyCard>
            </Col>
          </Row>
        </MyFieldset>
      </div>
    );
  }
}

export default CompaignTable;
