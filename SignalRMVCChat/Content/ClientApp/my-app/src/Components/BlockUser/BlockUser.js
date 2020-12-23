import React, {Component} from 'react';
import {Badge, Button} from "react-bootstrap";
import {DataHolder} from "../../Help/DataHolder";
import {_showMsg} from "../../Pages/LayoutPage";
import {CurrentUserInfo,MyCaller} from "../../Help/Socket";

class BlockUser extends Component {

    constructor(props) {
        super(props);
        CurrentUserInfo.BlockUser = this;
    }

    render() {
        if (!this.props.Customer) {
            return <></>;
        }


        return (
            <>


                {!this.props.Customer.IsBlocked && <Button
                aria-label="بلاک کردن" data-microtip-position="left" role="tooltip"
                 variant={'danger'} onClick={() => {


                    this.changeCustomerBlockStatus(true, "در حال بلاک کاربر");

                }}>


                        <i className="fa fa-ban" aria-hidden="true"></i>
                </Button>}


<hr/>
                {this.props.Customer.IsBlocked && <Button  aria-label="  خروج از بلاک" data-microtip-position="left" role="tooltip" variant={'danger'} onClick={() => {


                    this.changeCustomerBlockStatus(false, "در حال خروج از بلاک ");

                }}>


                  

                        <i className="fa fa-check-square" aria-hidden="true"></i>
                </Button>}


            </>
        );
    }


    changeCustomerBlockStatusCallback() {

        if (this.props.Customer.IsBlocked) {
            _showMsg("کاربر بلاک شد")

        } else {
            _showMsg("کاربر از بلاک خارج شد")

        }
    }

    changeCustomerBlockStatus(isBlocked, msg) {

        _showMsg(msg);

        this.props.Customer.IsBlocked = isBlocked;

        MyCaller.Send('changeCustomerBlockStatus',
            {customerId: this.props.Customer.Id, IsBlocked: isBlocked});
    }
}

export default BlockUser;