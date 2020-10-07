import React, {Component, useState} from 'react';
import SendFromHelpDesk from "../Components/SendFromHelpDesk";
import {Button} from "primereact/button";
import {CurrentUserInfo} from "../Help/Socket";
import {_showError, _showMsg} from "./LayoutPage";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import {DataHolder} from "../Help/DataHolder";
import Modal from "react-bootstrap/Modal";
import {Toast} from "primereact/toast";
import Col from "react-bootstrap/Col";

class HelpDeskPage extends Component {
    state={};
    constructor(arg) {
        super(arg);

        CurrentUserInfo.HelpDeskPage = this;
    }

    buttons(rowData) {
        return <React.Fragment>
            <Button label="حذف" icon={'pi pi-times'} className="p-button-danger" onClick={() => {

                DataHolder.HelpDeskRowDataSelected = rowData;

                CurrentUserInfo.HelpDeskPage.confirmModal()

            }}/>
            
            <Button label="ویرایش" icon={'pi pi-pencil'} className="p-button-success" onClick={() => {
                DataHolder.subPage = 'SaveArticle';

                DataHolder.HelpDeskRowDataSelected = rowData;

                CurrentUserInfo.LayoutPage.setState({tmp: Math.random()});


            }}/>
        </React.Fragment>
    }

    render() {
        return (
            <div>

                <Container>

                    <Row dir={'rtl'}>

                        <Button label="مقاله جدید" icon={'pi pi-plus'}
                                className="p-button-primary" onClick={() => {


                                    this.setState({deleteModal:true})
                          //  DataHolder.subPage = 'SaveArticle';

                          //  CurrentUserInfo.LayoutPage.setState({tmp: Math.random()});
                        }}/>
                    </Row>
                    <Row>
                        <SendFromHelpDesk actionButtons={r => this.buttons(r)}>

                        </SendFromHelpDesk>
                    </Row>

                </Container>


                <Toast ref={(el) => this.toastBC = el} position="bottom-center" />


               {/* {this.deleteModal &&
                <ConfirmModal messaage={'آیا از حذف این مقاله اطمینان دارید ؟'}
                              callback={() => {

                                  this.deleteArticle()
                              }}/>}*/}


            </div>
        );
    }

    hD_DeleteByIdCallback(res) {
        _showMsg("با موفقیت حذف شد")
        CurrentUserInfo.SendFromHelpDesk.onSearch();
        
    }
    
    
    confirmModal(){
        
        if (this.state.isToastOpen){
            return ;
        }
        this.setState({isToastOpen:true})
        
        this.toastBC.show({ severity: 'warn', sticky: true, content: (
              <Row>
                  <Col md={12} >
                      <div className="p-text-center">
                          <i className="pi pi-exclamation-triangle" style={{fontSize: '3rem'}}></i>
                          <p>آیا از حذف این مقاله اطمینان دارید ؟</p>
                      </div>

                  </Col>
                  <Col md={12}>
                     <Row>
                         <Col md={6}>
                             <Button style={{width:'100%'}} type="button" label="بله" className="p-button-success"

                                     onClick={()=>{
                                         this.setState({isToastOpen:false})
                                         this.toastBC.clear();
                                         this.deleteArticle()
                                     }}
                             />
                         </Col>
                         <Col  md={6}>
                             <Button onClick={()=>{
                                 this.setState({isToastOpen:false})
                                 this.toastBC.clear();

                             }} style={{width:'100%'}}  type="button" label="لغو" className="p-button-secondary" />

                         </Col>
                     </Row>

                  </Col>
                  </Row>
            ) });
    }

    deleteArticle() {
        if (!DataHolder.HelpDeskRowDataSelected) {
            _showError('هیچ مقاله ای برای حذف انتخاب نشده است');
            return;
        }

        _showMsg("در حال حذف مقاله")

        MyCaller.Send('HD_DeleteById', {id: DataHolder.HelpDeskRowDataSelected.Id})
    }
}

export default HelpDeskPage;

