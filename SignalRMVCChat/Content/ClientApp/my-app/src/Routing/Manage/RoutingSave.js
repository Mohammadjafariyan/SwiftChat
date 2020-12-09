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
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import Card from "react-bootstrap/Card";
import IsAuthenticated from "../ChildComps/IsAuthenticated";
import {Fieldset} from "primereact/fieldset";
import Badge from "react-bootstrap/Badge";
import RoutingInfo from "../ChildComps/RoutingInfo";
import IsResolved from "../ChildComps/IsResolved";

class RoutingSave extends Component {
    state = {};

    componentDidMount() {

        CurrentUserInfo.RoutingSave = this;

        this.props.parent.setState({Save: this});

        console.log('RoutingSave->this.props.selected', this.props.selected);
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
        if (!this.state.selected)
            return <></>
        return (
            <>
                <MyFieldset title="اطلاعات عمومی">
                    <Row>
                        <Col>
                            <MyCard header={'عنوان'} >
                                <RoutingInfo parent={this}/>
                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>
                <MyFieldset title="اختصاص اوپراتور ها">
                <Row>
                    <Col>
                        <MyCard header={'انتخاب اوپراتور ها'} title={'به چه اوپراتور هایی اختصاص یابد ؟'}>
                            <SelectAdmins parent={this}/>
                        </MyCard>
                    </Col>
                </Row>
                </MyFieldset>
                <MyFieldset title="در شرایط زیر کاربر به اوپراتور اختصاص می یابد">

                    <Row>
                        <Col>
                            <MyCard  header={'انتخاب برچسب ها'} title={'در صورتی که برچسب بخصوصی به بازدید کننده زده شود  ، برچسب های تعریف شده اینجا قابل انتخاب است'}>
                                <SelectSegments parent={this}/>
                            </MyCard>
                        </Col>

                        <Col>
                            <MyCard  header={'انتخاب استان ها'} title={'بازدید کننده از چه استانی باشد'}>
                                <SelectStates  parent={this}/>
                            </MyCard>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <MyCard  header={'انتخاب شهر ها'} title={' بازدید کننده از چه شهر باشد'}>
                                <SelectCities parent={this}/>
                            </MyCard>
                        </Col>
                        <Col>
                            <MyCard header={'شرط عضو بودن'} title={' بازدید کننده عضو باشد'}>
                                <IsAuthenticated parent={this}/>
                            </MyCard>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                           
                        </Col>
                        <Col>
                            <MyCard header={'شرط وضعیت گفتگو'} title={'گفتگو هایی که وضعیت آن ها حل شده یا نشده است'}>
                                <IsResolved parent={this}/>
                            </MyCard>
                        </Col>
                    </Row>

                    
                    <Row>
                        <Col>
                            <MyCard>
                                <AddUrlRoute parent={this}/>
                            </MyCard>
                        </Col>
                    </Row>
                </MyFieldset>




            {/*    <Row>
                    <Col>
                        <Button onClick={() => {

                            this.save();

                        }}>
                            ذخیره
                        </Button> </Col>

                    -------- based on segments ----------


                </Row>*/}
            


            </>
        );
    }
}

export default RoutingSave;


export const MyCard=(props)=>{
    return <Card className={props.className} >
        <Card.Header>{props.header}</Card.Header>
        <Card.Body>
            <Card.Title>{props.title}</Card.Title>
            <Card.Text>
                {props.children}
            </Card.Text>

        </Card.Body>
    </Card>
}

export const MyFieldset=(props)=>{
    return <>

        <br/>

        <span style={{textAlign:'right',float:'right'}}>
            <Badge variant={props.variant ? props.variant:'info'}>
            
            {props.title}
            
        </Badge>
        </span>
        <br/>
        <br/>
        {props.children}

    </>
}
