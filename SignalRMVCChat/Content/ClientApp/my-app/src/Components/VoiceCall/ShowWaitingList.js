import React, {Component} from 'react';
import {ListGroup} from "react-bootstrap";
import {ScrollPanel} from "primereact/scrollpanel";

class ShowWaitingList extends Component {
    render() {
        let list = this.props.list;

        if (!list) {
            list = [];
        }
        
        let arr = [...list];
        for (let i = 0; i < 20; i++) {
            arr.push(
                {anotherCallCustomer: {Name: 'کاربر تستی'}});
        }
        list = arr;


        return (
            <>

                <ScrollPanel style={{height: '100%'}}>
                    <ListGroup>

                        {list.map((rec) => {

                            let item = rec.anotherCallCustomer;
                            return (<ListGroup.Item>

                                <label>{item.Name}</label>

                                <button
                                    className={
                                        "p-button p-component p-button-rounded p-button-success p-button-icon-only"
                                    }
                                    onClick={() => {

                                        this.props.onWaitingCallSelect(rec);

                                    }}
                                >

                                    <i
                                        style={{fontSize: "30px"}}
                                        className="fa fa-phone"
                                        aria-hidden="true"
                                    ></i>
                                </button>

                            </ListGroup.Item>)

                        })}
                    </ListGroup>

                </ScrollPanel>

            </>
        );
    }
}

export default ShowWaitingList;