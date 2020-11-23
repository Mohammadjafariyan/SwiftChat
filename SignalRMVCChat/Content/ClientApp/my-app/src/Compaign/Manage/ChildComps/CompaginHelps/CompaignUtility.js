import {Dropdown} from "primereact/dropdown";
import {InputText} from "primereact/inputtext";
import {Calendar} from "primereact/calendar";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {SelectButton} from "primereact/selectbutton";
import {weekdays} from "../../../../Bot/Design/NodeSetting/BotEventCondition";
import {Countries} from "../../../../Components/HelpDesk/Language/Countries";
import React from "react";

const  criteriaStrList=`آدرس ایمیل-
 نام کامل  -
 به کاربران زن یا کاربران مرد  -
 با داده های سفارشی  -
 شماره تلفن -
 زبان -
 با کشور  -
 روز های هفته -
 به کاربران یک یا چند استان  -
  به کاربران یک یا چند شهر-
 عنوان شغلی-
 با آخرین تاریخ فعالیت  -
 تاریخ ایجاد -
 امتیاز داده-
 برچسب ها -
 نام شرکت`;


const criteriaStrEngList=`EmailAddress-
  fullName-
  gender-
  customData-
  phoneNumber-
  language-
  country-
  weekdays-
  region-
  city-
 JobTitle-
  lastActiveDate-
  creationDate-
 providedRating-
  segments-
 CompanyName`;



export function GetCriteriaList(){

    let arr=[];
    let tempArr=criteriaStrList.split('-');
    let tempArr2=criteriaStrEngList.split('-');
    for (let i = 0; i < tempArr.length; i++) {

        if (tempArr[i])
            arr.push({name:tempArr[i],engName:tempArr2[i]});
    }
    return arr;
}



export const MyDropDown = (props) => {

    return <>

        <Dropdown value={props.parent.state.selected[props.name]}
                  options={props.options}
                  onChange={(e) => {
                      props.parent.state.selected[props.name] = e.value;

                      props.parent.setState({mg: Math.random()});
                  }}
                  optionLabel="name" filter showClear
                  filterBy="name"
                  placeholder={props.title}
        />
        <label>{props.title}</label>

    </>
}

export const CompaignInputText = (props) => {

    return <InputText value={props.parent.state.selected['Text']} onChange={(e) => {
        props.parent.state.selected['Text'] = e.target.value;

        props.parent.setState({tm: Math.random()});
    }}/>

}


export const CompaignInputCalendar = (props) => {

    return <Calendar  value={props.parent.state.selected['SelectedDate']} onChange={(e) => {
        props.parent.state.selected['SelectedDate'] = e.target.value;

        props.parent.setState({tm: Math.random()});
    }}/>

}

export const ShowTable = (props) => {
    let list = props.list ? props.list : [];

    return <div className="card">
        <DataTable value={list} paginator
                   paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                   currentPageReportTemplate="Showing {first} to {last} of {totalRecords}" rows={10}
                   rowsPerPageOptions={[10, 20, 50]}
        >
            <Column field="Criteria" header="شاخص"></Column>
            <Column field="CompareType" header="نحوه اعمال"></Column>
            <Column field="Values" header="مقادیر"></Column>
        </DataTable>
    </div>
}
export const CompaignWeekDays = (props) => {
    return <>

        <label>روز های هفته را انتخاب کنید</label>
        <SelectButton value={props.parent.state.selected['CompaignWeekDays']} options={weekdays}
                      onChange={(e) => {
                          this.setState({weekdays: e.value})
                          props.parent.state.selected['CompaignWeekDays']=e.value;
                      }}

                      optionLabel="name" multiple/></>

}


export const CountryDropDown=(props)=>{

    return <>
        <label>انتخاب کشور</label>
        <Dropdown  value={props.parent.state.selected['Countries']} options={Countries} onChange={(e)=> {
            props.parent.state.selected['Countries']=e.value;
            this.setState({sf: Math.random()});

        }}
                   optionLabel="name" filter showClear filterBy="name" placeholder="انتخاب یک کشور و زبان"
                   valueTemplate={(option)=>{
                       return <div className="country-item country-item-value">
                           <img width={50} height={30} alt={option.name} src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                           <div>{option.name} <span  className={'nativeName'}>{option.nativeName!=option.name ? '- '+option.nativeName : ''}</span> </div>
                       </div>
                   }} itemTemplate={(option)=>{
            return <div className="country-item country-item-value">
                <img width={50} height={30} alt={option.name} src={option.flag}  className={`flag flag-${option.alpha2Code.toLowerCase()}`} />
                <div>{option.name} <span  className={'nativeName'}>{option.nativeName!=option.name ? '- '+option.nativeName : ''}</span> </div>
            </div>
        }}/>
    </>
}

export  const CustomData=(props)=>{

    return <>
        <label>{props.title}</label>
        <Chips value={props.parent.state.selected['CustomData']} onChange={(e) => {

            props.parent.state.selected['CustomData']=e.value;
            this.setState({sf: Math.random()});
        }
        }></Chips>
    </>
}

export const Gender=(props)=>{
    const genderList = [
        {label: 'مرد', value: 'man'},
        {label: 'زن', value: 'female'},
    ];
    return <>
        <label>مرد یا زن</label>
        <Dropdown  value={props.parent.state.selected['Gender']} options={genderList} onChange={(e)=> {
            props.parent.state.selected['Gender']=e.value;
            this.setState({sf: Math.random()});

        }}
                   optionLabel="name" filter showClear filterBy="name" placeholder="انتخاب جنسیت"
        />
    </>
}