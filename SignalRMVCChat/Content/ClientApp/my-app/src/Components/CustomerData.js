import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Table from "react-bootstrap/Table";
import {InputGroup, FormControl} from "react-bootstrap";
import {_showError, _showMsg} from "../Pages/LayoutPage";

import {MyCaller, CurrentUserInfo} from './../Help/Socket';


class CustomerData extends Component {
    state = {};


    getCustomerDataList(res) {
        if (!res || !res.Content) {
            return;
        }
        
        
        

        this.setState({customerDataList: res.Content});
    }

    componentDidMount() {

        if (!DataHolder.selectedCustomer) {
            return;
        }
        MyCaller.Send('GetCustomerDataList', {customerId: DataHolder.selectedCustomer.Id})
    }

    render() {

        if (!DataHolder.selectedCustomer) {
            return <></>;
        }

        return (
            <div>


                <div
                    className={"card "}>
                    <div className="card-header">
                        اطلاعات کاربر
                    </div>

                    <div className="card-body" style={{display: 'flex', textAlign: 'center'}}>

                        <Table striped bordered hover>
                            <thead>

                            </thead>
                            <tbody>
                            <tr>

                                <td colSpan={'2'} style={{
                                    padding: '0px',
                                    margin: '0px'
                                }}>

                                    <form autocomplete="on">
                                        <small style={{display: 'flex'}}>

                                            <InputGroup>
                                                <InputGroup.Prepend>
                                                    <InputGroup.Text id="basic-addon1" onClick={() => {
                                                        
                                                        
                                                        if (this.state.sending){
                                                            
                                                            _showMsg('لطفا تا ذخیره کلید قبلی صبر کنید')
                                                        }else{
                                                            this.AddCustomerData();
                                                        }
                                                        
                                                    }}>

                                                        {!this.state.sending &&
                                                        <i className={'fa fa-plus'}></i>

                                                        }

                                                        {this.state.sending &&
                                                        <i className={'fa fa-circle-o-notch'}></i>

                                                        }

                                                    </InputGroup.Text>
                                                </InputGroup.Prepend>
                                                <FormControl autocomplete="on"
                                                             placeholder="مقدار"
                                                             aria-label="Username"
                                                             aria-describedby="basic-addon1"

                                                             value={this.state.value}

                                                             onChange={(e) => {

                                                                 this.setState({value: e.target.value})
                                                             }}
                                                />
                                                <FormControl autocomplete="on"
                                                             placeholder="کلید"
                                                             aria-label="Username"
                                                             value={this.state.key}
                                                             aria-describedby="basic-addon1"
                                                             onChange={(e) => {

                                                                 this.setState({key: e.target.value})
                                                             }}
                                                />
                                            </InputGroup>
                                        </small>
                                        <small>

                                        </small>
                                    </form>
                                </td>

                                {this.state.customerDataList &&
                                this.state.customerDataList.map((data, i, arr) => {


                                    return <tr>
                                        <td>{data.Key}</td>
                                        <td>{data.Value}</td>
                                    </tr>
                                })}

                            </tr>
                            </tbody>
                        </Table>


                    </div>
                </div>
            </div>
        );
    }
    

    addCustomerDataCallback(res){
        this.setState({sending:false});
        
        this.componentDidMount();
    }
    AddCustomerData() {

        if (!this.state.key) {
            _showError('کلید خالی است لطفا عنوان کلید را وارد کنید')
            return;
        }

        if (!this.state.value) {
            _showError('مقدار کلید خالی است لطفا مقدار کلید را وارد کنید')
            return;
        }
        

        _showMsg('در حال ذخیره کلید جدید')
        
        this.setState({sending:true})

        MyCaller.Send('SaveKey', {
            Key: this.state.key,
            Value: this.state.Value
        })
    }
}

export default CustomerData;