import React, {Component} from 'react';
import EtFormInformation from "./TriggerHelpForms/ETFormInformation";
import EtFormTriggerActions from "./TriggerHelpForms/ETFormTriggerActions";
import EtFormTriggerEvents from "./TriggerHelpForms/ETFormTriggerEvents";
import EtFormTriggerBehaviours from "./TriggerHelpForms/ETFormTriggerBehaviours";
import EtFormTriggerPlatforms from "./TriggerHelpForms/ETFormTriggerPlatforms";
import EventTriggerDelete from "./EventTriggerDelete";
import {DataHolder} from "../../Help/DataHolder";
import Button from "react-bootstrap/Button";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import {_showError} from "../../Pages/LayoutPage";
import GlobalLoading, { _SetLoading } from './../Utilities/GlobalLoading';



class EventTriggerForm extends Component {
    state = {};

    render() {
        return (
            <div>


                <Row>
                    <Col></Col>
                    <Col style={{display: 'flex'}}>

                        <EventTriggerDelete/>
                        <SaveSelectedEventTrigger/>

                    </Col>
                    <Col></Col>

                </Row>

                <hr/>


                <EtFormInformation/>

                <br/>
                <EtFormTriggerActions/>

                <br/>


                <EtFormTriggerEvents/>


                <br/>

                <EtFormTriggerBehaviours/>
                <br/>

                <EtFormTriggerPlatforms/>


            </div>
        );
    }
}

export default EventTriggerForm;


const SaveSelectedEventTrigger = () => {
    if (!DataHolder.selectedEventTrigger) {

        return <></>;
    }

    return (
        <div>


            <Button

id="saveEventTrigger"
                variant={'success'}

                onClick={() => {


                    let isValid = 0;

                    /*------------------------------validate:------------------------------*/

                    /*info*/
                    isValid = CurrentUserInfo.EtFormInformation.isValid();
                    if (!isValid)
                        return false;

                    /*actions*/
                    isValid = CurrentUserInfo.EtFormTriggerActions.isValid();
                    if (!isValid)
                        return false;

                    /*Behaviours */
                    isValid = CurrentUserInfo.EtFormTriggerBehaviours.isValid();
                    if (!isValid)
                        return false;

                    /*Events*/
                    isValid = CurrentUserInfo.EtFormTriggerEvents.isValid();
                    if (!isValid)
                        return false;

                    /*EtFormTriggerPlatforms*/
                    isValid = CurrentUserInfo.EtFormTriggerPlatforms.isValid();


                    if (!isValid)
                        return false;

                    /*------------------------------end validate:------------------------------*/


                    /*------------------------------getvalues:------------------------------*/

                    let eventTrigger = DataHolder.selectedEventTrigger;
               /*     /!*info*!/
                    eventTrigger['Name'] = CurrentUserInfo.EtFormInformation.getName();


                    /!*actions*!/
                    eventTrigger['IsShowMessageEnabled'] = CurrentUserInfo.EtFormTriggerActions.getIsShowMessageEnabled();
                    eventTrigger['IsOpenChatBox'] = CurrentUserInfo.EtFormTriggerActions.getIsOpenChatBox();
                    eventTrigger['IsPlayASound'] = CurrentUserInfo.EtFormTriggerActions.getIsPlayASound();
                    eventTrigger['localizedMessages'] = CurrentUserInfo.EtFormTriggerActions.getlocalizedMessages();


                    /!*Behaviours *!/
                    eventTrigger['ExecuteOnlyIfOnline'] = CurrentUserInfo.EtFormTriggerBehaviours.getExecuteOnlyIfOnline();
                    eventTrigger['ExecuteOnlyIfFirstTimeVisit'] = CurrentUserInfo.EtFormTriggerBehaviours.getExecuteOnlyIfFirstTimeVisit();
                    eventTrigger['ExecuteOnlyIfNoOtherTriggerFired'] = CurrentUserInfo.EtFormTriggerBehaviours.getExecuteOnlyIfNoOtherTriggerFired();
                    eventTrigger['ExecuteOnlyIfFromACountry'] = CurrentUserInfo.EtFormTriggerBehaviours.getExecuteOnlyIfFromACountry();
                    eventTrigger['Counties'] = CurrentUserInfo.EtFormTriggerBehaviours.getCounties();

                    /!*Events*!/
                    eventTrigger['S_EventOnExitTab'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventOnExitTab();
                    eventTrigger['S_EventOnLinkClick'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventOnLinkClick();
                    eventTrigger['S_EventSpecificPages'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventSpecificPages();
                    eventTrigger['S_EventAddressParameters'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventAddressParameters();
                    eventTrigger['S_EventUserCustomName'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventUserCustomName();
                    eventTrigger['S_EventDelay'] = CurrentUserInfo.EtFormTriggerEvents.getS_EventDelay();


                    eventTrigger['links'] = CurrentUserInfo.EtFormTriggerEvents.getlinks();
                    eventTrigger['pages'] = CurrentUserInfo.EtFormTriggerEvents.getpages();
                    eventTrigger['pageParameters'] = CurrentUserInfo.EtFormTriggerEvents.getpageParameters();
                    eventTrigger['userEventNames'] = CurrentUserInfo.EtFormTriggerEvents.getuserEventNames();


                    /!*Platforms*!/
                    eventTrigger['RunInMobileDevices'] = CurrentUserInfo.EtFormTriggerPlatforms.getRunInMobileDevices()
                    eventTrigger['RunInDesktopDevices'] = CurrentUserInfo.EtFormTriggerPlatforms.getRunInDesktopDevices()
*/
                    /*------------------------------end getvalues:------------------------------*/


                    /*------------------------------در حال لود:------------------------------*/

                    CurrentUserInfo.EventTriggersPage.setState({loading: true})


                    /*------------------------------ذخیره :------------------------------*/

                    _SetLoading(true);
                    MyCaller.Send('EventTriggerSave', eventTrigger);


                }}
            >
                ذخیره
                <i className={'fa fa-check-circle-o'}></i>
            </Button>

        </div>
    )

}


export const SetEventTrigger=(eventTrigger)=>{
    /*info*/

    DataHolder.selectedEventTrigger=eventTrigger;

    /* if (CurrentUserInfo.EtFormInformation)
     CurrentUserInfo.EtFormInformation.setName(eventTrigger['Name']);
 
 
     /!*actions*!/
     if (CurrentUserInfo.EtFormTriggerActions){
         CurrentUserInfo.EtFormTriggerActions.setIsShowMessageEnabled(eventTrigger['IsShowMessageEnabled']);
         CurrentUserInfo.EtFormTriggerActions.setIsOpenChatBox( eventTrigger['IsOpenChatBox']);
         CurrentUserInfo.EtFormTriggerActions.setIsPlayASound(eventTrigger['IsPlayASound'] );
         CurrentUserInfo.EtFormTriggerActions.setlocalizedMessages(eventTrigger['localizedMessages']);
 
     }
      
 
     /!*Behaviours *!/
     if (CurrentUserInfo.EtFormTriggerBehaviours) {
 
         CurrentUserInfo.EtFormTriggerBehaviours.setExecuteOnlyIfOnline(eventTrigger['ExecuteOnlyIfOnline']);
         CurrentUserInfo.EtFormTriggerBehaviours.setExecuteOnlyIfFirstTimeVisit(eventTrigger['ExecuteOnlyIfFirstTimeVisit']);
         CurrentUserInfo.EtFormTriggerBehaviours.setExecuteOnlyIfNoOtherTriggerFired(eventTrigger['ExecuteOnlyIfNoOtherTriggerFired']);
         CurrentUserInfo.EtFormTriggerBehaviours.setExecuteOnlyIfFromACountry(eventTrigger['ExecuteOnlyIfFromACountry']);
     }
     if (CurrentUserInfo.EtFormTriggerBehaviours) {
     CurrentUserInfo.EtFormTriggerBehaviours.setCounties(eventTrigger['Counties']);
     }
 
     /!*Events*!/
     if (CurrentUserInfo.EtFormTriggerEvents) {
 
         CurrentUserInfo.EtFormTriggerEvents.setS_EventOnExitTab( eventTrigger['S_EventOnExitTab']);
     CurrentUserInfo.EtFormTriggerEvents.setS_EventOnLinkClick(eventTrigger['S_EventOnLinkClick'] );
     CurrentUserInfo.EtFormTriggerEvents.setS_EventSpecificPages( eventTrigger['S_EventSpecificPages']);
     CurrentUserInfo.EtFormTriggerEvents.setS_EventAddressParameters(eventTrigger['S_EventAddressParameters'] );
      CurrentUserInfo.EtFormTriggerEvents.setS_EventUserCustomName(eventTrigger['S_EventUserCustomName'] );
     CurrentUserInfo.EtFormTriggerEvents.setS_EventDelay(eventTrigger['S_EventDelay'] );
     }
 
     if (CurrentUserInfo.EtFormTriggerEvents) {
 
         CurrentUserInfo.EtFormTriggerEvents.setlinks(eventTrigger['links']);
     CurrentUserInfo.EtFormTriggerEvents.setpages(eventTrigger['pages']);
     CurrentUserInfo.EtFormTriggerEvents.setpageParameters(eventTrigger['pageParameters']);
     CurrentUserInfo.EtFormTriggerEvents.setuserEventNames(eventTrigger['userEventNames']);
     }
 
 
     
     
     /!*Platforms*!/
     if (CurrentUserInfo.EtFormTriggerPlatforms) {
 
         CurrentUserInfo.EtFormTriggerPlatforms.setRunInMobileDevices(eventTrigger['RunInMobileDevices'] )
      CurrentUserInfo.EtFormTriggerPlatforms.setRunInDesktopDevices(eventTrigger['RunInDesktopDevices']);
     }*/

}