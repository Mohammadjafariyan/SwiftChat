import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import Table from "react-bootstrap/Table";
import {MyCaller, CurrentUserInfo} from './../Help/Socket';
import {_showError} from "../Pages/LayoutPage";

class LastPagesVisited extends Component {
    state={}
    
    componentDidMount() {
        if (!DataHolder.selectedCustomer){
            return ;
        }
        MyCaller.Send("GetLastVisitedPages",{customerId:DataHolder.selectedCustomer.Id})
    }

    getLastVisitedPagesCallback(res){
        if (!res || !res.Content) {
            _showError("دیتای بازگشتی از سرور نال است")
            return;
        }

        if (!DataHolder.selectedCustomer){
            return ;
        }
        
        if (res.Content.CustomerId===DataHolder.selectedCustomer.Id)
        {
            this.setState({list:res.Content.EntityList})
        }

    }
    


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
                            {this.state.list && this.state.list.map((l,i,arr)=>{
                                
                                return <tr>
                                    <td>{l.Count}</td>
                                    <td>{l.Title}</td>
                                    
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

export default LastPagesVisited;