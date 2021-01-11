import React, {Component} from 'react';
import {_showError} from "../Pages/LayoutPage";
import {MyCaller} from './../Help/Socket';
import ReactDOM from 'react-dom'
import {CurrentUserInfo} from "../CurrentUserInfo";

class ScreenRecordShower extends Component {
    state = {
        videoData: []
    };

    constructor(arg) {
        super(arg);



    }
    videoElementRef(e){
        //debugger;
        console.log(e)
    }
    
    componentDidMount() {
        CurrentUserInfo.ScreenRecordShower = this;

    }
    
    componentWillUnmount() {
        CurrentUserInfo.ScreenRecordShower=null;
    }

    render() {
        return (
            <div>

                <video
                    style={{display:this.state.currentShowingPart ? null :'none'}}
                    ref={node => this.videoElem = node}
                    controls
                    id='videoElem'
                    src={this.state.currentShowingPart}
                    width={350} height={350}
                    onEnded={() => {
                        this.videoPartEnd()
                    }}
                    autoPlay></video>


            </div>
        );
    }

    screenRecordSaveCallback(res) {

        if (!res || !res.Content || !res.Content.buffer) {
            _showError("ویدئو برگشتی فرمت صحیح ندارد");
            return;
        }


        let base64=btoa(res.Content.buffer)
        let blob = b64toBlob(base64, 'video/webm');

        

        this.state.videoData.push(blob);

            this.showNextPart();
    }

    videoPartEnd() {

        //debugger;


       /* this.setState({currentShowingPart:null })*/



       /* this.showNextPart();*/

    }

    makeBlobAndShow(blob) {
        let blobUrl = URL.createObjectURL(blob);


        let blobUrl2 = URL.revokeObjectURL(blob);

        const videoElem = this.videoElem; 


      // برای اولین بار نال است
        if (videoElem){
            // blobUrl= URL.revokeObjectURL(blobUrl)

            let lastTime = this.videoElem.currentTime ? this.videoElem.currentTime : 0;
            //videoElem.src=blobUrl;
            videoElem.load()

            videoElem.currentTime = lastTime;
            videoElem.play();
        }
        return blobUrl;

    }

    showNextPart() {
        let wholeVideoBlob=new Blob(this.state.videoData,{ 'type' : 'video/webm' })
        this.setState({lastStopTime:this.videoElem.currentTime})

      
            let blob= this.makeBlobAndShow(wholeVideoBlob)
            this.setState({currentShowingPart:blob })

        
    }
}

export default ScreenRecordShower;

export const b64toBlob = (b64Data, contentType='', sliceSize=512) => {
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
