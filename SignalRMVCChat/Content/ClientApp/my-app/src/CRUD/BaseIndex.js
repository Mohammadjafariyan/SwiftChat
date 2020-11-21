import React, {Component} from 'react';
import {MyCaller, CurrentUserInfo} from "../Help/Socket";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/cjs/Col";
import ListGroup from "react-bootstrap/ListGroup";
import {Checkbox} from "primereact/checkbox";
import BaseSave from "./BaseSave";
import Card from "react-bootstrap/cjs/Card";
import Button from "react-bootstrap/cjs/Button";
import {_showMsg} from "../Pages/LayoutPage";

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
        if (!res || !res.Content || !res.Content.EntityList) {
            return;
        }

        this.setState({list: res.Content.EntityList});
    }

    RenderWelcome() {
        return <> {this.props.RenderWelcome}</>
    }


    render() {
        return (
            <>
                <Row>

                    <Col md={2}>

                        {this.createNew()}
                        {this.renderMenu()}
                    </Col>
                    <Col md={10}>

                        {!this.state.selected && <>
                            {this.props.RenderWelcome && this.props.RenderWelcome()}
                        </>}


                        {this.state.selected && <>
                            <BaseSave  {...this.props} parent={this} save={this.props.save}
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

    setIsEnabled(isEnabled) {
        MyCaller.Send(this.props.setIsEnabled, {isEnabled,id:this.props.parent.state.selected.Id});

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
                            selected:row
                            });
                        }}>
                            {row.Name}
                        </Card.Link>
                        <br/>
                        <Checkbox
                            onChange={e => {
                                this.setState({tm: Math.random()})
                                row.IsEnabled = e.checked;

                                this.setIsEnabled(row.IsEnabled);


                            }}
                            checked={row.IsEnabled}></Checkbox>

                    </ListGroup.Item>

                })
            }
        </ListGroup>


    }

    save(){

        if (this.props.saveDraft){
            MyCaller.Send(this.props.saveDraft);
        }else{
            MyCaller.Send(this.props.save);
        }
        _showMsg("در حال ایجاد رکورد جدید")

    }
    
    saveCallback(res){
        _showMsg("رکورد جدید ایجاد شد")

        this.componentDidMount();
    }

    createNew() {
        return <Button onClick={()=>{
            this.props.save();

        }}>
            
            <i className={'fa fa-plus'}></i>
            <span>جدید</span>
        </Button>;
    }
}

export default BaseIndex;