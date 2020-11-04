import React, {Component} from 'react';
import {AutoComplete} from "primereact/autocomplete";
import {ListBox} from "primereact/listbox";
import {InputText} from "primereact/inputtext";
import {Badge} from "react-bootstrap";
import Button from "react-bootstrap/cjs/Button";
import {_showMsg} from "../../Pages/LayoutPage";
import {MyCaller,CurrentUserInfo} from "../../Help/Socket";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/Col";

class ReadyPms extends Component {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.ReadyPms=this;
        this.searchCountry = this.searchCountry.bind(this);
        this.itemTemplate = this.itemTemplate.bind(this);
        this.countryTemplate = this.countryTemplate.bind(this);
        this.countries = [
            {Name: 'Australia', code: 'AU'},
            {Name: 'Brazil', code: 'BR'},
            {Name: 'China', code: 'CN'},
            {Name: 'Egypt', code: 'EG'},
            {Name: 'France', code: 'FR'},
            {Name: 'Germany', code: 'DE'},
            {Name: 'India', code: 'IN'},
            {Name: 'Japan', code: 'JP'},
            {Name: 'Spain', code: 'ES'},
            {Name: 'United States', code: 'US'}
        ];
    }


    componentDidMount() {

        this.GetReadyPmsList();
    }

    searchCountry(event) {
        setTimeout(() => {
            let filteredCountries;
            if (!event.query.trim().length) {
                filteredCountries = [...this.state.countries];
            } else {
                filteredCountries = this.state.countries.filter((country) => {
                    return country.Name.toLowerCase().startsWith(event.query.toLowerCase());
                });
            }

            this.setState({filteredCountries});
        }, 250);
    }

    itemTemplate(item) {
        return (
            <div className="country-item">
                <div>{item.Name}</div>
            </div>
        );
    }

    countryTemplate(option) {
        return (
            <Row className="country-item" >

                <Col style={{float:'left'}}>
                    <Badge variant={'danger'} onClick={()=>{

                        this.remove(option);

                    }}>

                        <i className={'fa fa-minus'}> </i>
                    </Badge>
                    <Badge variant={'success'} onClick={()=>{



                    }}>

                        <i className={'fa fa-send'}> </i>
                    </Badge>
                </Col>
           
                <Col>{option.Name}</Col>
               
            </Row>
        );
    }

    render() {
        return (
            <>
                 <span className="p-fluid justify">
                     <label>متنی از پیغام را وارد کنید</label>
                     
                     
                         <div className="p-inputgroup">
                                                               <InputText value={this.state.value1}
                                                                          onChange={(e) => this.setState({value1: e.target.value})}
                                                                          placeholder="پیغام آماده جدید"/>
                                     <span className="p-inputgroup-addon">
                                    
                                              <Badge variant={'primary'} onClick={() => {

                                                  this.save()

                                              }}>
                                        <i className="pi pi-plus"></i>
                                    </Badge>
                                        
                                </span>
                            </div>
              
                     
                   
                  <ListBox value={this.state.selectedCountries} options={this.state.arr}
                           onChange={(e) => this.setState({selectedCountries: e.value})} filter optionLabel="Name"
                           itemTemplate={this.countryTemplate} listStyle={{maxHeight: '250px'}}/>
                 </span>
            </>
        );
    }

    saveReadyPmsCallback(res) {

        _showMsg('با موفقیت ذخیره شد');
        
        this.componentDidMount();
    }

    save() {
        MyCaller.Send('SaveReadyPms', {
            Name: this.state.value1
        });
        
        
        this.setState({value1:''});
    }


    getReadyPmsListCallback(res) {
        if (!res || !res.Content) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content;


        this.setState({arr: arr,mat:Math.random()});
    }

    GetReadyPmsList() {
        _showMsg('در حال خواندن پیغام های آماده');
        MyCaller.Send('GetReadyPmsList')

    }


    removeReadyPmCallback(res){
        _showMsg('با موفقیت حذف شد');


        this.GetReadyPmsList();
    }
    

    remove(op){

        _showMsg('در حال حذف پیغام ');
        MyCaller.Send('RemoveReadyPm',{id:op.Id})
    }
}

export default ReadyPms;