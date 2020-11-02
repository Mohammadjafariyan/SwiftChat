import React, {Component} from 'react';
import {Badge, Button} from "react-bootstrap";
import {DataHolder} from "../../Help/DataHolder";

class BlockUser extends Component {
    render() {
        if (!DataHolder.selectedCustomer){
            return  <></>;
        }
        
        
        return (
            <>


                {!DataHolder.selectedCustomer.IsBlocked && <Button variant={'danger'}>

                    بلاک کردن
                    
                    <Badge variant={'danger'}>
                        <i className="fa fa-ban" aria-hidden="true"></i>
                    </Badge>
                </Button>    }

                {DataHolder.selectedCustomer.IsBlocked && <Button variant={'danger'}>
                

                    خروج از بلاک

                    <Badge variant={'danger'}>
                        <i className="fa fa-check-square" aria-hidden="true"></i>
                    </Badge>
                </Button>    }
             
            
            </>
        );
    }
}

export default BlockUser;