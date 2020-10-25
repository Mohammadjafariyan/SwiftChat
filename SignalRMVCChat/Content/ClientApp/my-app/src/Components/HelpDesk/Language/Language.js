import React, {Component} from 'react';
import {DataHolder} from "../../../Help/DataHolder";
import {CurrentUserInfo,MyCaller} from "../../../Help/Socket";
import {CountryService} from "./CountryService";
import {_showMsg} from "../../../Pages/LayoutPage";
import {Dropdown} from "primereact/dropdown";


import '../../../styles/myStyle.css'
import {_getHelpDesk, _setHelpDesk} from "../../../Pages/HelpDeskPage";

class Language extends Component {
    state={};
    constructor(ar) {
        super(ar);
        
        CurrentUserInfo.Language=this;
        this.onCountryChange = this.onCountryChange.bind(this);
  /*      this.countries = [
            {name: 'Australia', code: 'AU'},
            {name: 'Brazil', code: 'BR'},
            {name: 'China', code: 'CN'},
            {name: 'Egypt', code: 'EG'},
            {name: 'France', code: 'FR'},
            {name: 'Germany', code: 'DE'},
            {name: 'India', code: 'IN'},
            {name: 'Japan', code: 'JP'},
            {name: 'Spain', code: 'ES'},
            {name: 'United States', code: 'US'}
        ];*/
    }
    
    componentDidMount() {

        _showMsg("در حال خواندن اطلاعات")

        new CountryService().GetAll().then(re=>{
            
            console.log(re)
            _showMsg("لیست کشور ها خوانده شد")
            
            let _selectedCountry;
            if (_getHelpDesk().language && _getHelpDesk().language.alpha2Code){
                re.find(r=>r.alpha2Code===_getHelpDesk().language.alpha2Code)  ;  
            }
            
            this.setState({countries:re,selectedCountry:_selectedCountry})
        })
    }

    onCountryChange(e) {
        this.setState({ selectedCountry: e.value });


       // _setHelpDesk(null,e.value)
       // DataHolder.selectedCountryCode=e.value;
        this.props.onSelect(e.value);
    }

    selectedCountryTemplate(option, props) {
        if (option) {
            return (
                <div className="country-item country-item-value">
                    <img width={50} height={30} alt={option.name} src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                    <div>{option.name} <span  className={'nativeName'}>{option.nativeName!=option.name ? '- '+option.nativeName : ''}</span> </div>
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
                <img width={50} height={30} alt={option.name} src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                <div>{option.name} <span className={'nativeName'}>{option.nativeName!=option.name ? '- '+option.nativeName : ''}</span> </div>
            </div>
        );
    }


    render() {
        return (
            <div>


                {!this.props.noTitle &&
                <h5>

                    یک زبان به مرکز پشتیبانی اضافه کنید

                    <i className={'fa fa-plus'}></i>
                </h5>}

                {this.state.countries && 
                    

                        <Dropdown style={{position:this.props.noTitle ? 'absolute':''}} value={this.state.selectedCountry} options={this.state.countries} onChange={this.onCountryChange} optionLabel="name" filter showClear filterBy="name" placeholder="انتخاب یک کشور و زبان"
                                  valueTemplate={this.selectedCountryTemplate} itemTemplate={this.countryOptionTemplate} />
                }
                
            </div>
        );
    }
}

export default Language;