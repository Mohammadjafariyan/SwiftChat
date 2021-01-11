import React, {Component} from 'react';
import Card from "react-bootstrap/Card";
import {AddLocalizedMessage, MySwitcher} from "../../Utilities/Utility";
import Table from "react-bootstrap/Table";
import Badge from "react-bootstrap/Badge";
import {_showError} from "../../../Pages/LayoutPage";
import {SetEventTrigger} from "../EventTriggerForm";
import {DataHolder} from "../../../Help/DataHolder";
import {CurrentUserInfo} from "../../../CurrentUserInfo";

class EtFormTriggerBehaviours extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormTriggerBehaviours=this;
        
    }

    setExecuteOnlyIfOnline(ExecuteOnlyIfOnline){
        this.setState({ExecuteOnlyIfOnline})
    }


    setExecuteOnlyIfFirstTimeVisit(ExecuteOnlyIfFirstTimeVisit){
        this.setState({ExecuteOnlyIfFirstTimeVisit})
    }

    setExecuteOnlyIfNoOtherTriggerFired(ExecuteOnlyIfNoOtherTriggerFired){
        this.setState({ExecuteOnlyIfNoOtherTriggerFired})
    }

    setExecuteOnlyIfFromACountry(ExecuteOnlyIfFromACountry){
        this.setState({ExecuteOnlyIfFromACountry})
    }


    getExecuteOnlyIfOnline(){
        return this.state.ExecuteOnlyIfOnline;
    }


    getExecuteOnlyIfFirstTimeVisit(){
        return this.state.ExecuteOnlyIfFirstTimeVisit;
    }

    getExecuteOnlyIfNoOtherTriggerFired(){
        return this.state.  ExecuteOnlyIfNoOtherTriggerFired
    }

    getExecuteOnlyIfFromACountry(){
        return this.state.ExecuteOnlyIfFromACountry
    }

    getCounties(){
     return    this.state.countries;
    }
    setCounties(countries){
        this.setState({countries})
    }
    
    isValid(){
        if (DataHolder.selectedEventTrigger.ExecuteOnlyIfFromACountry && (!DataHolder.selectedEventTrigger.countries || DataHolder.selectedEventTrigger.countries.length==0))
        {
            _showError('درصورت انتخاب فیلتر کشور ، باید یک کشور را انتخاب کنید')
            return false
        }
        return true;
    }

    componentDidMount() {
        SetEventTrigger(DataHolder.selectedEventTrigger);
    }
    render() {
        return (
            <div>

                <Card
                    bg={'light'}
                >
                    <Card.Header>
                        <span className={'float-right'}>
                          Event Trigger شروط انجام این   
                        </span>

                        <small className={'float-left '} style={{color:'#6c757d'}}>
                            Event Trigger      تنظیمات مربوط به رفتار 
                        </small>

                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MySwitcher  title={'وقتی اجرا کن که حداقل یک ادمین آنلاین باشد'} 
                                        name={'ExecuteOnlyIfOnline'} parent={this}>


                            </MySwitcher>

                            <hr/>
                            <MySwitcher title={'فقط در اولین بازدید اجرا شود'} name={'ExecuteOnlyIfFirstTimeVisit'} parent={this}>

                            </MySwitcher>
                            <hr/>

                            <MySwitcher title={'اگر رویداد دیگری استفاده نشده باشد'} name={'ExecuteOnlyIfNoOtherTriggerFired'} parent={this}>

                            </MySwitcher>


                            <hr/>
                            <MySwitcher title={'اگر کاربر از کشور خاصی باشد'} name={'ExecuteOnlyIfFromACountry'} parent={this}>



                                <AddLocalizedMessage title={'انتخاب و افزودن یک کشور'} notinput={false} onConfirm={(textArea,lang)=>{

                                    let countries= DataHolder.selectedEventTrigger.countries;
                                    if(!countries){
                                        countries=[];
                                    }

                                    countries.push({textArea,lang});


                                    DataHolder.selectedEventTrigger['countries']=countries;
                                    this.setState({countries});
                                }}>


                                    {DataHolder.selectedEventTrigger.countries && DataHolder.selectedEventTrigger.countries.length>0 &&
                                    this.showAddesCounties()}


                                </AddLocalizedMessage>
                            </MySwitcher>

                       
                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }

    showAddesCounties() {
        debugger;


        console.warn('---------------DataHolder.selectedEventTrigger.countries------------');
        console.log(DataHolder.selectedEventTrigger.countries)
        console.warn('---------------------------');
        return <Table striped bordered hover>
            <thead>
            <tr>
                <th>پرچم</th>
                <th>زبان</th>
                <th>حذف</th>
            </tr>
            </thead>
            <tbody>
            {
                DataHolder.selectedEventTrigger.countries.map((m,i,arr)=>{


                    return  <tr>
                        <th><img width={50} height={30} alt={m.lang.name} src={m.lang.flag}  className={`flag flag-${m.lang.alpha2Code.toLowerCase()}`} /></th>
                        <th>{m.lang.name} {m.lang.Name}</th>
                        <th>
                            <Badge variant={'danger'} onClick={()=>{

                                let countries= DataHolder.selectedEventTrigger.countries;
                                if(!countries){
                                    countries=[];
                                }

                                countries=  countries.filter(lm=>lm!==m);
                                this.setState({countries});

                            }}>

                                <i className={'fa fa-trash'}></i>
                            </Badge>

                        </th>
                    </tr>
                })
            }
            </tbody>

        </Table>

    }


}

export default EtFormTriggerBehaviours;