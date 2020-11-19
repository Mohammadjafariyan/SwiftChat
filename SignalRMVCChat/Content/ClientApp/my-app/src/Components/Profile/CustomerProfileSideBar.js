import React, {Component, useState} from 'react';
import {Sidebar} from "primereact/sidebar";
import CustomerInfo from "../CustomerInfo";
import UsersSeparationData from "../UsersSeparation/UsersSeparationData";
import UserDeviceInfo from "../UserDeviceInfo";
import CustomerTimeSpent from "./CustomerTimeSpent";
import CustomerData from "../CustomerData";
import UserTags from "../UserTags";
import Col from "react-bootstrap/Col";
import {ScrollPanel} from "primereact/scrollpanel";
import {DataHolder} from "../../Help/DataHolder";
import LastPagesVisited from "../LastPagesVisited";

export const CustomerProfileSideBar = (props) => {
    const [visible, setVisible] = useState();

    React.useEffect(() => {
        setVisible(props.visible);
    }, [props.visible])

    return (
        <>

                <Sidebar position="right" visible={visible} onHide={() => {
                    setVisible(false);
                    props.parent.setState({customerProfileSideBarVisible: false})
                }}>
                    <ScrollPanel style={{width: '100%', height: '100vh'}} className="custombar1">
                    <CustomerInfo Customer={props.Customer}/>

                    <UsersSeparationData Customer={props.Customer}/>

                    <UserDeviceInfo Customer={props.Customer}/>


                    <CustomerTimeSpent Customer={props.Customer}/>

                    <CustomerData Customer={props.Customer}/>

                    <UserTags Customer={props.Customer}/>


                        <LastPagesVisited Customer={DataHolder.selectedCustomer}/>
                        
                        <div style={{height:'150px'}}></div>
                    </ScrollPanel>
                </Sidebar>

        </>
    );
}
