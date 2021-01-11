import React, {Component} from 'react';
import {_showError} from "../Pages/LayoutPage";
import {voiceData} from "./VoiceCall";
import {b64toBlob} from "./ScreenRecordShower";
import {Button, Spinner} from 'react-bootstrap';
import {CurrentUserInfo} from "../CurrentUserInfo";

class VoiceCallRunner extends Component {

    constructor(props) {
        super(props);
        CurrentUserInfo.VoiceCallRunner=this;
    }
    
    VC_CustomerSpeak(res){

        if (!res || !res.Content){
            _showError('vC_AdminCallInitCallback res is null')
        }

        let base64 = btoa(res.Content.buffer)
        let blob = b64toBlob(base64, 'voice/mp4');

        voiceData.push(blob);

        let wholeVideoBlob = new Blob(voiceData, {'type': 'video/webm'})

        let blobUrl = URL.createObjectURL(wholeVideoBlob);

        // برای اولین بار نال است
        if (this.voiceElem) {
            // blobUrl= URL.revokeObjectURL(blobUrl)

            let lastTime = this.voiceElem.currentTime ? this.voiceElem.currentTime : 0;

            this.voiceElem.src = blobUrl;

            this.voiceElem.load()

            this.voiceElem.currentTime = lastTime;
            this.voiceElem.play();
        }
    }
    render() {
        return (
            <div>
                

                تماس برقرار است 
                <Spinner animation="grow" variant="success" />

              {/*   <audio     
                    
                    ref={node => this.voiceElem = node}
                >
                    
                </audio> */}
                
                
                <Button variant={'danger'} onClick={()=>{
                    this.props.onClose();
                }}>
                    بستن تماس
                </Button>
            </div>
        );
    }
}

export default VoiceCallRunner;