import React, {Component} from "react";
import {CurrentUserInfo,MyCaller} from "../Help/Socket";
import {DataHolder} from "../Help/DataHolder";


export default class ChatForm extends Component {

    constructor(arg) {
        super(arg);
        this.submit = this.submit.bind(this);
        this.state = {};
        CurrentUserInfo.ChatForm = this;
    }

    submit(e) {
        e.preventDefault();

        this.props.onSubmit(e);

        this.setState({text: ''});

        return false;
    }
    
    
    addText(text){
        let prev=this.state.text ? this.state.text : '';
        this.setState({text:prev+" "+text})
    }
    

    render() {
        return (<div>

            <form onSubmit={this.submit}>
                <input style={{direction:'rtl'}} onKeyPress={() => {


                    bindIsTyping();
                }}
                    value={this.state.text}
                    placeholder='جهت ارسال فایل یا پیغام متنی اینجا تایپ نمایید'
                    className='form-control' onChange={(e) => {


                    this.setState({text: e.target.value});
                    this.props.onChange(e);
                }}
                    onPaste={(e) => {
                        this.props.onPaste(e);
                    }}

                       type={'textarea'}
                />


            </form>
            <button style={{display: 'block'}} className='btn btn-default' onClick={() => {
                document.getElementById('fileUpload').click();
            }}>ارسال فایل
            </button>
            <input  onChange={(e) => {
                this.props.upload(e);
            }} id='fileUpload' type='file' hidden='hidden'/>
        </div>);
    }
}


/*TYPING*/

var searchTimeout;
let searchTimeoutSelectedCustomerId;

function bindIsTyping() {

    if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.Id) {
        CurrentUserInfo.LayoutPage.showError('کاربری انتخاب نشده است');
        return;
    }
    
    if (searchTimeout != undefined) clearTimeout(searchTimeout);


    searchTimeoutSelectedCustomerId=DataHolder.selectedCustomer.Id;
    searchTimeout = setTimeout(callServerScript, 1000);
    MyCaller.Send('AdminStartTyping', {customerId: DataHolder.selectedCustomer.Id});

}

function callServerScript() {
    if (!searchTimeoutSelectedCustomerId) {
        CurrentUserInfo.LayoutPage.showError('کاربری انتخاب نشده است.');
        return;
    }
    MyCaller.Send('AdminStopTyping', {customerId: searchTimeoutSelectedCustomerId});

}

/*TYPING END*/