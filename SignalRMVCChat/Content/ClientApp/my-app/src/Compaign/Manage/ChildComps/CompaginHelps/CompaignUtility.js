import { Dropdown } from "primereact/dropdown";
import { InputText } from "primereact/inputtext";
import { Calendar } from "primereact/calendar";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { SelectButton } from "primereact/selectbutton";
import { weekdays } from "../../../../Bot/Design/NodeSetting/BotEventCondition";
import { Countries } from "../../../../Components/HelpDesk/Language/Countries";
import React, { Component, useEffect, useState } from "react";
import Card from "react-bootstrap/Card";
import { Badge, Carousel, Spinner } from "react-bootstrap";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { DataHolder } from "../../../../Help/DataHolder";
import { MyCard, MyFieldset } from "../../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { Chips } from "primereact/chips";
import { InputSwitch } from "primereact/inputswitch";
import SelectStates from "../../../../Routing/ChildComps/SelectStates";
import SelectCities from "../../../../Routing/ChildComps/SelectCities";
import SelectSegments from "../../../../Routing/ChildComps/SelectSegments";
import { CurrentUserInfo, MyCaller } from "../../../../Help/Socket";
import { Ripple } from "primereact/ripple";


import './helpStyle.css'

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
  let tempArr = criteriaStrList.split("-");
  let tempArr2 = criteriaStrEngList.split("-");
  for (let i = 0; i < tempArr.length; i++) {
    if (tempArr[i]) arr.push({ name: tempArr[i], engName: tempArr2[i].trim() });
  }
  return arr;
}

export const MyDropDown = (props) => {
  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      {props.hasTitle && (
        <>
          {" "}
          <label>{props.title}</label>
          <br />
        </>
      )}
      <Dropdown
        style={{ width: "100%" }}
        value={value}
        options={props.options}
        onChange={(e) => {
          props.parent.state.selected[props.name] = e.value;
          props.parent.state[props.name] = e.value;

          if (e.value) {
            props.parent.state.selected[props.name + "InValid"] = false;
          }
          setValue(e.value);

          props.parent.setState({ mg: Math.random() });
        }}
        optionLabel="name"
        filter
        showClear
        filterBy="name"
        placeholder={props.title}
        className={
          props.parent.state.selected[props.name + "InValid"]
            ? "p-invalid p-d-block"
            : ""
        }
      />
      {props.parent.state.selected[props.name + "InValid"] && (
        <small className="p-invalid p-d-block">انتخاب نشده است</small>
      )}
    </>
  );
};
export const CompaignInputSwitch = (props) => {
  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      <label>{props.title}</label>
      <br />
      <InputSwitch
        checked={value}
        onChange={(e) => {
          setValue(e.value);
          props.parent.state.selected[props.name] = e.value;
        }}
      />
    </>
  );
};
export const CompaignInputText = (props) => {
  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      <label>{props.title}</label>
      <br />
      <InputText
        value={value}
        onChange={(e) => {
          props.parent.state.selected[props.name] = e.target.value;
          setValue(e.target.value);

          props.parent.setState({ tm: Math.random() });
        }}
      />
    </>
  );
};

export const CompaignInputCalendar = (props) => {
  let prevalue=props.parent.state.selected[props.name]
      ? new Date(props.parent.state.selected[props.name])
      : null;
  const [value, setValue] = useState(
    prevalue
  );

  /*  React.useEffect(() => {
        
        if (props.parent.state.selected[props.name]){
            setValue(new Date(props.parent.state.selected[props.name]));
            
        }
    }, [props.parent.state.selected[props.name]])*/

  return (
    <>
      <label>{props.title}</label>
      <br />
      <Calendar
        dateFormat="yy/mm/dd"
        value={value}
        onChange={(e) => {
          let day = e.value.getDate();
          let mont = e.value.getMonth();
          let year = e.value.getFullYear();

          let hour = e.value.getHours();
          let min = e.value.getMinutes();
          let sec = e.value.getSeconds();

          props.parent.state.selected[
            props.name
          ] = `${day}/${mont}/${year} ${hour}:${min}:${sec}`;

          setValue(e.value);

          props.parent.setState({ tm: Math.random() });
        }}
      />
    </>
  );
};

export const ShowTable = (props) => {
  let list = props.list ? props.list : [];

  const selectedCriteria = (row) => {
    return <span> {row.selectedCriteria.name}</span>;
  };
  const selectedFilter = (row) => {
    return <span> {row.selectedFilter.name}</span>;
  };
  const showValue = (row) => {
    let name = "";

    try {
      switch (row.selectedCriteria.engName.toString().trim()) {
        case "EmailAddress":
          name = row[row.selectedCriteria.engName];

        case "fullName":
          name = row[row.selectedCriteria.engName];

        case "gender":
          name =
            row[row.selectedCriteria.engName].name ||
            row[row.selectedCriteria.engName].Name ||
            row[row.selectedCriteria.engName];

          break;
        case "customData":
          name = row["customData"] + "<=>" + row["CustomDataValue"];

          /*return <CustomData title={this.state.selectedCriteria.name} name={this.state.selectedCriteria.engName}
                                      parent={this}/>*/
          break;
        case "phoneNumber":
          name = row[row.selectedCriteria.engName];
          break;
        case "language":
          name =
            row[row.selectedCriteria.engName].Name ||
            row[row.selectedCriteria.engName].name;

          break;
        case "country":
          name =
            row[row.selectedCriteria.engName].Name ||
            row[row.selectedCriteria.engName].name;

          break;

        case "weekdays":
          if (
            row[row.selectedCriteria.engName] &&
            row[row.selectedCriteria.engName].length > 0
          )
            for (let i = 0; i < row[row.selectedCriteria.engName].length; i++) {
              name += row[row.selectedCriteria.engName][i].name + "-";
            }
          break;
        case "region":
          if (
            row[row.selectedCriteria.engName] &&
            row[row.selectedCriteria.engName].length > 0
          )
            for (let i = 0; i < row[row.selectedCriteria.engName].length; i++) {
              name += row[row.selectedCriteria.engName][i].name + "-";
            }
          break;
        case "city":
          if (
            row[row.selectedCriteria.engName] &&
            row[row.selectedCriteria.engName].length > 0
          )
            for (let i = 0; i < row[row.selectedCriteria.engName].length; i++) {
              name += row[row.selectedCriteria.engName][i].name + "-";
            }

          break;
        case "JobName":
          name = row[row.selectedCriteria.engName];
          break;
        case "JobTitle":
          name = row[row.selectedCriteria.engName];
          break;
        case "lastActiveDate":
          name = row[row.selectedCriteria.engName];

          break;
        case "creationDate":
          name = row[row.selectedCriteria.engName];

          break;
        case "providedRating":
          name = row[row.selectedCriteria.engName] ? "بله" : "خیر";

          break;
        case "segments":
          if (
            row[row.selectedCriteria.engName] &&
            row[row.selectedCriteria.engName].length > 0
          )
            for (let i = 0; i < row[row.selectedCriteria.engName].length; i++) {
              name += row[row.selectedCriteria.engName][i].Name;
            }
          break;
        case "CompanyName":
          name =
            row[row.selectedCriteria.engName].name ||
            row[row.selectedCriteria.engName].Name ||
            row[row.selectedCriteria.engName];

          break;
      }
    } catch (e) {}

    console.log(
      row.selectedCriteria.engName,
      row[row.selectedCriteria.engName]
    );
    console.log(row.selectedCriteria.engName, row);

    return <span> {name}</span>;
  };

  return (
    <div className="card">
      <DataTable
        emptyMessage={"رکوردی یافت نشد"}
        value={list}
        paginator
        paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
        currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
        rows={10}
        rowsPerPageOptions={[10, 20, 50]}
      >
        <Column field="Criteria" header="شاخص" body={selectedCriteria}></Column>
        <Column
          field="CompareType"
          header="نحوه اعمال"
          body={selectedFilter}
        ></Column>
        <Column field="Values" header="مقادیر" body={showValue}></Column>
      </DataTable>
    </div>
  );
};
export const CompaignWeekDays = (props) => {
  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      <label>روز های هفته را انتخاب کنید</label>
      <br />
      <SelectButton
        value={value}
        options={weekdays}
        onChange={(e) => {
          props.parent.setState({ weekdays: e.value });

          setValue(e.value);

          props.parent.state.selected[props.name] = e.value;
        }}
        optionLabel="name"
        multiple
      />
    </>
  );
};

export const CountryDropDown = (props) => {
  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      <label>انتخاب کشور</label>
      <br />
      <Dropdown
        value={value}
        options={Countries}
        onChange={(e) => {
          props.parent.state.selected[props.name] = e.value;

          setValue(e.value);

          props.parent.setState({ sf: Math.random() });
        }}
        optionLabel="name"
        filter
        showClear
        filterBy="name"
        placeholder="انتخاب یک کشور و زبان"
      />
    </>
  );
};

export const CustomData = (props) => {
  const [value, setValue] = useState([]);

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <div style={{ width: "100%", overflowX: "auto" }}>
      <label>{props.title}</label>
      <br />
      <Chips
        value={value}
        onChange={(e) => {
          props.parent.state.selected[props.name] = e.value;

          setValue(e.value);

          props.parent.setState({ sf: Math.random() });
        }}
      ></Chips>
    </div>
  );
};

export const Gender = (props) => {
  const genderList = [
    { name: "مرد", engName: "man" },
    { name: "زن", engName: "female" },
  ];

  const [value, setValue] = useState();

  React.useEffect(() => {
    setValue(props.parent.state.selected[props.name]);
  }, [props.parent.state.selected[props.name]]);

  return (
    <>
      <label>مرد یا زن</label>
      <br />
      <Dropdown
        value={value}
        options={genderList}
        onChange={(e) => {
          props.parent.state.selected[props.name] = e.value;

          setValue(e.value);

          props.parent.setState({ sf: Math.random() });
        }}
        optionLabel="name"
        filter
        showClear
        filterBy="name"
        placeholder="انتخاب جنسیت"
      />
    </>
  );
};

export const ShowListOfCards = (props) => {
  const [index, setIndex] = useState(0);
  
  
  if (props.loading) {
    return (
      <>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">لطفا منتظر بمانید ....</span>
          </Spinner>
        )}
      </>
    );
  }


  const handleSelect = (selectedIndex, e) => {
    setIndex(selectedIndex);
  };

  if (!props.list || !props.list.length) {
    return <>{props.emptyMessage}</>;
  }
  const productTemplate = (row) => {
    return (
          <Card
            style={{
              overflow: "hidden",
              height: "300px",
            }}

            className={
                'text-black'
            }
          >
            <Card.Body>
              <div
                className="overContent"
                style={{
                  position: "absolute",
                  width: "100%",
                  height: "100%",
                }}
              ></div>

              <Badge variant="info">{row.Name}</Badge>

              <Card.Subtitle className="mb-2 text-muted">
                {row.SubTitle}
              </Card.Subtitle>
              <Card.Text>{props.body && <>{props.body(row)}</>}</Card.Text>

              {/*<Card.Link href="#">Card Link</Card.Link>
    <Card.Link href="#">Another Link</Card.Link>*/}
            </Card.Body>
          </Card>
    );
  };

  return (
    <Carousel activeIndex={index} onSelect={handleSelect}>
      {props.list.map((row, i) => {
        return (
          <Carousel.Item>
            {productTemplate(row)}
            <Carousel.Caption className='bg-secondary '>
              {props.showLinks && <>{props.showLinks(row)}</>}
            </Carousel.Caption>
          </Carousel.Item>
        );
      })}
    </Carousel>
  );
};

export const RowHtml = (props) => {
  return <div dangerouslySetInnerHTML={{ __html: props.html }}></div>;
};

export const MyConfirm = (props) => {
  const [display, setDisplay] = useState(props.display);

  React.useEffect(() => {
    setDisplay(props.display);
  }, [props.display]);

  const onHide = () => {
    setDisplay(false);
    props.parent.setState({ displayConfirm: false });
  };

  return (
    <Dialog
      header="نیاز به تاکید"
      visible={display}
      modal
      style={{ width: "350px" }}
      footer={
        <div>
          <Button
            label="خیر"
            icon="pi pi-times"
            onClick={() => onHide()}
            className="p-button-text"
          />
          <Button
            label="بله"
            icon="pi pi-check"
            onClick={() => {
              onHide();

              props.onConfirm();
            }}
            autoFocus
          />
        </div>
      }
      onHide={() => onHide()}
    >
      <div className="confirmation-content">
        <i
          className="pi pi-exclamation-triangle p-mr-3"
          style={{ fontSize: "2rem" }}
        />
        <br />
        <span>{props.title}</span>
        <hr />
        {props.body}
      </div>
    </Dialog>
  );
};

export const ShowMessage = (props) => {
  if (!props.show) return <></>;

  return (
    <Card bg={"warning"}>
      <Card.Header>{props.header}</Card.Header>
      <Card.Body>
        {/*<Card.Title> Card Title </Card.Title>*/}
        {props.body}
      </Card.Body>
    </Card>
  );
};

export function GetMonthDays() {
  let days = [];
  for (let i = 1; i <= 30; i++) {
    days.push(i);
  }

  return days;
}

export const _GoCompaignTable = (dataType, title) => {
  CurrentUserInfo.CompaignLayout.setState({
    CompaignChildPage: "CompaignTable",
    dataType: dataType,
    title: title,
  }); //<CompaignTable currentStep={4}/>
};



export const _GoCompaignSubRoute = (routeName, title) => {
  CurrentUserInfo.CompaignLayout.setState({
    CompaignChildPage: routeName,
    title: title,
  }); //<CompaignTable currentStep={4}/>
};

export const _GoCompaignDefine = () => {
  CurrentUserInfo.CompaignLayout.setState({
    CompaignChildPage: null,
    dataType: null,
    title: null,
  }); //<CompaignTable currentStep={4}/>
  
};

export const _GoCompaignAnalicts = (title) => {
  CurrentUserInfo.CompaignLayout.setState({
    CompaignChildPage: "CompaignAnaylics",
    title: title,
  }); //<CompaignTable currentStep={4}/>
};

export function _TakeLazyTable(res, parent) {
  parent.setState({ loading: false });
  if (!res || !res.Content) {
    return;
  }

  parent.setState({
    list: res.Content.EntityList,
    totalRecords: res.Content.Total,
    first: res.Content.First,
  });
}

export class MyLazyTable extends Component {
  state = {
    list: [],
  };

  constructor(props) {
    super(props);

    this.actionBody = this.actionBody.bind(this);
    this.onPage = this.onPage.bind(this);
  }

  get(rows, first) {
    this.setState({ loading: true });

    let data = this.state.CustomData;

    data.rows = rows;
    data.first = first;

    this.setState({ loading: true });
    MyCaller.Send(this.state.getUrl, data);
  }

  actionBody(row) {
    return <></>;
  }

  componentDidMount() {
    CurrentUserInfo.MyLazyTable = this;
    this.get(10);
  }

  /*getCompaignLogReceiverListCallback*/
  getCallback(res) {
    _TakeLazyTable(res, this);
  }

  onPage(event) {
    //imitate delay of a backend call
    const { first, rows } = event;

    this.setState({
      first:first
    })
    this.get(rows,first );
  }
  render() {
    return (
      <div>
        {this.state.loading && (
          <Spinner animation="border" role="status">
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}

        <MyFieldset title={this.props.title ?this.props.title : "انتخاب ربات تعریف شده"}>
          <Row>
            <Col>
              <MyCard
                header={this.props.header ?this.props.header :"انتخاب یک ربات"}
                title={this.props.title ?this.props.title :  "وقتی ربات اجرا شد ، کمپین اجرا شود"}
              >

              {this.state.list && 
                <DataTable
                  value={this.state.list}
                  paginator
                  emptyMessage={"هیچ اطلاعاتی یافت نشد"}
                  paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                  currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}"
                  rows={10}
                  rowsPerPageOptions={[10, 20, 50]}
                  totalRecords={this.state.totalRecords}
                  lazy
                  first={this.state.first}
                  onPage={this.onPage}
                  loading={this.state.loading}
                >
                  {this.showColumns()}
                </DataTable>
              }
              </MyCard>
            </Col>
          </Row>
        </MyFieldset>
      </div>
    );
  }

  showColumns() {
    return (
      <>
        <Column field="CompaignName" header="عنوان"></Column>
        <Column field="ExecutionDateTimeStr" header="تاریخ اجرا"></Column>
        <Column field="StoppedLog" header="لاگ خطای توقف"></Column>
        <Column field="ReceiverCount" header="تعداد دریافت کننده"></Column>
        <Column field="DeliverCount" header="تعداد تحویل گیرنده"></Column>
        <Column
          field="ProgressPercent"
          header="عملیات"
          body={this.actionBody}
        ></Column>
      </>
    );
  }
}

export function _SelectCustomerForChat(user) {
  DataHolder.currentPage = null;
  CurrentUserInfo.LayoutPage.setState({
    temp: Math.random(),
  });

  MyCaller.Send("selectCustomerForChat", {
    customerId: user.Id,
  });
}
