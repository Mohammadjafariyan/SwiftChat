import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import * as useragent from "useragent";


import '../styles/myStyle.css'
class UserDeviceInfo extends Component {
    state={};
    on() {
        
        /*    {
                version: '3'
                webkit: false
                opera: false
                ie: false
                chrome: false
                safari: false
                mobile_safari: false
                firefox: true
                mozilla: true
                android: false
            }*/
        
    }
    
    setValues(){
        debugger;

       
    }

    render() {
   
     
        if (!DataHolder.selectedCustomer)
            return <></>;

       if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.LastTrackInfo ||
           !DataHolder.selectedCustomer.LastTrackInfo.Browser)
            return <></>;

        var agent = useragent.parse(DataHolder.selectedCustomer.LastTrackInfo.Browser);

        var ua = useragent.is(DataHolder.selectedCustomer.LastTrackInfo.Browser)
        var browser= agent.toAgent(); // 'Chrome 15.0.874'
        var version= agent.toVersion();
        var os= agent.os.toString();
        var osVersion= agent.os.toVersion();
        var device= agent.device.toString();
        
        var browserTypeName;
        
        if (ua.chrome){
            browserTypeName='chrome';
        } else if(ua.mozilla || ua.firefox){
            browserTypeName='firefox';
        } else if(ua.android){
            browserTypeName='android';
        }
        else if(ua.mobile_safari || ua.safari){
            browserTypeName='safari';
        }
        else if(ua.opera ){
            browserTypeName='opera';
        }
      



        this.values={
            ua,
            browser,
            version,
            os,
            osVersion,
            device
        };

        return (
            <div>
                <div
                    className={"card "}>
                    <div className="card-header">
                       دستگاه کاربر
                    </div>

                    <div className="card-body">

                        <ul style={{listStyle:'none',fontSize:'27px'}}>
                            <li aria-label="مرورگر کاربر" data-microtip-position="left" role="tooltip">
                                <i className={'floatLeft fa fa-'+browserTypeName}></i>
                                <Badge>

                                    {this.values.browser}

                                </Badge>
                            </li>
                            <li aria-label="آیپی کاربر" data-microtip-position="left" role="tooltip">
                                <i className={'floatLeft fa fa-cloud'}></i>
                                <Badge>

                                    {DataHolder.selectedCustomer.LastTrackInfo.ip}

                                </Badge>
                            </li>
                            <li style={{display:'flex'}} aria-label="آخرین صفحه بازدید" data-microtip-position="left" role="tooltip">
                                <i className={'floatLeft fa fa-send'}></i>
                                <Badge className={'text-summary2'} >

                                   <small title= {DataHolder.selectedCustomer.LastTrackInfo.PageTitle}>
                                       <a className={'smallLink'} href={DataHolder.selectedCustomer.LastTrackInfo.Url} target={'_blank'}>
                                           {DataHolder.selectedCustomer.LastTrackInfo.PageTitle}
                                       </a>
                                   </small>
                                    

                                </Badge>
                            </li>
                            <li aria-label="آخرین ساعت دریافتی از کاربر" data-microtip-position="left" role="tooltip">
                                <i className="floatLeft fa fa-clock-o" aria-hidden="true"></i>

                                <Badge>

                                    {DataHolder.selectedCustomer.LastTrackInfo.Time}

                                </Badge>
                            </li>

                            <li aria-label="سیستم عامل" data-microtip-position="left" role="tooltip">
                                <i className={'floatLeft fa fa-windows'}></i>
                                <Badge>

                                    {this.values.os}       

                                </Badge>
                            </li>


                            <li aria-label="دستگاه کاربر" data-microtip-position="left" role="tooltip">

                                <i className={'floatLeft fa fa-television'}></i>
                                <Badge>

                                    {this.values.device}

                                </Badge>
                            </li>

                            
                            {DataHolder.selectedCustomer.LastTrackInfo.CountryLanguage && 
                            <li aria-label="زبان مرورگر کاربر" data-microtip-position="left" role="tooltip">

                                <i className={'floatLeft fa fa-globe'}></i>
                                <Badge pill variant="light">
                                    <img width={50} height={50}
                                        src={`https://www.countryflags.io/${DataHolder.selectedCustomer.LastTrackInfo.CountryLanguage}/flat/64.png`}/>
                                </Badge>
                            </li>

                            }

                        </ul>
                     

                

         
               

            

           


                     
                    </div>
                </div>
            </div>
        );
    }
}

export default UserDeviceInfo;