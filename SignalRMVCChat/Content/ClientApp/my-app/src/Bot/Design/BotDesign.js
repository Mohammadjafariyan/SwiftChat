import React, {Component} from 'react';
import BotOrgChart from "./BotOrgChart";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Button} from "primereact/button";
import {_showError, _showMsg} from "../../Pages/LayoutPage";
import {CurrentUserInfo,MyCaller} from "../../Help/Socket";

class BotDesign extends Component {

    constructor(props) {
        super(props);
        CurrentUserInfo.BotDesign=this;
    }

    save() {
        if (!this.BotOrgChart) {
            _showError("هیچ دیزاینی یافت نشد لطفا مجددا امتحان کنید")
            return;
        }

        console.log('BotSave',JSON.stringify(this.BotOrgChart.state.data1[0]))

        MyCaller.Send('BotSave', this.BotOrgChart.state.data1[0]);

        _showMsg('در حال ذخیره')
    }

    botSaveCallback(res) {

        _showMsg('ذخیره شد')

        this.cancel()

    }

    delete() {
        
        if (!this.BotOrgChart.state.data1[0].Id){
            _showError('این سناریو هنوز ذخیره نشده است تا حذف شود');
            return ;
        }


        _showMsg('در حال حذف')
        MyCaller.Send('BotDelete', {id:this.BotOrgChart.state.data1[0].Id});


    }
    cancel() {

        CurrentUserInfo.BotLayout.setState({selectedBot: null});
    }


    botDeleteCallback(res){
        _showMsg('با موفقیت حذف شد')

        this.cancel();
    }


    
    render() {
        return (
            <div>

                <Row>

                    <Col>

                        <Button className={'p-button-raised  p-button-danger p-button-text'} label="حذف"
                                icon="pi pi-trash" onClick={() => {

                            this.delete();
                        }}/>


                    </Col>
                    <Col>
                        <Button className={'p-button-raised   p-button-warning p-button-text'} label="کنسل"
                                icon="pi pi-times"
                                onClick={() => {

                                    this.cancel();
                                }}/>
                        <Button className={'p-button-raised  p-button-text'} label="ذخیره" icon="pi pi-check"
                                onClick={() => {

                                    this.save();
                                }}/>

                    </Col>
                </Row>
                <br/>
                <BotOrgChart parent={this}  bot={CurrentUserInfo.BotLayout.state.selectedBot}/>

            </div>
        );
    }
}

export default BotDesign;