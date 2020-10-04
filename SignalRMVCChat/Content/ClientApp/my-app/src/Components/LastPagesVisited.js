import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import Table from "react-bootstrap/Table";

class LastPagesVisited extends Component {
    render() {

        if (!DataHolder.selectedCustomer)
        {
            return <></>;
        }

        return (
            <div>


                <div
                    className={"card "}>
                    <div className="card-header">
                        آخرین صفحات بازدید 
                    </div>

                    <div className="card-body" style={{display:'flex',textAlign:'center'}}>

                        <Table striped bordered hover>
                            <thead>
                            <tr>
                                <th><small>میزان بازدید</small></th>
                                <th><small>عنوان صفحه</small></th>
                            </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </Table>


                    </div>
                </div>
            </div>
        );
    }
}

export default LastPagesVisited;