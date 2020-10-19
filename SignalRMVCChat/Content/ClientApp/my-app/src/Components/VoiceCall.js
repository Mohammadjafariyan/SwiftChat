import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import {Dialog} from "primereact/dialog";
import VoiceCallRunner from "./VoiceCallRunner";
import {Button} from "react-bootstrap";
import {CurrentUserInfo, MyCaller} from "../Help/Socket";
import Spinner from "react-bootstrap/Spinner";
import {_showError} from "../Pages/LayoutPage";

export let voiceData = [];

class VoiceCall extends Component {
    state = {
        type: 'callToUser'

    };

    componentDidMount() {
        CurrentUserInfo.VoiceCall = this;

        if (!DataHolder.selectedCustomerForCall)
            return <></>;


        this.onClick('displayPosition', 'bottom-right')
    }

    show() {
        this.onClick('displayPosition', 'bottom-right')

    }

    render() {

        if (!DataHolder.selectedCustomerForCall)
            return <></>;

        return (
            <div>


                <Dialog rtl={true} modal={false} header="برقراری تماس صوتی  " visible={this.state.displayPosition}
                        position={this.state.position}
                        style={{width: '30vw'}}
                        onHide={() => this.onHide('displayPosition')}>

                    {this.state.type === 'callToUser' && this.callToUser()}
                    {this.state.type === 'userCalling' && this.userCalling()}
                    {this.state.type === 'openSpeak' && <VoiceCallRunner></VoiceCallRunner>}
                    {this.state.type === 'error' && this.showError()}
                    {this.state.type === 'callRequestFromCustomer' && this.callRequestFromCustomer()}
                    {/*  {this.state.isAnotherCallCome && this.showAnotherCallCome()}*/}

                    {this.state.anotherCallIsWaiting && this.anotherCallIsWaitings()}

                </Dialog>


                {/*another call:*/}
                <Dialog rtl={true} modal={false} header="برقراری تماس صوتی  " visible={this.state.isAnotherCallCome}
                        position={'bottom-left'}
                        style={{width: '30vw'}}
                        onHide={() => this.setState({isAnotherCallCome: false})}>

                    {this.state.isAnotherCallCome && this.showAnotherCallCome()}


                </Dialog>

            </div>
        );
    }

    showError() {
        return <div>
            <p style={{color: 'red'}}>{this.state.error}</p>
            <small style={{color: 'red'}}>{this.state.techError}</small>
        </div>
    }

    onClick(name, position) {
        let state = {
            [`${name}`]: true
        };

        if (position) {
            state = {
                ...state,
                position
            }
        }

        this.setState(state);
    }

    /*<i class="fa fa-microphone" aria-hidden="true"></i>
<i class="fa fa-headphones" aria-hidden="true"></i>
*/
    callToUser() {
        return <div>

            <small>ابتدا مطمئن شوید میکروفون یا کارت صوتی دستگاه شما درست کار میکند
                سپس برای شروع تماس دکمه زیر را فشار دهید</small>

            <hr/>
            شروع تماس
            <button className={'p-button p-component p-button-rounded p-button-success p-button-icon-only'}

                    onClick={() => {
                        this.AccessRequest();
                    }}
            >

                <i style={{fontSize: '30px'}} className="fa fa-phone" aria-hidden="true"></i>

            </button>
        </div>;
    }

    userCalling() {
        return <>


            <p>درخواست تماس ارسال شد ، در انتظار برای پذیرش کاربر</p>
            <Spinner animation="border" variant="info"/>


        </>;
    }

    //todo:
    vC_CustomerIsAcceptOrRejectCallback(res) {

        if (!res || !res.Content) {
            _showError('vC_AdminCallInitCallback res is null')
        }


        if (res.Content.IsAccepted === true) {
            startRecording();
        } else {
            this.setState({type: 'error', error: res.Content.Message});
        }

    }

    VC_AdminCallInit() {


        this.setState({type: 'userCalling'})
        MyCaller.Send('VC_AdminCallInit', {customerId: DataHolder.selectedCustomerForCall.Id})
    }

    vC_CustomerCallInitCallback(res) {
        if (!res || !res.Content) {
            _showError('vC_CustomerCallInitCallback res is null')
            return;
        }

        if (!res || !res.Content.chatId) {
            _showError('vC_CustomerCallInitCallback  chatId  is null')
            return;
        }

        // در حال مکالمه هستیم
        if (inCalling) {

            // آیا کسی دیگر زنگ میزند ؟
            if (DataHolder.selectedCustomerForCall && DataHolder.selectedCustomerForCall.Id != res.Content.Customer.Id) {


                this.setState({
                    isAnotherCallCome: true,
                    anotherCallCustomer: res.Content.Customer,
                    anotherCallChatId: res.Content.chatId
                });

                _showError('کاربر دیگری در حال تماس به شماست ، در انتظار ')
                return;
            }
        }

        DataHolder.selectedCustomerForCall = res.Content.Customer;
        DataHolder.selectedCustomerForCallChatId = res.Content.chatId;


        this.setState({type: 'callRequestFromCustomer'})


    }


    AccessRequest() {
        ACCESS_TO_VOICE((stream) => {


            this.VC_AdminCallInit();
        }, (err) => {
            this.setState({type: 'error'})

            this.setState({
                error: 'اجازه دسترسی به میکروفون شما صادر نشد',
                techError: err + ""
            })

        })
    }

    showAnotherCallCome() {
        return <div>

            <p>

                <b>{DataHolder.selectedCustomerForCall.Name}</b>
                <small>تماس دیگری رسید</small>
            </p>


            <hr/>
            در انتظار باشد
            <button className={'p-button p-component p-button-rounded p-button-success p-button-icon-only'}

                    onClick={() => {
                        this.Wait(this.state.anotherCallCustomer,
                            this.state.anotherCallChatId);

                        this.setState({isAnotherCallCome: null})
                        this.setState({anotherCallIsWaiting: true})

                    }}
            >

                <i style={{fontSize: '30px'}} className="fa fa-phone" aria-hidden="true"></i>

            </button>

            رد کردن
            <button className={'p-button p-component p-button-rounded p-button-success p-button-icon-only'}

                    onClick={() => {
                        this.Reject(this.state.anotherCallCustomer,
                            this.state.anotherCallChatId);

                        this.setState({isAnotherCallCome: null})

                    }}
            >

                <i style={{fontSize: '30px'}} className="fa fa-phone" aria-hidden="true"></i>

            </button>
        </div>;
    }

    Answer() {
        ACCESS_TO_VOICE((stream) => {

            MyCaller.Send('VC_AdminIsAcceptOrReject',
                {
                   err: null, isAccepted: true, customerId: DataHolder.selectedCustomerForCall.Id,
                    chatId: DataHolder.selectedCustomerForCallChatId
                });

            startRecording();

        }, (err) => {
            this.setState({type: 'error'})

            this.setState({
                error: 'اجازه دسترسی به میکروفون شما صادر نشد',
                techError: err + ""
            })

        })
    }

    Reject(customerId, chatId) {

        MyCaller.Send('VC_AdminIsAcceptOrReject',
            {
                err: null,
                isAccepted: false,
                customerId: customerId ? customerId : DataHolder.selectedCustomerForCall.Id,
                chatId: chatId ? chatId : DataHolder.selectedCustomerForCallChatId
            })

        if (customerId)
            this.onHide('displayPosition')
    }

    callRequestFromCustomer() {
        return <div>

            <p>

                <b>{DataHolder.selectedCustomerForCall.Name}</b>
                <small>در حال تماس به شماست</small>
            </p>


            <hr/>
            پاسخ
            <button className={'p-button p-component p-button-rounded p-button-success p-button-icon-only'}

                    onClick={() => {
                        this.Answer();
                    }}
            >

                <i style={{fontSize: '30px'}} className="fa fa-phone" aria-hidden="true"></i>

            </button>

            رد کردن
            <button className={'p-button p-component p-button-rounded p-button-success p-button-icon-only'}

                    onClick={() => {
                        this.Reject();

                    }}
            >

                <i style={{fontSize: '30px'}} className="fa fa-phone" aria-hidden="true"></i>

            </button>
        </div>;
    }

    Wait(customerId, chatId) {

        if (!customerId || !chatId) {
            _showError("Wait(customerId,chatId) is null")
            return;
        }

        MyCaller.Send('VC_AdminInAnotherCalling', {
            customerId: customerId,
            chatId: chatId
        })
    }

    anotherCallIsWaitings() {
        return (
            <>
                <i style={{fontSize: '30px'}} className="fa fa-frown-o" aria-hidden="true"></i>
                <small>کاربران در حال انتظار</small>
            </>
        );
    }
}

export default VoiceCall;


let streamrecorder, webcamstream, mediaRecorder, inCalling = false,chunks=[];

function startRecording() {
    inCalling = true;
    mediaRecorder = new MediaRecorder(webcamstream);
    mediaRecorder.start(3000);
    mediaRecorder.ondataavailable = function (e) {

        new Promise(resolve => {

            const fileReader = new FileReader();
            fileReader.addEventListener("loadend", () => {
//debugger
                // videoElem.src=fileReader.result;
                /*  var dataUrl = fileReader.result;
                  var base64 = dataUrl.split(',')[1];*/

                resolve(fileReader.result)
            });

            fileReader.readAsBinaryString(e.data);

        }).then(function (buffer) {


            MyCaller.Send('VC_AdminSpeak', {customerId: DataHolder.selectedCustomerForCall.Id, buffer})
        })
        chunks.push(e.data);
    }
}

function stopRecording() {
    mediaRecorder.stop();
    inCalling = false;

}


function ACCESS_TO_VOICE(successCallback, failCallback) {


    navigator.getUserMedia = (navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia);

// Get access to microphone
    navigator.getUserMedia(
        // Only request audio
        {video: false, audio: true},

        // Success callback
        function success(localAudioStream) {
            // Do something with audio stream

            webcamstream = localAudioStream;
            voiceData = [];
            successCallback(localAudioStream)

        },
        // Failure callback
        function error(err) {
            // handle error
            failCallback(err)
        }
    );
}