import React, {Component, useState} from 'react';
import Button from "react-bootstrap/cjs/Button";
import {Dialog} from "primereact/dialog";
import {ConfirmModal} from "./ConfirmModal";
import {DataHolder} from "../../../Help/DataHolder";
import {_showError, _showMsg} from "../../../Pages/LayoutPage";
import {CurrentUserInfo,MyCaller } from "../../../Help/Socket";
import {_getHelpDesk, _setHelpDesk} from "../../../Pages/HelpDeskPage";

class RemoveLanguage extends Component {

    constructor(props) {
        super(props);
        CurrentUserInfo.RemoveLanguage=this;
    }


    confirmBody=
            <div className="confirmation-content">
            <i className="pi pi-exclamation-triangle p-mr-3 text-danger" style={{ fontSize: '2rem' }} />
            <br />
            <span className={'text-danger'}>در صورت حذف این زبان تمامی دسته بندی ها و مقالات تعریف شده حذف خواهند شد</span>
            <span className={'text-danger'}>آیا ادامه می دهید ؟</span>
        </div>;
    
    state={};

    removeHelpDeskCallback(res){

        _showMsg("با موفقیت حذف شد")


      //  _setHelpDesk(null,null)
/*        DataHolder.selectedCountryCode=null;
        _getHelpDesk().helpDeskId=null;*/

if (!CurrentUserInfo.DefinedLanguages){
    _showError('عدم امکان اپدیت زبان ها ، سمت کلاینت')
    return ;
}
        
        CurrentUserInfo.DefinedLanguages.componentDidMount();


        MyCaller.Send("Language_GetCurrentHelpDesk_SelectedLanguage")

      

    }

    removeCurrentLanguage(){
        if (CurrentUserInfo.HelpDeskPage && !_getHelpDesk().language || !_getHelpDesk().helpDeskId)
        {
            _showError("هیچ زبانی انتخاب نشده است")
            return ;
        }
        
        
        _showMsg("در حال حذف زبان")
        MyCaller.Send('RemoveHelpDesk',{id:_getHelpDesk().helpDeskId})

    }
    render() {

        if ( !_getHelpDesk().language || !_getHelpDesk().helpDeskId)
            return <></>
        
        return (
            <div>
                <Button variant={'danger'} id={'removeLanguage'} onClick={()=>{
                    this.setState({showConfirm:true})
                }}>
                    <i className={'fa fa-times'}> </i>
                   حذف زبان کنونی
                </Button>
                
                
                {this.state.showConfirm && <ConfirmModal  body={this.confirmBody}
                    display={this.state.showConfirm}

                                                          onCancel={()=>{
                                                              this.setState({showConfirm:false})
                                                            
                                                          }}                      
                                                          
                                                          onConfirm={()=>{
                    
                        this.setState({showConfirm:false})
                    this.removeCurrentLanguage()
                }}/>}
            </div>
        );
    }
}

export default RemoveLanguage;

