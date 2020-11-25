import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {Button} from "primereact/button";
import {_showError, _showMsg} from "../Pages/LayoutPage";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";

class BaseSave extends Component {
    state={};

    constructor(props) {
        super(props);
        this.props.parent.props.parent.setState({BaseSave:this});

    }
    
    getBaseCrudLayoutState(){
        return this.props.parent.props.parent.state
    }

    save() {
        
        if (this.props.onSaveClick){
            this.props.onSaveClick(this.getBaseCrudLayoutState().selected);
            return ;
        }
        
        if (this.props.validateSave){
            if (!this.props.validateSave(this.state)){
                return ;
            }
        }

        MyCaller.Send(this.props.save,this.getBaseCrudLayoutState() .selected);

        _showMsg('در حال ذخیره')
    }

    saveCallback(res) {

        _showMsg('ذخیره شد')

        this.cancel()

    }

    delete() {

        _showMsg('در حال حذف')
        MyCaller.Send(this.props.delete
            , {id:this.props.selected.Id});


    }
    cancel() {

        this.props.parent.setState({selected: null});
    }


    deleteCallback(res){
        _showMsg('با موفقیت حذف شد')

        this.cancel();
    }

    render() {
        return (
            <>
                <Row>

                    <Col>

                        <Button className={'p-button-raised  p-button-danger p-button-text'} 
                                label="حذف"
                                icon="pi pi-trash" onClick={() => {

                            this.delete();
                        }}/>


                    </Col>
                    <Col>
                        <Button className={'p-button-raised   p-button-warning p-button-text'}
                                label="کنسل"
                                icon="pi pi-times"
                                onClick={() => {

                                    this.cancel();
                                }}/>
                        <Button className={'p-button-raised  p-button-text'} 
                                label="ذخیره" icon="pi pi-check"
                                onClick={() => {

                                    this.save();
                                }}/>

                    </Col>
                </Row>
                <hr/>


                {React.cloneElement(this.props.children, { parent: this })}

            </>
        );
    }
}

export default BaseSave;