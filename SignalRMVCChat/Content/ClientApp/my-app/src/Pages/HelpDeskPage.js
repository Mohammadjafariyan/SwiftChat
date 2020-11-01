import React, {Component, useState} from 'react';
import SendFromHelpDesk from "../Components/SendFromHelpDesk";
import {Button} from "primereact/button";
import {CurrentUserInfo,MyCaller} from "../Help/Socket";
import {_showError, _showMsg} from "./LayoutPage";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import {DataHolder} from "../Help/DataHolder";
import Modal from "react-bootstrap/Modal";
import {Toast} from "primereact/toast";
import Col from "react-bootstrap/Col";


import '../styles/myStyle.css'
import CategoryLayout from "../Components/HelpDesk/Category/CategoryLayout";
import {TabPanel, TabView} from "primereact/tabview";
import LanguageHolder from "../Components/HelpDesk/Language/LanguageHolder";
import SaveArticle from "../Components/SaveArticle";
import HelpDeskSetting from "../Components/HelpDesk/HelpDeskSetting/HelpDeskSetting";



export function _getHelpDesk(){

    if (CurrentUserInfo.HelpDeskPage){
      return   CurrentUserInfo.HelpDeskPage.getHelpDesk();
    }
   return  {};//{helpDeskId:null,language: {}}
}


export function _setHelpDesk(helpdeskId, selectedLanguage){

    if (selectedLanguage && selectedLanguage.name){
        selectedLanguage.Name=selectedLanguage.name;
    }
    if (CurrentUserInfo.HelpDeskPage){
        CurrentUserInfo.HelpDeskPage.setHelpDesk(helpdeskId,selectedLanguage);
    }else{
        _showError("CurrentUserInfo.HelpDeskPage is null")
    }
    
}


class HelpDeskPage extends Component {
    state={activeIndex:1,
   };
    constructor(arg) {
        super(arg);

        CurrentUserInfo.HelpDeskPage = this;
  

    }

    setHelpDesk(helpdeskId, selectedLanguage) {

        this.helpdeskId=helpdeskId;
        this.selectedLanguage=selectedLanguage;
        
        this.setState({helpdeskId,selectedLanguage});
        
        this.reset();

    }

    getHelpDesk(){
        let language=this.selectedLanguage;
        if (!language){
            language={};
        }
        return {helpDeskId:this.helpdeskId,language:language}
    }
    reset(){
        
        
        if (CurrentUserInfo.CategoryIndex){
            CurrentUserInfo.CategoryIndex.componentDidMount();
        }

        if (CurrentUserInfo.SendFromHelpDesk){
            CurrentUserInfo.SendFromHelpDesk.componentDidMount();
        }

        if (CurrentUserInfo.RemoveLanguage){
            CurrentUserInfo.RemoveLanguage.setState({tmp:Math.random()});
        }

        if (CurrentUserInfo.AddLanguage){
            CurrentUserInfo.AddLanguage.setState({tmp:Math.random()});
            //  CurrentUserInfo.AddLanguage.componentDidMount();
        }

        if (CurrentUserInfo.DefinedLanguages){
            CurrentUserInfo.DefinedLanguages.componentDidMount();
        }
        this.setState({tmp:Math.random()})

    }
    
 
    
    
    

    language_GetCurrentHelpDesk_SelectedLanguageCallback(res){
        this.setState({TMP:Math.random()})
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
        
        if (!_getHelpDesk().helpDeskId){
            return  <></>
        }
       

        return (
            <div>

                <Container className={"centered"}>
                    <br/>

                    <TabView activeIndex={this.state.activeIndex} onTabChange={(e) => this.setState({activeIndex: e.index})}>
                        <TabPanel header="دسته بندی ها">
                            <CategoryLayout/>
                        </TabPanel>
                        <TabPanel header="مقالات">
                            {!DataHolder.subPage &&
                            this.showArticles()
                            }

                            {DataHolder.subPage==='SaveArticle' &&
                            <SaveArticle/>}
                        </TabPanel>

                        <TabPanel header="تنظیمات">
                        
                            <HelpDeskSetting/>
                        </TabPanel>
                        
                    </TabView>
                    
                  

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

    articleDeleteByIdCallback(res){

        CurrentUserInfo.SendFromHelpDesk.readArticles(CurrentUserInfo.SendFromHelpDesk.state.helpDeskApi);
        
    }

    deleteArticle() {
        if (!DataHolder.HelpDeskRowDataSelected) {
            _showError('هیچ مقاله ای برای حذف انتخاب نشده است');
            return;
        }

        _showMsg("در حال حذف مقاله")

        MyCaller.Send('ArticleDeleteById', {id: DataHolder.HelpDeskRowDataSelected.Id})

        DataHolder.HelpDeskRowDataSelected=null;
    }

    showArticles() {
        return <>
            <Row dir={'rtl'}>

                <Button label="مقاله جدید" icon={'pi pi-plus'}
                        className="p-button-primary" onClick={() => {


                    this.setState({deleteModal:true})
                    //  DataHolder.subPage = 'SaveArticle';

                    //  CurrentUserInfo.LayoutPage.setState({tmp: Math.random()});

                    DataHolder.subPage = 'SaveArticle';

                    //   DataHolder.HelpDeskRowDataSelected = rowData;

                    CurrentUserInfo.LayoutPage.setState({tmp: Math.random()});
                }}/>
            </Row>
            <br/>

            <Row>
                <SendFromHelpDesk actionButtons={r => this.buttons(r)}>

                </SendFromHelpDesk>
            </Row></>
    }
}

export default HelpDeskPage;

