import React, {Component} from 'react';
import {AutoComplete} from "primereact/autocomplete";
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";

class SelectAdmin extends Component {
    state = {
        selectedAdmins: []
    };

    constructor(props) {
        super(props);
        this.searchCountry = this.searchCountry.bind(this);
        this.itemTemplate = this.itemTemplate.bind(this);

        CurrentUserInfo.SelectAdmin = this;
    }

    GetAdminsListCallback(res) {
        if (!res || !res.Content || !res.Content.EntityList || !res.Content.EntityList.length) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content.EntityList;


        this.setState({arr: arr, selectedAdmins: arr});
    }

    componentDidMount() {

        MyCaller.Send('GetAdminsList')
    }

    searchCountry(event) {
        if (!this.state.arr || !this.state.arr.length) {
            return;
        }


        setTimeout(() => {
            let filteredCountries;
            if (!event || !event.query || !event.query.trim().length) {
                filteredCountries = this.state.arr.filter((country) => country.Name);
                ;
            } else {
                filteredCountries = this.state.arr.filter((country) => {
                    return country.Name.toLowerCase().startsWith(event.query.toLowerCase());
                });
            }

            this.setState({filteredCountries, mg: Math.random()});
        }, 250);
    }

    itemTemplate(item) {
        return (
            <div className="country-item">
                <div>{item.Name}</div>
            </div>
        );
    }

    render() {
        return (
            <>
                 <span className="p-fluid">
                     
                     
                     {(!this.state.selectedAdmins || !this.state.selectedAdmins.length) &&
                     <label>نام ادمین های مورد نظر را وارد کنید</label>}

                     {(this.state.selectedAdmins && this.state.selectedAdmins.length) &&
                     <label>ادمین های انتخاب شده موقع ارسال پیغام اعمال خواهد شد</label>}
                     
                     
                     <AutoComplete dropdown={true} value={this.state.selectedAdmins}
                                   suggestions={this.state.filteredCountries}
                                   completeMethod={this.searchCountry}
                                   field="Name" multiple onChange={(e) => this.setState({selectedAdmins: e.value})}/>
                </span>
            </>
        );
    }
}

export default SelectAdmin;