import React, {Component} from 'react';
import Table from "react-bootstrap/Table";
import {DataHolder} from "../../Help/DataHolder";

import '../../styles/myStyle.css'
class UsersSeparationData extends Component {
    render() {
        
        if (!this.props.Customer || !this.props.Customer.UsersSeparationParams){
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
                    {this.props.Customer && this.props.Customer.UsersSeparationParams &&
                    this.props.Customer.UsersSeparationParams.map((data, i, arr) => {


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