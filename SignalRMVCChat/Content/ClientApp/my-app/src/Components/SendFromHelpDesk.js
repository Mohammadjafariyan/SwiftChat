import React, {Component} from 'react';
import {Dialog} from "primereact/dialog";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import {Spinner} from "react-bootstrap";

class SendFromHelpDesk extends Component {
    state={
        products:[
/*            {title:'code',id:Math.random(),url:'http://localhost:60518/'},
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
            {title:'code',id:Math.random(),url:'http://localhost:60518/'},*/
          

        ]
    }
    
    
    componentDidMount() {
        MyCaller.Send('GetSocialChannelsInfo')
        
        this.setState({loading:true})
        
        _showMsg("خواندن تنظیمات مرکز پشتیبانی")

    }

    getSocialChannelsInfoCallback(res){
        if (!res || !res.Content)
        {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }
        _showMsg("تنظیمات دریافت شد")


        this.setState({email:res.Content.email,
            telegram:res.Content.telegram,
            whatsapp:res.Content.whatsapp,
            helpDeskApi:res.Content.helpDeskApi,
            helpDeskUrlLink:res.Content.helpDeskUrlLink,
        })

        _showMsg("خواندن مقالات مرکز پشتیبانی")

        fetch(res.Content.helpDeskApi)
            .then(re=>re.json())
            .then(re=>{
                this.setState({products:re.array})
                this.setState({loading:false})

                _showMsg("اطلاعات رسید")

            }).catch(e=>{

            _showError('خطایی در درخواست اطلاعات help desk بوجود آمد');
            _showError(e);
        })
    }
    
    onSearch(){
       // _showMsg("خواندن مقالات از مرکز پشتیبانی")
        
        this.setState({loading:true})

        let filter=this.state.globalFilter ? this.state.globalFilter: '';
       
        fetch(this.state.helpDeskApi+"?searchTerm="+filter)
            .then(re=>re.json())
            .then(re=>{
                this.setState({products:re.array})
                this.setState({loading:false})

               // _showMsg("اطلاعات رسید")

            }).catch(e=>{

            _showError('خطایی در درخواست اطلاعات help desk بوجود آمد');
            _showError(e);
        })
    }
    
    constructor(re) {
        super(re);
        
        CurrentUserInfo.SendFromHelpDesk=this;
    }

    preViewPage(link){
        
        this.setState({preview:true,previewUrl:link});
        
    }
    statusBodyTemplate(rowData) {
        
        if (this.props.actionButtons){
            return this.props.actionButtons(rowData);
        }
        
        return   <React.Fragment>
            <Button label="مشاهده" icon={'pi pi-eye'} className="p-button-primary" onClick={()=>{
                CurrentUserInfo.SendFromHelpDesk.preViewPage(rowData.link)
            }}  />
            <Button label="ارسال" icon={'pi pi-reply'} className="p-button-info"  onClick={()=>{
                
                if (CurrentUserInfo.ChatPage){
                    CurrentUserInfo.ChatPage.addChat(
                        {Message:`
                        
                        <a onclick="gapHelpDeskLinkOpener('${rowData.link}')">${rowData.title}</a>
                        `,
                        IsReceive:false
                    })
                }else{
                    _showError('در صفحه چت نیستید')
                }

                if (CurrentUserInfo.SendFromHelpDeskModal)
                {
                    CurrentUserInfo.SendFromHelpDeskModal.hide();
                }
                else{
                    _showError('مدال یافت نشد')
                }
            }}  />

            <Button label="انتخاب" icon={'pi pi-pencil'} className="p-button-info"  onClick={()=>{

                if (CurrentUserInfo.ChatPage){
                    CurrentUserInfo.ChatForm.addText(
                        `
                        
                        <a onclick="GapSelectFromHelpDesk('${rowData.link}')">${rowData.title}</a>
                        `
                    )
                }else{
                    _showError('در صفحه چت نیستید')
                }

                if (CurrentUserInfo.SendFromHelpDeskModal)
                {
                    CurrentUserInfo.SendFromHelpDeskModal.hide();
                }
                else{
                    _showError('مدال یافت نشد')
                }
            }}  />
        </React.Fragment>
    }
    render() {
        const header = (
            <div className="table-header">
                <span className="p-input-icon-left">
                    <i className="pi pi-search" />
                    <InputText type="search" 
                               onInput={(e) => 
                                   
                               {
                                   this.setState({ globalFilter: e.target.value });
                                   this.onSearch();
                               }
                               
                               
                               }
                                   placeholder="جستجو..." />
                </span>
            </div>
        );
        return (
            <div>


                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>}


                {this.state.products && this.state.products.length==0 &&
                <p>اطلاعاتی یافت نشد</p>}
                
                
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

                    rows={10} selection={this.state.selectedProduct1} onSelectionChange={e => this.setState({ selectedProduct1: e.value })} selectionMode="single" dataKey="Id" paginator value={this.state.products}>
                    <Column field="title" header="عنوان مقاله"></Column>
                    <Column field="inventoryStatus" header="جستجو" body={b=>this.statusBodyTemplate(b)} ></Column>
                    </DataTable>}
                
            </div>
        );
    }
}

export default SendFromHelpDesk;