import React, {Component} from 'react';
import {MyCaller} from "../../../Help/Socket";
import Badge from "react-bootstrap/cjs/Badge";
import Button from "react-bootstrap/cjs/Button";
import {Container, Spinner} from "react-bootstrap";
import {MyCard, MyFieldset} from "../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {_TakeLazyTable} from "./CompaginHelps/CompaignUtility";
import {MyModal} from "../../../Components/Modal";
import CompaignLogReceiverTable from "../Logs/CompaignLogReceiverTable";
import {DataHolder} from "../../../Help/DataHolder";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class CompaignLogTable extends Component {
    state = {
        list: []
    }

    constructor(props) {
        super(props);

        CurrentUserInfo.CompaignLogTable = this;

        this.actionBody = this.actionBody.bind(this);
        this.onPage = this.onPage.bind(this);

    }


    actionBody(row) {

        return <>

            <Button onClick={() => {
                this.setState({
                    showModal: true,
                    selectedCompaignLogId: row.Id
                })
            }}>
                لیست دریافت کنندگان
            </Button>
        </>


    }

    componentDidMount() {


        this.setState({loading: true});
        MyCaller.Send('GetCompaignLogList',
            {
                dataType: this.props.dataType,
                rows: 10
            })
    }


    getCompaignLogListCallback(res) {

        _TakeLazyTable(res, this);
    }

    onPage(event) {
        this.setState({loading: true});

        //imitate delay of a backend call
        const {first, rows} = event;

        MyCaller.Send('GetCompaignLogList',
            {
                dataType: this.props.dataType,
                first, rows
            })

    }

    render() {
        return (
            <div>

                <MyModal show={this.state.showModal} handleClose={() => {
                    this.setState({showModal: false})
                }} title={"نمایش لیست کاربران دریافت کننده"} /*onSave={() => {
                this.setState({tmp: Math.random()})
            }}*/>


                    <CompaignLogReceiverTable CompaignId={this.state.selectedCompaignId}
                                              CompaignLogId={this.state.selectedCompaignLogId}>
                        
                    </CompaignLogReceiverTable>
                </MyModal>

                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>}

                <MyFieldset title="انتخاب ربات تعریف شده">
                    <Row>
                        <Col>
                            <MyCard header={'انتخاب یک ربات'} title={'وقتی ربات اجرا شد ، کمپین اجرا شود'}>


                                <DataTable value={this.state.list} paginator
                                           emptyMessage={'هیچ اطلاعاتی یافت نشد'}
                                           paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                                           currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
                                           rows={10} rowsPerPageOptions={[10, 20, 50]}

                                           totalRecords={this.state.totalRecords}
                                           lazy first={this.state.first} onPage={this.onPage}
                                           loading={this.state.loading}
                                >
                                    <Column field="CompaignName" header="عنوان"></Column>
                                    <Column field="ExecutionDateTimeStr" header="تاریخ اجرا"></Column>
                                    <Column field="StoppedLog" header="لاگ خطای توقف"></Column>
                                    <Column field="ReceiverCount" header="تعداد دریافت کننده"></Column>
                                    <Column field="DeliverCount" header="تعداد تحویل گیرنده"></Column>
                                    <Column field="ProgressPercent" header="عملیات" body={this.actionBody}></Column>
                                </DataTable>


                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>

            </div>
        );
    }
}

export default CompaignLogTable;



