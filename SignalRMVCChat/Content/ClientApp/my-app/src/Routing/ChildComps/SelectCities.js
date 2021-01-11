import React, {Component} from 'react';
import MyMultiSelect from "./MyMultiSelect";
import {IranCities, IranStates} from "../../Components/Utilities/Utility";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class SelectCities extends  MyMultiSelect {
    state = {
        propName: this.props.name ? this.props.name: 'Cities',
        label:'انتخاب شهر ها',
        optionLabel:'name'
    }

    componentDidMount() {

        CurrentUserInfo.SelectCities = this;


        this.makeList(IranCities())
    }

    compare(e1, e2) {
        return e1.engName == e2.engName
    }
}

export default SelectCities;