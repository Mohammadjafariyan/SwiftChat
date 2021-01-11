import React, {Component} from "react";
import {DataHolder} from "../Help/DataHolder";
import {Dialog} from "primereact/dialog";
import VoiceCallRunner from "./VoiceCallRunner";
import {Button} from "react-bootstrap";
import {MyCaller} from "../Help/Socket";
import Spinner from "react-bootstrap/Spinner";
import {_showError} from "../Pages/LayoutPage";
import ShowWaitingList from "./VoiceCall/ShowWaitingList";
import {CurrentUserInfo} from "../CurrentUserInfo";

export let voiceData = [];


class VoiceCall extends Component {
    state = {
        type: "callToUser",
    };
    videoData = [];

    constructor(props) {
        super(props);
        CurrentUserInfo.VoiceCall = this;

        this.audioRef = React.createRef();
    }

    componentDidMount() {
        CurrentUserInfo.VoiceCall = this;


        if (!DataHolder.selectedCustomerForCall) return;

        this.onClick("displayPosition", "bottom-right");
    }

    show() {
        this.setState({type: 'callToUser'});
        this.onClick("displayPosition", "bottom-right");
    }


    vC_CustomerCallInitCallback(res) {
        if (!res || !res.Content) {
            _showError("vC_CustomerCallInitCallback res is null");
            return;
        }

        if (!res || !res.Content.chatId) {
            _showError("vC_CustomerCallInitCallback  chatId  is null");
            return;
        }

        // در حال مکالمه هستیم
        if (inCalling) {
            // آیا کسی دیگر زنگ میزند ؟
            if (
                DataHolder.selectedCustomerForCall &&
                DataHolder.selectedCustomerForCall.Id != res.Content.Customer.Id
            ) {
                this.setState({
                    isAnotherCallCome: true,
                    anotherCallCustomer: res.Content.Customer,
                    anotherCallChatId: res.Content.chatId,
                });


                if (!this.state.waitingList) {
                    this.state.waitingList = [];
                }


                // --------------------- holding another Customers
                this.state.waitingList.push({
                    anotherCallCustomer: res.Content.Customer,
                    anotherCallChatId: res.Content.chatId,
                })

                this.setState({waitingList: this.state.waitingList, mg: Math.random()})

                _showError("کاربر دیگری در حال تماس به شماست ، در انتظار ");
                return;
            }
        }

        DataHolder.selectedCustomerForCall = res.Content.Customer;
        DataHolder.selectedCustomerForCallChatId = res.Content.chatId;

        this.setState({type: "callRequestFromCustomer"});

        this.onClick("displayPosition", "bottom-right");

    }

    render() {
        if (!DataHolder.selectedCustomerForCall) return <></>;

        return (
            <div>
                <audio controls ref={this.audioRef} style={{display: 'none'}}>
                    <source src={this.audioSource} type="audio/mp3"/>
                </audio>

                <Dialog
                    rtl={true}
                    modal={false}
                    header="برقراری تماس صوتی  "
                    visible={this.state.displayPosition}
                    position={this.state.position}
                    style={{width: "30vw"}}
                    onHide={() => {

                        this.setState({displayPosition: false});

              /*          this.setState({
                            type: 'callToUser'
                        });

                        if (DataHolder.selectedCustomerForCall) {
                            this.Reject();
                            DataHolder.selectedCustomerForCall=null;
                            DataHolder.selectedCustomerForCallChatId=null;
                            
                        }*/
                    }}
                >
                    {this.state.type === "callToUser" && this.callToUser()}
                    {this.state.type === "userCalling" && this.userCalling()}
                    {this.state.type === "openSpeak" && (
                        <VoiceCallRunner onClose={() => {

                            this.cancelCall()
                        }}></VoiceCallRunner>
                    )}
                    {this.state.type === "error" && this.showError()}
                    {this.state.type === "callRequestFromCustomer" &&
                    this.callRequestFromCustomer()}
                    {/*  {this.state.isAnotherCallCome && this.showAnotherCallCome()}*/}

                    {this.state.anotherCallIsWaiting && this.anotherCallIsWaitings()}
                </Dialog>

                {/*another call:*/}
                <Dialog
                    rtl={true}
                    modal={false}
                    header="برقراری تماس صوتی  "
                    visible={this.state.isAnotherCallCome}
                    position={"bottom-left"}
                    style={{width: "30vw"}}
                    onHide={() => this.setState({isAnotherCallCome: false})}
                >
                    {this.state.isAnotherCallCome && this.showAnotherCallCome()}
                </Dialog>


                {this.state.showWaitingList &&

                <Dialog
                    rtl={true}
                    modal={false}
                    header="برقراری تماس صوتی  "
                    visible={true}
                    position={"center"}
                    onHide={() => this.setState({showWaitingList: false})}

                    style={{height: '70vh', width: '50vw'}}
                >
                    <ShowWaitingList list={this.state.waitingList} onWaitingCallSelect={(onWaitingItem) => {

                        if (this.state.type === "openSpeak") {

                            _showError('در حال مکالمه هستید و امکان سوئیچ نیست ، برای مکالمه با این کاربر ابتدا تماس صوتی کنونی را قطع نمایید');
                            return;
                        }

                        debugger;
                        this.state.waitingList = this.state.waitingList.filter(f => f != onWaitingItem);


                        let isAnotherCallCome = this.state.isAnotherCallCome;
                        if (this.state.waitingList.length == 0) {
                            isAnotherCallCome = false;
                        }
                        this.setState({
                            waitingList: this.state.waitingList,
                            mg: Math.random(),
                            isAnotherCallCome: isAnotherCallCome
                        });

                        this.onWaitingCallSelect(onWaitingItem);

                    }}></ShowWaitingList>
                </Dialog>
                }

            </div>
        );
    }

    onWaitingCallSelect(onWaitingItem) {
        let anotherCallCustomer = onWaitingItem.anotherCallCustomer;
        let anotherCallChatId = onWaitingItem.anotherCallChatId;


        DataHolder.selectedCustomerForCall = anotherCallCustomer;
        DataHolder.selectedCustomerForCallChatId = anotherCallChatId;


        ACCESS_TO_VOICE(
            (stream) => {

                MyCaller.Send("VC_AdminIsAcceptOrReject", {
                    err: null,
                    isAccepted: true,
                    customerId: anotherCallCustomer.Id,
                    chatId: anotherCallChatId
                });

                this.setState({type: "openSpeak"});

                startRecording();
            },
            (err) => {
                this.setState({type: "error"});

                this.setState({
                    error: "اجازه دسترسی به میکروفون شما صادر نشد",
                    techError: err + "",
                });
            }
        );
    }

    showError() {
        return (
            <div>
                <p style={{color: "red"}}>{this.state.error}</p>
                <small style={{color: "red"}}>{this.state.techError}</small>
            </div>
        );
    }

    onClick(name, position) {
        let state = {
            [`${name}`]: true,
        };

        if (position) {
            state = {
                ...state,
                position,
            };
        }

        this.setState(state);
    }

    /*<i class="fa fa-microphone" aria-hidden="true"></i>
  <i class="fa fa-headphones" aria-hidden="true"></i>
  */
    callToUser() {
        return (
            <div>
                <small>
                    ابتدا مطمئن شوید میکروفون یا کارت صوتی دستگاه شما درست کار میکند سپس
                    برای شروع تماس دکمه زیر را فشار دهید
                </small>
                <hr/>
                شروع تماس
                <button
                    className={
                        "p-button p-component p-button-rounded p-button-success p-button-icon-only"
                    }
                    onClick={() => {

                        this.timer = setTimeout(() => {


                            this.Reject(
                                DataHolder.selectedCustomerForCall.Id,
                                DataHolder.selectedCustomerForCallChatId,
                            );

                            this.setState({
                                type: 'error',
                                error: 'مهلت زمانی پاسخگویی کاربر به پایان رسید و کاربر جواب نداد'
                            });

                        }, 20 * 1000);
                        this.AccessRequest();
                    }}
                >
                    <i
                        style={{fontSize: "30px"}}
                        className="fa fa-phone"
                        aria-hidden="true"
                    ></i>
                </button>
            </div>
        );
    }

    userCalling() {
        return (
            <>
                <p>درخواست تماس ارسال شد ، در انتظار برای پذیرش کاربر</p>
                <Spinner animation="border" variant="info"/>


                <Button variant={'danger'} onClick={() => {
                    this.cancelCall()
                }}>
                    بستن تماس
                </Button>
            </>
        );
    }

    //todo:
    vC_CustomerIsAcceptOrRejectCallback(res) {

        if (this.timer) {
            clearTimeout(this.timer);
        }

        if (!res || !res.Content) {
            _showError("vC_AdminCallInitCallback res is null");
        }

        if (res.Content.IsAccepted === true) {

            this.setState({type: "openSpeak"});

            startRecording();
        } else {
            stopRecording();
            this.setState({type: "error", error: res.Content.Message});
        }
    }

    VC_AdminCallInit() {
        this.setState({type: "userCalling"});
        MyCaller.Send("VC_AdminCallInit", {
            customerId: DataHolder.selectedCustomerForCall.Id,
        });
    }


    AccessRequest() {
        ACCESS_TO_VOICE(
            (stream) => {
                this.VC_AdminCallInit();
            },
            (err) => {
                this.setState({type: "error"});

                this.setState({
                    error: "اجازه دسترسی به میکروفون شما صادر نشد",
                    techError: err + "",
                });
            }
        );
    }

    showAnotherCallCome() {
        return (
            <div>
                <p>
                    <b>{DataHolder.selectedCustomerForCall.Name}</b>
                    <small>تماس دیگری رسید</small>
                </p>
                <hr/>
                در انتظار باشد
                <button
                    className={
                        "p-button p-component p-button-rounded p-button-danger p-button-icon-only"
                    }
                    onClick={() => {
                        this.Reject(
                            this.state.anotherCallCustomer.Id,
                            this.state.anotherCallChatId
                        );

                        this.setState({isAnotherCallCome: null});

                    }}
                >
                    <i
                        style={{fontSize: "30px"}}
                        className="fa fa-phone"
                        aria-hidden="true"
                    ></i>
                </button>
                رد کردن
                <button
                    className={
                        "p-button p-component p-button-rounded p-button-warning p-button-icon-only"
                    }
                    onClick={() => {


                        this.Wait(
                            this.state.anotherCallCustomer.Id,
                            this.state.anotherCallChatId
                        );

                        this.setState({isAnotherCallCome: null});
                        this.setState({anotherCallIsWaiting: true});
                    }}
                >
                    <i
                        style={{fontSize: "30px"}}
                        className="fa fa-phone"
                        aria-hidden="true"
                    ></i>
                </button>
            </div>
        );
    }


    Answer() {
        ACCESS_TO_VOICE(
            (stream) => {
                MyCaller.Send("VC_AdminIsAcceptOrReject", {
                    err: null,
                    isAccepted: true,
                    customerId: DataHolder.selectedCustomerForCall.Id,
                    chatId: DataHolder.selectedCustomerForCallChatId,
                });
                this.setState({type: "openSpeak"});

                startRecording();
            },
            (err) => {
                this.setState({type: "error"});

                this.setState({
                    error: "اجازه دسترسی به میکروفون شما صادر نشد",
                    techError: err + "",
                });
            }
        );
    }

    Reject(customerId, chatId) {
        MyCaller.Send("VC_AdminIsAcceptOrReject", {
            err: null,
            isAccepted: false,
            customerId: customerId
                ? customerId
                : DataHolder.selectedCustomerForCall.Id,
            chatId: chatId ? chatId : DataHolder.selectedCustomerForCallChatId,
        });

        // if (customerId) this.onHide("displayPosition");
    }

    callRequestFromCustomer() {
        return (
            <div>
                <p>
                    <b>{DataHolder.selectedCustomerForCall.Name}</b>
                    <small>در حال تماس به شماست</small>
                </p>
                <hr/>
                پاسخ
                <button
                    className={
                        "p-button p-component p-button-rounded p-button-danger p-button-icon-only"
                    }
                    onClick={() => {
                        this.setState({
                            type: 'callToUser'
                        });

                        this.Reject();
                    }}
                >
                    <i
                        style={{fontSize: "30px"}}
                        className="fa fa-phone"
                        aria-hidden="true"
                    ></i>
                </button>
                رد کردن
                <button
                    className={
                        "p-button p-component p-button-rounded p-button-success p-button-icon-only"
                    }
                    onClick={() => {

                        this.Answer();

                    }}
                >
                    <i
                        style={{fontSize: "30px"}}
                        className="fa fa-phone"
                        aria-hidden="true"
                    ></i>
                </button>
            </div>
        );
    }

    Wait(customerId, chatId) {
        if (!customerId || !chatId) {
            _showError("Wait(customerId,chatId) is null");
            return;
        }

        MyCaller.Send("VC_AdminInAnotherCalling", {
            customerId: customerId,
            chatId: chatId,
        });
    }

    vC_CustomerSpeakCallback(res) {

        // ----------- اگر صحبت کننده آنی نباشد که انتخاب شده است
        if (!this.audioRef ||
            !DataHolder.selectedCustomerForCall ||
            DataHolder.selectedCustomerForCall.Id != res.Content.CustomerId
        ) {
            MyCaller.Send("VC_AdminIsAcceptOrReject", {
                err: null,
                isAccepted: false,
                customerId: res.Content.CustomerId,
                // chatId: chatId ? chatId : DataHolder.selectedCustomerForCallChatId
            });
        }

        if (!res || !res.Content || !res.Content.buffer) {
            _showError("ویدئو برگشتی فرمت صحیح ندارد");
            return;
        }


        let base64 = btoa(res.Content.buffer)
        let blob = b64toBlob(base64, 'audio/mp3');

        const videoElem = this.audioRef.current;


        this.videoData.push(blob);

        let wholeVideoBlob = new Blob(this.videoData, {'type': 'audio/mp3'})

        let blobUrl = URL.createObjectURL(wholeVideoBlob);


        let lastTime = videoElem.currentTime ? videoElem.currentTime : 0;

        this.audioSource = blobUrl;

        videoElem.load()

        videoElem.currentTime = lastTime;
        videoElem.play();


        this.setState({rn: Math.random()})
    }

    anotherCallIsWaitings() {
        return (
            <>
                <i
                    style={{fontSize: "30px"}}
                    className="fa fa-frown-o"
                    aria-hidden="true"
                ></i>
                <small>کاربران در حال انتظار</small>

                <Button onClick={() => {

                    this.setState({showWaitingList: true})

                }}>
                    نمایش لیست در انتظار
                </Button>
            </>
        );
    }

    cancelCall() {
        console.log('<VoiceCallRunner->DataHolder.selectedCustomerForCall', DataHolder.selectedCustomerForCall)
        console.log('<VoiceCallRunner->DataHolder.selectedCustomerForCallChatId', DataHolder.selectedCustomerForCallChatId)


        this.setState({
            type: 'callToUser'
        });

        this.Reject(
            DataHolder.selectedCustomerForCall.Id,
            DataHolder.selectedCustomerForCallChatId,
        );

    }
}


export default VoiceCall;

let streamrecorder,
    webcamstream,
    mediaRecorder,
    inCalling = false,
    chunks = [];

function startRecording() {
    inCalling = true;
    mediaRecorder = new MediaRecorder(webcamstream);
    mediaRecorder.start(3000);
    mediaRecorder.ondataavailable = function (e) {
        new Promise((resolve) => {
            const fileReader = new FileReader();
            fileReader.addEventListener("loadend", () => {
                //debugger
                // videoElem.src=fileReader.result;
                /*  var dataUrl = fileReader.result;
                          var base64 = dataUrl.split(',')[1];*/

                resolve(fileReader.result);
            });

            fileReader.readAsBinaryString(e.data);
        }).then(function (buffer) {
            MyCaller.Send("VC_AdminSpeak", {
                customerId: DataHolder.selectedCustomerForCall.Id,
                buffer,
            });
        });
        chunks.push(e.data);
    };
}

function stopRecording() {
    if (mediaRecorder)
        mediaRecorder.stop();
    inCalling = false;
}

function ACCESS_TO_VOICE(successCallback, failCallback) {
    navigator.getUserMedia =
        navigator.getUserMedia ||
        navigator.webkitGetUserMedia ||
        navigator.mozGetUserMedia ||
        navigator.msGetUserMedia;

    // Get access to microphone
    navigator.getUserMedia(
        // Only request audio
        {video: false, audio: true},

        // Success callback
        function success(localAudioStream) {
            // Do something with audio stream

            webcamstream = localAudioStream;
            voiceData = [];
            successCallback(localAudioStream);
        },
        // Failure callback
        function error(err) {
            // handle error
            failCallback(err);
        }
    );
}


const b64toBlob = (b64Data, contentType = '', sliceSize = 512) => {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        const slice = byteCharacters.slice(offset, offset + sliceSize);

        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, {type: contentType});
    return blob;
}