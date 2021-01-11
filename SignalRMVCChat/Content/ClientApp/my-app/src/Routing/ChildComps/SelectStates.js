import React, {Component} from 'react';
import MyMultiSelect from "./MyMultiSelect";
import {IranCities, IranStates} from "../../Components/Utilities/Utility";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class SelectStates extends MyMultiSelect {
    state = {
        propName: this.props.name ? this.props.name :'States',
        label:'انتخاب استان ها',
        optionLabel:'name'
    }

    componentDidMount() {

        CurrentUserInfo.SelectStates = this;


        this.makeList(IranStates())
    }

    compare(e1, e2) {
        return e1.engName == e2.engName
    }
}

export default SelectStates;