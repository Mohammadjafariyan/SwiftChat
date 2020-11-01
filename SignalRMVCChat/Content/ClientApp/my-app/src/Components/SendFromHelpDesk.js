import React, {Component} from 'react';
import {Dialog} from "primereact/dialog";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import {Column} from "primereact/column";
import {DataTable} from "primereact/datatable";
import {Button} from "primereact/button";
import {InputText} from "primereact/inputtext";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import {Spinner} from "react-bootstrap";
import Badge from "react-bootstrap/Badge";
import '../styles/myStyle.css'

class SendFromHelpDesk extends Component {
    state = {
        products: [
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

        this.setState({loading: true})

        _showMsg("خواندن تنظیمات مرکز پشتیبانی")

    }

    getSocialChannelsInfoCallback(res) {
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }
        _showMsg("تنظیمات دریافت شد")


        let helpDeskApi = res.Content.helpDeskApi;

      


        this.setState({
            email: res.Content.email,
            telegram: res.Content.telegram,
            whatsapp: res.Content.whatsapp,
            helpDeskApi: helpDeskApi,
            helpDeskUrlLink: res.Content.helpDeskUrlLink,
        })

        this.readArticles(helpDeskApi )
       
    }
    
    readArticles(helpDeskApi ){
        _showMsg("خواندن مقالات مرکز پشتیبانی")

        let websiteToken = document.getElementById('websiteToken').value;

        let isAdmin=false;
        // از اینجا میفهمیم که در ادمین استفاده می شود
        if (this.props.actionButtons) {

            isAdmin= true;

        }
        
        
        fetch(helpDeskApi,{
            method: "POST",
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                websiteToken: websiteToken,
                isAdmin:  isAdmin,
                searchTerm:null
            })
        })
            .then(re => re.json())

            .then(re => {
                this.setState({products: re.array})
                this.setState({loading: false})

                _showMsg("اطلاعات رسید")

            }).catch(e => {

            _showError('خطایی در درخواست اطلاعات help desk بوجود آمد');
            console.error(e);
        })
    }

    onSearch() {
        // _showMsg("خواندن مقالات از مرکز پشتیبانی")

        this.setState({loading: true})

        let filter = this.state.globalFilter ? this.state.globalFilter : '';

        fetch(this.state.helpDeskApi + "?searchTerm=" + filter)
            .then(re => re.json())
            .then(re => {
                this.setState({products: re.array})
                this.setState({loading: false})

                // _showMsg("اطلاعات رسید")

            }).catch(e => {

            _showError('خطایی در درخواست اطلاعات help desk بوجود آمد');
            _showError(e);
        })
    }

    constructor(re) {
        super(re);

        CurrentUserInfo.SendFromHelpDesk = this;
    }

    preViewPage(link) {

        this.setState({preview: true, previewUrl: link});

    }

    statusBodyTemplate(rowData) {

        if (this.props.actionButtons) {
            return this.props.actionButtons(rowData);
        }

        return <React.Fragment>
            <Button label="مشاهده" icon={'pi pi-eye'} className="p-button-primary" onClick={() => {
                CurrentUserInfo.SendFromHelpDesk.preViewPage(rowData.link)
            }}/>
            <Button label="ارسال" icon={'pi pi-reply'} className="p-button-info" onClick={() => {

                if (CurrentUserInfo.ChatPage) {
                    CurrentUserInfo.ChatPage.addChat(
                        {
                            Message: `
                        
                        <a onclick="gapHelpDeskLinkOpener('${rowData.link}')">${rowData.title}</a>
                        `,
                            IsReceive: false
                        })
                } else {
                    _showError('در صفحه چت نیستید')
                }

                if (CurrentUserInfo.SendFromHelpDeskModal) {
                    CurrentUserInfo.SendFromHelpDeskModal.hide();
                } else {
                    _showError('مدال یافت نشد')
                }
            }}/>

            <Button label="انتخاب" icon={'pi pi-pencil'} className="p-button-info" onClick={() => {

                if (CurrentUserInfo.ChatPage) {
                    CurrentUserInfo.ChatForm.addText(
                        `
                        
                        <a onclick="GapSelectFromHelpDesk('${rowData.link}')">${rowData.title}</a>
                        `
                    )
                } else {
                    _showError('در صفحه چت نیستید')
                }

                if (CurrentUserInfo.SendFromHelpDeskModal) {
                    CurrentUserInfo.SendFromHelpDeskModal.hide();
                } else {
                    _showError('مدال یافت نشد')
                }
            }}/>
        </React.Fragment>
    }

    render() {
        const header = (
            <div className="table-header">
                <span className="p-input-icon-left">
                    <i className="pi pi-search"/>
                    <InputText type="search"
                               onInput={(e) => {
                                   this.setState({globalFilter: e.target.value});
                                   this.onSearch();
                               }


                               }
                               placeholder="جستجو..."/>
                </span>
            </div>
        );
        return (
            <div>


                {this.state.loading &&
                <Spinner animation="border" role="status">
                    <span className="sr-only">در حال خواندن اطلاعات...</span>
                </Spinner>}


             {/*   {this.state.products && this.state.products.length == 0 &&
                <p>اطلاعاتی یافت نشد</p>}*/}


                {this.state.preview
                &&
                <>

                    <Button label="بازگشت" icon={'pi pi-times'} className="p-button-rounded p-button-secondary"
                            onClick={() => {
                                this.setState({preview: false})
                            }}/>

                    <iframe src={this.state.previewUrl} style={{width: '100%', height: '100vh'}}></iframe>

                </>
                }

                {!this.state.preview
                &&
                <DataTable

                    header={header}

                    
                    emptyMessage={'اطلاعاتی یافت نشد'}
                    rows={10} selection={this.state.selectedProduct1}
                    onSelectionChange={e => this.setState({selectedProduct1: e.value})} selectionMode="single"
                    dataKey="Id" paginator value={this.state.products}>
                    <Column field="inventoryStatus" header="عملیات" body={b => this.statusBodyTemplate(b)}></Column>
                    <Column field="lastPublishTime" header="آخرین پابلیش"
                            body={b => this.lastPublishTimeTemplate(b)}></Column>
                    <Column field="LastUpdatedDescription" header="توضیح" ></Column>
                    <Column field="visitsCount" header="تعداد بازدید" body={b => this.visitsCountTemplate(b)}></Column>
                    <Column field="status" header="وضعیت نمایش" body={b => this.statusTemplate(b)}></Column>
                    <Column field="title" header="عنوان مقاله" bodyClassName={'text-summary'} ></Column>
                </DataTable>}

            </div>
        );
    }

    statusTemplate(rowData) {

        if (rowData.status === 2) {
            return <span className={'text-success'}>منتشر شده</span>
        }
        if (rowData.status === 0) {
            return <span className={'text-success'}>پیش نمایش</span>
        }
        return <span className={'text-warning'}>مخفی</span>

    }

    visitsCountTemplate(rowData) {

        return <Badge variant={'primary'}>{rowData.visitsCount}</Badge>
    }

    lastPublishTimeTemplate(rowData) {

        return <span className={'small'} dir="rtl">{rowData.lastPublishTime}</span>

    }
}

export default SendFromHelpDesk;