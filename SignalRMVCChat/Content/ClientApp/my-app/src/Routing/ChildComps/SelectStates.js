import React, {Component} from 'react';
import MyMultiSelect from "./MyMultiSelect";
import {CurrentUserInfo} from "../../Help/Socket";
import {IranCities, IranStates} from "../../Components/Utilities/Utility";

class SelectStates extends MyMultiSelect {
    state = {
        propName: 'States'
    }

    componentDidMount() {

        CurrentUserInfo.SelectStates = this;


        this.makeList(IranStates)
    }

    compare(e1, e2) {
        return e1.engName == e2.engName
    }
}

export default SelectStates;