import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import {CurrentUserInfo} from "../Help/Socket";

class MarkAsResovled extends Component {
    render() {
        if (!DataHolder.selectedCustomer){
            return  <></>;
        }
        
        
        return (
            <div>


                <div onClick={()=>{
                    if (CurrentUserInfo.CustomerInfo){
                        CurrentUserInfo.CustomerInfo.setState({tmp:Math.random()});

                    }
                    CurrentUserInfo.ChatPage.setState({tmp:Math.random()});

                    
                    if (!DataHolder.selectedCustomer.IsResolved){
                        DataHolder.selectedCustomer.IsResolved=true;
                    }else{
                        DataHolder.selectedCustomer.IsResolved=false;
                    }
                    this.setState({IsResolved:false});

                }}
                    className={"card  "}>
                    <div className={"card-header " + (DataHolder.selectedCustomer.IsResolved ? 'bg-success text-white' : 'bg-danger text-white') }>

                        {DataHolder.selectedCustomer.IsResolved &&

                      <>
                          <Badge  variant={'success'} aria-label="علامت بصورت حل نشده" data-microtip-position="top" role="tooltip">
                              <i className={'fa fa-check'}></i>



                          </Badge>
                          
                          <small>بصورت حل شده علامت زده شد</small>
                      
                      </>
                            
                            



                        }

                        {!DataHolder.selectedCustomer.IsResolved &&

                        <>
                            <Badge  variant={'danger'} aria-label="علامت بصورت حل شده" data-microtip-position="top" role="tooltip">

                                <i className={'fa fa-close'}></i>
                            </Badge>

                            <small>مشکل این مکالمه حل نشده است</small>


                        </>
                        }
                        
                    </div>
                </div>
                

                
            </div>
        );
    }
}

export default MarkAsResovled;