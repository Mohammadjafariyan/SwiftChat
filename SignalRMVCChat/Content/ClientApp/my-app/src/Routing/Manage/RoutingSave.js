import React, {Component} from 'react';
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import SelectAdmins from "../ChildComps/SelectAdmins";
import AddUrlRoute from "../ChildComps/AddUrlRoute";
import SelectSegments from "../ChildComps/SelectSegments";
import SelectStates from "../ChildComps/SelectStates";
import SelectCities from "../ChildComps/SelectCities";
import Button from "react-bootstrap/cjs/Button";
import {_showMsg} from "../../Pages/LayoutPage";
import {CurrentUserInfo,MyCaller} from "../../Help/Socket";

class RoutingSave extends Component {

    componentDidMount() {

        CurrentUserInfo.RoutingSave = this;

        this.setState({
            selected: this.props.selected
        });
    }

    save() {


        MyCaller.Send('RoutingSave', this.state.selected)

    }

    routingSaveCallback(res) {

        _showMsg('ذخیره شد')
        this.props.parent.setState({
            selected: null
        });
    }

    render() {
        return (
            <>
                <Row>

                    {/*-------- Select Admins----------*/}
                    <Col>
                        <SelectAdmins parent={this}/>
                    </Col>

                    {/*-------- add url route----------*/}
                    <Col>
                        <AddUrlRoute parent={this}/>
                    </Col>

                </Row>

                <Row>


                    {/*-------- based on segments ----------*/}
                    <Col>
                        <SelectSegments parent={this}/>
                    </Col>

                </Row>

                <Row>

                    {/*-------- تفکیک استانی----------*/}
                    <Col>
                        <SelectStates parent={this}/>
                    </Col>

                    {/*-------- تفکیک شهری ----------*/}
                    <Col>
                        <SelectCities parent={this}/>
                    </Col>

                </Row>

                <Row>

                    {/*-------- تفکیک استانی----------*/}
                    <Col>
                    </Col>

                    {/*-------- تفکیک شهری ----------*/}
                    <Col>


                        <Button onClick={() => {

                            this.save();

                        }}>
                            ذخیره
                        </Button>
                    </Col>

                </Row>


            </>
        );
    }
}

export default RoutingSave;