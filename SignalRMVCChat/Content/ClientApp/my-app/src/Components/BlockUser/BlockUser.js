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
        if (!DataHolder.selectedCustomer) {
            return <></>;
        }


        return (
            <>


                {!DataHolder.selectedCustomer.IsBlocked && <Button variant={'danger'} onClick={() => {


                    this.changeCustomerBlockStatus(true, "در حال بلاک کاربر");

                }}>

                    بلاک کردن

                    <Badge variant={'danger'}>
                        <i className="fa fa-ban" aria-hidden="true"></i>
                    </Badge>
                </Button>}

                {DataHolder.selectedCustomer.IsBlocked && <Button variant={'danger'} onClick={() => {


                    this.changeCustomerBlockStatus(false, "در حال خروج از بلاک ");

                }}>


                    خروج از بلاک

                    <Badge variant={'danger'}>
                        <i className="fa fa-check-square" aria-hidden="true"></i>
                    </Badge>
                </Button>}


            </>
        );
    }


    changeCustomerBlockStatusCallback() {

        if (DataHolder.selectedCustomer.IsBlocked) {
            _showMsg("کاربر بلاک شد")

        } else {
            _showMsg("کاربر از بلاک خارج شد")

        }
    }

    changeCustomerBlockStatus(isBlocked, msg) {

        _showMsg(msg);

        DataHolder.selectedCustomer.IsBlocked = isBlocked;

        MyCaller.Send('changeCustomerBlockStatus',
            {customerId: DataHolder.selectedCustomer.Id, IsBlocked: isBlocked});
    }
}

export default BlockUser;