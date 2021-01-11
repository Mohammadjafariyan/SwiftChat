import React, {Component} from 'react';
import Card from "react-bootstrap/Card";
import {MySwitcher} from "../../Utilities/Utility";
import {SetEventTrigger} from "../EventTriggerForm";
import {DataHolder} from "../../../Help/DataHolder";
import {CurrentUserInfo} from "../../../CurrentUserInfo";



class EtFormTriggerPlatforms extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.EtFormTriggerPlatforms=this;
    }
    setRunInDesktopDevices(RunInDesktopDevices){
        this.setState({RunInDesktopDevices})
    }
    componentDidMount() {
        SetEventTrigger(DataHolder.selectedEventTrigger);
    }

    setRunInMobileDevices(RunInMobileDevices){
        this.setState({RunInMobileDevices})
    }

    getRunInDesktopDevices(){
        return this.state.RunInDesktopDevices
    }

    getRunInMobileDevices(){
        return this.state.RunInMobileDevices
    }

    isValid() {
return true;
        
    }
    render() {
        return (
            <div>
                <Card
                    bg={'light'}
                >
                    <Card.Header>
                        <span className={'float-right'}>
                          Event Trigger پلتفرم های   
                        </span>

                        <small className={'float-left '} style={{color:'#6c757d'}}>
                            Event Trigger      تنظیمات مربوط به پلتفرم
                        </small>

                    </Card.Header>
                    <Card.Body>
                        {/*<Card.Title>{variant} Card Title </Card.Title>*/}
                        <Card.Text>

                            <MySwitcher  checked={true} icon={'television'} title={'در دستگاه های دسکتاپ اجرا شود'} name={'RunInDesktopDevices'} parent={this}>


                            </MySwitcher>

                            <hr/>
                            <MySwitcher checked={true} icon={'mobile'} title={'در دستگاه های موبایل اجرا شود'} name={'RunInMobileDevices'} parent={this}>

                            </MySwitcher>
                      


                        </Card.Text>
                    </Card.Body>
                </Card>
            </div>
        );
    }
}

export default EtFormTriggerPlatforms;