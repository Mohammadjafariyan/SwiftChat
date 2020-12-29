import React, {Component} from 'react';
import {DataHolder} from "../../Help/DataHolder";
import Card from "react-bootstrap/Card";
import EventTriggerForm from "./EventTriggerForm";
import { Alert } from 'react-bootstrap';
import GlobalLoading from '../Utilities/GlobalLoading';

class EventTriggerSave extends Component {
    render() {
        return (
            <div>


                {!DataHolder.selectedEventTrigger  && this.showMessage()}
                {DataHolder.selectedEventTrigger && <EventTriggerForm/>}
                
                
            </div>
        );
    }

    showMessage() {
        
        if (DataHolder.selectedEventTrigger)
return<></>
        
        
        return   <Card
            bg={'warning'}
        >
            <Card.Header>راه انداز چیست و چگونه می تواند مفید باشد؟</Card.Header>
            <Card.Body>
                {/*<Card.Title> Card Title </Card.Title>*/}
                <Card.Text>

                <Alert variant={'warning'}>

                    <div className="cap-plugin-content-box-inner">
                        <p className="cap-font-sans-regular">راه انداز ها پیغام های اتوماتیک هوشمند هستند
                        </p><p> <strong
                            className="cap-font-sans-semibold">یک عملیات را برنامه ریزی کرده و برای تمامی بازدیدکنندگانی که در این قاعده قرار میگیرند پیغامی ارسال کنید</strong> 
                          
                          <br/>
                          <br/>
                            برای مثال برای بازدیدکنندگانی که صفحه قیمت بسته ها
را بیشتر از 1 دقیقه مشاهده کنند پیغامی ارسال کنید.</p>


                        <p className="cap-font-sans-regular"> شما میتوانید از این امکان <strong
                            className="cap-font-sans-semibold">برای جلوگیری از ریزش مشتریان و استفاده کنید</strong>
                          </p>
                          <p >
                             و به کاربرانی که در حال خروج از سایت شما هستند پیغامی بفرستید</p>

                     
                    </div>

                    </Alert>
                </Card.Text>
            </Card.Body>
        </Card>;
    }
}

export default EventTriggerSave;

