import React, {Component} from 'react';
import useScript from "../Utilities/UseScript";

import '../../styles/leaflet.css'
import {Spinner} from "react-bootstrap";
import {TabPanel} from "primereact/tabview";
import {DataHolder} from "../../Help/DataHolder";
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";

class MyMap extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.MyMap=this;
    }

    componentDidMount() {
        window["addressArr"]=[];
        window["addressArrParent"]=this;
        let addressArr=[];
        
        if (this.props.users && this.props.users.length>0){

            for (let i = 0; i < this.props.users.length; i++) {
                
                addressArr.push({
                    latitude:this.props.users[i].latitude,
                    longitude:this.props.users[i].longitude,
                    customer:this.props.users[i]
                })
                
            }
        }
        
        
        if (DataHolder.selectedCustomer && DataHolder.selectedCustomer.LastTrackInfo){

            addressArr.push(
                {
                latitude:DataHolder.selectedCustomer.LastTrackInfo.latitude,
                longitude:DataHolder.selectedCustomer.LastTrackInfo.longitude,
                    customer:DataHolder.selectedCustomer,
                    
            });
        }
        
        window["addressArr"]=addressArr;
        
        
    }
    
    
    AddMarker(lat,lan,customer){
        if (window['AddMarker']){
            window['AddMarker'](lat,lan,customer);
        }
    }
    
    
    onOpenChat(user){

        DataHolder.currentPage = null;
        CurrentUserInfo.LayoutPage.setState({
            temp: Math.random(),
        });
        this.setState({ temp: Math.random() });

        if(CurrentUserInfo.CustomersPage){
            MyCaller.Send('selectCustomerForChat',{

                customerId:user.Id

            })
        }
        
    }
    
    /*        public double? latitude { get; set; }
        public double? longitude { get; set; }*/

    render() {
     /*   if (!DataHolder.selectedCustomer || 
        !this.state.latitude || !this.state.longitude){
            return  <></>
        }*/
        
        
        return (
            <div>
{/*

                <input value={this.state.latitude} type="hidden" hidden={'hidden'} id={'latitude'}/>
                <input  value={this.state.longitude}  type="hidden" hidden={'hidden'} id={'longitude'}/>*/}

<MyMapBody/>
                      
                      
          
            </div>
        );
    }
}

export default MyMap;




const MyMapBody=()=>{
    useScript('https://unpkg.com/leaflet@1.7.1/dist/leaflet.js')
    useScript('/Content/MyMap/mapInit.js')

    
    return                 <div id="mapid">       
   
        <div id={'loading'}>
            <Spinner animation="border" role="status">
                <span className="sr-only">در حال خواندن اطلاعات...</span>
            </Spinner>
            <p>در حال خواندن نقشه</p>
        </div>
    </div>

}