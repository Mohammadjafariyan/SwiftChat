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

class CategoryDelete extends Component {
    state={};
    showHeader(){
        if (DataHolder.selectedCategory && DataHolder.selectedCategory.Id)
            return <h3>ویرایش دسته بندی</h3>
        return <h3>دسته بندی جدید</h3>

    }

    constructor(arg) {
        super(arg);

        CurrentUserInfo.CategoryDelete = this;
    }



    category_Delete_Callback(res){
       /* if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }*/


        this.setState({loading:false});

        DataHolder.selectedCategory=null;
        CurrentUserInfo.CategoryLayout.setState({page:null})

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
                            <p className={'text-danger'}>آیا از حذف این دسته بندی اطمینان دارید ؟</p>
                        </Col>
                        <Col>

                            <Button disabled={this.state.loading} label="بله" icon={'pi pi-check'} className="p-button-danger" onClick={() => {


                                MyCaller.Send("Category_Delete",{id:DataHolder.selectedCategory.Id})

                                this.setState({loading:true})
                            }}/>

                            <Button disabled={this.state.loading} label="لغو" icon={'pi pi-times'} className="p-button-secendary" onClick={() => {

                                DataHolder.selectedCategory=null;
                                CurrentUserInfo.CategoryLayout.setState({page:null})

                            }}/>
                        </Col>


                    </Row>
                </Container>

            </div>
        );
    }
}

export default CategoryDelete;