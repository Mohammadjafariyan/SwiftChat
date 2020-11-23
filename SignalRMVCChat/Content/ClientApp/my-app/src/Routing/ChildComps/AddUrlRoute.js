import React, {Component} from 'react';
import Row from "react-bootstrap/cjs/Row";
import Col from "react-bootstrap/Col";
import {InputMask} from "primereact/inputmask";
import Button from "react-bootstrap/cjs/Button";
import {DataTable} from "primereact/datatable";
import {Column} from "primereact/column";
import {Dropdown} from "primereact/dropdown";
import {InputText} from "primereact/inputtext";
import Badge from "react-bootstrap/cjs/Badge";
import {RadioButton} from "primereact/radiobutton";


import './RoutingStyle.css'

class AddUrlRoute extends Component {
    constructor(props) {
        super(props);
        this.state = {
            type: null,
        };

        this.types = [
            {name: 'شامل', code: 'contains'},
            {name: 'برابر با', code: 'equals'},
        ];

        this.onCityChange = this.onCityChange.bind(this);
        this.typeTemplate = this.typeTemplate.bind(this);
        this.actionTemplate = this.actionTemplate.bind(this);


    }

    typeTemplate(row) {
        if (!row.type) {
            return <></>
        }
        return <span>{row.type.name}</span>
    }

    onCityChange(e) {
        this.setState({type: e.value});
    }

    componentDidMount() {
        if (!this.props.parent.state.selected.UrlRoutes) {
            this.props.parent.state.selected.UrlRoutes = [];
        }

        this.setState({
            type: this.types[0],
            applyWhich: 'route'
        })


    }

    actionTemplate(row) {

        return <Button variant={'danger'} onClick={() => {

            this.props.parent.state.selected.UrlRoutes = this.props.parent.state.selected.UrlRoutes.filter(f => f != row);

            this.setState({tmp: Math.random()});
        }}>

            حذف
            <Badge variant={'light'}>
                <i className={'fa fa-minus'}></i>
            </Badge>
        </Button>
    }


    newRecord() {
        if (!this.props.parent.state.selected.UrlRoutes) {
            this.props.parent.state.selected.UrlRoutes = [];
        }

        let url = this.state.urlRoute;
        let title = this.state.urlTitle;

        if (this.state.applyWhich == 'route')
            if (!url) {
                url = '*  به معنی تمامی آدرس ها'
            }

        if (this.state.applyWhich == 'title')
            if (!title) {
                title = '*  به معنی تمامی آدرس ها'
            }


        this.props.parent.state.selected.UrlRoutes.push({
            urlRoute: url,
            type: this.state.type,
            urlTitle: this.state.urlTitle,
            applyWhich:this.state.applyWhich
        })


        this.setState({
            urlRoute: '',
            urlTitle: '',
            mst: Math.random()
        });


    }

    render() {

        return (
            <>
                <Row>
                    <Col>

                        <Button onClick={() => {

                            this.newRecord();
                        }}>

                            <i className={'fa fa-plus'}></i>
                            <span>افزودن الگوی آدرس جدید</span>
                        </Button>

                    </Col>
                </Row>
                <hr/>
                <Row>


                    <Col>

                        <RadioButton value="title" name="applyWhich"
                                     onChange={(e) => this.setState({applyWhich: e.value})}
                                     checked={this.state.applyWhich === 'title'}/>
                        <label>در عنوان صفحه</label>
                        <RadioButton value="route" name="applyWhich"
                                     onChange={(e) => this.setState({applyWhich: e.value})}
                                     checked={this.state.applyWhich === 'route'}/>
                        <label>در آدرس صفحه</label>

                    </Col>

                    <Col>
                        <label>نحوه اعمال</label>
                        <br/>
                        <Dropdown value={this.state.type}
                                  options={this.types}
                                  onChange={this.onCityChange}
                                  optionLabel="name"
                                  placeholder="نوع اعمال"/>

                    </Col>
                </Row>
                {this.state.applyWhich == 'route' && <Row>
                    <Col>

                        <label>آدرس صفحه</label>
                        <br/>
                        <InputText style={{width: '100%'}}
                                   value={this.state.urlRoute}
                                   onChange={(e) =>
                                       this.setState({urlRoute: e.target.value})}

                                   placeholder="http://www. یا https://"/>
                        <small>خالی به معنای تمامی ادرس ها</small>


                    </Col>
                </Row>
                }
                {this.state.applyWhich == 'title' && <Row>
                    <Col>

                        <label>عنوان صفحه</label>
                        <br/>
                        <InputText style={{width: '100%'}}
                                   value={this.state.urlTitle}
                                   onChange={(e) =>
                                       this.setState({urlTitle: e.target.value})}

                                   placeholder="صفحه فروش"/>
                        <small>خالی به معنای تمامی عنوان ها</small>


                    </Col>
                </Row>
                }

                <hr/>

                <Row>
                    <Col>
                        {this.props.parent && this.props.parent.state && this.props.parent.state.selected && this.props.parent.state.selected.UrlRoutes &&
                        <DataTable value={this.props.parent.state.selected.UrlRoutes}
                                   paginator
                                   paginatorTemplate="CurrentPageReport FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink RowsPerPageDropdown"
                                   currentPageReportTemplate="نمایش {first} از {last} کل {totalRecords}" rows={10}
                                   rowsPerPageOptions={[10, 20, 50]}
                        >

                            <Column field="urlTitle" header="عنوان"></Column>
                            <Column field="urlRoute" header="آدرس"></Column>
                            <Column body={this.typeTemplate} header="نوع"></Column>
                            <Column body={this.actionTemplate} header="عملیات"></Column>
                        </DataTable>
                        }
                    </Col>
                </Row>

            </>
        );
    }
}

export default AddUrlRoute;