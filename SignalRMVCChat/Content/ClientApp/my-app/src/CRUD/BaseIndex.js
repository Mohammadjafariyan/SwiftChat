import React, {Component} from 'react';
import {MyCaller} from "../Help/Socket";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/cjs/Col";
import ListGroup from "react-bootstrap/ListGroup";
import {Checkbox} from "primereact/checkbox";
import BaseSave from "./BaseSave";
import Card from "react-bootstrap/cjs/Card";
import Button from "react-bootstrap/cjs/Button";
import {_showMsg} from "../Pages/LayoutPage";
import {InputSwitch} from "primereact/inputswitch";


import './CRUDStyle.css';
import {CurrentUserInfo} from "../CurrentUserInfo";
class BaseIndex extends Component {
    state={};

    constructor(props) {
        super(props);
        
        this.props.parent.setState({Index:this});
    }


    componentDidMount() {

        MyCaller.Send(this.props.get);
    }

    getCallback(res) {
        if (!res || !res.Content) {
            return;
        }

        this.setState({list: res.Content});
    }

    RenderWelcome() {
        return <> {this.props.RenderWelcome}</>
    }


    render() {
        return (
            <>
                <Row>

                    <Col md={this.props.menuCols ?this.props.menuCols :2}>

                        {this.createNew()}
                        {this.renderMenu()}
                    </Col>
                    <Col md={this.props.bodyCols ?this.props.bodyCols :10}>

                        {!this.state.selected && <>
                            {this.props.RenderWelcome && this.props.RenderWelcome()}
                        </>}


                        {this.state.selected && <>
                            <BaseSave onSaveClick={this.props.onSaveClick}  {...this.props} parent={this} save={this.props.save}
                                      delete={this.props.delete}
                                      item={this.state.selected}>

                                {this.props.children}

                            </BaseSave>
                        </>}

                    </Col>


                </Row>
            </>
        );
    }

    setIsEnabled(isEnabled,row) {
        MyCaller.Send(this.props.setIsEnabled, {isEnabled,id:row.Id});

    }

    setIsEnabledCallback(res) {
        this.componentDidMount();
    }


    renderMenu() {
        if (!this.state.list || !this.state.list.length) {
            return <></>;
        }

        return <ListGroup>
            {
                this.state.list.map((row, i, arr) => {

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

    save(){

        if (this.props.saveDraft){
            MyCaller.Send(this.props.saveDraft,{});
        }else{
            MyCaller.Send(this.props.save,{});
        }
        _showMsg("در حال ایجاد رکورد جدید")

    }
    
    saveCallback(res){
        _showMsg("رکورد جدید ایجاد شد")

        this.componentDidMount();
    }

    saveDraftCallback(res){
        this.saveCallback(res);
    }

    createNew() {
        return <Button onClick={()=>{
            this.save();

        }}>
            
            <i className={'fa fa-plus'}></i>
            <span>جدید</span>
        </Button>;
    }
}

export default BaseIndex;