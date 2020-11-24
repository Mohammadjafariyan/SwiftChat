import React, {Component} from 'react';
import CompaignLayout from "../Compaign/CompaignLayout";
import {Help} from "../Compaign/Manage/ChildComps/CompaignEditor";

class CompaignPage extends Component {
    render() {
        return (
            <>
             
             <CompaignLayout></CompaignLayout>   
            </>
        );
    }
}

export default CompaignPage;


/*
* this.state.selected.selectedCriteria
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

Template
        
        IsAutomatic

IsEnabled
compaignType
CompaignConditionsTypeIndex
CompaignRecipientsTypeIndex

        value: _GetSelectedCompaign().Template.Name,
            text1: _GetSelectedCompaign().Template.Html,
            SendToEmail: _GetSelectedCompaign().SendToEmail,
            SendToChat: _GetSelectedCompaign().SendToChat,
            saveAsTemplateName: _GetSelectedCompaign().saveAsTemplateName,
            saveAsTemplate: _GetSelectedCompaign().saveAsTemplate,


       _GetSelectedCompaign()['everyDateTime'] =this.state.everyDateTime;
        _GetSelectedCompaign()['everyWeekTime'] =this.state.everyWeekTime;
        _GetSelectedCompaign()['selectedDayOfEveryMonth']=this.state.selectedDayOfEveryMonth;
        _GetSelectedCompaign()['selectedEventTrigger']=this.state.selectedEventTrigger;
        _GetSelectedCompaign()['selectedBot']=this.state.selectedBot;

compaignType
----------
GetCompaignTemplates
GetCompaignList
CompaignSave
DeleteCompaign
SetIsEnabledCompaign
CompaignSave
SearchCustomers*/