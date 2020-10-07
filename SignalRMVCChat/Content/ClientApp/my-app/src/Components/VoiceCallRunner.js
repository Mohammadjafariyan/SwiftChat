import React, {Component} from 'react';
import {_showError} from "../Pages/LayoutPage";
import {voiceData} from "./VoiceCall";
import {b64toBlob} from "./ScreenRecordShower";

class VoiceCallRunner extends Component {

    VC_CustomerSpeark(res){

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
                
                <audio     
                    
                    ref={node => this.voiceElem = node}
                >
                    
                </audio>
            </div>
        );
    }
}

export default VoiceCallRunner;