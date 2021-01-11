import React, {Component} from 'react';
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {Spinner} from "react-bootstrap";
import {InputText} from "primereact/inputtext";
import {Button} from "primereact/button";
import {MyCaller} from "../../../Help/Socket";
import {_showError} from "../../../Pages/LayoutPage";
import {DataHolder} from "../../../Help/DataHolder";
import {_getHelpDesk} from "../../../Pages/HelpDeskPage";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class CategoryIndex extends Component {
state={};
    constructor(re) {
        super(re);

        CurrentUserInfo.CategoryIndex = this;
    }
    componentDidMount() {
        this.setState({loading:true})
        MyCaller.Send("Category_Get_List",{helpDeskId:_getHelpDesk().helpDeskId})
    }

    category_Get_List_Callback(res){
        this.setState({loading:false})
    
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }
        
        
        this.setState({list:res.Content})
    }
    
    
    statusBodyTemplate(rowData) {

   /*     if (this.props.actionButtons) {
            return this.props.actionButtons(rowData);
        }*/

        return <React.Fragment>
            <Button icon={'pi pi-pencil'} className="p-button-primary" onClick={() => {

                
                DataHolder.selectedCategory=rowData;
                CurrentUserInfo.CategoryLayout.setState({page:"Save"})
            }}/>
            <Button  icon={'pi pi-times'} className="p-button-danger" onClick={() => {
                
                DataHolder.selectedCategory=rowData;
                CurrentUserInfo.CategoryLayout.setState({page:"Delete"})

            }}/>

          
        </React.Fragment>
    }

    render() {
        return (
            <div>

                <h3>لیست دسته بندی ها</h3>
                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>}

                <Button label="دسته بندی جدید" icon={'pi pi-plus'} className="p-button-primary" onClick={() => {

                    CurrentUserInfo.CategoryLayout.setState({page:"Save"})
                }}/>
                <hr/>

                {this.state.list
                &&
                <DataTable


                    rows={10} selection={this.state.list}
                    onSelectionChange={e => this.setState({selectedCategory: e.value})} selectionMode="single"
                    dataKey="Id" paginator value={this.state.list}>
                    <Column field="inventoryStatus" header="عملیات" body={b => this.statusBodyTemplate(b)}></Column>
                    <Column field="Title" header="عنوان دسته بندی"></Column>
                </DataTable>}
            </div>
        );
    }
}

export default CategoryIndex;