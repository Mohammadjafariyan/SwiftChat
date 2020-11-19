import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import Badge from "react-bootstrap/Badge";
import Table from "react-bootstrap/Table";
import {MyCaller, CurrentUserInfo} from './../Help/Socket';
import {_showError} from "../Pages/LayoutPage";

class LastPagesVisited extends Component {
    state = {}

    constructor(props) {
        super(props);
        CurrentUserInfo.LastPagesVisited = this;
    }


    componentDidMount() {
        if (!this.props.Customer) {
            return;
        }
        MyCaller.Send("GetLastVisitedPages", {customerId: this.props.Customer.Id})
    }

    getLastVisitedPagesCallback(res) {
        if (!res || !res.Content) {
            _showError("دیتای بازگشتی از سرور نال است")
            return;
        }

        if (!res.Content.length)
            return;

        if (!this.props.Customer) {
            return;
        }

        if (res.Content[0].CustomerId === this.props.Customer.Id) {
            this.setState({list: res.Content})
        }

    }


    render() {

        if (!this.props.Customer) {
            return <></>;
        }

        return (
            <div>


                <div
                    className={"card "}>
                    <div className="card-header">
                        آخرین صفحات بازدید
                    </div>

                    <div className="card-body" style={{display: 'flex', textAlign: 'center'}}>

                        <Table striped bordered hover>
                            <thead>
                            <tr>
                                <th><small>عنوان صفحه</small></th>
                            </tr>
                            </thead>
                            <tbody>
                            {this.state.list && this.state.list.map((l, i, arr) => {

                                return <tr key={i}>

                                    <td><a target={'_blank'} href={l.Url}>{l.PageTitle}</a></td>

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