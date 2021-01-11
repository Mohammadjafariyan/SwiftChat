import React, {Component} from 'react';
import {MyCaller} from "../../Help/Socket";
import {_showError, _showMsg} from "../../Pages/LayoutPage";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import Badge from "react-bootstrap/cjs/Badge";
import Button from "react-bootstrap/cjs/Button";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class BotLogTable extends Component {

    constructor(props) {
        super(props);
        CurrentUserInfo.BotLogTable = this;

        this.state = {
            loading: false,
            first: 0,
            total: 0,
            EntityList: null
        };

        this.statusBodyTemplate = this.statusBodyTemplate.bind(this);

    }

    componentDidMount() {

        this.refreshData();
    }

    refreshData() {

        this.setState({loading: true});


        let obj = {};
        if (CurrentUserInfo.BotLogForm) {

            obj["order"] = CurrentUserInfo.BotLogForm.state.order ? CurrentUserInfo.BotLogForm.state.order .name== 'اولین' ? true:false :false;
            obj["botId"] = CurrentUserInfo.BotLogForm.state.selectedbot ? CurrentUserInfo.BotLogForm.state.selectedbot.Id : null;
        }


        obj["page"] = this.state.page;


        MyCaller.Send('GetBotLogList',obj);

    }

    getBotLogListCallback(res) {
        this.setState({loading: false});

        if (!res || !res.Content) {
            _showError('getBotLogListCallback res is null')

          
        }
        this.setState({
            EntityList: res.Content.EntityList,
            page: res.Content.page,
            total: res.Content.total
        });

    }

    onPage(event) {

        debugger;
        const {first, rows} = event;

        this.setState({
            first,
            loading: true
        });

        setTimeout(() => {
            this.refreshData();
        }, 100)
    }

    statusBodyTemplate(rowData) {
        if (rowData.IsDone) {

            return <Badge variant="success">انجام</Badge>

        } else {
            return <Badge variant="danger">عدم انجام</Badge>
        }

    }

    select(rowData) {

        return <Button variant="primary" onClick={() => {
                
            
            
            _showMsg("در حال خواندن اطلاعات ثبت شده برای ربات");
            MyCaller.Send('GetBotLogSingle',{id:rowData.Id});

        }}>انتخاب</Button>


    }

    render() {
        return (
            <>

                {/*  Name = q.Name,
                    IsDone = q.IsDone,
                    LogDateTime = q.LogDateTime,
                    Id = q.Id*/}
                <DataTable value={this.state.EntityList} paginator rows={10} totalRecords={this.state.total}
                           lazy first={this.state.first} onPage={this.onPage} loading={this.state.loading}>
                    <Column field="Name" header="عنوان"></Column>
                    <Column field="IsDone" header="وضعیت" body={this.statusBodyTemplate}></Column>
                    <Column field="LogDateTime" header="زمان"></Column>
                    <Column field="IsDone" header="انتخاب"
                            body={this.select}></Column>
                </DataTable>
            </>
        );
    }
}

export default BotLogTable;