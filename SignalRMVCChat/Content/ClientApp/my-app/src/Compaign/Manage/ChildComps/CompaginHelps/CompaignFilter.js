import React, {Component} from 'react';
import {MyCaller} from "../../../../Help/Socket";
import {_GetSelectedCompaign} from "../../CompaignSave";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {Dropdown} from "primereact/dropdown";
import {
    CompaignInputCalendar, CompaignInputSwitch,
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
import {CurrentUserInfo} from "../../../../CurrentUserInfo";

class CompaignFilter extends Component {
    criteriaList = GetCriteriaList();

    applytypeList = [
        {name: 'برابر',engName:'equals'},
        {name: 'شامل',engName:'include'},
        {name: 'نابرابر',engName:'notEquals'},
    ]
state={
    searchTerm: '',
    arr: [],
    selectedCriteria: null,
    selectedFilter: this.applytypeList[0],
    selected: {
    }
};

    init() {
        this.setState( {
            searchTerm: '',
            arr: [],
            selectedCriteria: null,
            selectedFilter: this.applytypeList[0],
            selected: {
            }
        });
    }

    constructor(props) {
        super(props);
        CurrentUserInfo.CompaignFilter = this;
    }

    componentDidMount() {

        this.init();
        let filters = _GetSelectedCompaign()['filters'];

        filters = filters ? filters : [];

        this.setState({filters: filters})

    }


    componentWillUnmount() {

        _GetSelectedCompaign()['filters'] = this.state.filters;
    }


    render() {
        return (
            <div>


                <MyFieldset title="انتخاب شاخص و نحوه اعمال">
                    <Row>
                        <Col>
                            <MyCard header={'انتخاب روش فیلتر کردن کاربران'}
                                    title={'به چه روشی کاربران انتخاب و ایمیل یا پیغام ارسال شود'}>

                                <Row>
                                    <Col>
                                        <MyDropDown parent={this} title={'نحوه اعمال'} name={'selectedFilter'}
                                                    options={this.applytypeList}/>

                                    </Col>
                                    <Col>
                                        <MyDropDown parent={this} title={'شاخص'} name={'selectedCriteria'}
                                                    options={this.criteriaList}/>

                                    </Col>
                                </Row>

                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>


                <MyFieldset title="تنظیمات یا پیکربندی شاخص">
                    <Row>
                        <Col md={12}>
                            <MyCard header={'مقادیر رفتار سیستم در شاخص انتخاب شده'}>
                                {this.ShowSwich()}

                                <hr/>
                                <Button variant={'info'} onClick={() => {
                                    this.addNewFilter();
                                }}>

                                    افزودن
                                </Button>
                            </MyCard>
                        </Col>
                        
                        
                       
                    </Row>
                    
                </MyFieldset>


              

                <hr/>
                <ShowTable parent={this} list={this.state.filters}/>

                

            </div>
        );
    }

    ShowSwich() {
        if (!this.state.selectedCriteria) {
            return <></>
        }

        console.log('this.state.selectedCriteria.engName==>', this.state.selectedCriteria.engName)

        switch (this.state.selectedCriteria.engName.toString().trim()) {
            case 'EmailAddress':
                return <CompaignInputText title={this.state.selectedCriteria.name}
                                          name={this.state.selectedCriteria.engName} parent={this}/>
                break;
            case 'fullName':
                return <CompaignInputText title={this.state.selectedCriteria.name}
                                          name={this.state.selectedCriteria.engName} parent={this}/>
                break;
            case 'gender':
                return <Gender title={this.state.selectedCriteria.name} name={this.state.selectedCriteria.engName}
                               parent={this}/>
                break;
            case 'customData':
                return <Row>
                    <Col>
                        <CompaignInputText title={'نام داده سفارشی'}
                                           name={this.state.selectedCriteria.engName} parent={this}/>
                    </Col>
                    <Col>
                        <CompaignInputText title={'مقدار داده سفارشی'}
                                           name={"CustomDataValue"} parent={this}/>
                    </Col>

            
        </Row>
                /*return <CustomData title={this.state.selectedCriteria.name} name={this.state.selectedCriteria.engName}
                                   parent={this}/>*/
                break;
            case 'phoneNumber':
                return <CompaignInputText title={this.state.selectedCriteria.name}
                                          name={this.state.selectedCriteria.engName} parent={this}/>
                break;
            case 'language':
                return <CountryDropDown parent={this} name={this.state.selectedCriteria.engName}/>
                break;
            case 'country':
                return <CountryDropDown parent={this} name={this.state.selectedCriteria.engName}/>

                break;

            case 'weekdays':
                return <CompaignWeekDays parent={this} name={this.state.selectedCriteria.engName}/>
                break;
            case 'region':
                return <SelectStates parent={this} name={this.state.selectedCriteria.engName}/>
                break;
            case 'city':
                return <SelectCities parent={this} name={this.state.selectedCriteria.engName}/>

                break;
            case 'JobName':
                return <CompaignInputText title={this.state.selectedCriteria.name} parent={this}
                                          name={this.state.selectedCriteria.engName}/>
                break;
            case 'JobTitle':
                return <CompaignInputText title={this.state.selectedCriteria.name} parent={this}
                                          name={this.state.selectedCriteria.engName}/>
                break;
            case 'lastActiveDate':
                return <CompaignInputCalendar title={this.state.selectedCriteria.name} parent={this}
                                              name={this.state.selectedCriteria.engName}/>
                break;
            case 'creationDate':
                return <CompaignInputCalendar title={this.state.selectedCriteria.name} parent={this}
                                              name={this.state.selectedCriteria.engName}/>

                break;
            case 'providedRating':
                return <CompaignInputSwitch name={this.state.selectedCriteria.engName}
                                            title={this.state.selectedCriteria.name} parent={this}/>

                break;
            case 'segments':
                return <SelectSegments name={this.state.selectedCriteria.engName} parent={this}/>
                break;
            case 'CompanyName':
                return <CompaignInputText title={this.state.selectedCriteria.name} parent={this}
                                          name={this.state.selectedCriteria.engName}/>
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
        this.state.selected.segments
        
        IsAutomatic
IsEnabled
----------
GetCompaignTemplates
GetCompaignList
CompaignSave
DeleteCompaign
SetIsEnabledCompaign
CompaignSave p-invalid p-d-block
SearchCustomers*/

        let filters = _GetSelectedCompaign()['filters'];

        filters = filters ? filters : [];

        let hasError=false;

        if (!this.state.selected.selectedCriteria){
            this.state.selected.selectedCriteriaInValid=true;
            this.setState({gm:Math.random()});
            hasError=true;
        }
        
        if (!this.state.selected.selectedFilter){
            this.state.selected.selectedFilterInValid=true;
            this.setState({gm:Math.random()});
            hasError=true;
        }
        
        if (hasError){
            return ;
        }
        
        filters.push(this.state.selected);

        _GetSelectedCompaign()['filters']=filters;
        this.setState({filters:filters})

        
        this.init();



    }
}

export default CompaignFilter;

