import {Button,Modal} from "react-bootstrap";
import React, {useEffect, useState} from "react";

export class  MyModal extends  React.Component{
    
    constructor(props) {
        super(props);
        this.state={}
        
        this.handleClose.bind(this);
    }
    
    componentDidMount() {


    }

    handleClose = () => {
       this.props.handleClose();
    };
    render() {
        
        return (
            <>

                <Modal dir={'rtl'} show={this.props.show} onHide={this.handleClose}>
                    <Modal.Header closeButton>
                        <Modal.Title>{this.props.title}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body className={'mymodal'}>
                        {this.props.children}

                    </Modal.Body>
                    <Modal.Footer>


                        {this.props.onSave && <Button id={'saveModal'} variant="primary" onClick={this.handleClose}>
                            ذخیره تغییرات
                        </Button>}

                        
                        
                        <Button  variant="secondary" onClick={this.handleClose}>
                            لغو
                        </Button>

                    </Modal.Footer>
                </Modal>
            </>
        );
    }

   
}