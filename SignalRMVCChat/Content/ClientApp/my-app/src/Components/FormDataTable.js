import React, {Component} from 'react';
import {MyCaller, CurrentUserInfo} from './../Help/Socket';
import {_showError, _showMsg} from "../Pages/LayoutPage";
import Table from "react-bootstrap/Table";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

import '../styles/myStyle.css'

class FormDataTable extends Component {

    constructor(prop) {
        super(prop);

        this.state = {};
        CurrentUserInfo.FormDataTable = this;
    }

    getFormDataCallback(res) {

        if (!res || !res.Content) {
            _showError('دیتای خوانده شده اشکال دارد');
            return;
        }

        console.log('FormDataTable==>getFormDataCallback')
        console.log(res)


        let formStructure = res.Content.Form;
        let formData = res.Content.formData;
        let Page = res.Content.Page;
        let total = res.Content.total ? res.Content.total : 0;


     /*   let formDateFixArr=[];
        for (let i = 0; i < formData.length; i++) {

            let row={};
            for (let j = 0; j < formStructure.elements; j++) {
                row[formData[i].Key]=formData[i].Value;

            }
            formDateFixArr[formData[i].Key]=formData[i].Value;    
            
        }*/


        this.setState({formStructure,formData: formData, Page, total});

    }


    componentDidMount() {


    }

    render() {
        return (
            <div>

                <Row dir={'rtl'}>


                    {!CurrentUserInfo.selectedForm &&
                    <Col>
                        <b dir={'rtl'}>یک فرم از لیست انتخاب کنید تا اطلاعات آن نمایش داده شود</b>

                    </Col>
                    }

                    {this.state.Page &&
                    <>
                        
                        <Col>
                             {CurrentUserInfo.selectedForm && <><span>فرم : </span><b>{CurrentUserInfo.selectedForm.Name}</b> </>}
                        </Col>
                        <Col>
                            <p dir={'rtl'}><span>صفحه : </span><span>{this.state.Page}</span></p>

                        </Col>
                        <Col>
                            <p dir={'rtl'}><span>مجموع :</span><span>{this.state.total}</span> رکورد</p>

                        </Col>

                    </>
                    }

                </Row>

                <Row>
                    {this.state.Page && this.showPaging()}

                </Row>
                {this.state.Page && <hr/>}

                <Table striped bordered hover size="sm">
                    <thead>
                    {this.showHeaders()}
                    </thead>
                    <tbody>

                    {this.showData()}


                    </tbody>
                </Table>


                {this.state.Page && this.showPaging()}

            </div>
        );
    }

    showHeaders() {
        if (!this.state.formStructure) {
            return <></>;
        }


        let str = {
            elements: this.state.formStructure.Elements,
            Name: this.state.formStructure.Name,
            AfterMessage: this.state.formStructure.AfterMessage,
            OnlyMe: this.state.formStructure.OnlyMe,
            Id: this.state.formStructure.Id
        };
        if (!str.elements || !str.elements.length) {
            _showError('المنت های فرم خالی است');
            return <></>;
        }


        return <tr>{str.elements.map((el, i, arr) => {
            return <th>{el.Name}</th>
        })}</tr>
    }

    showData() {
        if (!this.state.formData) {
            return <></>;
        }
 

        let str = {
            elements: this.state.formStructure.Elements,
            Name: this.state.formStructure.Name,
            AfterMessage: this.state.formStructure.AfterMessage,
            OnlyMe: this.state.formStructure.OnlyMe,
            Id: this.state.formStructure.Id
        };
        if (!str.elements || !str.elements.length) {
            _showError('المنت های فرم خالی است');
            return <></>;
        }


        //rows:List of FormElement lists

        return this.state.formData.map((rows, i, dataArr) => {

           return ( <tr key={i}>

                {
                    //row:FormElement
                     rows.map((row, j, dataArr) => {
                    return (

                    <td key={'d-'+ j}>{row.Value}</td>

                    )
                })
                }
            </tr>)

        });
    }


    showPaging() {
        let currstate = {
            formStructure: this.state.formStructure, formData: this.state.formData
            , Page: this.state.Page, total: this.state.total
        };

        if (CurrentUserInfo.selectedForm && currstate.Page === null || currstate.Page === undefined) {
            _showError("صفحه نال است");
            return <></>;
        }


        if (currstate.total === null || currstate.total === undefined) {
            _showError("total نال است");
            return <></>;
        }

        let pages = currstate.total / 20 + (currstate.total % 20 > 0 ? 1 : 0)


        let arr = [];
        for (let i = 1; i <= pages; i++) {
            arr.push(i);


        }

        let c = 1;

        return arr.map((p, i, arr) => {


            if (i <= 3) {
                if (p === this.state.Page) {
                    return <a onClick={() => {
                        this.selectPage(p)
                    }} className={'btn btn-primary text-default cursor'}>{p}</a>

                } else {

                    return <a onClick={() => {
                        this.selectPage(p)
                    }} className={'text-primary btn btn-default cursor'}>{p}</a>
                }
            } else {
                if (!(i > (pages - 2)) && i == 4) {
                    return <a>...</a>

                }

            }

            if (i > (pages - 2)) {
                if (p === this.state.Page) {
                    return <a onClick={() => {
                        this.selectPage(p)
                    }} className={'btn btn-primary text-default cursor'}>{p}</a>

                } else {

                    return <a onClick={() => {
                        this.selectPage(p)
                    }} className={'text-primary btn btn-default cursor'}>{p}</a>
                }
            }

            return <></>;


        });
    }

    selectPage(page) {

        if (!CurrentUserInfo.selectedForm) {
            _showError('فرمی انتخاب نشده است');
            return;
        }

        _showMsg("در حال خواندن اطلاعات")
        this.setState({Page: page, formData: null})

        MyCaller.Send('GetFormData', {formId: CurrentUserInfo.selectedForm.Id, Page: page});

    }
}

export default FormDataTable;