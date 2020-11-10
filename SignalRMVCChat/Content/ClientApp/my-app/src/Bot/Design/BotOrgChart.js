import React, {Component, useRef, useState} from 'react';


import './BotOrgChart.css'
import {OrganizationChart} from "primereact/organizationchart";
import {Fieldset} from "primereact/fieldset";
import {OverlayPanel} from "primereact/overlaypanel";
import {Button} from "primereact/button";
import {ScrollPanel} from "primereact/scrollpanel";
import {Dialog} from "primereact/dialog";
import BotEventNodeSetting from "./NodeSetting/BotEventNodeSetting";
import {MyHook} from "../../Components/Utilities/Utility";
import BotEventAction from "./NodeSetting/BotEventAction";
import BotEventCondition from "./NodeSetting/BotEventCondition";
import {ContextMenu} from "primereact/contextmenu";

import {Tooltip} from 'primereact/tooltip';
import {_showError} from "../../Pages/LayoutPage";
import {CurrentUserInfo, MyCaller} from "../../Help/Socket";
import {InputText} from "primereact/inputtext";
import {Checkbox} from "primereact/checkbox";
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/cjs/Col";
import {ToggleButton} from "primereact/togglebutton";

const eventBgColor = "#e9286f";
const conditionBgColor = "#a534b6";
const actionBgColor = "orange";


export const TypeNames = {
    Event: 1,
    Condition: 2,
    Action: 3,
    End: 4,
    Start: 5,
}


const white = "white";

class BotOrgChart extends Component {
    constructor(props) {
        super(props);


        this.props.parent.BotOrgChart = this;

        this.state = {
            selection: [],
            data1: []
        }

        this.data2 = [{
            label: 'F.C Barcelona',
            expanded: true,
            children: [
                {
                    label: 'F.C Barcelona',
                    expanded: true,
                    children: [
                        {
                            label: 'Chelsea FC'
                        },
                        {
                            label: 'F.C. Barcelona'
                        }
                    ]
                },
                {
                    label: 'Real Madrid',
                    expanded: true,
                    children: [
                        {
                            label: 'Bayern Munich'
                        },
                        {
                            label: 'Real Madrid'
                        }
                    ]
                }
            ]
        }];

        this.nodeTemplate = this.nodeTemplate.bind(this);
    }

    nodeTemplate(node) {
        switch (node.type) {

            case TypeNames.Event:

                return <BotOrgBox node={node} parent={this} bgColor={eventBgColor}/>
                break;

            case TypeNames.Action:
                return <BotOrgBox node={node} parent={this} bgColor={actionBgColor}/>
                break;

            case TypeNames.Condition:
                return <BotOrgBox node={node} parent={this} bgColor={conditionBgColor}/>
                break;

            case TypeNames.End:
                return <BotOrgBox node={node} parent={this} bgColor={"#25374f"}/>
                break;

            case TypeNames.Start:
                return <BotOrgBox node={node} parent={this} bgColor={"#25374f"}/>
                break;

        }

        if (node.type === "person") {

        }

        if (node.type === "department") {
            return node.label;
        }
    }

    componentDidMount() {

        let arr = [];
        arr.push(this.props.bot)

        this.setState({data1: arr})

        /* MyCaller.Send('BotGetScenario',{id:this.props.botId})*/
    }


    botGetScenarioCallback(res) {
        if (!res || !res.Content) {
            _showError(' مقدار بازگشتی از سرور نال است ');
            return;
        }


        this.setState({data1: res.Content})
    }


    render() {
        return (
            <ScrollPanel style={{width: '100%'}} className="custombar2">
                <div className="organizationchart-demo">


                    <div className="card">
                        <h5>
                            <br/>
                            {this.state.data1 && this.state.data1.length > 0 &&
                            <Row>
                                <Col>
                                    <InputText value={this.state.data1[0].Name}
                                               onChange={(e) => {

                                                   this.state.data1[0].Name = e.target.value;
                                                   this.setState({value: e.target.value});
                                               }}/>

                                    <label>عنوان</label>
                                </Col>
                                <Col>
                                    <div className="p-field-checkbox">
                                        <ToggleButton onIcon="pi pi-check" offIcon="pi pi-times"
                                                      onLabel="انتشار" offLabel="عدم انتشار"
                                                      
                                                      inputId="city1" name="city" value="Chicago"
                                                  onChange={(e) => {

                                                      if (this.state.data1[0].IsPublish) {
                                                          this.state.data1[0].IsPublish = false;
                                                      } else {
                                                          this.state.data1[0].IsPublish = true;
                                                      }

                                                      this.setState({mg: Math.random()});

                                                  }} checked={this.state.data1[0].IsPublish}/>
                                    </div>
                                </Col>
                                <Col>
                                    <div className="p-field-checkbox">
                                        <Checkbox inputId="city1" name="city" value="Chicago"
                                                  onChange={(e) => {

                                                      if (this.state.data1[0].ExecuteOnce) {
                                                          this.state.data1[0].ExecuteOnce = false;
                                                      } else {
                                                          this.state.data1[0].ExecuteOnce = true;
                                                      }

                                                      this.setState({mg: Math.random()});

                                                  }} checked={this.state.data1[0].ExecuteOnce}/>
                                        <label>فقط یک بار اجرا شود</label>
                                    </div>
                                </Col>

                            </Row>
                            }
                            سناریوی عملیات ربات</h5>
                        <hr/>


                        {this.state.data1 && this.state.data1.length > 0 &&
                        <OrganizationChart style={{minHeight: '500px', paddingBottom: '200px'}} value={this.state.data1}
                                           nodeTemplate={this.nodeTemplate}
                                           selection={this.state.selection} selectionMode="multiple"
                                           onSelectionChange={event => this.setState({selection: event.data})}
                                           className="custombar2"></OrganizationChart>}


                    </div>
                </div>
            </ScrollPanel>
        )
    }


    removeInTree(node) {


        let arr = [...this.state.data1]
        this.removeInTreeHelper(node, arr, this.state.data1)

        this.setState({data1: arr});

    }

    removeInTreeHelper(node, arr, parent) {
        if (!arr)
            return;

        arr = arr.filter(a => node != a)


        for (let i = 0; i < arr.length; i++) {

            this.removeInTreeHelper(node, arr[i].children, arr[i])
        }

        parent.children = arr;

    }
}

export default BotOrgChart;


const AddNode = (node, newNode) => {

    if (!node.children) {
        node.children = [];
    }

    node.children.push(newNode);
}

export const BotOrgBox = (props) => {

    let cm = useRef();

    const [openSetting, setOpenSetting] = useState();

    React.useEffect(() => {
        setOpenSetting(null);
    }, [props.value])


    let contextMenuItems = [
        {
            label: 'حذف',
            icon: 'pi pi-fw pi-trash',
            command: (event) => {
                // event.originalEvent: Browser event
                // event.item: MenuItem instance

                props.parent.removeInTree(props.node);
            },

        }
    ]


    const ToggleMe = () => {
        if (!props.node.data.panelCollapsed) {
            props.node.data.panelCollapsed = true
        } else {
            props.node.data.panelCollapsed = false;
        }

        props.parent.setState({panelCollapsed: true, md: Math.random()})
    }

    const NodeSetting = () => {

        switch (openSetting) {
            case TypeNames.Start:
                return <>

                </>
                break;
            case TypeNames.End:
                return <NodeSettingModal onHide={() => {

                    setOpenSetting(null)
                }} name={'End'}>

                </NodeSettingModal>
                break;
            case TypeNames.Event:
                return <NodeSettingModal title={'تنظیمات نود رویداد'} onHide={() => {

                    setOpenSetting(null)
                }} name={'BotEventNodeSettingOk'}>

                    <BotEventNodeSetting node={props.node}/>
                </NodeSettingModal>
                break;
            case TypeNames.Action:
                return <NodeSettingModal title={'تنظیمات نود عملیات'} onHide={() => {

                    setOpenSetting(null)
                }} name={'Action'}>


                    <BotEventAction node={props.node}/>

                </NodeSettingModal>
                break;
            case TypeNames.Condition:
                return <NodeSettingModal title={'تنظیمات نود شرط'} onHide={() => {

                    setOpenSetting(null)
                }} name={'Condition'}>


                    <BotEventCondition node={props.node}/>
                </NodeSettingModal>
                break;
        }

        return <></>


    }
    const Add = (type, text) => {

        AddNode(props.node, {
            type: type,
            expanded: true,
            data: {},
            label: text,
            botEvent: {},
            botAction: {},
            botCondition: {}
        });

        props.parent.setState({mg: Math.random()});
        ToggleMe();

    }

    return (
        <div style={{backgroundColor: props.bgColor, color: 'white'}} onContextMenu={(e) => cm.show(e)}
        >
            <ContextMenu model={contextMenuItems} ref={el => cm = el}></ContextMenu>
            <div className={'node-header'}>{props.node.label}</div>
            <div className="node-content">
                {props.node.type != TypeNames.End &&
                <Button icon="pi pi-sitemap"
                        className="p-button p-component p-button-rounded p-button-info p-button-icon-only"
                        onClick={() => {


                            ToggleMe();
                        }}></Button>

                }


                {props.node.type != TypeNames.End && props.node.type != TypeNames.Start &&
                <Button icon="pi pi-cog "
                        className="p-button p-component p-button-rounded p-button-info p-button-icon-only"
                        onClick={() => {


                            props.node.data.panelCollapsed = false;

                            setOpenSetting(props.node.type);

                        }}></Button>}


                <NodeSetting/>


                <Fieldset legend="Header" toggleable collapsed={!props.node.data.panelCollapsed}
                          style={{width: '200px'}}

                          onToggle={(e) => {
                              props.node.data.panelCollapsed = e.value
                              props.parent.this.setState({panelCollapsed: e.value, md: Math.random()})
                          }}>

                    <div>
                        <Button icon="pi pi-calendar-plus" label="افزودن رویداد" className="p-button-rounded "
                                style={{backgroundColor: eventBgColor, color: 'white'}}


                                onClick={() => {


                                    Add(TypeNames.Event, '(event) رویداد')

                                }}

                        ></Button>
                        <Button icon="pi pi-share-alt" label="افزودن شرط" className="p-button-rounded "
                                style={{backgroundColor: conditionBgColor, color: 'white'}}
                                onClick={() => {


                                    Add(TypeNames.Condition, 'شرط')


                                }}></Button>
                        <Button icon="pi pi-check-circle" label="افزودن عملیات"
                                className="p-button-rounded p-button-warning"
                                style={{backgroundColor: actionBgColor, color: 'white'}}
                                onClick={() => {

                                    Add(TypeNames.Action, 'عملیات')


                                }}></Button>
                        <Button icon="pi pi-download" label="افزودن خروج"
                                className="p-button-rounded p-button-secondary"
                                onClick={() => {

                                    Add(TypeNames.End, 'نود خروج')

                                }}></Button>
                    </div>

                </Fieldset>

            </div>
        </div>
    );


}


const NodeSettingModal = (props) => {

    const [show, setShow] = useState(true);


    const footer = () => {
        return <div>
            <Button label="تایید" icon="pi pi-check" onClick={() => {
                setShow(false)

                props.onHide();


                MyHook.call(props.name);

            }} autoFocus/>
        </div>;
    }


    return <Dialog header="تنظیمات نود" visible={show} position={'center'} modal footer={footer()}
                   onHide={() => {
                       setShow(false);
                       props.onHide();

                   }}>


        {props.children}

    </Dialog>
}








