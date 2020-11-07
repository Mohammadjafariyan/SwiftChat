import React, {Component} from 'react';


import './BotOrgChart.css'
import {OrganizationChart} from "primereact/organizationchart";
import {Fieldset} from "primereact/fieldset";
import {OverlayPanel} from "primereact/overlaypanel";
import {Button} from "primereact/button";

const eventBgColor = "#e9286f";
const conditionBgColor = "#a534b6";
const actionBgColor = "orange";


const TypeNames={
    Event:1,
    Condition:2,
    Action:3,
    End:4,
    Start:5,
    
    
}


const white = "white";

class BotOrgChart extends Component {
    constructor(props) {
        super(props);

        this.state= {
            selection: [],
            data1 : [{
                label: 'نود شروع',
                type: TypeNames.Start,
                className: 'p-person',
                expanded: true,
                data: {name: 'Walter White', 'avatar': 'walter.jpg'},
              /*  children: [
                    {
                        label: 'CFO',
                        type: 'person',
                        className: 'p-person',
                        expanded: true,
                        data: {name: 'Saul Goodman', 'avatar': 'saul.jpg'},
                        children: [{
                            label: 'Tax',
                            className: 'department-cfo'
                        },
                            {
                                label: 'Legal',
                                className: 'department-cfo'
                            }],
                    },
                    {
                        label: 'COO',
                        type: 'person',
                        className: 'p-person',
                        expanded: true,
                        data: {name: 'Mike E.', 'avatar': 'mike.jpg'},
                        children: [{
                            label: 'Operations',
                            className: 'department-coo'
                        }]
                    },
                    {
                        label: 'CTO',
                        type: 'person',
                        className: 'p-person',
                        expanded: true,
                        data: {name: 'Jesse Pinkman', 'avatar': 'jesse.jpg'},
                        children: [{
                            label: 'Development',
                            className: 'department-cto',
                            expanded: true,
                            children: [{
                                label: 'Analysis',
                                className: 'department-cto'
                            },
                                {
                                    label: 'Front End',
                                    className: 'department-cto'
                                },
                                {
                                    label: 'Back End',
                                    className: 'department-cto'
                                }]
                        },
                            {
                                label: 'QA',
                                className: 'department-cto'
                            },
                            {
                                label: 'R&D',
                                className: 'department-cto'
                            }]
                    }
                ]*/
            }]
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
        switch (node.type){

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
                return    <BotOrgBox node={node} parent={this} bgColor={"#25374f"}/>
                break;

            case TypeNames.Start:
                return    <BotOrgBox node={node} parent={this} bgColor={"#25374f"}/>
                break;

        }
        
        if (node.type === "person") {
            
        }

        if (node.type === "department") {
            return node.label;
        }
    }

    render() {
        return (
            <div className="organizationchart-demo">
                <div className="card">
                    <h5>سناریوی عملیات ربات</h5>
                    <OrganizationChart value={this.state.data1} nodeTemplate={this.nodeTemplate}
                                       selection={this.state.selection} selectionMode="multiple"
                                       onSelectionChange={event => this.setState({selection: event.data})}
                                       className="company"></OrganizationChart>


                </div>
            </div>
        )
    }
}

export default BotOrgChart;




const AddNode=(node,newNode)=>{
    
    if (!node.children){
        node.children=[];
    }

    node.children.push(newNode);
}

export const BotOrgBox=(props)=>{

    const ToggleMe=()=>{
        if (!props.node.data.panelCollapsed) {
            props.node.data.panelCollapsed = true
        } else {
            props.node.data.panelCollapsed = false;
        }

        props.parent.setState({panelCollapsed: true, md: Math.random()})
    }
    
    const Add=(type,text)=>{

        AddNode( props.node,{
            type: type,
            expanded: true,
            data:{},
            label:text
        });

        props.parent.setState({mg:Math.random()});
        ToggleMe();

    }
    return (
        <div  style={{backgroundColor: props.bgColor, color: 'white'}}>
            <div className={'node-header'}>{props.node.label}</div>
            <div className="node-content">
                <Button icon="pi pi-share-alt"
                        className="p-button p-component p-button-rounded p-button-info p-button-icon-only"
                        onClick={() => {


                            ToggleMe();
                        }}></Button>


                <Fieldset legend="Header" toggleable collapsed={!props.node.data.panelCollapsed}
                          style={{width: '200px'}}

                          onToggle={(e) => {
                              props.node.data.panelCollapsed = e.value
                              props.parent. this.setState({panelCollapsed: e.value, md: Math.random()})
                          }}>

                    <div>
                        <Button icon="pi pi-calendar-plus" label="افزودن رویداد" className="p-button-rounded "
                                style={{backgroundColor: eventBgColor, color: 'white'}}
                        
                        
                                onClick={()=>{




                                    Add(TypeNames.Event,'(event) رویداد')
                                    
                                }}
                        
                        ></Button>
                        <Button icon="pi pi-share-alt" label="افزودن شرط" className="p-button-rounded "
                                style={{backgroundColor: conditionBgColor, color: 'white'}}
                                onClick={()=>{


                                    Add(TypeNames.Condition,'شرط')
                             

                                }}></Button>
                        <Button icon="pi pi-check-circle" label="افزودن عملیات"
                                className="p-button-rounded p-button-warning"
                                style={{backgroundColor: actionBgColor, color: 'white'}}
                                onClick={()=>{

                                    Add(TypeNames.Action,'عملیات')


                                }}></Button>
                        <Button icon="pi pi-download" label="افزودن خروج"
                                className="p-button-rounded p-button-secondary"
                                onClick={()=>{

                                    Add(TypeNames.End,'نود خروج')

                                }}></Button>
                    </div>

                </Fieldset>
            </div>
        </div>
    );
    
    
}