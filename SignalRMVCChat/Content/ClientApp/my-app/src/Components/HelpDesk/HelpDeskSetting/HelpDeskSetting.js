import React, {Component} from 'react';
import {_getHelpDesk} from "../../../Pages/HelpDeskPage";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/cjs/Col";
import {ColorPicker} from "primereact/colorpicker";
import {DataHolder} from "../../../Help/DataHolder";
import Container from "react-bootstrap/Container";
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import {Input} from "../../FormInputs";
import {Button} from "primereact/button";
import {_showError, _showMsg} from "../../../Pages/LayoutPage";

class HelpDeskSetting extends Component {
    state={};
    
    constructor(ar) {
        super(ar);
        
        CurrentUserInfo.HelpDeskSetting=this;
    }

    helpDeskGetByIdCallback(res){

        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }

        this.setState({loading:false})

        this.setState({HelpDesk:res.Content});
        
    }
    
    componentDidMount() {



        this.setState({loading:true})
        MyCaller.Send('HelpDeskGetById',{id:_getHelpDesk().helpDeskId})
        
        
    }

    helpDeskSaveDetailCallback(res){
        this.setState({loading:false})

        _showMsg("با موفقیت ثبت شد")
        
    }

    save(){
        MyCaller.Send('HelpDeskSaveDetail',{
            id:_getHelpDesk().helpDeskId,
            HeaderText:this.state.HelpDesk.HeaderText,
            BgColor:this.state.HelpDesk.BgColor,
        })

    }

    render() {
        if (!_getHelpDesk().helpDeskId || !this.state.HelpDesk){
            return  <p>هیچ مرکز پشتیبانی انتخاب نشده است</p>
        }
        
        return (
            <div>


                <Row  className={'p-fluid p-grid p-formgrid'}>
                    <Col>

                        <Input value={this.state.HelpDesk.HeaderText}
                               type={'text'} name={'عنوان دسته بندی'}

                               onChange={(e)=>{
                                   this.state.HelpDesk.HeaderText=e.target.value;
                                   this.setState({tmp:Math.random()});
                               }}

                               placeholder={''}></Input>

                    </Col>

                    <Col>
                        <label>
                            رنگ دسته بندی
                        </label>
                        <div className="p-field p-col-12 p-md-3">

                            <ColorPicker value={this.state.HelpDesk.BgColor} onChange={(e) =>{
                                this.state.HelpDesk.BgColor= e.value;

                                this.setState({BgColor:e.value})
                            }} />
                        </div>
                    </Col>
                </Row>
                
                <Row>
                    <Col>

                        <Button disabled={this.state.loading} label="ذخیره " icon={'pi pi-plus'} className="p-button-primary" onClick={() => {

                            this.setState({loading:true})
                            this.save()
                        }}/>
                    </Col>
                </Row>
            </div>
        );
    }
}

export default HelpDeskSetting;