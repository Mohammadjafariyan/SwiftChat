import React, {Component} from 'react';
import BaseCrudLayout from "../CRUD/BaseCrudLayout";
import BaseIndex from "../CRUD/BaseIndex";
import RoutingSave from "./Manage/RoutingSave";
import {Alert, Card} from "react-bootstrap";
import Col from "react-bootstrap/Col";
import {CurrentUserInfo} from "../CurrentUserInfo";

class RoutingLayout extends BaseCrudLayout {
    state = {
        get: 'GetRoutingList',
        save: 'RoutingSave',
        delete: 'DeleteRouting',
        setIsEnabled: 'SetIsEnabledRouting',
        saveDraft: 'RoutingSave',
    };

    constructor(prp) {
        super(prp);

        CurrentUserInfo.RoutingLayout = this;
        
        console.log('hi');
    }


    RenderWelcome() {
        return <Card>
            <Card.Body>
                <Card.Text>
                    <Alert variant={'warning'}>
                        <div className="cap-plugin-content-box-inner">
                            <p className="cap-font-sans-regular"><span
                                className="cap-font-sans-semibold">
                                
                                اختصاص اوپراتور به صفحات به شما امکان می دهد 
                                <b> پشتیبانی خود را به صورت خودکار انجام دهید ، </b>
                                <b>  هدفمند انتخاب و هدفمند بفروشید </b>

                                با استفاده از این امکان می توانید صفحات سایت را به اوپراتور ها اختصاص دهید 
                                                               <span> ، </span>

                                مثلا اوپراتور های فروش برای صفحات فروش 
                                                                                        <span> ، </span>


                                اوپراتور های پشتیبانی به صفحات پشتیبانی 
                               
                                <span> ، </span>
 اوپراتور مخصوص کاربران عضو 
                                
                                یا برای مثال می توانید برای شهر یا استانی خاص سیاست های فروش خاصی در نظر بگیرید و به اوپراتور های خاصی دهید
                                و هوشمندانه تر مدیریت کنید
                                
                                                </span>
                            </p>
                            <p className="cap-font-sans-regular">
                                مسیریابی (که Assign نیز نامیده می شود) می تواند برای اختصاص دادن اپراتورهایی باشد که از پیام های دریافتی مطلع شوند. این به وب سایت هایی که چت زیادی دارند کمک می کند تا بتوانند در صندوق ورودی تیم شما تعادل برقرار کنند.
                            </p>

                            <p className="cap-font-sans-bold help-title">نحوه کار ربات بشرح زیر است:</p>

                            <ol className="cap-font-sans-semibold help-list" style={{textAlign:'right'}}>
                                <li>در سمت چپ روی دکمه جدید کلیک کنید</li>
                                <li>تنظیمات اختصاص اوپراتور را انجام دهید</li>
                                <li>اختصاص اوپراتور خود را انتشار دهید تا توسط کاربران دیگر مورد استفاده قرار گیرد</li>
                            </ol>

                            <div className="help-screen-wrap">
                                <div className="help-screen"></div>
                            </div>
                        </div>

                        <hr/>
                        در این بخش کاربران سایت (عضو و غیر عضو) با سیاست خاصی به اوپراتور ها اختصاص می یابند
                    </Alert>
                </Card.Text>
            </Card.Body>
        </Card>
    }

    RenderForm() {
        return <>{this.state.selected &&  <RoutingSave parent={this} selected={this.state.selected}/>}</>
    }
}

export default RoutingLayout;