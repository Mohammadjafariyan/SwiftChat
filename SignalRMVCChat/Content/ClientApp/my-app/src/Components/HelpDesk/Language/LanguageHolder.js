import React, {Component} from 'react';
import PrimengModal from "../../SendFromHelpDeskModal";
import Language from "./Language";
import {MyCaller} from "../../../Help/Socket";
import {Button} from "react-bootstrap";
import {DataHolder} from "../../../Help/DataHolder";
import {_showError, _showMsg} from "../../../Pages/LayoutPage";
import DefinedLanguages from "./DefinedLanguages";
import AddLanguage from "./AddLanguage";
import RemoveLanguage from "./RemoveLanguage";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import ChangeLanguage from "./ChangeLanguage";
import {_getHelpDesk, _setHelpDesk} from "../../../Pages/HelpDeskPage";
import GoToHelpCenter from "../GoToHelpCenter";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class LanguageHolder extends Component {
    state={};
    
    constructor(ar) {
        super(ar);
        
        CurrentUserInfo.LanguageHolder=this;
    }
    
    componentDidMount() {
            this.setState({loading:true})
            MyCaller.Send("Language_GetCurrentHelpDesk_SelectedLanguage")
    }

    onLanugageSelect(){

        if ( !_getHelpDesk() || !_getHelpDesk().language.Name) {

            console.log("هیچ زبانی انتخاب نشده است",_getHelpDesk())
            _showError("هیچ زبانی انتخاب نشده است");
            return ;
        }

        _showMsg("در حال ثبت")
        this.setState({loading:true})
        MyCaller.Send("CreateHelpDesk",{

            name:_getHelpDesk().language.Name,
            nativeName:_getHelpDesk().language.nativeName,
            flag:_getHelpDesk().language.flag,
            alpha2Code:_getHelpDesk().language.alpha2Code})
    }
    
    reRequestAllDatas(){
        if (CurrentUserInfo.CategoryIndex){
            CurrentUserInfo.CategoryIndex.componentDidMount();
        }

        if (CurrentUserInfo.SendFromHelpDesk){
            CurrentUserInfo.SendFromHelpDesk.componentDidMount();
        }
    }
    
    language_GetCurrentHelpDesk_SelectedLanguageCallback(res){
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }



        _setHelpDesk(res.Content.selectedHelpDeskId,{
            alpha2Code:res.Content.alpha2Code,
            nativeName:res.Content.nativeName,
            Name:res.Content.Name,
        })

      //  _getHelpDesk().helpDeskId=res.Content.selectedHelpDeskId;

      
        
        if (CurrentUserInfo.HelpDeskPage){
            CurrentUserInfo.HelpDeskPage.reset();
        }

       

    }
    create_HelpDesk_Callback(res){

        if (!res.Content || !res.Content.Id){
            _showError("کد بازگشتی نال است")
        }


        _showMsg("مرکز پشتیبانی با زبان انتخاب شده ایجاد شد")

        _setHelpDesk(res.Content.Id,res.Content.Language)
   /*     HelpDeskData.getHelpDesk().language.Name=res.Content.name;
        DataHolder.selectedCountryCode.selectedHelpDeskId=res.Content.id;*/

        MyCaller.Send("Language_GetCurrentHelpDesk_SelectedLanguage")

    }
    render() {
        return (
            <div>



                {(CurrentUserInfo.HelpDeskPage && (!_getHelpDesk() || !_getHelpDesk().language.Name)) && 
                    
                    <>
                    <p className={'small'}>مرکز پشتیبانی می تواند چند زبانه باشد ، اما برای شروع باید  حداقل یک زبان انتخاب نمایید</p>
                    <Language onSelect={(selectedLanguage)=>{
                        _setHelpDesk(selectedLanguage.Id,selectedLanguage)
                        this.onLanugageSelect()
                    }}/>

                    <br/>

                    <Button variant={"primary"} onClick={()=>{
                        this.props.onSelect();
                    }}>

                        انتخاب
                    </Button>

                    </>
                }
                {CurrentUserInfo.HelpDeskPage && _getHelpDesk().language && _getHelpDesk().language.Name &&

                <Row>
                    <Col md={2}>
                       <ChangeLanguage/>

                    </Col>
                    <Col md={{span: 5, offset: 5}} style={{display:'flex',direction:'rtl',marginTop:'5px'}}>
                        <AddLanguage/>
                        <RemoveLanguage/>
                        

                    </Col>

                    <Col>
                        <GoToHelpCenter/>

                    </Col>
                    
                    
                </Row>
                }
            </div>
        );
    }
}

export default LanguageHolder;
