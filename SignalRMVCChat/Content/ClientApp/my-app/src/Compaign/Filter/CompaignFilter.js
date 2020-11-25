import React, {Component} from 'react';
import {TabMenu} from "primereact/tabmenu";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/cjs/Col";
import {Card} from "primereact/card";
import {Menubar} from "primereact/menubar";
import {
    _GoCompaignAnalicts,
    _GoCompaignDefine,
    _GoCompaignTable
} from "../Manage/ChildComps/CompaginHelps/CompaignUtility";

 
import './CompaignFilterStyle.css';
class CompaignFilter extends Component {
    constructor(props) {
        super(props);


        this.filters = [
            {
                label: 'آمار و اطلاعات', icon: 'pi pi-fw pi-home',
                command: () => {

                    _GoCompaignAnalicts('آمار و اطلاعات');
                }
            },
            {
                label: 'تعریف کمپین', icon: 'pi pi-fw pi-home',
                command: () => {

                    _GoCompaignDefine();
                }
            },
            ,{
                label: 'لیست کمپین ها', icon: 'pi pi-fw pi-home',

                items:[
                    {
                        label: 'همه', icon: 'pi pi-fw pi-home',
                        command: () => {

                            _GoCompaignTable('all','کل');
                        }
                    } ,
                    {
                        label: 'اتوماتیک', icon: 'pi pi-fw pi-calendar',
                        command: () => {
                            _GoCompaignTable('automatic','اتوماتیک');

                        }
                    },
                    {
                        label: 'دستی', icon: 'pi pi-fw pi-pencil',
                        command: () => {
                            _GoCompaignTable('manual','دستی');

                        }
                    },
                ]
            },

            {
                label: 'لیست کمپین ها بر اساس وضعیت', icon: 'pi pi-fw pi-home',

                items:[

                    {
                        label: 'در حال ارسال', icon: 'pi pi-fw pi-home',
                        command: () => {
                            _GoCompaignTable('sending','در حال ارسال');

                        }
                    },
                    {
                        label: 'آماده ارسال', icon: 'pi pi-fw pi-calendar',
                        command: () => {
                            _GoCompaignTable('readyToSend','آماده ارسال');

                        }
                    },
                    {
                        label: 'متوقف', icon: 'pi pi-fw pi-pencil',
                        command: () => {
                            _GoCompaignTable('stoped','متوقف');

                        }
                    },
                    {
                        label: 'پیکربندی نشده', icon: 'pi pi-fw pi-pencil',
                        command: () => {
                            _GoCompaignTable('notConfigured','پیکربندی نشده');

                        }
                    },
                    {
                        label: 'ارسال شده', icon: 'pi pi-fw pi-pencil',
                        command: () => {
                            _GoCompaignTable('sent','ارسال شده');

                        }
                    }
                ]
            },
           
           
        ];
    }


    render() {
        return (
            <Card>
                <Row>
                  {/*  <Col md={4}>
                        <Menubar model={this.items}/>
                    </Col>*/}
                    <Col>
                        <Menubar model={this.filters}/>
                    </Col>

                </Row>
            </Card>

        );
    }
}

export default CompaignFilter;