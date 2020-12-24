import React, {Component} from 'react';
import {CurrentUserInfo,MyCaller} from "../../../Help/Socket";
import {Dropdown} from "primereact/dropdown";
import {DataHolder} from "../../../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import {_getHelpDesk, _setHelpDesk} from "../../../Pages/HelpDeskPage";
import {_showError} from "../../../Pages/LayoutPage";

class DefinedLanguages extends Component {
    
    state={};
    
    constructor(r) {
        super(r);
        CurrentUserInfo.DefinedLanguages=this;
        
        this.onCountryChange.bind(this);
    }
    componentDidMount() {
        
        MyCaller.Send("GetDefinedLanguages")
    }


    selectHelpDeskCallback(res){
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }


        _setHelpDesk(res.Content.Id,res.Content.Language);


        let selected= this.state.countries.find(c=>c.alpha2Code===res.Content.Language.alpha2Code);
        
        if (!selected){
            _showError("کشور برای انتخاب یافت نشد سمت کلاینت")
        }
        this.setState({selectedCountry:selected})

        MyCaller.Send("Language_GetCurrentHelpDesk_SelectedLanguage")

        /*   DataHolder.selectedCountryCode=res.Content;
           _getHelpDesk().helpDeskId=res.Content.Id;*/
  
    }
    
    onCountryChange(e) {


      //  _setHelpDesk(e.value.Id,e.value);

     //   DataHolder.selectedCountryCode=e.value;
        MyCaller.Send("SelectHelpDesk",{

            name:e.value.Name,
            nativeName:e.value.nativeName,
            flag:e.value.flag,
            alpha2Code:e.value.alpha2Code
        })
        


    }

    getDefinedLanguagesCallback(res){
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }



        let selectedCountry;
        if (res.Content.SelectedLanguage){
            
            selectedCountry= res.Content.EntityList.find(f=>f.alpha2Code===res.Content.SelectedLanguage.alpha2Code);

        }


        this.setState({countries:res.Content.EntityList,selectedCountry:selectedCountry})
        
        
    }

    render() {
        return (
            <div>
                
            {/*   <Badge variant={'primary'}>{DataHolder.selectedCountryCode.Name}</Badge>*/}
                
                {this.state.countries && <Dropdown value={this.state.selectedCountry}
                                                   options={this.state.countries} onChange={(e)=>{
                    this.onCountryChange(e)
                }}
                                                   optionLabel="Name"
                                                   placeholder="انتخاب یک کشور و زبان"
                                                   valueTemplate={this.selectedCountryTemplate} 
                                                   itemTemplate={this.countryOptionTemplate} />
                }
                
            </div>
        );
    }

    selectedCountryTemplate(option, props) {
        if (option) {
            return (
                <div className="country-item country-item-value">
                    <img width={50} height={30} alt={option.Name} 
                         src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                    <div>{option.Name} {option.nativeName!=option.Name ? '- '+option.nativeName : ''} </div>
                </div>
            );
        }

        return (
            <span>
                {props.placeholder}
            </span>
        );
    }

    countryOptionTemplate(option) {
        return (
            <div className="country-item">
                <img width={50} height={30} alt={option.Name} src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                <div>{option.Name} {option.nativeName!=option.Name ? '- '+option.nativeName : ''} </div>
            </div>
        );
    }
}

export default DefinedLanguages;