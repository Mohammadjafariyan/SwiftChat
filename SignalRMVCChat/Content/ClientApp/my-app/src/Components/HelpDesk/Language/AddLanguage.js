import React, {Component} from 'react';
import Button from "react-bootstrap/cjs/Button";
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import Language from "./Language";
import {BootstrapModal} from "./ConfirmModal";
import {Modal} from "react-bootstrap";

import '../../../styles/myStyle.css'
import {_getHelpDesk} from "../../../Pages/HelpDeskPage";
class AddLanguage extends Component {
    state={};

    confirmBody=
        <div >

        <div className="confirmation-content">

            <Language onSelect={(selectedLanguage)=>{
              //  this.onLanugageSelect()
                this.AddLanguage(selectedLanguage);
            }}/>
        </div>
        </div>;
    
    
    AddLanguage(selectedLanguage){

      this.selectedLanguage=selectedLanguage;


    
    }
    
    constructor(props) {
        super(props);
        CurrentUserInfo.AddLanguage=this;
    }
    render() {
        return (
            <div>
                
                <Button variant={'info'} id={'addLanguage'}  onClick={()=>{
                    this.setState({showConfirm:true})
                }}>
                    <i className={'fa fa-plus'}> </i>
                    اضافه کردن زبان جدید
                </Button>


                {this.state.showConfirm && <BootstrapModal

                    title={'افزودن زبان جدید'}  body={this.confirmBody}
                                                          display={this.state.showConfirm}


                                                            onCancel={()=> {
                                                                this.setState({showConfirm:false})

                                                            }}
                                                                onConfirm={()=>{
                                                                    this.setState({showConfirm:false})


                                                                    MyCaller.Send("CreateHelpDesk",{

                                                                        name:this.selectedLanguage.name,
                                                                        nativeName:this.selectedLanguage.nativeName,
                                                                        flag:this.selectedLanguage.flag,
                                                                        alpha2Code:this.selectedLanguage.alpha2Code})


                 
                }}/>}
                
            </div>
        );
    }
}

export default AddLanguage;