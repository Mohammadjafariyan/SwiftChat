import {Dropdown} from "primereact/dropdown";
import {InputText} from "primereact/inputtext";
import {Calendar} from "primereact/calendar";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {SelectButton} from "primereact/selectbutton";
import {weekdays} from "../../../../Bot/Design/NodeSetting/BotEventCondition";
import {Countries} from "../../../../Components/HelpDesk/Language/Countries";
import React, {useEffect, useState} from "react";
import Card from "react-bootstrap/Card";
import {Spinner} from "react-bootstrap";
import {Button} from "primereact/button";
import {Dialog} from "primereact/dialog";
import {DataHolder} from "../../../../Help/DataHolder";
import {MyCard, MyFieldset} from "../../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Chips} from "primereact/chips";
import {InputSwitch} from "primereact/inputswitch";
import SelectStates from "../../../../Routing/ChildComps/SelectStates";
import SelectCities from "../../../../Routing/ChildComps/SelectCities";
import SelectSegments from "../../../../Routing/ChildComps/SelectSegments";

const criteriaStrList = `آدرس ایمیل-
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


const criteriaStrEngList = `EmailAddress-
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


export function GetCriteriaList() {

    let arr = [];
    let tempArr = criteriaStrList.split('-');
    let tempArr2 = criteriaStrEngList.split('-');
    for (let i = 0; i < tempArr.length; i++) {

        if (tempArr[i])
            arr.push({name: tempArr[i], engName: tempArr2[i].trim()});
    }
    return arr;
}


export const MyDropDown = (props) => {

    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])
    
    
    return <>

        {props.hasTitle && <> <label>{props.title}</label>
            <br/>
        </>
        }
        <Dropdown style={{width: '100%'}} value={value}
                  options={props.options}
                  onChange={(e) => {
                      props.parent.state.selected[props.name] = e.value;
                      props.parent.state[props.name] = e.value;

                      
                      if (e.value) {

                          props.parent.state.selected[props.name+'InValid']=false;
                      }
                      setValue(e.value)

                      props.parent.setState({mg: Math.random()});
                  }}
                  optionLabel="name" filter showClear
                  filterBy="name"
                  placeholder={props.title}

                  className={ props.parent.state.selected[props.name+'InValid'] ?  "p-invalid p-d-block" : ''}    
         />
        {props.parent.state.selected[props.name+'InValid'] &&  <small  className="p-invalid p-d-block">انتخاب نشده است</small>}


    </>
}
export const CompaignInputSwitch = (props) => {
    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])


    return <>

        <label>{props.title}</label><br/>
        <InputSwitch checked={value} onChange={(e) => {
            setValue(e.value);
            props.parent.state.selected[props.name]=e.value;

        }
        }/>
        
        </>
}
export const CompaignInputText = (props) => {

    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <>

        <label>{props.title}</label><br/>
        <InputText value={value} onChange={(e) => {
            props.parent.state.selected[props.name] = e.target.value;
            setValue(e.target.value);

            props.parent.setState({tm: Math.random()});
        }}/>

    </>

}


export const CompaignInputCalendar = (props) => {
    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <>
        <label>{props.title}</label><br/>
        <Calendar dateFormat="yy/mm/dd" value={value} onChange={(e) => {
            props.parent.state.selected[props.name] = e.target.value;

            setValue(e.target.value);

            props.parent.setState({tm: Math.random()});
        }}/></>


}

export const ShowTable = (props) => {
    let list = props.list ? props.list : [];

    
    

    const selectedCriteria=(row)=>{
        return <span> {row.selectedCriteria.name}</span>
    }
    const selectedFilter=(row)=>{
        return <span> {row.selectedFilter.name}</span>

    }
    const showValue=(row)=>{
        let name;

     

        if (!name){
            name=row[row.selectedCriteria.engName].name ? row[row.selectedCriteria.engName].name: name;
        }

        if (!name){
            name=row[row.selectedCriteria.engName].length  ? ' (انتخاب شده)  '+ row[row.selectedCriteria.engName].length  :null;
        }
        
        if (!name){
            name=row[row.selectedCriteria.engName];
        }

        return <span> {name}</span>
    }

    return <div className="card">
        <DataTable emptyMessage={'رکوردی یافت نشد'}  value={list} paginator
                   paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                   currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}" rows={10}
                   rowsPerPageOptions={[10, 20, 50]}
        >
            
            <Column field="Criteria" header="شاخص" body={selectedCriteria}></Column>
            <Column field="CompareType" header="نحوه اعمال" body={selectedFilter}></Column>
            <Column field="Values" header="مقادیر" body={showValue}></Column>
        </DataTable>
    </div>
}
export const CompaignWeekDays = (props) => {

    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <>

        <label>روز های هفته را انتخاب کنید</label><br/>
        <SelectButton value={value} options={weekdays}
                      onChange={(e) => {
                          props.parent.setState({weekdays: e.value})

                          setValue(e.value);


                          props.parent.state.selected[props.name] = e.value;
                      }}

                      optionLabel="name" multiple/></>

}


export const CountryDropDown = (props) => {
    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <>
        <label>انتخاب کشور</label><br/>
        <Dropdown value={value} options={Countries} onChange={(e) => {
            props.parent.state.selected[props.name] = e.value;

            setValue(e.value);

            props.parent.setState({sf: Math.random()});

        }}
                  optionLabel="name" filter showClear filterBy="name" placeholder="انتخاب یک کشور و زبان"
               />
    </>
}

export const CustomData = (props) => {
    const [value, setValue] = useState([]);

    React.useEffect(() => {
        if (!props.parent.state.selected[props.name]){
            props.parent.state.selected[props.name]=[];
        }
        
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <div style={{width:'100%',overflowX:'auto'}}>
        <label>{props.title}</label><br/>
        <Chips value={value} onChange={(e) => {

            props.parent.state.selected[props.name] = e.value;

            setValue(e.value);

            props.parent.setState({sf: Math.random()});
        }
        }></Chips>
    </div>
}

export const Gender = (props) => {
    const genderList = [
        {name: 'مرد', engName: 'man'},
        {name: 'زن', engName: 'female'},
    ];

    const [value, setValue] = useState();

    React.useEffect(() => {
        setValue(props.parent.state.selected[props.name]);
    }, [props.parent.state.selected[props.name]])

    return <>
        <label>مرد یا زن</label><br/>
        <Dropdown value={value} options={genderList} onChange={(e) => {
            props.parent.state.selected[props.name] = e.value;

            setValue(e.value);

            props.parent.setState({sf: Math.random()});

        }}
                  optionLabel="name" filter showClear filterBy="name" placeholder="انتخاب جنسیت"
        />
    </>
}


export const ShowListOfCards = (props) => {

    if (props.loading) {
        return <>
            {this.state.loading &&
            <Spinner animation="border" role="status">
                <span className="sr-only">لطفا منتظر بمانید ....</span>
            </Spinner>}</>
    }

    if (!props.list || !props.list.length) {
        return <>{props.emptyMessage}</>
    }

    return <>
        {props.list.map((row, i, arr) => {

            return <Card>
                <Card.Body>
                    <Card.Title>{row.Name}</Card.Title>
                    <Card.Subtitle className="mb-2 text-muted">
                        {row.SubTitle}
                    </Card.Subtitle>
                    <Card.Text>

                        {props.body && <>{props.body(row)}</>}
                    </Card.Text>

                    {props.showLinks && <>{props.showLinks(row)}</>}
                    {/*<Card.Link href="#">Card Link</Card.Link>
                    <Card.Link href="#">Another Link</Card.Link>*/}
                </Card.Body>
            </Card>

        })}
    </>
}


export const RowHtml = (props) => {


    return <div dangerouslySetInnerHTML={{__html: props.html}}></div>
}


export const MyConfirm = (props) => {
    const [display, setDisplay] = useState(props.display);

    React.useEffect(() => {
        setDisplay(props.display);
    }, [props.display])

    const onHide = () => {

        setDisplay(false);
        props.parent.setState({displayConfirm: false});
    }

    return <Dialog header="نیاز به تاکید" visible={display} modal
                   style={{width: '350px'}}
                   footer={ <div>
                       <Button label="خیر" icon="pi pi-times" onClick={() => onHide()}
                               className="p-button-text"/>
                       <Button label="بله" icon="pi pi-check" onClick={() => {
                           onHide();

                           props.onConfirm();

                       }} autoFocus/>
                   </div>} onHide={() => onHide()}>
        <div className="confirmation-content">
            <i className="pi pi-exclamation-triangle p-mr-3" style={{fontSize: '2rem'}}/>
            <br/>
            <span>{props.title}</span>
            <hr/>
            {props.body}
        </div>
    </Dialog>
}


export const ShowMessage = (props) => {

    if (!props.show)
        return <></>


    return <Card
        bg={'warning'}
    >
        <Card.Header>{props.header}</Card.Header>
        <Card.Body>
            {/*<Card.Title> Card Title </Card.Title>*/}
            {props.body}

        </Card.Body>
    </Card>;
}


export function GetMonthDays() {
    let days = [];
    for (let i = 1; i <= 30; i++) {
        days.push(i);
    }

    return days;

}