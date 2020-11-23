import React, {Component} from 'react';
import {CurrentUserInfo, MyCaller} from "../../../../Help/Socket";
import {_GetSelectedCompaign} from "../../CompaignSave";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {Dropdown} from "primereact/dropdown";
import {
    CompaignInputCalendar,
    CompaignInputText,
    CompaignWeekDays,
    CountryDropDown,
    CustomData,
    Gender,
    GetCriteriaList, MyDropDown, ShowTable
} from "./CompaignUtility";
import {InputText} from "primereact/inputtext";
import SelectSegments from "../../../../Routing/ChildComps/SelectSegments";
import {Calendar} from "primereact/calendar";
import SelectStates from "../../../../Routing/ChildComps/SelectStates";
import {SelectButton} from "primereact/selectbutton";
import {weekdays} from "../../../../Bot/Design/NodeSetting/BotEventCondition";
import {Countries} from "../../../../Components/HelpDesk/Language/Countries";
import SelectCities from "../../../../Routing/ChildComps/SelectCities";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SelectAdmins from "../../../../Routing/ChildComps/SelectAdmins";
import {MyCard, MyFieldset} from "../../../../Routing/Manage/RoutingSave";
import Button from "react-bootstrap/cjs/Button";

class CompaignFilter extends Component {
    state = {
        searchTerm: '',
        arr: [],
        selectedCriteria: null
    }

    criteriaList = GetCriteriaList();

    applytypeList=[
        {name:'برابر'},
        {name:'شامل'},
        {name:'نابرابر'},
    ]
    constructor(props) {
        super(props);
        CurrentUserInfo.CompaignFilter = this;
    }

    componentDidMount() {
        let filters = _GetSelectedCompaign().Filters;

        filters = filters ? filters : [];

        this.setState({filters: filters})

    }


    render() {
        return (
            <div>


                <MyFieldset title="انتخاب شاخص و نحوه اعمال">
                    <Row>
                        <Col>
                            <MyCard header={'انتخاب روش فیلتر کردن کاربران'} title={'به چه روشی کاربران انتخاب و ایمیل یا پیغام ارسال شود'}>
                                <MyDropDown parent={this} title={'شاخص'} name={'selectedCriteria'} list={this.criteriaList}/>
                                <MyDropDown parent={this} title={'نحوه اعمال'} name={'selectedFilter'} list={this.applytypeList}/>
                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>
        


                <MyFieldset title="تنظیمات یا پیکربندی شاخص">
                    <Row>
                        <Col>
                            <MyCard header={'مقادیر رفتار سیستم در شاخص انتخاب شده'} >
                                {this.ShowSwich()}
                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>

                <hr/>
                
                <Button onClick={()=>{
                    this.addNewFilter();
                }}>
                    
                    افزودن
                </Button>

                <hr/>
                <ShowTable parent={this} list={filters}/>
            </div>
        );
    }

    ShowSwich() {
        if (!this.state.selectedCriteria) {
            return <></>
        }


        switch (this.state.selectedCriteria.engName) {
            case 'EmailAddress':
              return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'fullName':
                return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'gender':
                return <Gender title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'customData':
                return <CustomData title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'phoneNumber':
                return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'language':
                return <CountryDropDown parent={this}/>
                break;
            case 'country':
                return <CountryDropDown parent={this}/>

                break;
                
            case 'weekdays':
               return <CompaignWeekDays parent={this}/>
                break;
            case 'region':
              return  <SelectStates parent={this}/>
                break;
            case 'city':
                return  <SelectCities parent={this}/>

                break;
            case 'JobName':
                return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'JobTitle':
                return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'lastActiveDate':
                return   <CompaignInputCalendar title={this.state.selectedCriteria.name} parent={this}/>
                break;
            case 'creationDate':
                return   <CompaignInputCalendar title={this.state.selectedCriteria.name} parent={this}/>
                
                break;
            case 'providedRating':
                break;
            case 'segments':
                return <SelectSegments/>
                break;
            case 'CompanyName':
                return   <CompaignInputText title={this.state.selectedCriteria.name} parent={this}/>
                break;

        }
    }

    addNewFilter() {
        /*this.state.selected.selectedCriteria
        this.state.selected.selectedFilter
        this.state.selected.Text
        this.state.selected.Gender
        this.state.selected.CustomData
        this.state.selected.Countries
        this.state.selected.CompaignWeekDays
        this.state.selected.States
        this.state.selected.Cities
        this.state.selected.SelectedDate
        this.state.selected.segments*/
        
        let filters=filters ? filters:[];

        filters.push(this.state.selected);
        
        this.setState({
            filters:filters,
            selected:{}
        })
    }
}

export default CompaignFilter;

