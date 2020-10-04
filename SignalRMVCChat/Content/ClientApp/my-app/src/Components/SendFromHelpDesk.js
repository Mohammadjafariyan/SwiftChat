import React, {Component} from 'react';
import {Dialog} from "primereact/dialog";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";

class SendFromHelpDesk extends Component {
    state={
        products:[
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
          

        ]
    }
    
    constructor(re) {
        super(re);
        
        CurrentUserInfo.SendFromHelpDesk=this;
    }

    preViewPage(url){
        
        this.setState({preview:true,previewUrl:url});
        
    }
    statusBodyTemplate(rowData) {
        return   <React.Fragment>
            <Button label="مشاهده" className="p-button-primary" onClick={()=>{
                CurrentUserInfo.SendFromHelpDesk.preViewPage(rowData.url)
            }}  />
            <Button label="انتخاب"  className="p-button-info"  />
        </React.Fragment>
    }
    render() {
        const header = (
            <div className="table-header">
                <span className="p-input-icon-left">
                    <i className="pi pi-search" />
                    <InputText type="search" 
                               onInput={(e) => this.setState({ globalFilter: e.target.value })}
                                   placeholder="جستجو..." />
                </span>
            </div>
        );
        return (
            <div>
                {this.state.preview
                &&
<>

    <Button label="بازگشت" icon={'pi pi-times'} className="p-button-rounded p-button-secondary"
            onClick={()=>{
        this.setState({preview:false})
    }}  />
    <hr/>
    
    <iframe src={this.state.previewUrl} style={{width:'100%',height:'100vh'}}></iframe>

</>
                }
                
                {!this.state.preview
&&
                <DataTable
                    
                    header={header}

                    rows={10} selection={this.state.selectedProduct1} onSelectionChange={e => this.setState({ selectedProduct1: e.value })} selectionMode="single" dataKey="id" paginator value={this.state.products}>
                    <Column field="title" header="عنوان مقاله"></Column>
                    <Column field="inventoryStatus" header="Status" body={this.statusBodyTemplate} ></Column>
                    </DataTable>}
                
            </div>
        );
    }
}

export default SendFromHelpDesk;