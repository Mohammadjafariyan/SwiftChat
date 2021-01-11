import React, {Component} from 'react';
import {_showConstantMsg, _showError, _showMsg} from "../Pages/LayoutPage";
import Spinner from "react-bootstrap/Spinner";
import {DataHolder} from "../Help/DataHolder";
import {MyCaller} from "../Help/Socket";
import {CurrentUserInfo} from "../CurrentUserInfo";

class ShowMyScreen extends Component {
    state={msg:'\n' +
            '                در انتظار تایید دسترسی به اسکرین لطفا منتظر بمانید\n' +
            '            '}
    componentDidMount() {
        
        this.ScreenRecordAdminShareRequest();
        
        CurrentUserInfo.ShowMyScreen=this;
       
    }
    
    //todo
    ScreenRecordAdminShareRequest(){
        
        MyCaller.Send('ScreenRecordAdminShareRequest',{
            customerId:DataHolder.selectedCustomer.Id
        });
    }

    //todo
    screenRecordAdminShareRequestCallback(res){
        if (!res || !res.Content){
            _showError('screenRecordAdminShareRequestCallback res  is null')
            return ;
        }
        this.setState({chatId:res.Content.chatId})
        VIDEO_CALL_INIT(this,(parent,msg,err)=>{

            parent.setState({msg:msg,err:err})

        },res.Content.chatId);
        
    }
   
    screenRecordAdminShareOpenByCustomer(res){
        //todo


        //VIDEO_CALL_STOP(this.state.chatId);

        //  _showConstantMsg(null)

        _showMsg('کاربر صفحه مانیتور آنلاین شما را باز کرد')

    }
    screenRecordAdminShareCloseByCustomer(res){
        //todo


        //VIDEO_CALL_STOP(this.state.chatId);

      //  _showConstantMsg(null)
        
        _showError('کاربر صفحه مانیتور آنلاین شما را بست')
        
    }
    
    
    componentWillUnmount() {

        //todo
        MyCaller.Send('ScreenRecordAdminShareClose', { targetCustomerId: DataHolder.selectedCustomer.Id,
            chatId:this.state.chatId})
    }


    render() {
        
        if (!DataHolder.selectedCustomer){
            return <p className={'text-danger'}>کاربری انتخاب نشده است</p>
        }
        
        return (
            <div>
                {this.state.err &&
                <p style={{color:'darkred'}}>{this.state.msg}</p>}
                <Spinner animation="grow" variant="info" />


                {!this.state.err &&
                <p style={{color:'green'}}>{this.state.msg}</p>}
                
                
                
            </div>
        );
    }
}

export default ShowMyScreen;


/*VIDEO_CALL :*/
let videoElem;
let mediaSource;
let chunks = [];
let mediaRecorder;

let captureStream;

function configStream() {
    /*   let blob = new Blob(buffer, { 'type' : 'video/webm' });
                   let audioURL = window.URL.createObjectURL(blob);
                   */


    videoElem.captureStream();
}

/*VOICE CALL END*/
function VIDEO_CALL_STOP(chatId) {
    try {
        mediaRecorder.stop();

    } catch (e) {
        //ingore
    }
    console.log(mediaRecorder.state);
    console.log("recorder stopped");


    captureStream.getTracks()
        .forEach(track => track.stop())


    /*MyCaller.Send('ScreenRecordCustomerClose', {myAccountId: CurrentUserInfo.targetId,
        msg:msg});*/

    MyCaller.Send('ScreenRecordAdminShareClose', { targetCustomerId: DataHolder.selectedCustomer.Id,
        chatId:chatId})

}

function setRecordVideo() {
 
}

function VIDEO_CALL_INIT(THIS, callback,chatId) {


    if (navigator.mediaDevices.getDisplayMedia) {
        console.log('getUserMedia supported.');

        var constraints = {
            video: {
                cursor: "always"
            },
            audio: false
        };


        let onSuccess = function (stream) {

            _showConstantMsg('در حال ارسال صفحه مرورگر انتخابی خود هستید',null,
                ()=>{
                    VIDEO_CALL_STOP(chatId);

                    _showConstantMsg(null)
                })

            setRecordVideo();


            captureStream = stream;
            debugger;


            callback(THIS, 'اجازه دسترسی داده شد');
            //debugger;
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start(3000);


            //videoElem.srcObject=stream;


            console.log(mediaRecorder.state);


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



                    //todo
                    MyCaller.Send('ScreenRecordAdminShare',
                        { targetCustomerId: DataHolder.selectedCustomer.Id,chatId:chatId ,buffer})
                })
                chunks.push(e.data);
            }
        }

        let onError = function (err) {
            callback(THIS, 'اجازه دسترسی داده نشد', err);
            console.error('The following error occured: ' + err);


        }

        navigator.mediaDevices.getDisplayMedia(constraints).then(onSuccess, onError);

    } else {
        callback(THIS, 'این امکان در مرورگر شما پشتیبانی نمی شود');
        alert('این امکان در مرورگر شما پشتیبانی نمی شود')
        console.error('getUserMedia not supported on your browser!');
    }


    window.onresize = function () {
        //canvas.width = mainSection.offsetWidth;
    }

    window.onresize();


}

/*VOICE CALL:*/

/*

export function onTheFlyVideoRunning(){
    
    CurrentUserInfo.onTheFlyVideoRunning=this;
    
    return <div style="position: fixed;
    /!* width: 200px; *!/
    /!* height: 200px; *!/
    top: 55vh;
    left: 10vh;
    background-color: #f8f9fa;
    padding: 20px;
}">
        <p>صفحه مانیتور شما در حال نمایش برای پشتیبانی است</p>

        <video autoPlay id="gapOnlineVideo" width="200" height="200">

        </video>

        <button className="gapMainColor "
                style="background-color: lightgrey;padding: 5px;border-radius: 3px 3px 3px 3px ;border:none"
                onClick="VIDEO_CALL_STOP(this,'توسط کاربر بسته شد');setMsg(this,'توسط کاربر بسته شد',null,false)">لغو
            نمایش
        </button>

        <button className="gapMainColor GAP_VIDEO_CALL_STOP" style="display: none"
                onClick="VIDEO_CALL_STOP(this,'توسط پشتیبانی بسته شد');setMsg(this,'توسط پشتیبانی بسته شد',null,false)"></button>
    </div>;
}*/
