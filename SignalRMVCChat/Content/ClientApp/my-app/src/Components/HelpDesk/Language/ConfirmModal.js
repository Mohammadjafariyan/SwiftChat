import React, {useState} from "react";
import {Dialog} from "primereact/dialog";
import {Button} from "primereact/button";
import {Modal} from "react-bootstrap";
import '../../../styles/myStyle.css'

export const ConfirmModal=(props)=>{
    const [displayConfirmation,setdisplayConfirmation]= useState(true);

    React.useEffect(() => {
        setdisplayConfirmation(true);
    }, [props.showConfirm])


    const footer= <div>
        <Button label="بله" id={'removeLanguageConfirm'} icon="pi pi-check" onClick={() => {

            setdisplayConfirmation(false)
            props.onConfirm();
        }
        } autoFocus />
        <Button label="لغو" icon="pi pi-times" onClick={() => {

            setdisplayConfirmation(false);

            props.onCancel();
        }
        } className="p-button-text" />
       
    </div>;

    return  <Dialog header="تاکید" visible={displayConfirmation}
                    modal style={{ width: '350px' }} footer={footer}
                    onHide={() => setdisplayConfirmation(false)}>
     
        
        {props.body}
    </Dialog>
}



export function BootstrapModal(props) {
    const [show, setShow] = useState(true);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);


    React.useEffect(() => {
        setShow(true);
    }, [props.showConfirm]);
    
    return (
        <>
            <Modal show={show} onHide={handleClose}

                   dialogClassName={props.dialogClassName}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{props.title}</Modal.Title>
                </Modal.Header>
                <Modal.Body  >
                    
                        {props.body}

                  
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={()=>{
                        handleClose(false);
                        props.onCancel();

                    }}>
                        لغو
                    </Button>
                    <Button variant="primary" onClick={()=>{
                        props.onConfirm();
                    }}>
                        ذخیره
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}
