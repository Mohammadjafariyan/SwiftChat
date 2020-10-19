import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import {Input} from "../../FormInputs";
import {DataHolder} from "../../../Help/DataHolder";
import Container from "react-bootstrap/Container";
import CategoryLayout from "./CategoryLayout";
import Col from "react-bootstrap/cjs/Col";
import {Button} from "primereact/button";
import {Spinner} from "react-bootstrap";
import {CurrentUserInfo,MyCaller} from "../../../Help/Socket";
import {ColorPicker} from "primereact/colorpicker";

import '../../../styles/myStyle.css';
import {InputTextarea} from "primereact/inputtextarea";
import {InputNumber} from "primereact/inputnumber";
import {_showError, _showMsg} from "../../../Pages/LayoutPage";
import {_getHelpDesk} from "../../../Pages/HelpDeskPage";
class CategorySave extends Component {
    state={};
    showHeader(){
        if (DataHolder.selectedCategory && DataHolder.selectedCategory.Id)
            return <h3>ویرایش دسته بندی</h3>
        return <h3>دسته بندی جدید</h3>

    }

    constructor(arg) {
        super(arg);

        CurrentUserInfo.CategorySave = this;
    }



    category_Save_Callback(res){
   /*     if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }
*/

        this.setState({loading:false});

        DataHolder.selectedCategory=null;
        CurrentUserInfo.CategoryLayout.setState({page:null})

    }

    category_GetById_Callback(res){
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }
        
        DataHolder.selectedCategory=res.Content;
        this.setState({loading:false});

    }
    
    componentDidMount() {
        if (DataHolder.selectedCategory && DataHolder.selectedCategory.Id){
            this.setState({loading:true});
            MyCaller.Send("CategoryGetById",{id:DataHolder.selectedCategory.Id})
        }else{
            if (DataHolder.selectedCategory){
                DataHolder.selectedCategory.BgColor="#ddd"
            }
        }

    }




    render() {
        
        if (!DataHolder.selectedCategory){
            DataHolder.selectedCategory={};
        }
        
        return (
            <div>
                
                
                <Container>

                    {this.showHeader()}
                    
                    
                    {this.state.loading &&
                    <Spinner animation="border" role="status">
                        <span className="sr-only">لطفا منتظر بمانید ....</span>
                    </Spinner>}
                    <Row>

                        <Col>
                            <Input value={DataHolder.selectedCategory.Title}
                                   type={'text'} name={'عنوان دسته بندی'}

                                   onChange={(e)=>{
                                       DataHolder.selectedCategory.Title=e.target.value;
                                       this.setState({tmp:Math.random()});
                                   }}

                                   placeholder={''}></Input>
                        </Col>
                        <Col>

                            <Button disabled={this.state.loading} label="ذخیره دسته بندی" icon={'pi pi-plus'} className="p-button-primary" onClick={() => {
                                
                                if(!_getHelpDesk().helpDeskId){
                                    
                                    _showError("هیچ زبانی انتخاب نشده است")
                                    return ;
                                }
                                DataHolder.selectedCategory.HelpDeskId= _getHelpDesk().helpDeskId
                                
                                MyCaller.Send("Category_Save",DataHolder.selectedCategory)
                                
                                this.setState({loading:true})
                            }}/>
                        </Col>
                        
                     
                    </Row>
                    
                    <hr/>

                    <h4 className={'small'}></h4>
                    <Row className={'p-fluid p-grid p-formgrid'}>
                        <Col>
                            <div className="p-field p-col-12 p-md-3">
                            <label>
                                توضیحات
                            </label>
                            <InputTextarea rows={5} cols={30} value={DataHolder.selectedCategory.Description} 
                                           
                                           
                                           onChange={(e) => {
                                               DataHolder.selectedCategory.Description=e.target.value;
                                               this.setState({Description: e.target.value})
                                           }} />
                            </div>
                        </Col>

                        <Col>
                            <div className="p-field p-col-12 p-md-3">
                                <label>
                                    اولویت نمایش
                                </label>
                                <InputNumber showButtons  value={DataHolder.selectedCategory.Order}
                                             mode="decimal" useGrouping={false}       onValueChange={(e) =>

                                {
                                    DataHolder.selectedCategory.Order= e.value;
                                    this.setState({Order: e.value});
                                }} />
                            </div>
                        </Col>
                    </Row>

                    <Row  className={'p-fluid p-grid p-formgrid'}>
                        <Col>
                     
                        </Col>

                        <Col>
                            <label>
                                رنگ دسته بندی
                            </label>
                            <div className="p-field p-col-12 p-md-3">
                               
                                <ColorPicker value={DataHolder.selectedCategory.BgColor} onChange={(e) =>{
                                    DataHolder.selectedCategory.BgColor= e.value;
                                    this.setState({BgColor:e.value})
                                }} />
                            </div>
                        </Col>
                    </Row>
                </Container>
                
            </div>
        );
    }
}

export default CategorySave;