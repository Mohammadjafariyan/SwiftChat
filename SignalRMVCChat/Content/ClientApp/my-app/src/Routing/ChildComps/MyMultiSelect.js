import React, {Component} from 'react';
import {CurrentUserInfo} from "../../Help/Socket";
import {IranStates} from "../../Components/Utilities/Utility";
import {MultiSelect} from "primereact/multiselect";

class MyMultiSelect extends Component {
    state = {
        propName: '',
        CountryList:[]
    }

    componentDidMount() {

        CurrentUserInfo.SelectAdmins = this;


        this.makeList(IranStates)
    }

    compare(e1, e2) {
        return e1.engName == e2.engName
    }

    makeList(arr) {

        if (!arr){
            arr=[];
        }

        console.log('makeList',arr);

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
        if (!this.props.parent || !this.props.parent.state ||
            !this.props.parent.state.selected
            || !this.props.parent.state.selected[this.state.propName])
            return <></>

        return (
            <>
                
                {this.state.CountryList && 
                <MultiSelect  emptyFilterMessage={'هیچ رکوردی یافت نشد'} style={{width:'100%'}} filter  optionLabel={this.state.optionLabel} value={this.props.parent.state.selected[this.state.propName]}
                             options={this.state.CountryList}
                             selectedItemsLabel={'بتعداد {0} آیتم انتخاب شده'}
                             name={this.state.label}
                             tooltip={this.state.label}
                             onChange={(e) => {

                                 this.setState({MATH: Math.random()});
                                 this.props.parent.state.selected[this.state.propName] = e.value;

                             }}/>
                }
                <label>{this.state.label}</label>
            </>
        );
    }
}

export default MyMultiSelect;