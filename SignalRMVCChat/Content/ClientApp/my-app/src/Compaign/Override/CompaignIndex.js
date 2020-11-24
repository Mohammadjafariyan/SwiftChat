import React, {Component} from 'react';
import BaseIndex from "../../CRUD/BaseIndex";
import ListGroup from "react-bootstrap/ListGroup";
import Card from "react-bootstrap/cjs/Card";
import {InputSwitch} from "primereact/inputswitch";
import CompaignFilter from "../Filter/CompaignFilter";

class CompaignIndex extends BaseIndex {
    getCallback(res) {
        if (!res || !res.Content) {
            return;
        }

        let automatic= res.Content.filter(f=>f.IsAutomatic);
        let oneshot= res.Content.filter(f=>!f.IsAutomatic);
        this.setState({automatic: automatic,
            oneshot:oneshot});
    }
    
    
    render(): * {
        return  <>

            {!this.state.selected && <>
                <CompaignFilter></CompaignFilter>            <br/>
            </>}
            { super.render()} </>;
    }

    renderMenu() {
     

        return <>
            {this.renderMenuHelp('کمپین های اتوماتیک',this.state.automatic)}
            {this.renderMenuHelp('کمپین های دستی',this.state.oneshot)}
            
            </>


    }

    renderMenuHelp(title,list) {
        if (!list || !list.length) {
            return <></>;
        }
        return <ListGroup>

            <ListGroup.Item>{title}</ListGroup.Item>
            {
                list.map((row, i, arr) => {

                    return <ListGroup.Item>


                        <Card.Link onClick={()=>{

                            this.setState({
                                selected:null
                            });

                            this.props.parent.setState({
                                selected:null
                            });

                            setTimeout(()=>{
                                this.setState({
                                    selected:row
                                });

                                this.props.parent.setState({
                                    selected:row
                                });

                            },100)
                        }}>
                            {row.Name}
                        </Card.Link>
                        <br/>
                        <InputSwitch
                            onChange={e => {
                                row.IsEnabled = e.value;

                                this.setIsEnabled(row.IsEnabled,row);

                                this.setState({tm: Math.random()})

                            }}
                            checked={row.IsEnabled}></InputSwitch>

                    </ListGroup.Item>

                })
            }
        </ListGroup>
    }
}

export default CompaignIndex;