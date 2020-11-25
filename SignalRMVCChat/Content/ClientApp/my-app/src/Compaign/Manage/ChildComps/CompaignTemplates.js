import React, {Component} from 'react';
import CompaignChildCompBase from "./CompaignChildCompBase";
import {CurrentUserInfo, MyCaller} from "../../../Help/Socket";
import {MyCard, MyFieldset} from "../../../Routing/Manage/RoutingSave";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {MyConfirm, RowHtml, ShowListOfCards} from "./CompaginHelps/CompaignUtility";
import Card from "react-bootstrap/cjs/Card";
import {_GetSelectedCompaign} from "../CompaignSave";
import {Button} from "primereact/button";
import {_showError} from "../../../Pages/LayoutPage";

class CompaignTemplates extends CompaignChildCompBase {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.CompaignTemplates = this;
    }

    componentDidMount() {

        MyCaller.Send('GetCompaignTemplates')
    }

    getCompaignTemplatesCallback(res) {
        if (!res || !res.Content) {
            //  CurrentUserInfo.LayoutPage.showError('هیچ اطلاعاتی دریافت نشد');
            return;
        }
        let arr = res.Content;


        this.setState({arr: arr});
    }
    
    componentWillUnmount() {
        
        
        
    }

    render() {
        return (
            <div>

                <h4>الگو های موجود و در دسترس</h4>

                <p>
                    ما در اینجا تمام الگوهای HTML را که برای استفاده بعدی ذخیره کرده اید ، لیست کردیم. برای ایجاد الگوی
                    HTML جدید یا استفاده از Markdown به جای آن ، می توانید از این مرحله صرف نظر کنید.
                </p>
                <hr/>


                <MyFieldset title="لیست الگو های ایمیل (یا پیغام) آماده">
                    <Row>
                        <Col>
                            <MyCard header={'الگو های آماده'} title={'میتوانید از الگو های آماده استفاده کنید'}>
                                <ShowListOfCards loading={this.state.loading}
                                                 list={this.state.arr}
                                                 emptyMessage={'هیچ الگویی تعریف نشده است یک الگو تعریف کنید'}
                                                 showLinks={(row) => {

                                                     return <Card.Link onClick={()=>{
                                                       
                                                         this.useTemplate(row);
                                                     }}>انتخاب این قالب</Card.Link>
                                                 }}

                                                 body={(row) => {

                                                     return <RowHtml html={row.Html}/>
                                                 }}
                                />
                                
                                
                                
                            </MyCard>
                        </Col>
                    </Row>
                    
                    <br/>
                    <Row>
                        <Col>

                        </Col>
                        <Col md={4}>

                            <Button onClick={()=>{
                                
                                // خالی
                                this.useTemplateForce({});
                            }}>
                                رد شدن از این بخش
                            </Button>
                        </Col>
                    </Row>
                </MyFieldset>

               <MyConfirm parent={this} title={'این کمپین قبلا دارای قالب و نوشتار است آیا جایگزین می کنید؟'}  display={this.state.displayConfirm} onConfirm={()=>{
                   
                   this.useTemplateForce(this.state.selectedTemplate);
               }}/>
            </div>
        );
    }

    onHide(name) {
        this.setState({
            [`${name}`]: false
        });
    }
    useTemplate(row) {
        
        this.setState({selectedTemplate:row});
        if (_GetSelectedCompaign().Template){
            this.setState({displayConfirm:true})
            return ;
        }
        
        this.useTemplateForce(row);
    }

    useTemplateForce(row) {

        if (!row){
            _showError('هیچ قالب انتخاب شده ای یافت نشد لطفا مجددا انتخاب کنید');
            return ;
        }
        
        _GetSelectedCompaign().Template=row;
        this.goNext();
    }
}

export default CompaignTemplates;



