import React, {Component} from 'react';
import {CurrentUserInfo} from "../../Help/Socket";
import {IranStates} from "../../Components/Utilities/Utility";
import {MultiSelect} from "primereact/multiselect";

class MyMultiSelect extends Component {
    state = {
        propName: ''
    }

    componentDidMount() {

        CurrentUserInfo.SelectAdmins = this;


        this.makeList(IranStates)
    }

    compare(e1, e2) {
        return e1.engName == e2.engName
    }

    makeList(arr) {


        let selected = this.props.parent.state.selected[this.state.propName] ?
            this.props.parent.state.selected[this.state.propName] : [];

        let newArr = [];
        for (let i = 0; i < selected.length; i++) {
            let find = arr.find(f => this.compare(f, selected[i]));
            if (find) {
                newArr.push(find);
            }
        }

        this.props.parent.state.selected[this.state.propName] = newArr;

        this.setState({CountryList: arr});
    }

    render() {
        return (
            <>
                <MultiSelect value={this.props.parent.state.selected[this.state.propName]}
                             options={this.state.CountryList}
                             onChange={(e) => {
                               
                                 this.setState({MATH: Math.random()});
                                 this.props.parent.state.selected[this.state.propName] = e.value;

                             }}/>

            </>
        );
    }
}

export default MyMultiSelect;