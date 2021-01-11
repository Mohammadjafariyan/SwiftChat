import React, {Component} from 'react';
import {Dropdown} from "primereact/dropdown";
import {RadioButton} from "primereact/radiobutton";
import {Calendar} from "primereact/calendar";
import SelectAdmin from "./SelectAdmin";
import Button from "react-bootstrap/cjs/Button";
import {DataTable} from "primereact/datatable";
import {MyCaller} from "../../Help/Socket";
import {_showMsg} from "../../Pages/LayoutPage";
import {Column} from "primereact/column";
import {TabPanel, TabView} from "primereact/tabview";

import '../../styles/myStyle.css'
import {DataHolder} from "../../Help/DataHolder";
import Badge from "react-bootstrap/cjs/Badge";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class RemindMe extends Component {

    state = {
        Type: 'Normal',
        SelectedTime: 0,
        arr: []

    }

    constructor(props) {
        super(props);
        this.times = [
            {name: 'یک ساعت بعد', code: 1},
            {name: 'دو ساعت بعد', code: 2},
            {name: 'فردا همین ساعت', code: 24},
            {name: 'پس فردا همین ساعت', code: 48},
            {name: 'هفته بعد همین ساعت', code: 84}
        ];

        CurrentUserInfo.RemindMe = this;

        this.onTimeChange = this.onTimeChange.bind(this);
        this.statusBodyTemplate = this.statusBodyTemplate.bind(this);

    }

    onTimeChange(e) {
        this.setState({SelectedTime: e.value});
    }

    componentDidMount() {

        MyCaller.Send('GetRemindMeList')
    }

    statusBodyTemplate(rowData) {
        return <span>
            
           {/* <Badge   variant={'primary'} onClick={() => {


                var selectedTime = this.times.find(f => f.code === rowData.SelectedTime);

                this.setState({
                    Type: rowData.Type,
                    SelectedTime: selectedTime,
                    Date: rowData.Date,
                    Id: rowData.Id,
                    CustomerId:rowData.CustomerId
                })
            }}>
                <i className={'fa fa-pencil-square'}></i>
            </Badge>*/}

            <Badge variant={'danger'} onClick={() => {


                this.delete(rowData);
            }}>
                <i className={'fa fa-minus'}></i>
            </Badge>
            
            
            
        </span>;
    }


    render() {
        return (
            <>

                <TabView

                    activeIndex={this.state.activeIndex}
                    className={"bg-light text-black"} onTabChange={(event) => {

                    if (event.index == 1) {

                        this.componentDidMount();
                    }

                    this.setState({activeIndex: event.index})

                }}>
                    <TabPanel header="جدید" className={"bg-light text-black"}>

                        <div style={{display: 'flex', direction: 'rtl'}}>
                            <div className="p-field-radiobutton">
                                <RadioButton inputId="type" name="type" value="Normal"
                                             onChange={(e) => this.setState({Type: e.value})}
                                             checked={this.state.Type === 'Normal'}/>
                                <label htmlFor="Type2">انتخاب معمولی</label>
                            </div>
                            <div className="p-field-radiobutton">
                                <RadioButton inputId="type" name="type" value="Manual"
                                             onChange={(e) => this.setState({Type: e.value})}
                                             checked={this.state.Type === 'Manual'}/>
                                <label htmlFor="Type1">ساعت مشخص</label>

                            </div>
                        </div>

                        <hr/>

                        {this.state.Type === 'Normal' &&
                        <Dropdown value={this.state.SelectedTime} options={this.times} onChange={this.onTimeChange}
                                  optionLabel="name"
                                  placeholder="یک زمان انتخاب کنید"/>}

                        {this.state.Type !== 'Normal' &&

                        <div className="p-field p-col-12 p-md-4">
                            <Calendar id="time24" value={this.state.Date}
                                      onChange={(e) => {
                                          let day = e.value.getDate();
                                          let mont = e.value.getMonth();
                                          let year = e.value.getFullYear();

                                          let hour = e.value.getHours();
                                          let min = e.value.getMinutes();
                                          let sec = e.value.getSeconds();
                                       
                                          this.setState({
                                              Date: e.value,
                                              DateStr: `${day}/${mont}/${year} ${hour}:${min}:${sec}`
                                          });
                                      }}
                                      showTime showSeconds/>
                            <label htmlFor="time24">:انتخاب از تقویم</label>
                        </div>
                        }

                        <hr/>
                        <SelectAdmin/>

                        <Button onClick={() => {

                            this.save()
                        }}>
                            ذخیره

                        </Button>
                        <hr/>
                    </TabPanel>
                    <TabPanel header="لیست" className={"bg-light text-black"}>
                        <DataTable value={this.state.arr} paginator
                                   emptyMessage={'هیچ یادآوری ثبت نشده است'}
                                   paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                                   currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}" rows={3}
                                   rowsPerPageOptions={[3, 10, 20]}
                        >
                            <Column field="DateTimeShow" header="تاریخ و ساعت"></Column>
                            <Column header="عملیات" body={this.statusBodyTemplate}></Column>

                        </DataTable> </TabPanel>

                </TabView>


            </>
        );
    }


    deleteRemindMeCallback(res) {

        _showMsg("با موفقیت حذف شد")

        this.componentDidMount();
    }

    delete(rowData) {
        MyCaller.Send("DeleteRemindMe", {
            id: rowData.Id
        })
    }

    getRemindMeListCallback(res) {
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('saveRemindMeCallback returns null');
            return;
        }


        this.setState({arr: res.Content});
    }

    saveRemindMeCallback(res) {

        _showMsg("با موفقیت ذخیره شد");
        this.componentDidMount();

    }

    save() {

        MyCaller.Send("SaveRemindMe", {
            Type: this.state.Type,
            SelectedTime: this.state.SelectedTime.code,
            Date: this.state.Date,
            Id: this.state.Id,
            CustomerId: DataHolder.selectedCustomer.Id,
            DateStr: this.state.DateStr
        });
    }
}

export default RemindMe;