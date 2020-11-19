import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import {_showError} from "../../Pages/LayoutPage";
import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";

class CustomerTrackingList extends Component {
    state={};
    componentDidMount() {

        CurrentUserInfo.CustomerProfile=this;


        if (!this.props.Customer)
            return;


        MyCaller.Send('GetCustomerTrackingInfoDetail',
            {
                customerId: this.props.Customer.Id,
                withList:true
            }
            /*withList:FALSE*/)

        this.pageTitleBodyTemplate=this.pageTitleBodyTemplate.bind(this);
        this.cityBodyTemplate=this.cityBodyTemplate.bind(this);
        this.stateBodyTemplate=this.stateBodyTemplate.bind(this);


          }

    getCustomerTrackingInfoDetailCallback(res) {
        if (!res || !res.Content) {
            _showError('screenRecordCustomerCloseCallback res is null');
            return;
        }

        if (!this.props.Customer || this.props.Customer.Id != res.Content.customerId) {
            return;
        }
        /*
                    EnterTrack=enter,
                    LastTrack=lastTrack,
                    List=list*/


        this.setState({
            List:res.Content.List
        })
    }
    pageTitleBodyTemplate(rowData){
        return <span ><a href={rowData.Url} 
                         target='_blank'>{rowData.PageTitle}</a>
        </span>;

    }
    cityBodyTemplate(rowData){
        let cityName=rowData.UserCity ? rowData.UserCity.name : rowData.city;

        return <span >{cityName}
        </span>;
    }

    stateBodyTemplate(rowData){
        let stateName=rowData.UserState ? rowData.UserState.name : rowData.region_name;

        return <span >{stateName}
        </span>;
    }
    
    
    render() {
        return (
            <div>
                {this.state.List && 
                <DataTable value={this.state.List}
                           paginator
                           paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                           currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}" 
                           rows={40}
                           rowsPerPageOptions={[40,80,120]}>
                  
                    <Column field="PageTitle" header="صفحه"
                            body={this.pageTitleBodyTemplate}></Column>

                    <Column field="CustomerTrackInfoTypeText"
                            header="رفتار"></Column>

                    <Column field="ip"
                            header="ip"></Column>
                    <Column field="DateTimeText" header="تاریخ و ساعت"></Column>
                    <Column field="TimeSpent" header="مدت حضور "></Column>


                    <Column field="PageTitle" header="شهر"
                            body={this.cityBodyTemplate}></Column>

                    <Column field="PageTitle" header="استان"
                            body={this.stateBodyTemplate}></Column>

                </DataTable>
                }
            </div>
        );
    }
}

export default CustomerTrackingList;