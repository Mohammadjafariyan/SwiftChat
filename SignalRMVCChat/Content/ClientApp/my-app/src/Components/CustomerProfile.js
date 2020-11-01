import React, {Component} from 'react';
import Button from "react-bootstrap/Button";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../Help/Socket";
import Badge from "react-bootstrap/Badge";
import {Form} from "react-bootstrap";

class CustomerProfile extends Component {
    render() {
        return (
            <div>
                <Button variant={'light'} onClick={()=>{


                    CurrentUserInfo.Menu.setPage(DataHolder.Back)
                }}>
                    <i className={'fa fa-arrow-left'}></i>

                    بازگشت

                </Button>
            </div>
        );
    }
}

export default CustomerProfile;





export const BackButton=()=>{
    
    return   <div>
        <Button variant={'light'} onClick={()=>{


            CurrentUserInfo.Menu.setPage(DataHolder.Back)
        }}>
            <i className={'fa fa-arrow-left'}></i>

            بازگشت

        </Button>
    </div>
    
}