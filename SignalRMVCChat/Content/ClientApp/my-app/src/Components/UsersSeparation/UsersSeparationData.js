import React, {Component} from 'react';
import Table from "react-bootstrap/Table";
import {DataHolder} from "../../Help/DataHolder";

import '../../styles/myStyle.css'
class UsersSeparationData extends Component {
    render() {
        
        if (!DataHolder.selectedCustomer || !DataHolder.selectedCustomer.UsersSeparationParams){
            return <></>
        }
        
        return (
            <div>

                <div
                    className={"card  "} >
                    <div className="card-header bg-info text-white">
                       کاربر عضو است
                    </div>

                    <div className="card-body usData " >
                <Table>

                    <tbody>
                    {DataHolder.selectedCustomer && DataHolder.selectedCustomer.UsersSeparationParams &&
                    DataHolder.selectedCustomer.UsersSeparationParams.map((data, i, arr) => {


                        return <tr>
                            <td>

                                {data.paramText}


                            </td>
                            <td>
                                {data.paramValue}


                            </td>
                        </tr>
                    })}
                    </tbody>
                </Table>


                    </div>
                </div>
            </div>
        );
    }
}

export default UsersSeparationData;